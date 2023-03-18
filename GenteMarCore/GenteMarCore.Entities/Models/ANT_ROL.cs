namespace GenteMarCore.Entities.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ANT_ROL", Schema = "DBA")]
    public partial class ANT_ROL
    {
        [Key]
        public int ID_ROL { get; set; }

        [Required]
        [StringLength(75)]
        public string ROL { get; set; }

        public int ID_ESTADO { get; set; }
    }
}
