using System.ComponentModel.DataAnnotations;

namespace GenteMarCore.Entities.Helpers
{
    public class GENTEMAR_SECCION
    {
        [Key]
        public int id_seccion { get; set; }
        [StringLength(200)]
        public string actividad_a_bordo { get; set; }
        public bool activo { get; set; }
    }
}
