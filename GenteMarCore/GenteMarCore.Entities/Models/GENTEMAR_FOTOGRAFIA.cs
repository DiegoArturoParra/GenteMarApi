namespace GenteMarCore.Entities.Models
{
    using GenteMarCore.Entities.Helpers;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_FOTOGRAFIA", Schema = "DBA")]
    public partial class GENTEMAR_FOTOGRAFIA : GENTEMAR_CAMPOS_AUDITORIA
    {
        [Key]
        public int id_fotografia { get; set; }

        public long id_gentemar { get; set; }

        [StringLength(100)]
        public string detalle { get; set; }

        [Required]
        public string fotografia { get; set; }

        public bool activo { get; set; }
    }
}
