using DIMARCore.Repositories;
using DIMARCore.Utilities.Config;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
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
                            MESSAGE_INFO = !string.IsNullOrWhiteSpace(response.Mensaje) && (int)response.StatusCode >= 200
                                && (int)response.StatusCode < 400 ? response.Mensaje : null,
                            MESSAGE_EXCEPTION = !string.IsNullOrWhiteSpace(response.MensajeExcepcion) ? response.MensajeExcepcion : null,
                            MESSAGE_WARNING = !string.IsNullOrWhiteSpace(response.Mensaje) && (int)response.StatusCode >= 400
                                && (int)response.StatusCode < 500 ? response.Mensaje : null,
                            STATUS_CODE = (int)response.StatusCode,
                            USER_SESSION = loginName,
                            DATE_CREATED = DateTime.Now,
                            SEVERITY_LEVEL = GetSeverityLevel(response.StatusCode),
                            STACK_TRACE = response.Data != null ? JsonConvert.SerializeObject(response.Data) : null
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


        public Task InsertLogsToDatabase(List<Respuesta> responses)
        {
            var loginName = ClaimsHelper.GetLoginName();
            _ = Task.Run(async () =>
            {
                var errores = new List<GENTEMAR_LOGS>();
                ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                try
                {
                    using (var logRepository = new LogRepository())
                    {
                        foreach (var item in responses)
                        {
                            var log = new GENTEMAR_LOGS
                            {
                                MESSAGE_INFO = !string.IsNullOrWhiteSpace(item.Mensaje) && (int)item.StatusCode >= 200
                                && (int)item.StatusCode < 400 ? item.Mensaje : null,
                                MESSAGE_EXCEPTION = !string.IsNullOrWhiteSpace(item.MensajeExcepcion) ? item.MensajeExcepcion : null,
                                MESSAGE_WARNING = !string.IsNullOrWhiteSpace(item.Mensaje) && (int)item.StatusCode >= 400
                                && (int)item.StatusCode < 500 ? item.Mensaje : null,
                                STATUS_CODE = (int)item.StatusCode,
                                USER_SESSION = loginName,
                                DATE_CREATED = DateTime.Now,
                                SEVERITY_LEVEL = GetSeverityLevel(item.StatusCode),
                                STACK_TRACE = item.Data != null ? JsonConvert.SerializeObject(item.Data) : null
                            };
                            errores.Add(log);
                        }
                        await logRepository.CreateSomeLogs(errores);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
            });
            return Task.FromResult(true);
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
    }
}
