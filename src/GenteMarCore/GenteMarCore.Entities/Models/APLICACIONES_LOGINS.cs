namespace GenteMarCore.Entities.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("APLICACIONES_LOGINS", Schema = "DBA")]
    public partial class APLICACIONES_LOGINS
    {
        [Key]
        public int ID_LOGIN { get; set; }

        [Required]
        [StringLength(100)]
        public string NOMBRES { get; set; }

        [Required]
        [StringLength(100)]
        public string APELLIDOS { get; set; }

        [Required]
        [StringLength(50)]
        public string LOGIN_NAME { get; set; }

        [Required]
        [StringLength(250)]
        public string PASSWORD { get; set; }

        [Required]
        [StringLength(50)]
        public string CORREO { get; set; }

        public DateTime FECHA_CREACION { get; set; }

        public DateTime FECHA_MODIFICACION { get; set; }

        public byte NUMERO_INTENTOS { get; set; }

        public byte ID_ESTADO { get; set; }

        public int ID_UNIDAD { get; set; }

        public byte ID_TIPO_ESTADO { get; set; }

        public int ID_USUARIO_REGISTRO { get; set; }

        public int? ID_CAPITANIA { get; set; }
    }
}
