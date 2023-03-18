using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenteMarCore.Entities.Models
{
    [Table("DimImpresion", Schema = "DBA")]
    public partial class DIM_IMPRESION
    {
        [Key]
        public decimal idimpresion { get; set; }
        public Nullable<int> idusuario { get; set; }
        public string cedula { get; set; }
        public bool impreso { get; set; }
        public Nullable<System.DateTime> fechaimpreso { get; set; }
        public Nullable<int> etiqueta { get; set; }
        public Nullable<int> libreta { get; set; }
        public Nullable<int> idPersona { get; set; }
        public Nullable<decimal> idDocumentos { get; set; }
    }
}
