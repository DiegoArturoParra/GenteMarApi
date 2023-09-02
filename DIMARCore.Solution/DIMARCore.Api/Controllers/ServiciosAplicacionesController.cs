using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
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
        [Authorize]
        [Route("tipos-solicitud")]
        public IHttpActionResult GetTiposSolicitud()
        {
            var response = _serviciosBO.GetTiposSolicitud();
            var listado = Mapear<IEnumerable<APLICACIONES_TIPO_SOLICITUD>, IEnumerable<TipoSolicitudDTO>>(response);
            return Ok(listado);
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
        [Authorize]
        [Route("tipos-refrendo")]
        public IHttpActionResult ListarRefrendos()
        {
            var response = _serviciosBO.GetTipoRefrendos();
            var listado = Mapear<IEnumerable<APLICACIONES_TIPO_REFRENDO>, IEnumerable<TipoRefrendoDTO>>(response);
            return Ok(listado);
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
        [Authorize]
        [Route("capitanias")]
        public IHttpActionResult GetCapitaniasFirma()
        {
            var response = _serviciosBO.GetCapitanias();
            var listado = Mapear<IEnumerable<APLICACIONES_CAPITANIAS>, IEnumerable<CapitaniaDTO>>(response);
            return Ok(listado);
        }
        /// <summary>
        /// servicio para las capitanias firmante
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
        [Authorize]
        [Route("capitanias-firmante")]
        public IHttpActionResult GetCapitaniasFirmante()
        {
            var response = _serviciosBO.GetCapitaniasFirmante();
            var listado = Mapear<IEnumerable<APLICACIONES_CAPITANIAS>, IEnumerable<CapitaniaDTO>>(response);
            return Ok(listado);
        }
    }
}
