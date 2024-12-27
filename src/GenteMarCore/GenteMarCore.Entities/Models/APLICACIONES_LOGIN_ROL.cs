namespace GenteMarCore.Entities.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("APLICACIONES_LOGIN_ROL", Schema = "DBA")]
    public partial class APLICACIONES_LOGIN_ROL
    {
        [Key]
        public int ID_LOGIN_ROL { get; set; }

        public int ID_LOGIN { get; set; }

        public int ID_ROL { get; set; }

        public DateTime FECHA_ASIGNACION { get; set; }

        public DateTime? FECHA_MODIFICACION { get; set; }

        public byte? ID_ESTADO { get; set; }
    }
}
