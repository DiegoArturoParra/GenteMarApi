
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.Models
{
    [Table("GENTEMAR_LIMITANTE", Schema = "DBA")]
    public partial class GENTEMAR_LIMITANTE
    {
        [Key]
        public int id_limitante { get; set; }
        public string descripcion { get; set; }
        public bool activo { get; set; }
    }
}
