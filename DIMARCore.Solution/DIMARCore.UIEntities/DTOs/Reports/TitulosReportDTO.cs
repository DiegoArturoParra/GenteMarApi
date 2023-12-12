using CsvHelper.Configuration.Attributes;
using System;

namespace DIMARCore.UIEntities.DTOs.Reports
{
    public class TitulosReportDTO
    {
        [Name("Documento de identificación")]
        public string DocumentoIdentificacion { get; set; }
        [Name("Tipo de documento")]
        public string TipoDocumento { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        [Name("Género")]
        public string Genero { get; set; }
        [Ignore]
        public DateTime FechaNacimiento { get; set; }
        public short Edad { get; set; }
        [Name("Capitanía")]
        public string Capitania { get; set; }
        [Ignore]
        public DateTime FechaExpedicion { get; set; }
        [Ignore]
        public DateTime FechaVencimiento { get; set; }
        [Name("Fecha de expedición")]
        public string FechaExpedicionString => this.FechaExpedicion.ToString("dd/MM/yyyy");
        [Name("Fecha de vencimiento")]
        public string FechaVencimientoString => this.FechaVencimiento.ToString("dd/MM/yyyy");
        [Name("Estado del tramite")]
        public string EstadoTramite { get; set; }
        public string Radicado { get; set; }
        [Name("Sección")]
        public string Seccion { get; set; }
        public string Reglas { get; set; }
        public string Cargos { get; set; }
    }
}
