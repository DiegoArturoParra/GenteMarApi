using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.Models
{
    public partial class GENTEMAR_TITULOS
    {
        [NotMapped]
        public GENTEMAR_OBSERVACIONES_TITULOS Observacion { get; set; }
        [NotMapped]
        public List<int> HabilitacionesId { get; set; }
        [NotMapped]
        public List<int> FuncionesId { get; set; }
    }
}
