using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.Models
{
    [Table("GENTEMAR_CARGO_LICENCIA_CATEGORIA", Schema = "DBA")]
    public class GENTEMAR_CARGO_LICENCIA_CATEGORIA
    {
        [Key]
        public int id_cargo_categoria { get; set; }

        public int id_categoria { get; set; }

        public int id_cargo_licencia { get; set; }

        public bool activo { get; set; }
    }
}
