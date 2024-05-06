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
        private readonly IReadWriteToCSVFile _readWriteToCSV;
        private readonly ReportesTituloRepository _reportestituloRepository;
        private readonly ReportesLicenciaRepository _reporteslicenciaRepository;
        private readonly ReportesDatosBasicosRepository _reportesdatosbasicosRepository;
        public ReportesGDMBO()
        {
            _readWriteToCSV = new ReadWriteToCSVFile();
            _reportestituloRepository = new ReportesTituloRepository();
            _reporteslicenciaRepository = new ReportesLicenciaRepository();
            _reportesdatosbasicosRepository = new ReportesDatosBasicosRepository();
        }
        public async Task<Respuesta> GenerateReportDatosBasicosCSV(DatosBasicosReportFilter reportFilter, CancellationToken cancellationToken)
        {
            using (var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                var data = await _reportesdatosbasicosRepository.GetDataByReportCsv(reportFilter, tokenSource);

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
                var data = await _reporteslicenciaRepository.GetDataByReportCsv(reportFilter, tokenSource);

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
                var data = await _reportestituloRepository.GetDataByReportCsv(reportFilter, tokenSource);

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
