namespace GenteMarCore.Entities.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_FORMACION", Schema = "DBA")]
    public partial class GENTEMAR_FORMACION
    {
        public GENTEMAR_FORMACION()
        {
            GENTEMAR_FORMACION_GRADO = new HashSet<GENTEMAR_FORMACION_GRADO>();
        }

        [Key]
        public int id_formacion { get; set; }

        [StringLength(50)]
        public string formacion { get; set; }

        public bool activo { get; set; }

        public virtual ICollection<GENTEMAR_FORMACION_GRADO> GENTEMAR_FORMACION_GRADO { get; set; }
    }
}
