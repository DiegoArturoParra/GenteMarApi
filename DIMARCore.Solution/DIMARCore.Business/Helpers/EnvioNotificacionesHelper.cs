using DIMARCore.Utilities.CorreoSMTP;
using DIMARCore.Utilities.Helpers;
using System.Threading.Tasks;

namespace DIMARCore.Business.Helpers
{
    public class EnvioNotificacionesHelper
    {

        public async Task<Respuesta> SendNotificationByEmail(SendEmailRequest request)
        {
            var response = await new EMailService().SendMail(request);

            _ = new DbLoggerHelper().InsertLogToDatabase(response);
            return response;
        }
    }
}
