using DIMARCore.Utilities.Helpers;
using log4net;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace System.Web.Http.Filters
{
    /// <summary>
    /// filtro para mostrar invalido el modelo
    /// </summary>
    public class ValidationActionFilter : ActionFilterAttribute
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
                response.Mensaje = string.Join(" , ", modelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                response.Estado = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                string json = JsonConvert.SerializeObject(response);
                _logger.Warn(json);
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, response);
            }
        }
    }
}