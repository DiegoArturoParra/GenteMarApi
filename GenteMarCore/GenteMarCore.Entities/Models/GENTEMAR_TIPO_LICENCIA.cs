namespace GenteMarCore.Entities.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_TIPO_LICENCIA", Schema = "DBA")]
    public partial class GENTEMAR_TIPO_LICENCIA
    {
        [Key]
        public int id_tipo_licencia { get; set; }
        public string tipo_licencia { get; set; }
        public bool? activo { get; set; }
    }
}
