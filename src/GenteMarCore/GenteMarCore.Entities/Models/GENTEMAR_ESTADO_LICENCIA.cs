namespace GenteMarCore.Entities.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_ESTADO_LICENCIA", Schema = "DBA")]
    public partial class GENTEMAR_ESTADO_LICENCIA
    {
        [Key]
        public int id_estado_licencias { get; set; }

        [StringLength(100)]
        public string descripcion_estado { get; set; }

        public bool activo { get; set; }
    }
}
