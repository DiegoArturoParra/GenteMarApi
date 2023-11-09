namespace GenteMarCore.Entities.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_TRAMITE_ANTECEDENTE", Schema = "DBA")]
    public partial class GENTEMAR_TRAMITE_ANTECEDENTE
    {
        public GENTEMAR_TRAMITE_ANTECEDENTE()
        {
            GENTEMAR_ANTECEDENTES = new HashSet<GENTEMAR_ANTECEDENTES>();
        }

        [Key]
        public int id_tipo_tramite { get; set; }
        public bool activo { get; set; } = true;
        [StringLength(250)]
        public string descripcion_tipo_tramite { get; set; }

        public virtual ICollection<GENTEMAR_ANTECEDENTES> GENTEMAR_ANTECEDENTES { get; set; }
    }
}
