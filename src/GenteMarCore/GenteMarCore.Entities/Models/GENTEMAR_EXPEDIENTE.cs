namespace GenteMarCore.Entities.Models
{
    using GenteMarCore.Entities.Helpers;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_EXPEDIENTE", Schema = "DBA")]
    public partial class GENTEMAR_EXPEDIENTE : GENTEMAR_CAMPOS_AUDITORIA
    {
        [Key]
        public int id_expediente { get; set; }
        [Required]
        public string numero_expediente { get; set; }

    }
}
