namespace GenteMarCore.Entities.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("APLICACIONES_ROLES", Schema = "DBA")]
    public partial class APLICACIONES_ROLES
    {
        [Key]
        public int ID_ROL { get; set; }

        [Required]
        [StringLength(100)]
        public string ROL { get; set; }

        [Required]
        [StringLength(500)]
        public string DESCRIPCION { get; set; }

        public DateTime FECHA_CREACION { get; set; }

        public int ID_APLICACION { get; set; }

        public byte ID_ESTADO { get; set; }

        public virtual APLICACIONES_ESTADO APLICACIONES_ESTADO { get; set; }
    }
}
