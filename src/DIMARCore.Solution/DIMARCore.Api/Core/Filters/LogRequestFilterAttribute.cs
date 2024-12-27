using DIMARCore.Business.Helpers;
using DIMARCore.Utilities.Helpers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace DIMARCore.Api.Core.Filters
{
    /// <summary>
    /// log de peticiones
    /// </summary>
    public class LogRequestFilterAttribute : ActionFilterAttribute
    {
        private readonly DbLoggerHelper _dbLoggerHelper;
        /// <summary>
        /// constructor
        /// </summary>
        public LogRequestFilterAttribute()
        {
            _dbLoggerHelper = new DbLoggerHelper();
        }
        /// <summary>
        /// log de peticiones
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // Aquí puedes agregar el código para el registro de peticiones
            string methodName = actionContext.ActionDescriptor.ActionName;
            string controllerName = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
            string httpMethod = actionContext.Request.Method.Method;

            Respuesta respuesta = new Respuesta
            {
                Mensaje = $"Se ha ejecutado el método [{methodName}] en el controlador [{controllerName}] con el verbo [{httpMethod}]",
                StatusCode = System.Net.HttpStatusCode.OK
            };

            // Puedes utilizar methodName, controllerName y httpMethod para el registro
            _dbLoggerHelper.LogToFile(respuesta);
            base.OnActionExecuting(actionContext);
        }

    }
}