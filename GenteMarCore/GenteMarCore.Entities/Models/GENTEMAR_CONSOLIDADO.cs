namespace GenteMarCore.Entities.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_CONSOLIDADO", Schema = "DBA")]
    public partial class GENTEMAR_CONSOLIDADO
    {
        [Key]
        public int id_consolidado { get; set; }
        [Required]
        public string numero_consolidado { get; set; }

        [Required]
        public string usuario_creador_registro { get; set; }

        public DateTime fecha_hora_creacion { get; set; }
    }
}
