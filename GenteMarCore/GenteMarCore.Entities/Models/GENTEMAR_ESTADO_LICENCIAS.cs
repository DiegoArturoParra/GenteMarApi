namespace GenteMarCore.Entities.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENTEMAR_ESTADO_LICENCIAS", Schema = "DBA")]
    public partial class GENTEMAR_ESTADO_LICENCIAS
    {
        [Key]
        public int id_estado_licencias { get; set; }

        [StringLength(20)]
        public string descripcion_estado { get; set; }
    }
}
