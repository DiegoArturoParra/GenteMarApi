namespace GenteMarCore.Entities.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("APLICACIONES_GRADO", Schema = "DBA")]
    public partial class APLICACIONES_GRADO
    {
        public APLICACIONES_GRADO()
        {
            GENTEMAR_FORMACION_GRADO = new HashSet<GENTEMAR_FORMACION_GRADO>();
        }

        [Key]
        public int id_grado { get; set; }

        [StringLength(50)]
        public string grado { get; set; }

        public int? id_rango { get; set; }

        [StringLength(6)]
        public string sigla { get; set; }
        public bool activo { get; set; }
        public virtual APLICACIONES_RANGO APLICACIONES_RANGO { get; set; }
        public virtual ICollection<GENTEMAR_FORMACION_GRADO> GENTEMAR_FORMACION_GRADO { get; set; }
    }
}
