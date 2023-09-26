using DIMARCore.Api.Core.Atributos;
using DIMARCore.Api.Core.Models;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
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
    /// servicios para los cargos de titulos
    /// <Autor>Diego Parra</Autor>  
    /// <Fecha>23/05/2022</Fecha>
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/cargo-titulos")]

    public class CargoTituloController : BaseApiController
    {

        private readonly CargoTituloBO _service;

        /// <summary>
        /// Constructor
        /// </summary>
        public CargoTituloController()
        {
            _service = new CargoTituloBO();
        }
        /// <summary>
        ///  Se obtiene el listado de cargos por sección.
        /// </summary>
        /// <param name="SeccionId"> parametro que contiene el id de la sección</param>
        /// <response code="200">OK. Devuelve la lista de cargos por sección solicitado.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No ha encontrado información de cargos por la sección.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>23/05/2022</Fecha>
        /// </remarks>
        [ResponseType(typeof(List<CargoTituloDTO>))]
        [HttpGet]
        [Route("lista-by-seccion/{SeccionId}")]
        [AuthorizeRoles(RolesEnum.AdministradorGDM, RolesEnum.GestorSedeCentral)]
        public async Task<IHttpActionResult> GetCargoTitulosBySeccionId(int SeccionId)
        {
            var query = await _service.GetCargoTitulosBySeccionId(SeccionId);
            var listado = Mapear<IEnumerable<GENTEMAR_CARGO_TITULO>, IEnumerable<CargoTituloDTO>>(query);
            return Ok(listado);
        }


        /// <summary>
        /// Listado de cargos de titulos de navegación
        /// </summary>
        /// <returns></returns>
        /// <param name="filter"> parametro que contiene el filtro por: SeccionId y ClaseId</param>
        /// <summary>
        ///  Se obtiene el listado de cargos por sección.
        /// </summary>
        /// <response code="200">OK. Devuelve la lista de cargos por sección solicitado.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No ha encontrado información de cargos por la sección.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>23/05/2022</Fecha>
        /// </remarks>
        [ResponseType(typeof(List<CargoTituloDTO>))]
        [HttpGet]
        [Route("lista")]
        [AuthorizeRoles(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> Listado([FromUri] CargoTituloFilter filter)
        {
            var data = await _service.GetAllByFilter(filter);
            return Ok(data);
        }

        /// <summary>
        /// servicio get cargo del titulo por id
        /// </summary>
        /// <param name="id">parametro que filtra por id</param>
        /// <returns></returns>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/05/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(ResponseTypeSwagger<CargoTituloDTO>))]
        [HttpGet]
        [Route("{id}")]
        [AuthorizeRoles(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetCargoTitulo(int id)
        {
            var entidad = await _service.GetByIdAsync(id);
            var obj = Mapear<GENTEMAR_CARGO_TITULO, CargoTituloDTO>((GENTEMAR_CARGO_TITULO)entidad.Data);
            entidad.Data = obj;
            return Ok(entidad);
        }

        /// <summary>
        /// Servicio para crear un cargo
        /// </summary>        
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>28/04/2022</Fecha>
        /// </remarks>
        /// <param name="cargo">objeto para crear un cargo.</param>
        /// <response code="201">Created. la solicitud ha tenido éxito y ha llevado a la creación del cargo.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud ya existe el cargo.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseCreatedTypeSwagger))]
        [HttpPost]
        [Route("crear")]
        [AuthorizeRoles(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> Crear([FromBody] CargoTituloDTO cargo)
        {
            var data = Mapear<CargoTituloDTO, GENTEMAR_CARGO_TITULO>(cargo);
            var response = await _service.CrearAsync(data);
            return Created(string.Empty, response);
        }


        /// <summary>
        /// Servicio para editar un cargo
        /// </summary>
        /// <param name="cargo">objeto para editar un cargo.</param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="200">OK. se ha actualizado el recurso (cargo).</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud ya existe el cargo.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [HttpPut]
        [Route("editar")]
        [AuthorizeRoles(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> Editar([FromBody] CargoTituloDTO cargo)
        {
            var data = Mapear<CargoTituloDTO, GENTEMAR_CARGO_TITULO>(cargo);
            var response = await _service.ActualizarAsync(data);
            return Ok(response);
        }

        /// <summary>
        /// Servicio para Inactivar un cargo de un titulo de navegación
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/05/2022</Fecha>
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
            var response = await _service.AnulaOrActivaAsync(id);
            return Ok(response);
        }
    }
}
