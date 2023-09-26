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
    /// servicios para las funciones titulos de navegación
    /// <Autor>Diego Parra</Autor>
    /// <Fecha>05/03/2022</Fecha>
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/funciones")]
    public class FuncionController : BaseApiController
    {

        private readonly FuncionBO _serviceFuncion;

        /// <summary>
        /// Constructor
        /// </summary>
        public FuncionController()
        {
            _serviceFuncion = new FuncionBO();
        }
        /// <summary>
        /// Servicio que lista las funciones por regla.
        /// </summary>
        /// <remarks>
        /// Servicio que lista las funciones por regla.
        /// </remarks>
        /// <response code="200">OK. Devuelve el listado de funciones por una regla.</response>        
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <param name="ReglaId">objeto que filtra las funciones por regla</param>
        /// <returns></returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/05/2022</Fecha>
        [ResponseType(typeof(List<NivelDTO>))]
        [HttpGet]
        [AuthorizeRoles(RolesEnum.AdministradorGDM, RolesEnum.GestorSedeCentral)]
        [Route("lista-by-regla/{ReglaId}")]
        public async Task<IHttpActionResult> FuncionesByRegla(int ReglaId)
        {
            var query = await _serviceFuncion.GetFuncionesByRegla(ReglaId);
            var listado = Mapear<IEnumerable<GENTEMAR_REGLA_FUNCION>, IEnumerable<FuncionDTO>>(query);
            return Ok(listado);
        }


        /// <summary>
        /// Servicio que lista las funciones.
        /// </summary>
        /// <remarks>
        /// Servicio que lista las funciones.
        /// </remarks>
        /// <response code="200">OK. Devuelve el listado de funciones.</response>        
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <param name="dto">objeto que filtra las funciones por activo o inactivo</param>
        /// <returns></returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        [ResponseType(typeof(List<NivelDTO>))]
        [HttpGet]
        [Route("lista")]
        [AuthorizeRoles(RolesEnum.AdministradorGDM)]
        public IHttpActionResult Listado([FromUri] ActivoDTO dto)
        {
            var query = _serviceFuncion.GetAll(dto != null ? dto.Activo : null);
            var listado = Mapear<IEnumerable<GENTEMAR_FUNCIONES>, IEnumerable<FuncionDTO>>(query);
            return Ok(listado);
        }


        /// <summary>
        /// servicio que retorna la función por id
        /// </summary>
        /// <remarks>
        /// Muestra objeto tipo respuesta con la función.
        /// </remarks>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>     
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado data.</response>
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <param name="id">parametro del id funci´´on</param>
        /// <returns></returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/04/2022</Fecha>
        [ResponseType(typeof(ResponseTypeSwagger<FuncionDTO>))]
        [HttpGet]
        [Route("{id}")]
        [AuthorizeRoles(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetFuncion(int id)
        {
            var entidad = await _serviceFuncion.GetByIdAsync(id);
            var obj = Mapear<GENTEMAR_FUNCIONES, FuncionDTO>((GENTEMAR_FUNCIONES)entidad.Data);
            entidad.Data = obj;
            return Ok(entidad);
        }

        /// <summary>
        /// Servicio para crear una función.
        /// </summary>
        /// <remarks>
        ///  Servicio para crear una función.
        /// </remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/04/2022</Fecha>
        /// <param name="funcion">objeto para crear función</param>
        /// <response code="201">Created. Crea y muestra el objeto respuesta con el mensaje de creación.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. Ya existe el nombre de la función.</response>      
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(ResponseCreatedTypeSwagger))]
        [HttpPost]
        [Route("crear")]
        [AuthorizeRoles(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> Crear([FromBody] FuncionDTO funcion)
        {
            var data = Mapear<FuncionDTO, GENTEMAR_FUNCIONES>(funcion);
            var response = await _serviceFuncion.CrearAsync(data);
            return Created(string.Empty, response);
        }


        /// <summary>
        /// Servicio para editar un nivel.
        /// </summary>
        /// <remarks>
        ///  Servicio para editar un nivel de un titulo.
        /// </remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/04/2022</Fecha>
        /// <param name="funcion">objeto para editar una función</param>
        /// <response code="200">OK. Devuelve el mensaje de tipo respuesta.</response>        
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. Ya existe el nombre de la función.</response>       
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [HttpPut]
        [Route("editar")]
        [AuthorizeRoles(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> Editar([FromBody] FuncionDTO funcion)
        {
            var data = Mapear<FuncionDTO, GENTEMAR_FUNCIONES>(funcion);
            var response = await _serviceFuncion.ActualizarAsync(data);
            return Ok(response);
        }

        /// <summary>
        /// Servicio para inactivar o activar una función.
        /// </summary>
        /// <remarks>
        ///  Servicio para inactivar o activar una función a partir del parametro id.
        /// </remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/04/2022</Fecha>
        /// <param name="id">id de la función</param>
        /// <response code="200">OK. Devuelve el mensaje de tipo respuesta.</response>        
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(Respuesta))]
        [HttpPut]
        [Route("anula-or-activa/{id}")]
        [AuthorizeRoles(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> AnularOrActivar(int id)
        {
            var response = await _serviceFuncion.AnulaOrActivaAsync(id);
            return Ok(response);
        }
    }
}
