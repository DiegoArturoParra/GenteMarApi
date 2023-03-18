using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

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
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("tipos-solicitud")]
        public IHttpActionResult GetTiposSolicitud()
        {
            var response = _serviciosBO.GetTiposSolicitud();
            var listado = Mapear<IEnumerable<APLICACIONES_TIPO_SOLICITUD>, IEnumerable<TipoSolicitudDTO>>(response);
            return Ok(listado);
        }

        /// <summary>
        /// servicio para las capitanias 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
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
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("capitanias-firmante")]
        public IHttpActionResult GetCapitaniasFirmante()
        {
            var response = _serviciosBO.GetCapitaniasFirmante();
            var listado = Mapear<IEnumerable<APLICACIONES_CAPITANIAS>, IEnumerable<CapitaniaDTO>>(response);
            return Ok(listado);
        }
    }
}
