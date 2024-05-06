using GenteMarCore.Entities.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.Models
{
    public partial class GENTEMAR_CARGO_LICENCIA : GENTEMAR_CAMPOS_AUDITORIA
    {
        [NotMapped]
        public int IdTipoLicencia { get; set; }
        [NotMapped]
        public int IdActividad { get; set; }
        [NotMapped]
        public int IdSeccion { get; set; }
        [NotMapped]
        public int IdClase { get; set; }
        public List<int> IdCategoria { get; set; }
        public List<int> IdLimitacion { get; set; }
        public List<int> IdLimitante { get; set; }
    }
}
