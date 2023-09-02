namespace GenteMarCore.Entities.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_EXPEDIENTE", Schema = "DBA")]
    public partial class GENTEMAR_EXPEDIENTE
    {
        [Key]
        public int id_expediente { get; set; }
        [Required]
        public string numero_expediente { get; set; }

        [Required]
        public string usuario_creador_registro { get; set; }

        public DateTime fecha_hora_creacion { get; set; }
    }
}
