using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.Models
{
    [Table("DimImpresion", Schema = "DBA")]
    public partial class DIM_IMPRESION
    {
        [Key]
        public decimal idimpresion { get; set; }
        public int? idusuario { get; set; }
        public string cedula { get; set; }
        public bool impreso { get; set; }
        public DateTime? fechaimpreso { get; set; }
        public int? etiqueta { get; set; }
        public int? libreta { get; set; }
        public int? idPersona { get; set; }
        public decimal? idDocumentos { get; set; }
    }
}
