using System;
using System.Collections.Generic;

namespace DIMARCore.UIEntities.QueryFilters.Reports
{
    public class LicenciasReportFilter
    {
        public List<int> EstadosTramiteId { get; set; } = new List<int>();
        public List<int> CapitaniasId { get; set; } = new List<int>();
        public int? GeneroId { get; set; }
        public List<int> TiposDeLicenciaId { get; set; } = new List<int>();
        public List<int> ActividadesId { get; set; } = new List<int>();
        public List<int> SeccionesId { get; set; } = new List<int>();
        public List<int> ClasesId { get; set; } = new List<int>();
        public List<int> CargosLicenciaId { get; set; } = new List<int>();
        public DateTime? FechaExpedicionInicial { get; set; }
        public DateTime? FechaExpedicionFinal { get; set; }
        public DateTime? FechaVencimientoInicial { get; set; }
        public DateTime? FechaVencimientoFinal { get; set; }
    }
}
