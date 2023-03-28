namespace GenteMarCore.Entities.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("APLICACIONES_RANGO", Schema = "DBA")]
    public partial class APLICACIONES_RANGO
    {
        public APLICACIONES_RANGO()
        {
            APLICACIONES_GRADO = new HashSet<APLICACIONES_GRADO>();
        }

        [Key]
        public int id_rango { get; set; }

        [StringLength(20)]
        public string rango { get; set; }

        public bool activo { get; set; }

        public virtual ICollection<APLICACIONES_GRADO> APLICACIONES_GRADO { get; set; }
    }
}
