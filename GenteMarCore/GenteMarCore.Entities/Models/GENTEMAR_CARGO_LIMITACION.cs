namespace GenteMarCore.Entities.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENTEMAR_CARGO_LIMITACION", Schema = "DBA")]
    public partial class GENTEMAR_CARGO_LIMITACION
    {
        [Key]
        public int id_cargo_limitacion { get; set; }

        public int? id_limitacion { get; set; }

        public int? id_cargo_licencia { get; set; }
    }
}
