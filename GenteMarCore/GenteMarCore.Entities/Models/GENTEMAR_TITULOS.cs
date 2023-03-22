namespace GenteMarCore.Entities.Models
{
    using GenteMarCore.Entities.Helpers;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_TITULOS", Schema = "DBA")]
    public partial class GENTEMAR_TITULOS : GENTEMAR_CAMPOS_AUDITORIA
    {
        [Key]
        public long id_titulo { get; set; }

        public int id_cargo_regla { get; set; }

        public long id_gentemar { get; set; }

        public DateTime fecha_vencimiento { get; set; }

        public DateTime fecha_expedicion { get; set; }

        [StringLength(3)]
        public string cod_pais { get; set; }

        public int id_estado_tramite { get; set; }

        public int id_capitania { get; set; }

        public int id_tipo_solicitud { get; set; }

        public string radicado { get; set; }

        public int id_capitania_firmante { get; set; }

        public virtual GENTEMAR_ESTADO_TITULO APLICACIONES_ESTADO_TRAMITE { get; set; }

        public virtual APLICACIONES_TIPO_SOLICITUD APLICACIONES_TIPO_SOLICITUD { get; set; }
    }
}
