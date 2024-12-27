namespace GenteMarCore.Entities.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_CARGO_TITULO", Schema = "DBA")]
    public partial class GENTEMAR_CARGO_TITULO
    {
        public GENTEMAR_CARGO_TITULO()
        {
            GENTEMAR_CARGO_REGLAS = new HashSet<GENTEMAR_REGLAS_CARGO>();
        }

        [Key]
        public int id_cargo_titulo { get; set; }
        [MaxLength(500, ErrorMessage = "El campo Descripción debe tener máximo 500 caracteres")]
        public string cargo { get; set; }
        public int id_clase { get; set; }
        public int id_seccion { get; set; }
        public bool activo { get; set; } = true;
        public virtual ICollection<GENTEMAR_REGLAS_CARGO> GENTEMAR_CARGO_REGLAS { get; set; }
    }
}
