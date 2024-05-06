using GenteMarCore.Entities.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.Models
{
    [Table("GENTEMAR_HISTORIAL_ACLARACION_ANTECEDENTES", Schema = "DBA")]
    public partial class GENTEMAR_HISTORIAL_ACLARACION_ANTECEDENTES : GENTEMAR_CAMPOS_AUDITORIA
    {
        [Key]
        public long id_aclaracion { get; set; }

        public long id_expediente_observacion { get; set; }

        [Required]
        public string detalle_aclaracion { get; set; }

        public string detalle_observacion_anterior_json { get; set; }

        public string ruta_archivo { get; set; }
    }
}
