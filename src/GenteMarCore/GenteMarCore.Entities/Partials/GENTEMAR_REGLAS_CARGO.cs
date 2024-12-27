using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.Models
{
    public partial class GENTEMAR_REGLAS_CARGO
    {
        [NotMapped]
        public List<int> Habilitaciones { get; set; }
    }
}
