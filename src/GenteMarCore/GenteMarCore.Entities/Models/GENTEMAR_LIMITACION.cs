namespace GenteMarCore.Entities.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_LIMITACION", Schema = "DBA")]
    public partial class GENTEMAR_LIMITACION
    {
        [Key]
        public int id_limitacion { get; set; }

        [StringLength(600)]
        public string limitaciones { get; set; }

        public bool activo { get; set; }
    }
}
