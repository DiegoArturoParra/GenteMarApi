namespace GenteMarCore.Entities.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_REGLA", Schema = "DBA")]
    public partial class GENTEMAR_REGLAS
    {
        public GENTEMAR_REGLAS()
        {
            GENTEMAR_CARGO_REGLAS = new HashSet<GENTEMAR_REGLAS_CARGO>();
        }

        [Key]
        public int id_regla { get; set; }

        [StringLength(20)]
        public string Regla { get; set; }

        public bool activo { get; set; } = true;

        public virtual ICollection<GENTEMAR_REGLAS_CARGO> GENTEMAR_CARGO_REGLAS { get; set; }

    }

}
