namespace GenteMarCore.Entities.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_ESTADO_TITULO", Schema = "DBA")]
    public partial class GENTEMAR_ESTADO_TITULO
    {
        [Key]
        public int id_estado_tramite { get; set; }

        [StringLength(100)]
        public string descripcion_tramite { get; set; }

        public bool activo { get; set; }
    }
}
