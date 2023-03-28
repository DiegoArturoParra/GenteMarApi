namespace GenteMarCore.Entities.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENTEMAR_ESTADO_ANTECEDENTES", Schema = "DBA")]
    public partial class GENTEMAR_ESTADO_ANTECEDENTES
    {
        public GENTEMAR_ESTADO_ANTECEDENTES()
        {
            GENTEMAR_ANTECEDENTES = new HashSet<GENTEMAR_ANTECEDENTES>();
        }

        [Key]
        public int id_estado_antecedente { get; set; }

        [StringLength(50)]
        public string descripcion_estado_antecedente { get; set; }

        public virtual ICollection<GENTEMAR_ANTECEDENTES> GENTEMAR_ANTECEDENTES { get; set; }
    }
}
