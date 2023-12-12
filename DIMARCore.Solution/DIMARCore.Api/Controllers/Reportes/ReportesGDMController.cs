using DIMARCore.Api.Core.Atributos;
using DIMARCore.Api.Core.Models;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs.Reports;
using DIMARCore.UIEntities.QueryFilters.Reports;
using DIMARCore.Utilities.Enums;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers.Reportes
{
    /// <summary>
    /// Servicios que genera los reportes de Gente de Mar, datos Basicos, licencias y titulos (GDM)
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/reportes-gdm")]
    [AuthorizeRoles(RolesEnum.AdministradorGDM)]
    public class ReportesGDMController : BaseApiController
    {
        private readonly ReportesGDMBO _reportesGDMBusiness;
        /// <summary>
        /// ctor
        /// </summary>
        public ReportesGDMController()
        {
            _reportesGDMBusiness = new ReportesGDMBO();
        }

        /// <summary>
        /// Servicio para generar el reporte de datos basicos de gente de mar en formato CSV
        /// </summary>        
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>10/08/2023</Fecha>
        /// </remarks>
        /// <param name="reportFilter">objeto para filtrar la consulta del reporte.</param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Ok. la solicitud ha tenido éxito y ha llevado a la generación del reporte.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado data a partir del filtro.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseTypeSwagger<FileTocsvDTO>))]
        [HttpPost]
        [Route("generar-csv-datos-basicos")]
        public async Task<IHttpActionResult> GenerarCSVDatosBasicos([FromBody] DatosBasicosReportFilter reportFilter, CancellationToken cancellationToken)
        {
            var report = await _reportesGDMBusiness.GenerateReportDatosBasicosCSV(reportFilter, cancellationToken);
            return Ok(report);
        }


        /// <summary>
        /// Servicio para generar el reporte de titulos de navegación de gente de mar en formato CSV
        /// </summary>        
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>10/08/2023</Fecha>
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
        [Route("generar-csv-titulos-navegacion")]
        public async Task<IHttpActionResult> GenerarCSVTitulosNavegacion([FromBody] TitulosReportFilter reportFilter, CancellationToken cancellationToken)
        {
            var report = await _reportesGDMBusiness.GenerateReportTitulosCSV(reportFilter, cancellationToken);
            return Ok(report);
        }

        /// <summary>
        /// Servicio para generar el reporte de licencias de gente de mar en formato CSV
        /// </summary>        
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>10/08/2023</Fecha>
        /// </remarks>
        /// <param name="reportFilter">objeto para filtrar la consulta del reporte.</param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Ok. la solicitud ha tenido éxito y ha llevado a la generación del reporte.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado data a partir del filtro.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseTypeSwagger<FileTocsvDTO>))]
        [HttpPost]
        [Route("generar-csv-licencias")]
        public async Task<IHttpActionResult> GenerarCSVLicencias([FromBody] LicenciasReportFilter reportFilter, CancellationToken cancellationToken)
        {
            var report = await _reportesGDMBusiness.GenerateReportLicenciasCSV(reportFilter, cancellationToken);
            return Ok(report);
        }
    }
}
