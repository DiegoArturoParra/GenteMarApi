using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs.Reports;
using DIMARCore.UIEntities.QueryFilters.Reports;
using DIMARCore.Utilities.Helpers;
using System;
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
        public async Task<Respuesta> GenerateReportDatosBasicosCSV(DatosBasicosReportFilter reportFilter)
        {
            var data = await _reportesgdmRepository.GetDataByReportDatosBasicos(reportFilter);
            var file = new FileTocsvDTO
            {
                FileName = $"ReporteDatosBasicos_{DateTime.Now:yyyyMMddHHmmss}.csv",
                FileCSVToBase64 = await _readWriteToCSV.WriteNewCSVToBase64(data),
                Extension = "csv"
            };
            return Responses.SetOkResponse(file);
        }
    }
}
