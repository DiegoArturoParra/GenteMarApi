using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.Models
{
    public partial class GENTEMAR_ANTECEDENTES
    {
        [NotMapped]
        public GENTEMAR_OBSERVACIONES_ANTECEDENTES Observacion { get; set; }
        [NotMapped]
        public List<GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES> Expedientes { get; set; }
        [NotMapped]
        public GENTEMAR_HISTORIAL_ACLARACION_ANTECEDENTES HistorialAClaracion { get; set; }
    }
}
