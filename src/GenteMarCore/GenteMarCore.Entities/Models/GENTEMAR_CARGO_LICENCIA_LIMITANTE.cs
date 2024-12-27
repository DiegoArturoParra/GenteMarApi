
namespace GenteMarCore.Entities.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_CARGO_LICENCIA_LIMITANTE", Schema = "DBA")]
    public partial class GENTEMAR_CARGO_LICENCIA_LIMITANTE
    {
        [Key]
        public int id_cargo_licencia_limitante { get; set; }
        public int id_cargo_licencia { get; set; }
        public int id_limitante { get; set; }
    }
}
