using DIMARCore.Business;
using DIMARCore.Utilities.Config;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using log4net;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace DIMARCore.Api.Core.Filters
{
    /// <summary>
    /// Clase para autorizar varios roles en los controladores
    /// </summary>
    public class AuthorizeRolesFilterAttribute : ActionFilterAttribute
    {
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        List<int> _allowedRoles = new List<int>();
        /// <summary>
        /// se concatena los ids de los roles 
        /// </summary>
        /// <param name="rolesEnum"></param>
        public AuthorizeRolesFilterAttribute(params RolesEnum[] rolesEnum)
        {
            ValidateRoles(rolesEnum);
        }
        /// <summary>
        /// Metodo que se ejecuta antes de la accion del controlador para validar los roles
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            // Check if user is authenticated
            if (!actionContext.RequestContext.Principal.Identity.IsAuthenticated)
            {
                var unauthorizedResponse = GenerateResponse("Se ha denegado la autorización para esta solicitud.", HttpStatusCode.Unauthorized);
                actionContext.Response = unauthorizedResponse;
                return;
            }

            // Check if user has any of the allowed roles
            var userRoles = await GetCurrentRolesByLoginId();
            if (!_allowedRoles.Any(role => userRoles.Contains(role)))
            {
                var forbiddenResponse = GenerateResponse("Acceso denegado. Prohibida la solicitud por reglas administrativas comuniquese con el administrador.", HttpStatusCode.Forbidden);
                actionContext.Response = forbiddenResponse;
                return;
            }

            await base.OnActionExecutingAsync(actionContext, cancellationToken);
        }

        private async Task<List<int>> GetCurrentRolesByLoginId()
        {
            var loginId = ClaimsHelper.GetLoginId();
            var rolesIds = await new UsuarioBO().GetRolesByLoginId(loginId);
            return rolesIds;
        }

        private HttpResponseMessage GenerateResponse(string message, HttpStatusCode statusCode)
        {
            var response = new HttpResponseMessage(statusCode);
            var respuesta = new Respuesta
            {
                Mensaje = message,
                StatusCode = statusCode,
                Estado = false
            };

            string json = JsonConvert.SerializeObject(respuesta);
            _logger.Info(json);
            response.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            return response;
        }

        private void ValidateRoles(RolesEnum[] rolesEnum)
        {
            foreach (var item in rolesEnum)
            {
                int rol = (int)item;
                _allowedRoles.Add(rol);
            }
        }
    }
}