using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs.Reports;
using DIMARCore.UIEntities.QueryFilters.Reports;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class ReportesGDMBO
    {
        private readonly ReadWriteToCSVFile _readWriteToCSV;
        private readonly ReportesgdmRepository _reportesgdmRepository;
        public ReportesGDMBO()
        {
            _readWriteToCSV = new ReadWriteToCSVFile();
            _reportesgdmRepository = new ReportesgdmRepository();
        }
        public async Task<Respuesta> GenerateReportDatosBasicosCSV(DatosBasicosReportFilter reportFilter, CancellationToken cancellationToken)
        {
            using (var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                var data = await _reportesgdmRepository.GetDataByReportDatosBasicos(reportFilter, tokenSource);

                if (!data.Any())
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No hay datos para generar el reporte."));

                var file = new FileTocsvDTO
                {
                    FileName = $"reporte_datosbasicos_{DateTime.Now:dd-MM-yyyy_HH-mm-ss}.csv",
                    ArchivoBase64 = await _readWriteToCSV.WriteNewCSVToBase64(data),
                    Extension = Constantes.EXTENSION_CSV
                };
                return Responses.SetOkResponse(file, "Se ha generado el reporte satisfactoriamente.");
            }
        }

        public async Task<Respuesta> GenerateReportLicenciasCSV(LicenciasReportFilter reportFilter, CancellationToken cancellationToken)
        {
            using (var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                var data = await _reportesgdmRepository.GetDataByReportLicencias(reportFilter, tokenSource);

                if (!data.Any())
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No hay datos para generar el reporte."));

                var file = new FileTocsvDTO
                {
                    FileName = $"reporte_licencias_{DateTime.Now:dd-MM-yyyy_HH-mm-ss}.csv",
                    ArchivoBase64 = await _readWriteToCSV.WriteNewCSVToBase64(data),
                    Extension = Constantes.EXTENSION_CSV
                };
                return Responses.SetOkResponse(file, "Se ha generado el reporte satisfactoriamente.");
            }
        }

        public async Task<Respuesta> GenerateReportTitulosCSV(TitulosReportFilter reportFilter, CancellationToken cancellationToken)
        {
            using (var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                var data = await _reportesgdmRepository.GetDataByReportTitulosDapper(reportFilter, tokenSource);

                if (!data.Any())
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No hay datos para generar el reporte."));

                var file = new FileTocsvDTO
                {
                    FileName = $"reporte_titulos_{DateTime.Now:dd-MM-yyyy_HH-mm-ss}.csv",
                    ArchivoBase64 = await _readWriteToCSV.WriteNewCSVToBase64(data),
                    Extension = Constantes.EXTENSION_CSV
                };
                return Responses.SetOkResponse(file, "Se ha generado el reporte satisfactoriamente.");
            }
        }
    }
}
