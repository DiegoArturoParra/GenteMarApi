using DIMARCore.Api.Core.Filters;
using DIMARCore.Api.Core.Models;
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

namespace DIMARCore.Api.Controllers.TitulosDeNavegacion
{
    /// <summary>
    /// servicios para las habilitaciones del titulo de navegación
    /// <Autor>Diego Parra</Autor>
    /// versión 1.0
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/habilitaciones")]
    public class HabilitacionController : BaseApiController
    {

        private readonly HabilitacionBO _serviceHabilitacion;
        /// <summary>
        /// Constructor
        /// </summary>
        public HabilitacionController()
        {
            _serviceHabilitacion = new HabilitacionBO();
        }


        /// <summary>
        /// Servicio para el listado de habilitaciones dependiendo la regla y el cargo.
        /// </summary>
        /// <remarks>
        /// listado de habilitaciones con filtro.
        /// </remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>01/06/2022</Fecha>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado data.</response>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <param name="items">(IdsReglaCargoDTO) objeto que tiene los ids de regla y cargo (llave compuesta)</param>
        /// <returns></returns>

        [ResponseType(typeof(List<HabilitacionDTO>))]
        [HttpGet]
        [Route("lista-by-regla-cargo")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM, RolesEnum.GestorSedeCentral)]
        public async Task<IHttpActionResult> GetHabilitacionesActivasByReglaId([FromUri] IdsTablasForaneasDTO items)
        {
            var existeRelacion = await new ReglaCargoBO().GetIdByTablasForaneas(items);
            int CargoReglaId = (int)existeRelacion.Data;
            var query = await _serviceHabilitacion.GetHabilitacionesActivasByReglaCargoId(CargoReglaId);
            var listado = Mapear<IEnumerable<GENTEMAR_REGLA_CARGO_HABILITACION>, IEnumerable<HabilitacionDTO>>(query);
            return Ok(listado);
        }


        /// <summary>
        /// Servicio para el listado de habilitaciones.
        /// </summary>
        /// <remarks>
        /// listado de habilitaciones con filtro de activo.
        /// </remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>01/06/2022</Fecha>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado data.</response>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <param name="dto">(dto) objeto que tiene el filtro de activo</param>
        /// <returns></returns>

        [ResponseType(typeof(List<HabilitacionDTO>))]
        [HttpGet]
        [Route("lista")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM, RolesEnum.GestorSedeCentral)]
        public async Task<IHttpActionResult> ListadoAsync([FromUri] ActivoDTO dto)
        {
            var query = await _serviceHabilitacion.GetAllAsync(dto != null ? dto.Activo : null);
            var listado = Mapear<IEnumerable<GENTEMAR_HABILITACION>, IEnumerable<HabilitacionDTO>>(query);
            return Ok(listado);
        }


        /// <summary>
        /// servicio que retorna la habilitación por id
        /// </summary>
        /// <remarks>
        /// Muestra objeto tipo respuesta con la habilitación.
        /// </remarks>
        /// <param name="id">parametro del id habilitación</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>    
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado data.</response>
        /// <response code="500">Internal Server. Error En el servidor.</response>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>18/06/2022</Fecha>
        /// <returns></returns>
        [ResponseType(typeof(ResponseTypeSwagger<HabilitacionDTO>))]
        [HttpGet]
        [Route("{id}")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetHabilitacion(int id)
        {
            var entidad = await _serviceHabilitacion.GetByIdAsync(id);
            var obj = Mapear<GENTEMAR_HABILITACION, HabilitacionDTO>((GENTEMAR_HABILITACION)entidad.Data);
            entidad.Data = obj;
            return Ok(entidad);
        }

        /// <summary>
        /// Servicio para crear una habilitación
        /// </summary>
        /// <remarks>
        ///  Servicio para crear una habilitación
        /// </remarks>
        /// <param name="habilitacion">objeto para crear una habilitación</param>
        /// <response code="201">Created. Crea y muestra el objeto respuesta con el mensaje de creación.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud ya existe la habilitación.</response>
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>20/06/2022</Fecha>
        /// <returns></returns>
        [ResponseType(typeof(ResponseCreatedTypeSwagger))]
        [HttpPost]
        [Route("crear")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> Crear([FromBody] HabilitacionDTO habilitacion)
        {
            var data = Mapear<HabilitacionDTO, GENTEMAR_HABILITACION>(habilitacion);
            var response = await _serviceHabilitacion.CrearAsync(data);
            return Created(string.Empty, response);
        }


        /// <summary>
        /// Servicio para editar una habilitación.
        /// </summary>
        /// <remarks>
        ///  Servicio para editar una habilitación.
        /// </remarks>
        /// <param name="habilitacion">objeto para editar una habilitación </param>
        /// <response code="200">OK. Devuelve el mensaje se ha editado.</response>        
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. Ya existe el nombre de la habilitación.</response>       
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>20/06/2022</Fecha>
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [HttpPut]
        [Route("editar")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> Editar([FromBody] HabilitacionDTO habilitacion)
        {
            var data = Mapear<HabilitacionDTO, GENTEMAR_HABILITACION>(habilitacion);
            var response = await _serviceHabilitacion.ActualizarAsync(data);
            return Ok(response);
        }

        /// <summary>
        /// Servicio para inactivar o activar una habilitación.
        /// </summary>
        /// <remarks>
        ///  Servicio para inactivar o activar una habilitación a partir del parametro id.
        /// </remarks>
        /// <param name="id">id de la habilitación</param>
        /// <response code="200">OK. Devuelve el mensaje si se activo o inactivo.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>20/06/2022</Fecha>
        [ResponseType(typeof(Respuesta))]
        [HttpPut]
        [Route("anula-or-activa/{id}")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> AnularOrActivar(int id)
        {
            var response = await _serviceHabilitacion.AnulaOrActivaAsync(id);
            return Ok(response);
        }
    }
}
