using CsvHelper.Configuration.Attributes;
using DIMARCore.Utilities.Helpers;
using System;

namespace DIMARCore.UIEntities.DTOs.Reports
{
    public class LicenciasReportDTO
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
        public short Edad => Reutilizables.CalcularEdad(this.FechaNacimiento);
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
        public decimal Radicado { get; set; }
        [Name("Código de la licencia")]
        public string CodigoLicencia { get; set; }
        [Name("Tipo de licencia")]
        public string TipoLicencia { get; set; }
        public string Actividad { get; set; }
        [Name("Sección")]
        public string Seccion { get; set; }
        [Name("Clase")]
        public string Categoria { get; set; }
        [Name("Cargo")]
        public string CargoLicencia { get; set; }

    }
}
