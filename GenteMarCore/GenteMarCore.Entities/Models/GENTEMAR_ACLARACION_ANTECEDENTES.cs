namespace GenteMarCore.Entities.Models
{
    using GenteMarCore.Entities.Helpers;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_ACLARACION_ANTECEDENTES", Schema = "DBA")]
    public partial class GENTEMAR_ACLARACION_ANTECEDENTES : GENTEMAR_CAMPOS_AUDITORIA
    {
        [Key]
        public long id_aclaracion { get; set; }
        [Required]
        public long id_antecedente { get; set; }
        [Required]
        public int id_entidad { get; set; }
        [Required]
        public string descripcion { get; set; }
        [Required]
        public bool verificacion_exitosa { get; set; }
        [Required]
        public string numero_expediente { get; set; }
        [Required]
        public DateTime fecha_respuesta_entidad { get; set; }
    }
}
