using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.ViewsSQL
{
    [Table("VwGenteMarReporteDatosBasicos", Schema = "DBA")]
    public class ViewGenteMarReporteDatosBasicos
    {
        [Key]
        public long GenteMarId { get; set; }
        public int EstadoId { get; set; }
        public int GeneroId { get; set; }
        public int FormacionId { get; set; }
        public int GradoId { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string TipoDocumento { get; set; }
        public string DocumentoIdentificacion { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Genero { get; set; }
        public string Estado { get; set; }
        public string Formacion { get; set; }
        public string Grado { get; set; }
        public short Edad { get; set; }
        public string PaisNacimiento { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string CorreoElectronico { get; set; }
        public string NumeroContacto { get; set; }
        public string PaisResidencia { get; set; }
    }
}
