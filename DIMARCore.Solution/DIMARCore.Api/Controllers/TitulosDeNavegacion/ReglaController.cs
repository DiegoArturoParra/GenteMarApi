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
    /// servicios  reglas
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/reglas")]
    public class ReglaController : BaseApiController
    {
        private readonly ReglaBO _serviceReglas;

        /// <summary>
        /// constructor
        /// </summary>
        public ReglaController()
        {
            _serviceReglas = new ReglaBO();
        }

        /// <summary>
        /// Retorna la lista de reglas por cargo del titulo
        /// </summary>
        /// <param name="CargoId">Parametro para filtrar por cargo.</param>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>2022/02/26</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve el listado de las reglas por cargo.</response>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(List<ReglaDTO>))]
        [HttpGet]
        [AuthorizeRoles(RolesEnum.AdministradorGDM, RolesEnum.GestorSedeCentral)]
        [Route("lista-by-cargo-titulo/{CargoId}")]
        public async Task<IHttpActionResult> ReglasActivasByCargoTitulo(int CargoId)
        {
            var listado = await _serviceReglas.GetReglasActivasByCargoTitulo(CargoId);
            return Ok(listado);
        }

        /// <summary>
        /// Metodo para listar las reglas
        /// </summary>
        /// <param name="dto">Parametro para filtrar por activo.</param>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>2022/02/26</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve el listado de las reglas.</response>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(List<ReglaDTO>))]
        [HttpGet]
        [Route("lista")]
        [AuthorizeRoles(RolesEnum.AdministradorGDM)]
        public IHttpActionResult ListReglas([FromUri] ActivoDTO dto)
        {
            var query = _serviceReglas.GetAll(dto != null ? dto.Activo : null);
            var listado = Mapear<IEnumerable<GENTEMAR_REGLAS>, IEnumerable<ReglaDTO>>(query);
            return Ok(listado);
        }



        /// <summary>
        /// Servicio que retorna la información de la regla solicitada.
        /// </summary>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// <param name="id">Id (int) de la regla.</param>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información de la regla.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>     
        /// <response code="404">NotFound. No existe la regla solicitada.</response>
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(ResponseTypeSwagger<ReglaDTO>))]
        [HttpGet]
        [Route("{id}")]
        [AuthorizeRoles(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetRegla(int id)
        {
            var regla = await _serviceReglas.GetByIdAsync(id);
            var obj = Mapear<GENTEMAR_REGLAS, ReglaDTO>((GENTEMAR_REGLAS)regla.Data);
            regla.Data = obj;
            return Ok(regla);
        }

        /// <summary>
        ///  Creación regla
        /// </summary>
        /// <remarks>
        /// Servicio para crear una regla
        /// </remarks>
        /// <param name="regla">Objeto dto para crear la regla</param>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>28/04/2022</Fecha>
        /// <response code="201">Created. la solicitud ha tenido éxito y ha llevado a la creación de la regla.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud ya existe el nombre de la regla.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseCreatedTypeSwagger))]
        [HttpPost]
        [Route("crear")]
        [AuthorizeRoles(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> Crear([FromBody] ReglaDTO regla)
        {
            var data = Mapear<ReglaDTO, GENTEMAR_REGLAS>(regla);
            var response = await _serviceReglas.CrearAsync(data);
            return Created(string.Empty, response);
        }



        /// <summary>
        ///  Edición regla
        /// </summary>
        /// <remarks>
        /// Servicio para editar una regla
        /// </remarks>
        /// <param name="regla">Objeto dto para editar la regla</param>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>28/04/2022</Fecha>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud ya existe el nombre de la regla.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [HttpPut]
        [Route("update")]
        [AuthorizeRoles(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> Editar([FromBody] ReglaDTO regla)
        {
            var data = Mapear<ReglaDTO, GENTEMAR_REGLAS>(regla);
            var response = await _serviceReglas.ActualizarAsync(data);
            return ResultadoStatus(response);
        }

        /// <summary>
        /// Servicio para inactivar o activar una regla.
        /// </summary>
        /// <remarks>
        ///  Servicio para inactivar o activar una regla a partir del parametro id.
        /// </remarks>
        /// <param name="id">id de la regla</param>
        /// <response code="200">OK. Devuelve el mensaje de tipo respuesta.</response>        
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        [ResponseType(typeof(Respuesta))]
        [HttpPut]
        [Route("anula-or-activa/{id}")]
        [AuthorizeRoles(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> AnularOrActivar(int id)
        {
            var response = await _serviceReglas.AnulaOrActivaAsync(id);
            return ResultadoStatus(response);
        }
    }
}
