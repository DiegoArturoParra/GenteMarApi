using DIMARCore.Utilities.Config;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace DIMARCore.Api.Core.Atributos
{
    /// <summary>
    ///  Especifica el filtro de autorización con base a la aplicación
    /// </summary>
    public class AplicacionAutorizada : AuthorizeAttribute
    {
        /// <summary>
        /// Aplicaciones autorizadas
        /// </summary>
        private readonly int[] aplicacionesAutorizadas;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="aplicaciones">Lista de aplicaciones autorizadas</param>
        public AplicacionAutorizada(params int[] aplicaciones)
        {
            this.aplicacionesAutorizadas = aplicaciones;
        }

        /// <summary>
        /// Valida si la aplicación esta autorizada
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);

            string sIdAplicacion = "";

            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;

            // valida si el usuario esta autenticado
            if (!principal.Identity.IsAuthenticated)
            {
                HandleUnauthorizedRequest(actionContext);
                return;
            }

            if (principal.Identity is ClaimsIdentity identity)
            {
                // se obtiene el perfil
                List<Claim> claims = identity.Claims.ToList();
                sIdAplicacion = claims.Where(p => p.Type == ClaimsConfig.ID_APLICACION).FirstOrDefault()?.Value;
                sIdAplicacion = string.IsNullOrEmpty(sIdAplicacion) ? "" : sIdAplicacion;
            }

            if (string.IsNullOrEmpty(sIdAplicacion))
            {   // no tiene una aplicación, no se puede validar - no esta autorizado
                HandleUnauthorizedRequest(actionContext);
                return;
            }
            else if (aplicacionesAutorizadas.Count() == 0)
            {
                // tiene un aplicación
                // todas las aplicaciones son validas (Si no se ha definido ninguna)
                // esta autorizado
                IsAuthorized(actionContext);
                return;
            }

            // se valida si la aplicación esta autorizada
            int idAplicacion = 0;
            if (int.TryParse(sIdAplicacion, out idAplicacion) && aplicacionesAutorizadas.Contains(idAplicacion))
            {
                // la aplicación existe en la lista de aplicaciones autorizadas
                IsAuthorized(actionContext);
                return;
            }
            else
            {
                // la aplicación no esta autorizado
                HandleUnauthorizedRequest(actionContext);
                return;
            }

        }

        //protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        //{
        // Prueba en caso que se desee reemplazar el mensaje
        //    actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
        //    {
        //        ReasonPhrase = "Perfil no autorizado"
        //    };
        //}
    }
}