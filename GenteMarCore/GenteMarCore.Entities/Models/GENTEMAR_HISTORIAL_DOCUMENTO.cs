using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.Models
{
    [Table("GENTEMAR_HISTORIAL_DOCUMENTO", Schema = "DBA")]
    public partial class GENTEMAR_HISTORIAL_DOCUMENTO
    {
        [Key]
        public int id_historial_documento { get; set; }
        public long id_gentemar { get; set; }
        public string documento_identificacion { get; set; }
        public int id_tipo_documento { get; set; }
        public DateTime fecha_cambio { get; set; }
    }
}
