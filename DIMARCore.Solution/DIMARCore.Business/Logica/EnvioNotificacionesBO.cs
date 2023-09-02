using DIMARCore.Utilities.CorreoSMTP;
using log4net;
using System;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class EnvioNotificacionesBO
    {
        public async Task SendNotificationByEmail(ILog logger, SendEmailRequest request)
        {
            try
            {
                await new EMailService().SendMail(correosDestino: request.CorreosAEnviar,
                    mensaje: request.Asunto, body: request.CuerpoDelMensaje, "GENTE DE MAR", request.Footer);
            }
            catch (Exception ex)
            {
                // Utiliza el logger pasado como parámetro
                logger?.Error(ex);
                throw; // Propaga la excepción si es necesario
            }
        }
    }
}
