using System.ComponentModel.DataAnnotations;

namespace GenteMarCore.Entities.Helpers
{
    public partial class GENTEMAR_CLASE
    {
        [Key]
        public int id_clase { get; set; }
        [StringLength(100)]
        public string descripcion_clase { get; set; }
        [StringLength(2)]
        public string sigla { get; set; }
        public bool activo { get; set; } = true;
    }
}
