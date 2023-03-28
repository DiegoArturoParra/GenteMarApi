namespace GenteMarCore.Entities.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENTEMAR_FORMACION_GRADO", Schema = "DBA")]
    public partial class GENTEMAR_FORMACION_GRADO
    {
        [Key]
        public int id_formacion_grado { get; set; }

        public int? id_formacion { get; set; }

        public int? id_grado { get; set; }

        public virtual APLICACIONES_GRADO APLICACIONES_GRADO { get; set; }

        public virtual GENTEMAR_FORMACION GENTEMAR_FORMACION { get; set; }
    }
}
