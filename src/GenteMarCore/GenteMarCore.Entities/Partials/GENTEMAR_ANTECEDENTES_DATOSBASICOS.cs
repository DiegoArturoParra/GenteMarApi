using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.Models
{
    public partial class GENTEMAR_ANTECEDENTES_DATOSBASICOS
    {
        [NotMapped]
        public bool IsExist { get; set; }
    }
}
