namespace GenteMarCore.Entities.Models
{
    using GenteMarCore.Entities.Helpers;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES", Schema = "DBA")]
    public partial class GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES : GENTEMAR_CAMPOS_AUDITORIA
    {
        [Key]
        public long id_expediente_observacion { get; set; }
        [Required]
        public long id_antecedente { get; set; }
        [Required]
        public int id_entidad { get; set; }
        [Required]
        public int id_expediente { get; set; }
        [Required]
        public int id_consolidado { get; set; }
        public string descripcion { get; set; }
        public bool? verificacion_exitosa { get; set; }
        public DateTime? fecha_respuesta_entidad { get; set; }
    }
}
