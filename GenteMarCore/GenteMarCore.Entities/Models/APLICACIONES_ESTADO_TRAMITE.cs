namespace GenteMarCore.Entities.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("APLICACIONES_ESTADO_TRAMITE", Schema = "DBA")]
    public partial class APLICACIONES_ESTADO_TRAMITE
    {
        public APLICACIONES_ESTADO_TRAMITE()
        {
            GENTEMAR_TITULOS = new HashSet<GENTEMAR_TITULOS>();
        }

        [Key]
        public int ID_ESTADO_TRAMITE { get; set; }

        [StringLength(100)]
        public string DESCRIPCION_TRAMITE { get; set; }

        public virtual ICollection<GENTEMAR_TITULOS> GENTEMAR_TITULOS { get; set; }
    }
}
