using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.ViewsSQL
{
    [Table("VwGenteMarReporteTitulos", Schema = "DBA")]
    public class ViewGenteMarReporteTitulos
    {
        [Key]
        public long TituloId { get; set; }
        public long GenteMarId { get; set; }
        public int SeccionId { get; set; }
        public int GeneroId { get; set; }
        public int CapitaniaId { get; set; }
        public int EstadoTramiteId { get; set; }
        public int CargoTituloId { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string TipoDocumento { get; set; }
        public string DocumentoIdentificacion { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Genero { get; set; }
        public string Capitania { get; set; }
        public string Cargos { get; set; }
        public string Reglas { get; set; }
        public DateTime FechaExpedicion { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string EstadoTramite { get; set; }
        public string Radicado { get; set; }
        public string Seccion { get; set; }
        public short Edad { get; set; }
    }
}
