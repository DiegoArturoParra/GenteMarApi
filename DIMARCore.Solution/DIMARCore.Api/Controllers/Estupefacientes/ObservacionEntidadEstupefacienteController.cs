using DIMARCore.Api.Core.Filters;
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
    /// servicios para las observacion por entidad de los estupefacientes
    /// <Autor>Diego Parra</Autor>
    /// <Fecha>20/10/2022</Fecha>
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/observacion-entidad")]
    public class ObservacionEntidadEstupefacienteController : BaseApiController
    {
        private readonly ObservacionEntidadEstupefacienteBO _service;

        /// <summary>
        /// Constructor
        /// </summary>
        public ObservacionEntidadEstupefacienteController()
        {
            _service = new ObservacionEntidadEstupefacienteBO();
        }


        /// <summary>
        /// servicio get observaciones entidad por estupefaciente
        /// </summary>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>20/10/2022</Fecha>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns>retorna un listado de aclaraciones dependiendo el estupefaciente.</returns>

        [ResponseType(typeof(List<DetalleExpedienteObservacionEstupefacienteDTO>))]
        [HttpGet]
        [Route("estupefaciente/lista/{EstupefacienteId}")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorVCITE, RolesEnum.JuridicaVCITE)]
        public async Task<IHttpActionResult> GetObservacionesEntidadPorEstupefaciente(long EstupefacienteId)
        {
            var data = await _service.GetObservacionesEntidadPorEstupefacienteId(EstupefacienteId);
            return Ok(data);
        }


        /// <summary>
        /// Servicio para crear observación por las entidades de un estupefaciente
        /// </summary>
        /// <param name="observacionesPorEntidad"></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>08/07/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="201">Created. la solicitud ha tenido éxito y ha llevado a la creación de la observación del estupefaciente por cada entidad.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseCreatedTypeSwagger))]
        [HttpPost]
        [Route("estupefaciente/crear-masivo")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorVCITE, RolesEnum.JuridicaVCITE)]
        public async Task<IHttpActionResult> CrearMasivo([FromBody] ObservacionesEntidadBulkDTO observacionesPorEntidad)
        {
            var data = Mapear<IList<ObservacionEntidadEstupefacienteDTO>, IList<GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES>>(observacionesPorEntidad.ObservacionesPorEntidad);
            var response = await _service.CrearObservacionesEntidad(data, observacionesPorEntidad.AntecedenteId);
            return Created(string.Empty, response);
        }

        /// <summary>
        /// Servicio para crear observación por entidad de un estupefaciente
        /// </summary>
        /// <param name="obj"></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>08/07/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="201">Created. la solicitud ha tenido éxito y ha llevado a la creación de la observación del estupefaciente por entidad.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseCreatedTypeSwagger))]
        [HttpPost]
        [Route("estupefaciente/crear")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorVCITE, RolesEnum.JuridicaVCITE)]
        public async Task<IHttpActionResult> Crear([FromBody] CrearObservacionEntidadVciteDTO obj)
        {
            var data = Mapear<ObservacionEntidadEstupefacienteDTO, GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES>(obj.ObservacionPorEntidad);
            data.id_antecedente = obj.AntecedenteId;
            var response = await _service.CrearObservacionPorEntidad(data);
            return Created(string.Empty, response);
        }

        /// <summary>
        /// Servicio para editar la observación a verificación exitosa de varios estupefacientes
        /// </summary>
        /// <param name="observacionDeEstupefacientes"></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>08/07/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="201">Created. la solicitud ha tenido éxito y ha llevado a la creación de la observación del estupefaciente por cada entidad.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [HttpPut]
        [Route("edicion-masiva-parcial-de-estupefacientes")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorVCITE, RolesEnum.JuridicaVCITE)]
        public async Task<IHttpActionResult> EdicionParcialDeEstupefacientesIds([FromBody] EditBulkPartialEstupefacientesDTO observacionDeEstupefacientes)
        {
            ValidateModelAndThrowIfInvalid(observacionDeEstupefacientes.ObservacionEntidad);
            var response = await _service.EdicionParcialDeEstupefacientes(observacionDeEstupefacientes, PathActual);
            return Ok(response);
        }
        /// <summary>
        /// Servicio para editar masivamente estados de estupefacientes
        /// </summary>
        /// <param name="estupefacientes"></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>28/04/2023</Fecha>
        /// </remarks>
        /// <response code="200">OK. se ha actualizado los recursos (estupefacientes).</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud con el estado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [HttpPut]
        [Route("edicion-masiva-de-estupefacientes")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorVCITE, RolesEnum.JuridicaVCITE)]
        public async Task<IHttpActionResult> EditarMasivoDeEstupefacientesIds([FromBody] EditBulkEstupefacientesDTO estupefacientes)
        {
            ValidateModelAndThrowIfInvalid(estupefacientes.ObservacionesPorEntidad);
            var response = await _service.EdicionBulkDeEstupefacientes(estupefacientes, PathActual);
            return Ok(response);
        }

    }
}
