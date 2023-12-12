using DIMARCore.Api.Core.Atributos;
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
    /// servicios para las capacidad del titulo de navegación
    /// <Autor>Diego Parra</Autor>
    /// <Fecha>23/03/2022</Fecha>
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/capacidades")]

    public class CapacidadController : BaseApiController
    {
        private readonly CapacidadBO _serviceCapacidad;

        /// <summary>
        /// Constructor
        /// </summary>
        public CapacidadController()
        {
            _serviceCapacidad = new CapacidadBO();
        }
        /// <summary>
        ///  Se obtiene el listado de capacidades por Id regla.
        /// </summary>
        /// <param name="items"> parametros que contiene los ids de regla y cargo</param>
        /// <response code="200">OK. Devuelve la lista de capacidades por regla cargo solicitada.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>23/03/2022</Fecha>
        /// </remarks>
        [ResponseType(typeof(List<CapacidadDTO>))]
        [HttpGet]
        [AuthorizeRoles(RolesEnum.AdministradorGDM, RolesEnum.GestorSedeCentral)]
        [Route("lista-by-regla-cargo")]
        public async Task<IHttpActionResult> CapacidadesActivasByReglaCargo([FromUri] IdsLlaveCompuestaDTO items)
        {
            var query = await _serviceCapacidad.CapacidadesActivasByReglaCargo(items);
            var listado = Mapear<IEnumerable<GENTEMAR_REGLAS_CARGO>, IEnumerable<CapacidadDTO>>(query);
            return Ok(listado);
        }


        /// <summary>
        ///  Se obtiene el listado de capacidades.
        /// </summary>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <param name="dto"> parametro que contiene el tipo de estado, activo/anulado</param>
        /// <returns></returns>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>23/03/2022</Fecha>
        /// </remarks>
        [ResponseType(typeof(List<CapacidadDTO>))]
        [HttpGet]
        [Route("lista")]
        [AuthorizeRoles(RolesEnum.AdministradorGDM)]
        public IHttpActionResult Listado([FromUri] ActivoDTO dto)
        {
            var query = _serviceCapacidad.GetAll(dto != null ? dto.Activo : null);
            var listado = Mapear<IEnumerable<GENTEMAR_CAPACIDAD>, IEnumerable<CapacidadDTO>>(query);
            return Ok(listado);
        }

        /// <summary>
        /// servicio get capacidad
        /// </summary>
        /// <param name="id">parametro que filtra por id</param>
        /// <returns></returns>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(ResponseTypeSwagger<CapacidadDTO>))]
        [HttpGet]
        [Route("{id}")]
        [AuthorizeRoles(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetCapacidad(int id)
        {
            var entidad = await _serviceCapacidad.GetByIdAsync(id);
            var obj = Mapear<GENTEMAR_CAPACIDAD, CapacidadDTO>((GENTEMAR_CAPACIDAD)entidad.Data);
            entidad.Data = obj;
            return Ok(entidad);
        }

        /// <summary>
        /// Servicio para crear una capacidad
        /// </summary>        
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>28/04/2022</Fecha>
        /// </remarks>
        /// <param name="capacidad">objeto para crear una capacidad.</param>
        /// <response code="201">Created. la solicitud ha tenido éxito y ha llevado a la creación de la capacidad.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud ya existe la capacidad.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseCreatedTypeSwagger))]
        [HttpPost]
        [Route("crear")]
        [AuthorizeRoles(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> Crear([FromBody] CapacidadDTO capacidad)
        {
            var data = Mapear<CapacidadDTO, GENTEMAR_CAPACIDAD>(capacidad);
            var response = await _serviceCapacidad.CrearAsync(data);
            return Created(string.Empty, response);
        }

        /// <summary>
        /// Servicio para editar una capacidad
        /// </summary>
        /// <param name="capacidad">objeto para editar una capacidad.</param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="200">OK. se ha actualizado el recurso (capacidad).</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud ya existe la capacidad.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [HttpPut]
        [Route("editar")]
        [AuthorizeRoles(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> Editar([FromBody] CapacidadDTO capacidad)
        {
            var data = Mapear<CapacidadDTO, GENTEMAR_CAPACIDAD>(capacidad);
            var response = await _serviceCapacidad.ActualizarAsync(data);
            return Ok(response);
        }

        /// <summary>
        /// Servicio para Inactivar una capacidad
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="200">OK. Devuelve el mensaje si se activo o inactivo.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(Respuesta))]
        [HttpPut]
        [Route("anula-or-activa/{id}")]
        [AuthorizeRoles(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> AnularOrActivar(int id)
        {
            var response = await _serviceCapacidad.AnulaOrActivaAsync(id);
            return Ok(response);
        }
    }
}