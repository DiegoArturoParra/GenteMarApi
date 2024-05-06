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
    /// servicios de los estados tramite para un titulo
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/estados-tramite-titulo")]
    public class EstadoTituloController : BaseApiController
    {
        private readonly EstadoTituloBO _service;

        /// <summary>
        /// Constructor
        /// </summary>
        public EstadoTituloController()
        {
            _service = new EstadoTituloBO();
        }


        /// <summary>
        /// Servicio que lista los estados de tramite para un titulo.
        /// </summary>
        /// <remarks>
        /// lista los estados de tramite para un titulo.
        /// </remarks>
        /// <response code="200">OK. Devuelve el listado de niveles.</response>        
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <param name="dto">objetoi que filtra el estado por activo o inactivo o todos</param>
        /// <returns></returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>16/12/2022</Fecha>
        [ResponseType(typeof(List<EstadoTituloDTO>))]
        [HttpGet]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM, RolesEnum.GestorSedeCentral)]
        [Route("lista")]
        public async Task<IHttpActionResult> ListadoAsync([FromUri] ActivoDTO dto)
        {
            var query = await _service.GetAllAsync(dto != null ? dto.Activo : null);
            var listado = Mapear<IEnumerable<GENTEMAR_ESTADO_TITULO>, IEnumerable<EstadoTituloDTO>>(query);
            return Ok(listado);
        }


        /// <summary>
        /// servicio que retorna el estado tramite por id
        /// </summary>
        /// <remarks>
        /// Muestra objeto tipo respuesta con el estado tramite.
        /// </remarks>
        /// <param name="id">parametro del id estado</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>    
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado data.</response>
        /// <response code="500">Internal Server. Error En el servidor.</response>
        /// <returns></returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>16/12/2022</Fecha>
        [ResponseType(typeof(ResponseTypeSwagger<EstadoTituloDTO>))]
        [HttpGet]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        [Route("{id}")]
        public async Task<IHttpActionResult> GetEstadoTramiteTitulo(int id)
        {
            var entidad = await _service.GetByIdAsync(id);
            var obj = Mapear<GENTEMAR_ESTADO_TITULO, EstadoTituloDTO>((GENTEMAR_ESTADO_TITULO)entidad.Data);
            entidad.Data = obj;
            return Ok(entidad);
        }

        /// <summary>
        /// Servicio para crear un estado para los titulos
        /// </summary>
        /// <remarks>
        ///  Servicio para crear un estado de tramite para un titulo.
        /// </remarks>
        /// <param name="estado">objeto para crear un estado tramite</param>
        /// <response code="201">Created. Crea y muestra el objeto respuesta con el mensaje de creación.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud ya existe el estado.</response>
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>16/12/2022</Fecha>
        [ResponseType(typeof(ResponseCreatedTypeSwagger))]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        [HttpPost]
        [Route("crear")]
        public async Task<IHttpActionResult> Crear([FromBody] EstadoTituloDTO estado)
        {
            var data = Mapear<EstadoTituloDTO, GENTEMAR_ESTADO_TITULO>(estado);
            var response = await _service.CrearAsync(data);
            return Created(string.Empty, response);
        }

        /// <summary>
        /// Servicio para editar un estado.
        /// </summary>
        /// <remarks>
        ///  Servicio para editar un estado de tramite de un titulo.
        /// </remarks>
        /// <param name="estado">objeto para editar un estado</param>
        /// <response code="200">OK. Devuelve el mensaje se ha editado.</response>        
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. Ya existe el nombre del estado.</response>       
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>16/12/2022</Fecha>
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        [HttpPut]
        [Route("editar")]
        public async Task<IHttpActionResult> Editar([FromBody] EstadoTituloDTO estado)
        {
            var data = Mapear<EstadoTituloDTO, GENTEMAR_ESTADO_TITULO>(estado);
            var response = await _service.ActualizarAsync(data);
            return Ok(response);
        }

        /// <summary>
        /// Servicio para inactivar o activar un estado de tramite.
        /// </summary>
        /// <remarks>
        ///  Servicio para inactivar o activar un estado de tramite a partir del parametro id.
        /// </remarks>
        /// <param name="id">id del estado</param>
        /// <response code="200">OK. Devuelve el mensaje si se activo o inactivo.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>16/12/2022</Fecha>
        [ResponseType(typeof(Respuesta))]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        [HttpPut]
        [Route("anula-or-activa/{id}")]
        public async Task<IHttpActionResult> AnularOrActivar(int id)
        {
            var response = await _service.AnulaOrActivaAsync(id);
            return Ok(response);
        }
    }
}
