using DIMARCore.Business.Helpers;
using DIMARCore.Utilities.Helpers;
using log4net;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace DIMARCore.Api.Core.Filters
{
    /// <summary>
    /// filtro para mostrar invalido el modelo
    /// </summary>
    public class ValidationExceptionFilterAttribute : ActionFilterAttribute
    {
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// metodo que muestra los errores
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var modelState = actionContext.ModelState;
            var response = new Respuesta();
            if (!modelState.IsValid)
            {
                var errorMessages = modelState.Values
                    .SelectMany(v => v.Errors)
                    .Where(e => e != null && !string.IsNullOrEmpty(e.ErrorMessage))
                    .Select(e => e.ErrorMessage);

                if (errorMessages.Any())
                {
                    response.Mensaje = string.Join(", ", errorMessages);
                    response.Estado = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    _ = new DbLoggerHelper().InsertLogToDatabase(response);
                    string json = JsonConvert.SerializeObject(response);
                    _logger.Warn(json);
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, response);
                }
                else
                {
                    var errorExceptions = modelState.Values
                    .SelectMany(v => v.Errors)
                    .Where(e => e != null)
                    .Select(e => e.Exception);
                    response.MensajeExcepcion = string.Join(", ", errorExceptions);
                    response.Estado = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    _ = new DbLoggerHelper().InsertLogToDatabase(response);
                    string json = JsonConvert.SerializeObject(response);
                    _logger.Error(json);
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.InternalServerError, response);
                }
            }
        }
    }
}