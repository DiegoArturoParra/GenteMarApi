namespace GenteMarCore.Entities.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_SECCION_LICENCIAS", Schema = "DBA")]
    public partial class GENTEMAR_SECCION_LICENCIAS
    {
        [Key]
        public int id_seccion { get; set; }
        public string actividad_a_bordo { get; set; }
        public bool activo { get; set; }
    }
}
