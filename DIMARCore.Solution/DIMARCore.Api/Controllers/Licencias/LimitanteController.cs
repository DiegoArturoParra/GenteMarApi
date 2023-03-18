
using DIMARCore.Business;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;



namespace DIMARCore.Api.Controllers.Licencias
{
    /// <summary>
    /// api de las limitantes 
    /// </summary>
    [Authorize]
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/limitante")]
    public class LimitanteController : BaseApiController
    {

        private readonly LimitanteBO _service;
        /// <summary>
        /// constructor 
        /// </summary>
        public LimitanteController()
        {
            _service = new LimitanteBO();
        }

        /// <summary>
        /// Listado de Limitantes con información basica
        /// </summary>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>2022/02/26</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información del estado.</response>
        /// <response code="204">No Content. No hay estado.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(List<LimitanteDTO>))]
        [HttpGet]
        [Route("lista")]
        public IHttpActionResult GetLimitantes()
        {
            var limitaciones = _service.GetLimitantes();
            var data = Mapear<IList<GENTEMAR_LIMITANTE>, IList<LimitanteDTO>>(limitaciones);
            return Ok(data);
        }

        /// <summary>
        /// Listado de Limitantes activos con información basica
        /// </summary>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>2022/02/26</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información del estado.</response>
        /// <response code="204">No Content. No hay estado.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(List<LimitanteDTO>))]
        [HttpGet]
        [Route("lista-activo")]
        public IHttpActionResult GetLimitantesActivo()
        {
            var limitaciones = _service.GetLimitantesActivo();
            var data = Mapear<IList<GENTEMAR_LIMITANTE>, IList<LimitanteDTO>>(limitaciones);
            return Ok(data);
        }


        /// <summary>
        /// Retorna una Limitación dado un Id
        /// </summary>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>2022/02/26</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información de la actividad .</response>
        /// <response code="204">No Content. No hay estado.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(GENTEMAR_ACTIVIDAD))]
        [HttpGet]
        [Route("id")]
        public IHttpActionResult GetLimitante(int id)
        {
            var limitacion = _service.GetLimitante(id);
            return Ok(limitacion);
        }


        /// <summary>
        /// Crea una Limitación
        /// </summary>
        /// <remarks>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>28/04/2022</Fecha>
        /// </remarks>
        /// <param name="datos">objeto para crear un estado.</param>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud con el estado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(Respuesta))]
        [HttpPost]
        [Route("crear")]
        public async Task<IHttpActionResult> CrearLimitante(LimitanteDTO datos)
        {

            var data = Mapear<LimitanteDTO, GENTEMAR_LIMITANTE>(datos);
            var limitacion = await _service.CrearLimitante(data);
            return ResultadoStatus(limitacion);
        }


        /// <summary>
        /// Edita / Modifica una Limitación
        /// </summary>
        /// <param name="datos">objeto para editar una limitacion.</param>
        /// <remarks>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud con el estado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(Respuesta))]
        [HttpPut]
        [Route("editar")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> EditarLimitante(LimitanteDTO datos)
        {
            var data = Mapear<LimitanteDTO, GENTEMAR_LIMITANTE>(datos);
            var limitacion = await _service.EditarLimitanteAsync(data);
            return ResultadoStatus(limitacion);
        }

        /// <summary>
        /// cambia el estado de un rango 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(Respuesta))]
        [HttpPut]
        [Route("inhabilitar/{id}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> CambiarRangoAsync(int id)
        {
            var respuesta = await _service.cambiarLimitante(id);
            return ResultadoStatus(respuesta);
        }

    }
}