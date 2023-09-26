using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.Models
{
    public partial class GENTEMAR_ANTECEDENTES
    {
        [NotMapped]
        public GENTEMAR_OBSERVACIONES_ANTECEDENTES Observacion { get; set; }
        [NotMapped]
        public GENTEMAR_ANTECEDENTES_DATOSBASICOS DatosBasicos { get; set; }
    }
}
