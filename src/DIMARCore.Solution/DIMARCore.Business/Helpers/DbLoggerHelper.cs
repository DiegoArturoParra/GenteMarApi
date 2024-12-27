using DIMARCore.Repositories;
using DIMARCore.Utilities.Config;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DIMARCore.Business.Helpers
{
    /// <summary>
    /// Clase que inserta en la base de datos los logs de la aplicación
    /// </summary>
    public class DbLoggerHelper
    {
        ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DbLoggerHelper()
        {
        }
        public async Task InsertLogToDatabase(Respuesta response)
        {
            var loginName = ClaimsHelper.GetLoginName();
            try
            {
                using (var logRepository = new LogRepository())
                {
                    var log = CreateLog(response, loginName);
                    await logRepository.Create(log);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public async Task InsertSomeLogsToDatabase(List<Respuesta> responses)
        {
            var loginName = ClaimsHelper.GetLoginName();

            var errores = new List<GENTEMAR_LOGS>();
            try
            {
                using (var logRepository = new LogRepository())
                {
                    foreach (var item in responses)
                    {
                        var log = CreateLog(item, loginName);
                        errores.Add(log);
                    }
                    await logRepository.CreateSomeLogs(errores);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private GENTEMAR_LOGS CreateLog(Respuesta response, string loginName)
        {
            var log = new GENTEMAR_LOGS
            {
                MESSAGE_INFO = GetMessageInfo(response),
                MESSAGE_EXCEPTION = GetMessageException(response),
                MESSAGE_WARNING = GetMessageWarning(response),
                STATUS_CODE = (int)response.StatusCode,
                USER_SESSION = loginName,
                DATE_CREATED = DateTime.Now,
                SEVERITY_LEVEL = GetSeverityLevel(response.StatusCode),
                STACK_TRACE = response.StackTrace
            };
            return log;
        }

        private string GetMessageInfo(Respuesta item)
        {
            return !string.IsNullOrWhiteSpace(item.Mensaje) && (int)item.StatusCode >= 200
                && (int)item.StatusCode < 400 ? item.Mensaje : null;
        }

        private string GetMessageException(Respuesta item)
        {
            return !string.IsNullOrWhiteSpace(item.MensajeExcepcion) ? item.MensajeExcepcion : null;
        }

        private string GetMessageWarning(Respuesta item)
        {
            return !string.IsNullOrWhiteSpace(item.Mensaje) && (int)item.StatusCode >= 400
                && (int)item.StatusCode < 500 ? item.Mensaje : null;
        }

        private string GetSeverityLevel(HttpStatusCode statusCode)
        {
            if (statusCode >= HttpStatusCode.OK && statusCode < HttpStatusCode.BadRequest)
            {
                return "INFO";
            }
            else if (statusCode >= HttpStatusCode.BadRequest && statusCode < HttpStatusCode.InternalServerError)
            {
                return "WARNING";
            }
            else
            {
                return "ERROR";
            }
        }

        public void LogToFile(Respuesta respuesta)
        {
            var loginName = ClaimsHelper.GetLoginName();
            var log = CreateLog(respuesta, loginName);
            _logger.Info(log);
        }
    }
}
