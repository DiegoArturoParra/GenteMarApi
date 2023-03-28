namespace GenteMarCore.Entities.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("APLICACIONES_CATEGORIA", Schema = "DBA")]
    public partial class APLICACIONES_CATEGORIA
    {
        public APLICACIONES_CATEGORIA()
        {
            APLICACIONES_CAPITANIAS = new HashSet<APLICACIONES_CAPITANIAS>();
        }

        [Key]
        public int ID_CATEGORIA { get; set; }

        [StringLength(50)]
        public string DESCRIPCION { get; set; }

        [StringLength(5)]
        public string SIGLA_CATEGORIA { get; set; }

        public virtual ICollection<APLICACIONES_CAPITANIAS> APLICACIONES_CAPITANIAS { get; set; }
    }
}
