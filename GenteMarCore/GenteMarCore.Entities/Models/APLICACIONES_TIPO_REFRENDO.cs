using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.Models
{
    [Table("APLICACIONES_TIPO_CERTIFICADO", Schema = "DBA")]
    public class APLICACIONES_TIPO_REFRENDO
    {
        [Key]
        public int ID_TIPO_CERTIFICADO { get; set; }

        [Required]
        [StringLength(40)]
        public string DESCRIPCION { get; set; }

    }
}
