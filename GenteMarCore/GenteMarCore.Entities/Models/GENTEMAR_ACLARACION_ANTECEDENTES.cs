namespace GenteMarCore.Entities.Models
{
    using GenteMarCore.Entities.Helpers;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("DBA.GENTEMAR_ACLARACION_ANTECEDENTES")]
    public partial class GENTEMAR_ACLARACION_ANTECEDENTES : GENTEMAR_CAMPOS_AUDITORIA
    {
        [Key]
        public long id_aclaracion { get; set; }

        public long id_antecedente { get; set; }

        public int id_entidad { get; set; }

        [Required]
        public string descripcion { get; set; }
        [Required]
        public DateTime fecha_respuesta_entidad { get; set; }
    }
}
