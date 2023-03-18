namespace GenteMarCore.Entities.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_TERRITORIO", Schema = "DBA")]
    public partial class GENTEMAR_TERRITORIO
    {
        [Key]
        public int id_territorio { get; set; }
        public string territorio { get; set; }
        public bool? activo { get; set; }
    }
}
