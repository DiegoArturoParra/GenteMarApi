using System;
using System.Collections.Generic;

namespace DIMARCore.UIEntities.QueryFilters.Reports
{
    /// <summary>
    /// clase filtro Reporte datos basicos (gente de mar) filtros: (estado, género formación, grado y rangos por fecha de creación)
    /// </summary>
    public class DatosBasicosReportFilter
    {
        public List<int> EstadosId { get; set; } = new List<int>();
        public int? GeneroId { get; set; }
        public int? FormacionId { get; set; }
        public List<int> GradosId { get; set; } = new List<int>();
        public DateTime? FechaCreacionInicial { get; set; }
        public DateTime? FechaCreacionFinal { get; set; }
    }
}
