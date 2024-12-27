using DIMARCore.Utilities.Config;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Seguridad;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DIMARCore.Utilities.CorreoSMTP
{
    public interface IEmailService
    {
        Task<Respuesta> SendMail(SendEmailRequest request);
    }
    public class EMailService : IEmailService
    {
        private readonly string _from;
        private readonly int _port;
        private readonly string _password;
        private readonly string _host;
        public EMailService()
        {
            _from = EncryptDecryptService.GenerateDecrypt(ConfigurationManager.AppSettings[SmtpConfig.EMAIL]);
            _password = EncryptDecryptService.GenerateDecrypt(ConfigurationManager.AppSettings[SmtpConfig.PWD]);
            _host = ConfigurationManager.AppSettings[SmtpConfig.HOST];
            _port = Convert.ToInt16(ConfigurationManager.AppSettings[SmtpConfig.PORT]);
        }

        public EMailService(string from, string password, string host, int port)
        {
            _from = from;
            _password = password;
            _host = host;
            _port = port;
        }

        public async Task<Respuesta> SendMail(SendEmailRequest datosNotificacion)
        {
            try
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory.Insert(AppDomain.CurrentDomain.BaseDirectory.Length, "\\Recursos\\PlantillaCorreo.html");
                var Emailtemplate = new StreamReader(ruta);
                var strBody = string.Format(Emailtemplate.ReadToEnd());
                Emailtemplate.Close();
                Emailtemplate.Dispose();
                Emailtemplate = null;
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_from, "Sistema GDM"),
                    SubjectEncoding = Encoding.UTF8,
                    BodyEncoding = Encoding.UTF8,
                    IsBodyHtml = true // Set this to true if your body contains HTML
                };

                foreach (var item in datosNotificacion.CorreosDestino)
                {
                    mailMessage.To.Add(item);
                }

                if (!string.IsNullOrWhiteSpace(datosNotificacion.CC))
                {
                    if (datosNotificacion.CC.Contains(","))
                    {
                        foreach (string item in datosNotificacion.CC.Split(','))
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                mailMessage.CC.Add(item);
                            }
                        }
                    }
                    else
                    {
                        mailMessage.CC.Add(datosNotificacion.CC);
                    }
                }


                mailMessage.Subject = datosNotificacion.Asunto;
                strBody = strBody.Replace("@TITLE", datosNotificacion.Titulo);
                strBody = strBody.Replace("@BODY", datosNotificacion.CuerpoDelMensaje);
                strBody = strBody.Replace("@FOOTER", datosNotificacion.Footer);

                mailMessage.Body = strBody;
                mailMessage.IsBodyHtml = true;
                mailMessage.Priority = MailPriority.Normal;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using (var client = new SmtpClient(_host, _port))
                {
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    // Pass SMTP credentials
                    client.Credentials = new NetworkCredential(_from, _password);
                    await client.SendMailAsync(mailMessage);
                    client.Dispose();
                    return Responses.SetOkResponse(null, "Correo enviado correctamente.");
                }
            }
            catch (Exception ex)
            {
                return Responses.SetConflictResponse($"Error: No se pudo enviar el correo, {ex.Message}");
            }
        }
    }
}
