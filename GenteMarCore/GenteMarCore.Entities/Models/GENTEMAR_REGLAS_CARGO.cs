namespace GenteMarCore.Entities.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_REGLA_CARGO", Schema = "DBA")]
    public partial class GENTEMAR_REGLAS_CARGO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_cargo_regla { get; set; }

        public int id_regla { get; set; }

        public int id_cargo_titulo { get; set; }

        public int id_nivel { get; set; }

        public int id_capacidad { get; set; }

        public virtual GENTEMAR_CARGO_TITULO GENTEMAR_CARGO_TITULO { get; set; }

        public virtual GENTEMAR_NIVEL GENTEMAR_NIVEL { get; set; }

        public virtual GENTEMAR_REGLAS GENTEMAR_REGLAS { get; set; }

        public virtual GENTEMAR_CAPACIDAD GENTEMAR_CAPACIDAD { get; set; }
    }
}
