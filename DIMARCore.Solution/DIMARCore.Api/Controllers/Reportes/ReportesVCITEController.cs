using DIMARCore.Api.Core.Atributos;
using DIMARCore.Api.Core.Models;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs.Reports;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.UIEntities.QueryFilters.Reports;
using DIMARCore.Utilities.Enums;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers.Reportes
{
    /// <summary>
    /// Servicios que genera los reportes de estupefacientes
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/reportes-vcite")]
    [AuthorizeRoles(RolesEnum.AdministradorVCITE)]
    public class ReportesVCITEController : BaseApiController
    {
        private readonly ReportesVciteBO _reportesVciteBusiness;

        /// <summary>
        /// ctor
        /// </summary>
        public ReportesVCITEController()
        {
            _reportesVciteBusiness = new ReportesVciteBO();
        }

        /// <summary>
        /// Servicio para generar el reporte de estupefacientes en formato CSV
        /// </summary>        
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>25/10/2023</Fecha>
        /// </remarks>
        /// <param name="reportFilter">Filtro para el reporte</param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Ok. la solicitud ha tenido éxito y ha llevado a la generación del reporte.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado data a partir del filtro.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseTypeSwagger<FileTocsvDTO>))]
        [HttpPost]
        [Route("generar-csv")]
        public async Task<IHttpActionResult> GenerarCSVEstupefacientes([FromBody] EstupefacientesReportFilter reportFilter, CancellationToken cancellationToken)
        {
            var report = await _reportesVciteBusiness.GenerateReportEstupefacientesCSV(reportFilter, cancellationToken);
            return Ok(report);
        }

        // GET: Historico de estupefacientes de una persona
        /// <summary>
        ///   Historico de estupefacientes de una persona
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>25/10/2023</Fecha>
        /// </remarks>
        /// <response code="200">OK. Devuelve el historico de estupefacientes de una persona.</response>        
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(VciteHistoricoPersonaDTO))]
        [HttpGet]
        [Route("historico-persona")]
        public async Task<IHttpActionResult> GetHistoricoEstupefacientesPersona([FromUri] DocumentFilter documentoFilter)
        {
            var data = await _reportesVciteBusiness.GetHistoricoByPersonaIdentificacion(documentoFilter);
            return Ok(data);
        }

        // GET: Grafico Pie Chart de estupefacientes por estado
        /// <summary>
        ///   Grafico Pie Chart de estupefacientes por estado
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>08/11/2023</Fecha>
        /// </remarks>
        /// <response code="200">OK. Devuelve el conteo de estados de los estupefacientes para pintarlo en un grafico pie chart.</response>        
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(List<ReportPieChartVciteDTO>))]
        [HttpPost]
        [Route("pie-chart-estados")]
        public async Task<IHttpActionResult> GetPieChartEstupefacientesPorEstado([FromBody] ReportPieChartVciteFilter reportPieChartFilter)
        {
            var data = await _reportesVciteBusiness.GetDataByPieChartEstadosEstupefaciente(reportPieChartFilter);
            return Ok(data);
        }
    }
}
