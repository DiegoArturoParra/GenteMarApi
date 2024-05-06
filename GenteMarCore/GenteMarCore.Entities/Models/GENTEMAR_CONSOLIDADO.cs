namespace GenteMarCore.Entities.Models
{
    using GenteMarCore.Entities.Helpers;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_CONSOLIDADO", Schema = "DBA")]
    public partial class GENTEMAR_CONSOLIDADO : GENTEMAR_CAMPOS_AUDITORIA
    {
        [Key]
        public int id_consolidado { get; set; }
        [Required]
        public string numero_consolidado { get; set; }
    }
}
