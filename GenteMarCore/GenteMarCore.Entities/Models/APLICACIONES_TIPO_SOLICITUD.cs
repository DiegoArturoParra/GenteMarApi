namespace GenteMarCore.Entities.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("APLICACIONES_TIPO_SOLICITUD", Schema = "DBA")]
    public partial class APLICACIONES_TIPO_SOLICITUD
    {
        public APLICACIONES_TIPO_SOLICITUD()
        {
            GENTEMAR_TITULOS = new HashSet<GENTEMAR_TITULOS>();
        }

        [Key]
        public int ID_TIPO_SOLICITUD { get; set; }

        [StringLength(100)]
        public string DESCRIPCION { get; set; }

        public virtual ICollection<GENTEMAR_TITULOS> GENTEMAR_TITULOS { get; set; }
    }
}
