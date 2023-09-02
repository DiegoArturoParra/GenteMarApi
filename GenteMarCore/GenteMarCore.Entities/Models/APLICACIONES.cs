namespace GenteMarCore.Entities.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("APLICACIONES", Schema = "DBA")]
    public partial class APLICACIONES
    {
        [Key]
        public int ID_APLICACION { get; set; }

        [Required]
        [StringLength(150)]
        public string NOMBRE { get; set; }

        [Required]
        [StringLength(20)]
        public string VERSION { get; set; }

        public DateTime FECHA_ACTUALIZACION { get; set; }

        public int ID_TIPO_AUTENTICACION { get; set; }

        public byte ID_ESTADO { get; set; }

        public string LLAVE_APLICACION { get; set; }
    }
}
