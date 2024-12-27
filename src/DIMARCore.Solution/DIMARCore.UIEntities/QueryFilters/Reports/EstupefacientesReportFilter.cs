using System;
using System.Collections.Generic;

namespace DIMARCore.UIEntities.QueryFilters.Reports
{
    public class EstupefacientesReportFilter
    {
        public List<int> EstadosId { get; set; } = new List<int>();
        public List<int> ConsolidadosId { get; set; } = new List<int>();
        public bool? IsVigente { get; set; }
        public DateTime? FechaCreacionInicial { get; set; }
        public DateTime? FechaCreacionFinal { get; set; }
    }
}
