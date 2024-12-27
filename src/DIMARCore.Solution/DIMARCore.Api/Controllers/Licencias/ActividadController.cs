using DIMARCore.Api.Core.Filters;
using DIMARCore.Api.Core.Models;
using DIMARCore.Business;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Enums;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers
{
    /// <summary>
    /// API Actividad
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/actividades")]
    public class ActividadController : BaseApiController
    {
        private readonly ActividadBO _service;

        /// <summary>
        /// Constructor
        /// </summary>
        public ActividadController()
        {
            _service = new ActividadBO();
        }

        /// <summary>
        /// Listado de Actividades dependiendo el tipo de licencia 
        /// </summary>    
        /// <param name="id"></param>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>2022/02/26</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información del estado.</response>
        /// <response code="204">No Content. No hay estado.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(List<ActividadTipoLicenciaDTO>))]
        [HttpGet]
        [Route("lista-activas-tipo-licencia/{id}")]
        [AuthorizeRolesFilter(RolesEnum.Consultas, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetActividadesIdTipoLicenciaAsync(int id)
        {
            var actividades = await _service.GetActividadesActivoTipoLicencia(id);
            var data = Mapear<IEnumerable<GENTEMAR_ACTIVIDAD>, IEnumerable<ActividadTipoLicenciaDTO>>(actividades);
            return Ok(data);
        }

        /// <summary>
        /// Listado de Actividades dependiendo uno o varios tipos de licencia 
        /// </summary>    
        /// <param name="idsTipoLicencia">ids de tipo licencia</param>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>2022/02/26</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información del estado.</response>
        /// <response code="204">No Content. No hay estado.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(List<ActividadLicenciaDTO>))]
        [HttpPost]
        [Route("lista-activas-tipos-licencia")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM, RolesEnum.Capitania)]
        public async Task<IHttpActionResult> GetActividadesPorTiposLicencia(List<int> idsTipoLicencia)
        {
            var data = await _service.GetActividadesActivasPorTiposDeLicencia(idsTipoLicencia);
            return Ok(data);
        }

        /// <summary>
        /// Listado de Actividades con información basica
        /// </summary>    
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>2022/02/26</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información del estado.</response>
        /// <response code="204">No Content. No hay estado.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(List<ActividadTipoLicenciaDTO>))]
        [HttpGet]
        [Route("lista")]
        [AuthorizeRolesFilter(RolesEnum.Consultas, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetActividadesAsync()
        {
            var actividades = await _service.GetActividadesAsync();
            return Ok(actividades);
        }

        /// <summary>
        /// Listado de Actividades con información basica activos
        /// </summary>    
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>2022/02/26</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información del estado.</response>
        /// <response code="204">No Content. No hay estado.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(List<ActividadTipoLicenciaDTO>))]
        [HttpGet]
        [Route("lista-activo")]
        [AuthorizeRolesFilter(RolesEnum.Consultas, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetActividadesActivoAsync()
        {
            var actividades = await _service.GetActividadesActivo();
            var data = Mapear<IEnumerable<GENTEMAR_ACTIVIDAD>, IEnumerable<ActividadTipoLicenciaDTO>>(actividades);
            return Ok(data);
        }

        /// <summary>
        /// Retorna una Actividad dado un Id
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
        [AuthorizeRolesFilter(RolesEnum.Consultas, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetActividadAsync(int id)
        {
            var actividad = await new ActividadBO().GetActividad(id);
            return Ok(actividad);
        }

        /// <summary>
        /// Crea una Actividad
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
        [ResponseType(typeof(ResponseCreatedTypeSwagger))]
        [HttpPost]
        [Route("crear")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> CrearActividadAsync(ActividadTipoLicenciaDTO datos)
        {
            var data = Mapear<ActividadTipoLicenciaDTO, GENTEMAR_ACTIVIDAD>(datos);
            var actividad = await _service.CrearActividadAsync(data);
            return Created(string.Empty, actividad);
        }


        /// <summary>
        /// Edita / Modifica una Actividad
        /// </summary>
        /// <param name="datos">objeto para editar una actividad.</param>
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
        public async Task<IHttpActionResult> EditarActividadAsync(ActividadTipoLicenciaDTO datos)
        {
            var data = Mapear<ActividadTipoLicenciaDTO, GENTEMAR_ACTIVIDAD>(datos);
            var actividad = await _service.EditarActividadAsync(data);
            return Ok(actividad);

        }

        /// <summary>
        /// cambia el estado de una actividad 
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
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [HttpPut]
        [Route("inhabilitar/{id}")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> CambiarActividadAsync(int id)
        {
            var respuesta = await _service.CambiarActividad(id);
            return Ok(respuesta);
        }
    }
}
