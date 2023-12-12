using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DIMARCore.UIEntities.QueryFilters.Reports
{
    public class TitulosReportFilter
    {
        public List<int> EstadosTramiteId { get; set; } = new List<int>();
        public List<int> CapitaniasId { get; set; } = new List<int>();
        public int? GeneroId { get; set; }
        public List<int> SeccionesId { get; set; } = new List<int>();
        public List<int> CargosTituloId { get; set; } = new List<int>();
        public DateTime? FechaExpedicionInicial { get; set; }
        public DateTime? FechaExpedicionFinal { get; set; }
        public DateTime? FechaVencimientoInicial { get; set; }
        public DateTime? FechaVencimientoFinal { get; set; }
        [JsonIgnore]
        public string EstadosTramiteIdArrayIn => this.EstadosTramiteId.Any() ? string.Join(",", this.EstadosTramiteId) : null;
        [JsonIgnore]
        public string CapitaniasIdArrayIn => this.CapitaniasId.Any() ? string.Join(",", this.CapitaniasId) : null;
        [JsonIgnore]
        public string SeccionesIdArrayIn => this.SeccionesId.Any() ? string.Join(",", this.SeccionesId) : null;
        [JsonIgnore]
        public string CargosTituloIdArrayIn => this.CargosTituloId.Any() ? string.Join(",", this.CargosTituloId) : null;
    }
}
