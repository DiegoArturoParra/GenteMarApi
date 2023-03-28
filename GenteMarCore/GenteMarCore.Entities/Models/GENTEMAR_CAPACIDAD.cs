namespace GenteMarCore.Entities.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENTEMAR_CAPACIDAD", Schema = "DBA")]
    public partial class GENTEMAR_CAPACIDAD
    {
        [Key]
        public int id_capacidad { get; set; }

        public string capacidad { get; set; }

        public bool activo { get; set; } = true;
    }
}
