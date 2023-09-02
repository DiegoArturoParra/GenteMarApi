using DIMARCore.Utilities.Config;
using DIMARCore.Utilities.Middleware;
using DIMARCore.Utilities.Seguridad;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DIMARCore.Utilities.CorreoSMTP
{
    public interface IEmailService
    {
        Task SendMail(string[] correoDestino, string mensaje, string body, string title = "GDM", string footer = "por favor comuniquese con el administrador.");
    }
    public class EMailService : IEmailService
    {
        private readonly string _from;
        private readonly int _port;
        private readonly string _password;
        private readonly string _host;
        public EMailService()
        {
            _from = SecurityEncrypt.GenerateDecrypt(ConfigurationManager.AppSettings[SmtpConfig.EMAIL]);
            _password = SecurityEncrypt.GenerateDecrypt(ConfigurationManager.AppSettings[SmtpConfig.PWD]);
            _host = ConfigurationManager.AppSettings[SmtpConfig.HOST];
            _port = Convert.ToInt16(ConfigurationManager.AppSettings[SmtpConfig.PORT]);
        }

        public async Task SendMail(string[] correosDestino, string mensaje, string body, string title = "GDM", string footer = "por favor comuniquese con el administrador.")
        {
            try
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory.Insert(AppDomain.CurrentDomain.BaseDirectory.Length, "Recursos\\PlantillaCorreo.html");
                var Emailtemplate = new StreamReader(ruta);
                var strBody = string.Format(Emailtemplate.ReadToEnd());
                Emailtemplate.Close();
                Emailtemplate.Dispose();
                Emailtemplate = null;
                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(_from, "Sistema GDM");

                foreach (var item in correosDestino)
                {
                    mailMessage.To.Add(item);
                }

                mailMessage.Subject = mensaje;
                strBody = strBody.Replace("@TITLE", title);
                strBody = strBody.Replace("@BODY", body);
                strBody = strBody.Replace("@FOOTER", footer);

                mailMessage.Body = strBody;

                mailMessage.IsBodyHtml = true;
                mailMessage.Priority = MailPriority.Normal;
                using (var client = new SmtpClient(_host, _port))
                {

                    client.EnableSsl = false;
                    client.UseDefaultCredentials = false;
                    // Pass SMTP credentials
                    client.Credentials = new NetworkCredential(_from, _password);
                    await client.SendMailAsync(mailMessage);
                    client.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, $"Error: No se pudo enviar el correo, {ex.Message}");
            }
        }
    }
}
