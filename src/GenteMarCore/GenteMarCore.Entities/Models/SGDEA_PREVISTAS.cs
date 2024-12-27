using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.Models
{
    [Table("SGDEA_PREVISTAS", Schema = "DBA")]
    public partial class SGDEA_PREVISTAS
    {
        [Key]
        public long id_sgdea_prevista { get; set; }
        [Column(TypeName = "numeric")]
        public decimal radicado { get; set; }

        [StringLength(20)]
        public string expediente { get; set; }

        [StringLength(200)]
        public string ruta_prevista { get; set; }

        [StringLength(50)]
        public string estado { get; set; }

        public DateTime fecha_estado { get; set; }

        [StringLength(50)]
        public string sigla_capitania_entrega { get; set; }

        [StringLength(15)]
        public string numero_identificacion_usuario { get; set; }

        [StringLength(20)]
        public string tipo_documento_usuario { get; set; }

        [StringLength(15)]
        public string numero_identificacion_firmante { get; set; }

        [StringLength(200)]
        public string tipo_tramite { get; set; }
    }
}
