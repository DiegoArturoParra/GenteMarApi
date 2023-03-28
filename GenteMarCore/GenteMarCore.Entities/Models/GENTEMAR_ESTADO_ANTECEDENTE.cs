namespace GenteMarCore.Entities.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_ESTADO_ANTECEDENTE", Schema = "DBA")]
    public partial class GENTEMAR_ESTADO_ANTECEDENTE
    {
        [Key]
        public int id_estado_antecedente { get; set; }
        public bool activo { get; set; } = true;

        [StringLength(250)]
        public string descripcion_estado_antecedente { get; set; }
    }
}
