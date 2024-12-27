using DIMARCore.Utilities.CorreoSMTP;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using System.Configuration;
using System;
using System.Threading.Tasks;

namespace DIMARCore.Business.Helpers
{
    public class EnvioNotificacionesHelper
    {
        private readonly int _environmentValue;
        public EnvioNotificacionesHelper()
        {
            _environmentValue = Convert.ToInt32(ConfigurationManager.AppSettings[Constantes.KEY_ENVIRONMENT]);
        }

        public async Task<Respuesta> SendNotificationByEmail(SendEmailRequest request)
        {
            if ((int)EnvironmentEnum.Production != _environmentValue)
            {
                request.Titulo = $"[TEST] {request.Titulo}";
                request.Asunto = $"[TEST] {request.Asunto}";
                request.CuerpoDelMensaje = $"[TEST] {request.CuerpoDelMensaje}";
            }

            var response = await new EMailService().SendMail(request);

            _ = new DbLoggerHelper().InsertLogToDatabase(response);
            return response;
        }
    }
}
