using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.Models
{

    [Table("GENTEMAR_REPOSITORIO_ARCHIVOS", Schema = "DBA")]
    public partial class GENTEMAR_REPOSITORIO_ARCHIVOS
    {
        [Key]
        public int Id { get; set; }
        public int? IdAplicacion { get; set; }
        public string IdModulo { get; set; }
        public string NombreModulo { get; set; }
        public string TipoDocumento { get; set; }
        public DateTime FechaCargue { get; set; }
        public string Nombre { get; set; }
        public string DescripcionDocumento { get; set; }
        public string NombreArchivo { get; set; }
        public string RutaArchivo { get; set; }
        public string IdUsuarioCreador { get; set; }
        public DateTime FechaHoraCreacion { get; set; }
        public string IdUsuarioUltimaActualizacion { get; set; }
        public DateTime? FechaHoraUltimaActualizacion { get; set; }
    }
}
