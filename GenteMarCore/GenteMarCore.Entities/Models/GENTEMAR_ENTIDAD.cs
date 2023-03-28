namespace GenteMarCore.Entities.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_ENTIDAD", Schema = "DBA")]
    public partial class GENTEMAR_ENTIDAD
    {
        [Key]
        public int id_entidad { get; set; }

        [StringLength(100)]
        public string entidad { get; set; }

        public bool activo { get; set; }
    }
}
