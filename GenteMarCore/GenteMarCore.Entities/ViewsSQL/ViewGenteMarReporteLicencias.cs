using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.ViewsSQL
{
    [Table("VwGenteMarReporteLicencias", Schema = "DBA")]
    public class ViewGenteMarReporteLicencias
    {
        [Key]
        public long LicenciaId { get; set; }
        public long GenteMarId { get; set; }
        public int EstadoTramiteId { get; set; }
        public int CapitaniaId { get; set; }
        public int GeneroId { get; set; }
        public int TipoLicenciaId { get; set; }
        public int ActividadId { get; set; }
        public int SeccionId { get; set; }
        public int ClaseId { get; set; }
        public int CargoLicenciaId { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string DocumentoIdentificacion { get; set; }
        public string TipoDocumento { get; set; }
        public string EstadoTramite { get; set; }
        public string Capitania { get; set; }
        public decimal Radicado { get; set; }
        public DateTime FechaExpedicion { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string CodigoLicencia { get; set; }
        public string TipoLicencia { get; set; }
        public string Seccion { get; set; }
        public string Actividad { get; set; }
        public string Categoria { get; set; }
        public string CargoLicencia { get; set; }
        public string Genero { get; set; }
        public int Edad { get; set; }
    }
}
