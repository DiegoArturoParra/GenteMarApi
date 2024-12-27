namespace GenteMarCore.Entities.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_NIVEL", Schema = "DBA")]
    public partial class GENTEMAR_NIVEL
    {
        public GENTEMAR_NIVEL()
        {
            GENTEMAR_CARGO_REGLAS = new HashSet<GENTEMAR_REGLAS_CARGO>();
        }

        [Key]
        public int id_nivel { get; set; }

        [StringLength(100)]
        public string nivel { get; set; }
        public bool activo { get; set; } = true;

        public virtual ICollection<GENTEMAR_REGLAS_CARGO> GENTEMAR_CARGO_REGLAS { get; set; }
    }
}
