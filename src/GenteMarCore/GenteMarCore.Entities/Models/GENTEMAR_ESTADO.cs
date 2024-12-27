namespace GenteMarCore.Entities.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_ESTADO", Schema = "DBA")]
    public partial class GENTEMAR_ESTADO
    {
        [Key]
        public int id_estado { get; set; }
        [Required(ErrorMessage = "Descripción requerida.")]
        [StringLength(100)]
        public string descripcion { get; set; }
        [Required(ErrorMessage = "Sigla requerida.")]
        public string sigla { get; set; }
        public bool activo { get; set; }
    }
}
