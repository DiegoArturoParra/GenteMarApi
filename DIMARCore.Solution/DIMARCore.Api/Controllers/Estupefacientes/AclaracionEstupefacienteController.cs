using DIMARCore.Api.Core.Atributos;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers.Estupefacientes
{
    /// <summary>
    /// servicios para las aclaraciones de los estupefacientes
    /// <Autor>Diego Parra</Autor>
    /// <Fecha>20/10/2022</Fecha>
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/aclaraciones-por-estupefaciente")]
    public class AclaracionEstupefacienteController : BaseApiController
    {
        private readonly AclaracionEstupefacienteBO _service;

        /// <summary>
        /// Constructor
        /// </summary>
        public AclaracionEstupefacienteController()
        {
            _service = new AclaracionEstupefacienteBO();
        }




        /// <summary>
        /// servicio get aclaraciones por id estupefaciente
        /// </summary>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>20/10/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns>retorna un listado de aclaraciones dependiendo el estupefaciente.</returns>

        [ResponseType(typeof(IEnumerable<DetalleAclaracionesEstupefacienteDTO>))]
        [HttpGet]
        [Route("lista/{EstupefacienteId}")]
        [AuthorizeRoles(RolesEnum.AdministradorEstupefacientes, RolesEnum.JuridicaEstupefacientes)]
        public async Task<IHttpActionResult> GetAclaracionesPorEstupefaciente(long EstupefacienteId)
        {
            var data = await _service.GetAclaracionesPorEstupefacienteId(EstupefacienteId);
            return Ok(data);
        }


        /// <summary>
        /// Servicio para crear aclaraciones por las entidades
        /// </summary>
        /// <param name="aclaraciones"></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>08/07/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="201">Created. la solicitud ha tenido éxito y ha llevado a la creación de las aclaraciones del estupefaciente.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(Respuesta))]
        [HttpPost]
        [Route("crear")]
        [AuthorizeRoles(RolesEnum.AdministradorEstupefacientes, RolesEnum.JuridicaEstupefacientes)]
        public async Task<IHttpActionResult> Crear([FromBody] AclaracionCreateDTO aclaraciones)
        {
            var data = Mapear<IList<AclaracionEstupefacienteDTO>, IList<GENTEMAR_ACLARACION_ANTECEDENTES>>(aclaraciones.Aclaraciones);
            var response = await _service.CrearAclaracionesPorEntidades(data, aclaraciones.AntecedenteId);
            return Created(string.Empty, response);
        }


        /// <summary>
        /// Servicio para editar las aclaraciones por las entidades
        /// </summary>
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
        [ResponseType(typeof(Respuesta))]
        [HttpPut]
        [Route("editar")]
        [AuthorizeRoles(RolesEnum.AdministradorEstupefacientes)]
        public async Task<IHttpActionResult> Editar()
        {
            Respuesta response;
            var req = HttpContext.Current.Request;
            var archivo = req.Files["File"];
            var format = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
            var obj = JsonConvert.DeserializeObject<object>(req["Data"], dateTimeConverter);

            AclaracionEditDTO dto = JsonConvert.DeserializeObject<AclaracionEditDTO>(req["Data"], dateTimeConverter);
            if (dto != null)
                dto.Observacion.Archivo = archivo;
            response = ValidarAclaracionConObservacion(dto);
            if (response.Estado)
            {
                var data = Mapear<IList<AclaracionEstupefacienteDTO>, IList<GENTEMAR_ACLARACION_ANTECEDENTES>>(dto.Aclaraciones);
                var dataObservacion = Mapear<ObservacionDTO, GENTEMAR_OBSERVACIONES_ANTECEDENTES>(dto.Observacion);
                dataObservacion.id_antecedente = dto.AntecedenteId;
                response = await _service.EditarAclaracionesPorEntidades(data, PathActual, dataObservacion);
            }
            return ResultadoStatus(response);
        }

        /// <summary>
        /// Valida los datos
        /// </summary>
        /// <param name="aclaracionEdit"></param>
        /// <returns></returns>
        private Respuesta ValidarAclaracionConObservacion(AclaracionEditDTO aclaracionEdit)
        {
            Respuesta res = new Respuesta();
            this.Validate(aclaracionEdit);
            if (!ModelState.IsValid)
            {
                var errores = GetErrorListFromModelState(ModelState);
                res.Estado = false;
                res.StatusCode = HttpStatusCode.BadRequest;
                res.Mensaje = string.Join(";", errores.Select(x => x.Key + "=" + x.Value).ToArray());
            }
            else
            {
                res.Estado = true;
            }

            return res;
        }
    }
}
