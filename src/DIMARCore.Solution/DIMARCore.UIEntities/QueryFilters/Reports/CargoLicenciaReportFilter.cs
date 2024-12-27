using System.Collections.Generic;

namespace DIMARCore.UIEntities.QueryFilters.Reports
{
    public class CargoLicenciaReportFilter
    {
        public List<int> SeccionesLicenciaId { get; set; } = new List<int>();
        public List<int> ActividadesId { get; set; } = new List<int>();
        public List<int> ClasesLicenciaId { get; set; } = new List<int>();
        public List<int> TiposDeLicenciaId { get; set; } = new List<int>();
    }
}
