namespace GenteMarCore.Entities.Models
{
    using GenteMarCore.Entities.Helpers;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_ANTECEDENTES", Schema = "DBA")]
    public partial class GENTEMAR_ANTECEDENTES : GENTEMAR_CAMPOS_AUDITORIA
    {
        [Key]
        public long id_antecedente { get; set; }

        public long id_gentemar_antecedente { get; set; }

        public string numero_sgdea { get; set; }

        public DateTime? fecha_sgdea { get; set; }

        public int id_capitania { get; set; }

        public int id_estado_antecedente { get; set; }

        public int id_tipo_tramite { get; set; }

        public DateTime fecha_solicitud_sede_central { get; set; }

        public DateTime? fecha_aprobacion { get; set; }

        public DateTime? fecha_vigencia { get; set; }

        public virtual APLICACIONES_CAPITANIAS APLICACIONES_CAPITANIAS { get; set; }

        public virtual GENTEMAR_ESTADO_ANTECEDENTE GENTEMAR_ESTADO_ANTECEDENTES { get; set; }

        public virtual GENTEMAR_TRAMITE_ANTECEDENTE GENTEMAR_TIPO_TRAMITE { get; set; }
    }
}
