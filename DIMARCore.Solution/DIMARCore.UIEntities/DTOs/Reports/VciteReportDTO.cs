using CsvHelper.Configuration.Attributes;
using DIMARCore.Utilities.Enums;
using System;

namespace DIMARCore.UIEntities.DTOs.Reports
{
    public class VciteReportDTO
    {
        [Name("Documento de identificación")]
        public string Documento { get; set; }
        [Name("Tipo de documento")]
        public string TipoDocumento { get; set; }
        [Ignore]
        public DateTime FechaNacimiento { get; set; }
        [Name("Fecha de nacimiento")]
        public string FechaNacimientoString => this.FechaNacimiento.ToString("dd/MM/yyyy");
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Estado { get; set; }
        [Name("Número de radicado")]
        public string NumeroRadicadoSGDEA { get; set; }
        [Ignore]
        public DateTime? FechaRadicadoSGDEA { get; set; }
        [Name("Fecha radicado")]
        public string FechaRadicadoSGDEAString => this.FechaRadicadoSGDEA.HasValue ? this.FechaRadicadoSGDEA.Value.ToString("dd/MM/yyyy") : "No aplica.";
        [Name("Tipo de tramite")]
        public string TipoTramite { get; set; }
        [Ignore]
        public DateTime FechaSolicitud { get; set; }
        [Name("Fecha de solicitud")]
        public string FechaSolicitudString => this.FechaSolicitud.ToString("dd/MM/yyyy");
        [Ignore]
        public DateTime? FechaAprobacion { get; set; }
        [Name("Fecha de aprobación")]
        public string FechaAprobacionString => this.FechaAprobacion.HasValue ? this.FechaAprobacion.Value.ToString("dd/MM/yyyy") :
            this.Estado.Equals(EnumConfig.GetDescription(EstadoEstupefacienteEnum.Negativa)) ? "No aplica." : "No contiene aún.";
        [Ignore]
        public DateTime? FechaVigencia { get; set; }
        [Name("Fecha de vigencia")]
        public string FechaVigenciaString => this.FechaVigencia.HasValue ? this.FechaVigencia.Value.ToString("dd/MM/yyyy") :
              this.Estado.Equals(EnumConfig.GetDescription(EstadoEstupefacienteEnum.Negativa)) ? "No aplica." : "No contiene aún.";
        [Name("Es Vigente")]
        public string EsVigente { get; set; }
        [Name("Fecha de creación")]
        public DateTime FechaCreacion { get; set; }
    }
}
