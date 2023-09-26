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
    /// servicios para los niveles de los titulos
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api")]
    public class NivelController : BaseApiController
    {
        private readonly NivelTituloBO _service;

        /// <summary>
        /// Constructor
        /// </summary>
        public NivelController()
        {
            _service = new NivelTituloBO();
        }

        /// <summary>
        /// Servicio para el nivel del titulo dependiendo la regla, cargo y capacidad.
        /// </summary>
        /// <remarks>
        /// Nivel del titulo.
        /// </remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado data.</response>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <param name="ids">objeto que contiene las foraneas para busquedes de nivel por cargo-regla-id</param>
        /// <returns></returns>
        [ResponseType(typeof(NivelDTO))]
        [HttpGet]
        [AuthorizeRoles(RolesEnum.AdministradorGDM, RolesEnum.GestorSedeCentral)]
        [Route("nivel-by-regla-cargo")]
        public async Task<IHttpActionResult> NivelPorCargoRegla([FromUri] IdsTablasForaneasDTO ids)
        {
            var query = await _service.GetNivelTituloByCargoReglaId(ids);
            return Ok(query);
        }

        /// <summary>
        /// Servicio que lista los niveles de un titulo.
        /// </summary>
        /// <remarks>
        /// Servicio que lista los niveles.
        /// </remarks>
        /// <response code="200">OK. Devuelve el listado de niveles.</response>        
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <param name="dto">objeto que filtra los niveles por activo o inactivo</param>
        /// <returns></returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        [ResponseType(typeof(List<NivelDTO>))]
        [HttpGet]
        [Route("niveles/lista")]
        [AuthorizeRoles(RolesEnum.AdministradorGDM)]
        public IHttpActionResult Listado([FromUri] ActivoDTO dto)
        {
            var query = _service.GetAll(dto != null ? dto.Activo : null);
            var listado = Mapear<IEnumerable<GENTEMAR_NIVEL>, IEnumerable<NivelDTO>>(query);
            return Ok(listado);
        }


        /// <summary>
        /// servicio que retorna el nivel por id
        /// </summary>
        /// <remarks>
        /// Muestra objeto tipo respuesta con el nivel.
        /// </remarks>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>     
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado data.</response>
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <param name="id">parametro del id nivel</param>
        /// <returns></returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        [ResponseType(typeof(ResponseTypeSwagger<NivelDTO>))]
        [HttpGet]
        [Route("niveles/{id}")]
        [AuthorizeRoles(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetNivel(int id)
        {
            var entidad = await _service.GetByIdAsync(id);
            var obj = Mapear<GENTEMAR_NIVEL, NivelDTO>((GENTEMAR_NIVEL)entidad.Data);
            entidad.Data = obj;
            return Ok(entidad);
        }


        /// <summary>
        /// Servicio para crear un nivel de un titulo
        /// </summary>
        /// <remarks>
        ///  Servicio para crear un nivel de un titulo.
        /// </remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// <param name="nivel">objeto para crear un nivel</param>
        /// <response code="201">Created. Crea y muestra el objeto respuesta con el mensaje de creación.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>    
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud ya existe el nivel.</response>
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(ResponseCreatedTypeSwagger))]
        [HttpPost]
        [Route("niveles/crear")]
        [AuthorizeRoles(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> Crear([FromBody] NivelDTO nivel)
        {
            var data = Mapear<NivelDTO, GENTEMAR_NIVEL>(nivel);
            var response = await _service.CrearAsync(data);
            return Created(string.Empty, response);
        }


        /// <summary>
        /// Servicio para editar un nivel.
        /// </summary>
        /// <remarks>
        ///  Servicio para editar un nivel de un titulo.
        /// </remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// <param name="nivel">objeto para editar un nivel</param>
        /// <response code="200">OK. Devuelve el mensaje de tipo respuesta.</response>        
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud ya existe el nivel.</response>
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [HttpPut]
        [Route("niveles/editar")]
        [AuthorizeRoles(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> Editar([FromBody] NivelDTO nivel)
        {
            var data = Mapear<NivelDTO, GENTEMAR_NIVEL>(nivel);
            var response = await _service.ActualizarAsync(data);
            return Ok(response);
        }


        /// <summary>
        /// Servicio para inactivar o activar un nivel.
        /// </summary>
        /// <remarks>
        ///  Servicio para inactivar o activar un nivel a partir del parametro id.
        /// </remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// <param name="id">id del nivel</param>
        /// <response code="200">OK. Devuelve el mensaje de tipo respuesta.</response>        
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(Respuesta))]
        [AuthorizeRoles(RolesEnum.AdministradorGDM)]
        [HttpPut]
        [Route("niveles/anula-or-activa/{id}")]
        public async Task<IHttpActionResult> AnularOrActivar(int id)
        {
            var response = await _service.AnulaOrActivaAsync(id);
            return Ok(response);
        }
    }
}
