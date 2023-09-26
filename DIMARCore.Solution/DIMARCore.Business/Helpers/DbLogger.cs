using DIMARCore.Repositories;
using DIMARCore.Utilities.Config;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using log4net;
using System;
using System.Threading.Tasks;

namespace DIMARCore.Business.Helpers
{
    /// <summary>
    /// Clase que inserta en la base de datos los logs de la aplicación
    /// </summary>
    public class DbLogger
    {
        public DbLogger()
        {
        }
        public Task InsertLogToDatabase(Respuesta response)
        {
            var loginName = ClaimsHelper.GetLoginName();
            _ = Task.Run(async () =>
            {
                ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                try
                {
                    using (var logRepository = new LogRepository())
                    {
                        var log = new GENTEMAR_LOGS
                        {
                            MESSAGE_EXCEPTION = !string.IsNullOrWhiteSpace(response.MensajeExcepcion) ? response.MensajeExcepcion : null,
                            MESSAGE_WARNING = !string.IsNullOrWhiteSpace(response.Mensaje) ? response.Mensaje : null,
                            STATUS_CODE = (int)response.StatusCode,
                            USER_SESSION = loginName,
                            DATE_CREATED = DateTime.Now,
                            SEVERITY_LEVEL = (int)response.StatusCode >= 400 && (int)response.StatusCode < 500 ? "WARNING" : "ERROR"
                        };
                        await logRepository.Create(log);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
            });
            return Task.FromResult(true);
        }
    }
}
