using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DIMARCore.UIEntities.DTOs.Reports
{
    public class VciteHistoricoPersonaDTO
    {
        public long GenteDeMarId { get; set; }
        public string DocumentoIdentificacion { get; set; }
        public string TipoDocumento { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        [JsonIgnore]
        public DateTime FechaNacimiento { get; set; }
        public string FechaNacimientoString => FechaNacimiento.ToString("dd/MM/yyyy");
        public IEnumerable<VciteInfoHistoricoPersonaDTO> Historico { get; set; }
    }
    public class VciteInfoHistoricoPersonaDTO
    {
        public string Estado { get; set; }
        public string NumeroRadicadoSGDEA { get; set; }
        [JsonIgnore]
        public DateTime? FechaRadicadoSGDEA { get; set; }
        public string FechaRadicadoSGDEAString => this.FechaRadicadoSGDEA.HasValue ? this.FechaRadicadoSGDEA.Value.ToString("dd/MM/yyyy") : "No aplica";
        public string TipoTramite { get; set; }
        [JsonIgnore]
        public DateTime FechaSolicitud { get; set; }
        public string FechaSolicitudString => FechaSolicitud.ToString("dd/MM/yyyy");
        [JsonIgnore]
        public DateTime? FechaAprobacion { get; set; }
        public string FechaAprobacionString => FechaAprobacion.HasValue ? FechaAprobacion.Value.ToString("dd/MM/yyyy") : "No aplica";
        [JsonIgnore]
        public DateTime? FechaVigencia { get; set; }
        public string FechaVigenciaString => FechaVigencia.HasValue ? FechaVigencia.Value.ToString("dd/MM/yyyy") : "No aplica";
        public string EsVigente { get; set; }
        [JsonIgnore]
        public DateTime FechaCreacion { get; set; }
        public string FechaCreacionString => FechaCreacion.ToString("dd/MM/yyyy hh:mm:ss tt");
    }
}
