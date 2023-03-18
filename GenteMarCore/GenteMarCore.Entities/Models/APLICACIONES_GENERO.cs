namespace GenteMarCore.Entities.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("APLICACIONES_GENERO", Schema = "DBA")]
    public partial class APLICACIONES_GENERO
    {
        [Key]
        public int ID_GENERO { get; set; }

        [StringLength(100)]
        public string DESCRIPCION { get; set; }
    }
}
