using DIMARCore.Api.Core.Atributos;
using DIMARCore.Api.Core.Models;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Enums;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers.Estupefacientes
{
    /// <summary>
    /// servicios para los tipos de tramite de estupefacientes
    /// <Autor>Diego Parra</Autor>
    /// <Fecha>08/07/2022</Fecha>
    /// </summary>
    [EnableCors("*", "*", "*")]

    [RoutePrefix("api/tramite-estupefaciente")]
    public class TipoTramiteController : BaseApiController
    {
        private readonly TramiteEstupefacienteBO _serviceTramite;

        /// <summary>
        /// Constructor
        /// </summary>
        public TipoTramiteController()
        {
            _serviceTramite = new TramiteEstupefacienteBO();
        }



        /// <summary>
        /// servicio get tramites por filtro de estado
        /// </summary>
        /// <param name="dto"> objeto para filtro por estado</param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>08/07/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(List<TramiteEstupefacienteDTO>))]
        [HttpGet]
        [Route("lista")]
        [AuthorizeRoles(RolesEnum.AdministradorEstupefacientes, RolesEnum.GestorEstupefacientes, RolesEnum.JuridicaEstupefacientes,
            RolesEnum.ConsultasEstupefacientes)]
        public IHttpActionResult Listado([FromUri] ActivoDTO dto)
        {
            var query = _serviceTramite.GetAll(dto != null ? dto.Activo : null);
            var listado = Mapear<IEnumerable<GENTEMAR_TIPO_TRAMITE>, IEnumerable<TramiteEstupefacienteDTO>>(query);
            return Ok(listado);
        }


        /// <summary>
        /// servicio get tramite por id
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>08/07/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(ResponseTypeSwagger<TramiteEstupefacienteDTO>))]
        [HttpGet]
        [Route("{id}")]
        [AuthorizeRoles(RolesEnum.AdministradorEstupefacientes)]
        public async Task<IHttpActionResult> GetTramite(int id)
        {
            var tramite = await _serviceTramite.GetByIdAsync(id);

            var obj = Mapear<GENTEMAR_TIPO_TRAMITE, TramiteEstupefacienteDTO>((GENTEMAR_TIPO_TRAMITE)tramite.Data);
            tramite.Data = obj;

            return ResultadoStatus(tramite);
        }



        /// <summary>
        /// Servicio para crear una tramite
        /// </summary>
        /// <param name="tramite"></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>08/07/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="201">Created. la solicitud ha tenido éxito y ha llevado a la creación de la tramite de estupefaciente.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [HttpPost]
        [Route("crear")]
        [AuthorizeRoles(RolesEnum.AdministradorEstupefacientes)]
        public async Task<IHttpActionResult> Crear([FromBody] TramiteEstupefacienteDTO tramite)
        {
            var data = Mapear<TramiteEstupefacienteDTO, GENTEMAR_TIPO_TRAMITE>(tramite);
            var response = await _serviceTramite.CrearAsync(data);
            return ResultadoStatus(response);
        }


        /// <summary>
        /// Servicio para editar una tramite
        /// </summary>
        /// <param name="tramite"></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>08/07/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. se ha actualizado el recurso (capacidad).</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud con el estado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [HttpPut]
        [Route("editar")]
        [AuthorizeRoles(RolesEnum.AdministradorEstupefacientes)]
        public async Task<IHttpActionResult> Editar([FromBody] TramiteEstupefacienteDTO tramite)
        {
            var data = Mapear<TramiteEstupefacienteDTO, GENTEMAR_TIPO_TRAMITE>(tramite);
            var response = await _serviceTramite.ActualizarAsync(data);
            return ResultadoStatus(response);
        }

        /// <summary>
        /// Servicio para Inactivar una tramite
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. anular o activa la tramite indicada.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [HttpPut]
        [Route("anula-or-activa/{id}")]
        [AuthorizeRoles(RolesEnum.AdministradorEstupefacientes)]
        public async Task<IHttpActionResult> AnularOrActivar(int id)
        {
            var response = await _serviceTramite.AnulaOrActivaAsync(id);
            return ResultadoStatus(response);
        }
    }
}
