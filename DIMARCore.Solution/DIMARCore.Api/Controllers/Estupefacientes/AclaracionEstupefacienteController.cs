using DIMARCore.Api.Core.Filters;
using DIMARCore.Api.Core.Models;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers.Estupefacientes
{
    /// <summary>
    /// servicios para las aclaraciones de un estupefaciente
    /// <Autor>Diego Parra</Autor>
    /// <Fecha>08/07/2022</Fecha>
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/aclaracion-estupefaciente")]
    public class AclaracionEstupefacienteController : BaseApiController
    {
        private readonly AclaracionEstupefacienteBO _Aclaracionservice;
        /// <summary>
        /// constructor
        /// </summary>
        public AclaracionEstupefacienteController()
        {
            _Aclaracionservice = new AclaracionEstupefacienteBO();
        }

        // GET: Historial de las aclaraciones de un estupefaciente
        /// <summary>
        ///  Historial de las aclaraciones de un estupefaciente por cada entidad
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>01/08/2023</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el listado de expedientes.</response>        
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(List<HistorialAclaracionDTO>))]
        [HttpGet]
        [AuthorizeRolesFilter(RolesEnum.AdministradorVCITE, RolesEnum.JuridicaVCITE)]
        [Route("historico/{id}")]
        public async Task<IHttpActionResult> GetHistorialPorEstupefacienteId(long id)
        {
            var historialAclaraciones = await _Aclaracionservice.GetHistorialPorEstupefacienteId(id, PathActual);
            return Ok(historialAclaraciones);
        }

        // Post: Agregar una aclaración a la observación de un estupefaciente
        /// <summary>
        /// Servicio para agregar una aclaración al expediente de un estupefaciente
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>08/07/2023</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. se ha actualizado el recurso (capacidad).</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud con el estado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [HttpPost]
        [Route("agregar-al-expediente")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorVCITE, RolesEnum.JuridicaVCITE)]
        public async Task<IHttpActionResult> AgregarAclaracionAlExpediente([FromBody] AclaracionEditDTO aclaracionEdit)
        {
            var response = await _Aclaracionservice.AgregarAclaracionEstupefaciente(aclaracionEdit, PathActual);
            return ResultadoStatus(response);
        }
    }
}
