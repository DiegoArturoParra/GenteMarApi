namespace GenteMarCore.Entities.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_HABILITACION", Schema = "DBA")]
    public partial class GENTEMAR_HABILITACION
    {
        [Key]
        public int id_habilitacion { get; set; }

        public string habilitacion { get; set; }

        public bool activo { get; set; } = true;
    }
}
