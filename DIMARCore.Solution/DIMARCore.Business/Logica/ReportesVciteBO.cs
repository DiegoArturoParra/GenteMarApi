using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs.Reports;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.UIEntities.QueryFilters.Reports;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class ReportesVciteBO
    {
        private readonly ReadWriteToCSVFile _readWriteToCSV;
        private readonly ReportesVciteRepository _reportesvciteRepository;
        public ReportesVciteBO()
        {
            _readWriteToCSV = new ReadWriteToCSVFile();
            _reportesvciteRepository = new ReportesVciteRepository();
        }
        public async Task<Respuesta> GenerateReportEstupefacientesCSV(EstupefacientesReportFilter reportFilter, CancellationToken cancellationToken)
        {
            using (var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                var data = await _reportesvciteRepository.GetDataByReportEstupefacientes(reportFilter, tokenSource);

                if (!data.Any())
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No hay datos para generar el reporte."));

                var file = new FileTocsvDTO
                {
                    FileName = $"reporte_VCITE_{DateTime.Now:dd-MM-yyyy_HH-mm-ss}.csv",
                    ArchivoBase64 = await _readWriteToCSV.WriteNewCSVToBase64(data),
                    Extension = Constantes.EXTENSION_CSV
                };
                return Responses.SetOkResponse(file, "Se ha generado el reporte satisfactoriamente.");
            }
        }

        public async Task<IEnumerable<ReportPieChartVciteDTO>> GetDataByPieChartEstadosEstupefaciente(ReportPieChartVciteFilter reportPieChartFilter)
        {
            return await _reportesvciteRepository.GetDataByPieChartEstadosEstupefaciente(reportPieChartFilter);
        }

        public async Task<VciteHistoricoPersonaDTO> GetHistoricoByPersonaIdentificacion(DocumentFilter documentoFilter)
        {
            var data = await new EstupefacienteDatosBasicosRepository().GetPersonaConDocumento(documentoFilter);
            if (data is null)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se ha encontrado la persona."));

            var historico = await _reportesvciteRepository.GetHistoricoByPersonaIdentificacion(data.GenteDeMarId);

            data.Historico = historico;
            return data;
        }
    }
}
