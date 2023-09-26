using DIMARCore.Business.Helpers;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using log4net;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace DIMARCore.Api.Core
{
    /// <summary>
    /// Excepcion generica filtro que guarda el log (warning y error)
    /// </summary>
    public class CustomExceptionFilter : ExceptionFilterAttribute
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
            if (actionExecutedContext.Exception is HttpStatusCodeException)
            {
                var exception = (HttpStatusCodeException)actionExecutedContext.Exception;
                response.Mensaje = exception.Message;
                response.Estado = false;
                response.StatusCode = exception.StatusCode;
                status = exception.StatusCode;
                string json = JsonConvert.SerializeObject(response);
                _logger.Warn(json);
            }
            else if (actionExecutedContext.Exception is ValidationException validationException)
            {
                // Manejo de excepciones de validación (basadas en Data Annotations)
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Mensaje = "Error de validación";
                response.Estado = false;
                response.Data = validationException.ValidationResult;
                status = HttpStatusCode.BadRequest;
                string json = JsonConvert.SerializeObject(response);
                _logger.Warn(json);
            }
            else
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.MensajeExcepcion = actionExecutedContext.Exception.Message;
                response.Data = actionExecutedContext.Exception.InnerException;
                status = HttpStatusCode.InternalServerError;
                string json = JsonConvert.SerializeObject(response);
                _logger.Error(json);

            }
            new DbLogger().InsertLogToDatabase(response);
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(status, response);
        }
    }

}