namespace GenteMarCore.Entities.Models
{
    using GenteMarCore.Entities.Helpers;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_LICENCIAS", Schema = "DBA")]
    public partial class GENTEMAR_LICENCIAS : GENTEMAR_CAMPOS_AUDITORIA
    {
        [Key]
        public long id_licencia { get; set; }

        public long id_gentemar { get; set; }

        public long id_cargo_licencia { get; set; }

        public DateTime fecha_expedicion { get; set; }

        public DateTime fecha_vencimiento { get; set; }

        public int id_capitania { get; set; }

        public int id_estado_licencia { get; set; }

        public decimal radicado { get; set; }

        public int id_capitania_firmante { get; set; }

        public bool activo { get; set; }
    }
}
