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
    /// servicios para l de estupefacientes
    /// <Autor>Diego Parra</Autor>
    /// <Fecha>08/07/2022</Fecha>
    /// </summary>
    [EnableCors("*", "*", "*")]

    [RoutePrefix("api/estado-estupefaciente")]
    public class EstadoEstupefacienteController : BaseApiController
    {
        private readonly EstadoEstupefacienteBO _serviceEstado;

        /// <summary>
        /// Constructor
        /// </summary>
        public EstadoEstupefacienteController()
        {
            _serviceEstado = new EstadoEstupefacienteBO();
        }



        /// <summary>
        /// servicio get estados antecedente por filtro de estado
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
        [ResponseType(typeof(List<EstadoEstupefacienteDTO>))]
        [HttpGet]
        [Route("lista")]
        [AuthorizeRoles(RolesEnum.AdministradorVCITE, RolesEnum.GestorVCITE, RolesEnum.JuridicaVCITE,
            RolesEnum.ConsultasVCITE)]
        public IHttpActionResult Listado([FromUri] ActivoDTO dto)
        {
            var query = _serviceEstado.GetAll(dto != null ? dto.Activo : null);
            var listado = Mapear<IEnumerable<GENTEMAR_ESTADO_ANTECEDENTE>, IEnumerable<EstadoEstupefacienteDTO>>(query);
            return Ok(listado);
        }


        /// <summary>
        /// servicio get estadoEstupefaciente por id
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
        [ResponseType(typeof(ResponseTypeSwagger<EstadoEstupefacienteDTO>))]
        [HttpGet]
        [Route("{id}")]
        [AuthorizeRoles(RolesEnum.AdministradorVCITE)]
        public async Task<IHttpActionResult> GetestadoEstupefaciente(int id)
        {
            var estadoEstupefaciente = await _serviceEstado.GetByIdAsync(id);

            var obj = Mapear<GENTEMAR_ESTADO_ANTECEDENTE, EstadoEstupefacienteDTO>((GENTEMAR_ESTADO_ANTECEDENTE)estadoEstupefaciente.Data);
            estadoEstupefaciente.Data = obj;

            return Ok(estadoEstupefaciente);
        }



        /// <summary>
        /// Servicio para crear una estadoEstupefaciente
        /// </summary>
        /// <param name="estadoEstupefaciente"></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>08/07/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="201">Created. la solicitud ha tenido éxito y ha llevado a la creación de la estadoEstupefaciente de estupefaciente.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseCreatedTypeSwagger))]
        [HttpPost]
        [Route("crear")]
        [AuthorizeRoles(RolesEnum.AdministradorVCITE)]
        public async Task<IHttpActionResult> Crear([FromBody] EstadoEstupefacienteDTO estadoEstupefaciente)
        {
            var data = Mapear<EstadoEstupefacienteDTO, GENTEMAR_ESTADO_ANTECEDENTE>(estadoEstupefaciente);
            var response = await _serviceEstado.CrearAsync(data);
            return Created(string.Empty, response);
        }


        /// <summary>
        /// Servicio para editar una estadoEstupefaciente
        /// </summary>
        /// <param name="estadoEstupefaciente"></param>
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
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [HttpPut]
        [Route("editar")]
        [AuthorizeRoles(RolesEnum.AdministradorVCITE)]
        public async Task<IHttpActionResult> Editar([FromBody] EstadoEstupefacienteDTO estadoEstupefaciente)
        {
            var data = Mapear<EstadoEstupefacienteDTO, GENTEMAR_ESTADO_ANTECEDENTE>(estadoEstupefaciente);
            var response = await _serviceEstado.ActualizarAsync(data);
            return Ok(response);
        }

        /// <summary>
        /// Servicio para Inactivar una estadoEstupefaciente
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. anular o activa la estadoEstupefaciente indicada.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [HttpPut]
        [Route("anula-or-activa/{id}")]
        [AuthorizeRoles(RolesEnum.AdministradorVCITE)]
        public async Task<IHttpActionResult> AnularOrActivar(int id)
        {
            var response = await _serviceEstado.AnulaOrActivaAsync(id);
            return Ok(response);
        }
    }
}
