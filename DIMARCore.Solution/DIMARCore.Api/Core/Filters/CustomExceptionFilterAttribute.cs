using DIMARCore.Business.Helpers;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using log4net;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace DIMARCore.Api.Core.Filters
{
    /// <summary>
    /// Excepcion generica filtro que guarda el log (warning y error)
    /// </summary>
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Metodo onException que retorna y guarda el log.
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            HttpStatusCode status;
            var response = new Respuesta();
            if (actionExecutedContext.Exception is HttpStatusCodeException exception)
            {
                response.Mensaje = exception.Message;
                response.MensajeIngles = exception.MessageEnglish;
                response.MensajeExcepcion = exception.MessageException;
                response.Estado = false;
                response.StackTrace = !string.IsNullOrWhiteSpace(exception.StackTrace)
                                    && string.IsNullOrWhiteSpace(exception.StackTraced) ? exception.StackTrace :
                                    !string.IsNullOrWhiteSpace(exception.StackTraced) ? exception.StackTraced : null;
                response.StatusCode = exception.StatusCode;
                status = exception.StatusCode;
                string json = JsonConvert.SerializeObject(response);
                _logger.Warn(json);
            }
            else
            {
                string erroEspecifico = actionExecutedContext.Exception.InnerException?.Message;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.MensajeExcepcion = $"{actionExecutedContext.Exception.Message} {erroEspecifico}";
                response.StackTrace = actionExecutedContext.Exception.StackTrace;
                status = HttpStatusCode.InternalServerError;
                string json = JsonConvert.SerializeObject(response);
                _logger.Error(json);
            }
            _ = new DbLoggerHelper().InsertLogToDatabase(response);
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(status, response);
        }
    }

}