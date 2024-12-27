using DIMARCore.Api.Core.Models;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers
{
    /// <summary>
    /// servicios para consultas a las tablas de aplicaciones en común
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/servicios-aplicaciones")]
    [Authorize]
    public class ServiciosAplicacionesController : BaseApiController
    {

        private readonly ServiciosAplicacionesBO _serviciosBO;
        /// <summary>
        /// Constructor
        /// </summary>
        public ServiciosAplicacionesController()
        {
            _serviciosBO = new ServiciosAplicacionesBO();
        }

        /// <summary>
        /// servicio para traer la versión de la aplicación gdm
        /// </summary>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>07/11/2024</Fecha>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>              
        /// <response code="404">NotFound. No se ha encontrado data.</response>
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <returns></returns>
        [AllowAnonymous]
        [ResponseType(typeof(ResponseTypeSwagger<VersionDTO>))]
        [HttpGet]
        [Route("get-version-app")]
        public async Task<IHttpActionResult> GetVersionApp()
        {
            var response = await _serviciosBO.GetVersionApp();
            return Ok(response);
        }


        /// <summary>
        /// servicio para los tipos de solicitud
        /// </summary>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado data.</response>
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<TipoSolicitudDTO>))]
        [HttpGet]
        [Route("tipos-solicitud")]
        public async Task<IHttpActionResult> GetTiposSolicitudAsync()
        {
            var response = await _serviciosBO.GetTiposSolicitud();
            return Ok(response);
        }

        /// <summary>
        /// Servicio para listar los tipos de refrendo.
        /// </summary>
        /// <remarks>
        /// Tipo de refrendo.
        /// </remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/05/2023</Fecha>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado data.</response>
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<TipoRefrendoDTO>))]
        [HttpGet]
        [Route("tipos-refrendo")]
        public async Task<IHttpActionResult> ListarRefrendosAsync()
        {
            var response = await _serviciosBO.GetTiposRefrendoAsync();
            return Ok(response);
        }

        /// <summary>
        /// servicio para las capitanias 
        /// </summary>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado data.</response>
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <returns></returns>

        [ResponseType(typeof(IEnumerable<CapitaniaDTO>))]
        [HttpGet]
        [Route("capitanias")]
        public async Task<IHttpActionResult> GetCapitaniasAsync()
        {
            var response = await _serviciosBO.GetCapitaniasAsync();
            return Ok(response);
        }
    }
}
