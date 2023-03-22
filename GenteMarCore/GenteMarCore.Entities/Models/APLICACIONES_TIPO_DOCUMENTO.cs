namespace GenteMarCore.Entities.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("APLICACIONES_TIPO_DOCUMENTO", Schema = "DBA")]
    public partial class APLICACIONES_TIPO_DOCUMENTO
    {
        [Key]
        public int ID_TIPO_DOCUMENTO { get; set; }

        [Required]
        [StringLength(50)]
        public string DESCRIPCION { get; set; }

        [StringLength(6)]
        public string SIGLA { get; set; }

        [NotMapped]
        public string SiglaDescripcion => $"{this.SIGLA} - {this.DESCRIPCION}";
    }
}
