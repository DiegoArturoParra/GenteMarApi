namespace GenteMarCore.Entities.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_SECCION_TITULOS", Schema = "DBA")]
    public partial class GENTEMAR_SECCION_TITULOS
    {
        [Key]
        public int id_seccion { get; set; }

        [StringLength(100)]
        public string actividad_a_bordo { get; set; }
        public bool activo { get; set; } = true;
    }
}
