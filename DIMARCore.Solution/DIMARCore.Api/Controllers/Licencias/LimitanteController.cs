
using DIMARCore.Api.Core.Filters;
using DIMARCore.Api.Core.Models;
using DIMARCore.Business;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Enums;
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
        [AuthorizeRolesFilter(RolesEnum.Consultas, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetLimitantesAsync()
        {
            var limitaciones = await _service.GetLimitantesAsync();
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
        [AuthorizeRolesFilter(RolesEnum.Consultas, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetLimitantesActivoAsync()
        {
            var limitaciones = await _service.GetLimitantesActivoAsync();
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
        [ResponseType(typeof(GENTEMAR_LIMITANTE))]
        [HttpGet]
        [Route("id")]
        [AuthorizeRolesFilter(RolesEnum.Consultas, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public IHttpActionResult GetLimitante(int id)
        {
            var limitante = _service.GetLimitanteAsync(id);
            return Ok(limitante);
        }


        /// <summary>
        /// Crea una Limitación
        /// </summary>
        /// <remarks>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>28/04/2022</Fecha>
        /// </remarks>
        /// <param name="datos">objeto para crear una limitación.</param>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud con el estado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseCreatedTypeSwagger))]
        [HttpPost]
        [Route("crear")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> CrearLimitante(LimitanteDTO datos)
        {

            var data = Mapear<LimitanteDTO, GENTEMAR_LIMITANTE>(datos);
            var response = await _service.CrearLimitante(data);
            return Created(string.Empty, response);
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
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [HttpPut]
        [Route("editar")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> EditarLimitante(LimitanteDTO datos)
        {
            var data = Mapear<LimitanteDTO, GENTEMAR_LIMITANTE>(datos);
            var response = await _service.EditarLimitanteAsync(data);
            return Ok(response);
        }

        /// <summary>
        /// cambia el estado de una limitante 
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
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> CambiarLimitanteAsync(int id)
        {
            var respuesta = await _service.CambiarLimitante(id);
            return ResultadoStatus(respuesta);
        }

    }
}