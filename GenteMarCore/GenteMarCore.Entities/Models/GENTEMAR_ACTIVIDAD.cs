namespace GenteMarCore.Entities.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_ACTIVIDAD", Schema = "DBA")]
    public partial class GENTEMAR_ACTIVIDAD
    {
        [Key]
        public int id_actividad { get; set; }
        [StringLength(200)]
        public string actividad { get; set; }
        public bool activo { get; set; }
        public int id_tipo_licencia { get; set; }
        public virtual GENTEMAR_TIPO_LICENCIA GENTEMAR_TIPO_LICENCIA { get; set; }
    }
}
