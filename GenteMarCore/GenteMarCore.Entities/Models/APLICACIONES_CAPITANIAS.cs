namespace GenteMarCore.Entities.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("APLICACIONES_CAPITANIAS", Schema = "DBA")]
    public partial class APLICACIONES_CAPITANIAS
    {
        public APLICACIONES_CAPITANIAS()
        {
         
        }

        [Key]
        public int ID_CAPITANIA { get; set; }

        [Required]
        [StringLength(10)]
        public string SIGLA_CAPITANIA { get; set; }

        [StringLength(50)]
        public string DESCRIPCION { get; set; }

        public int ID_CATEGORIA { get; set; }

        [Required]
        [StringLength(6)]
        public string consecutivo { get; set; }

        public short ID_ESTADO { get; set; }

        public virtual APLICACIONES_CATEGORIA APLICACIONES_CATEGORIA { get; set; }
    }
}
