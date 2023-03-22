namespace GenteMarCore.Entities.Models
{
    using GenteMarCore.Entities.Helpers;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_OBSERVACIONES_ANTECEDENTES", Schema = "DBA")]
    public partial class GENTEMAR_OBSERVACIONES_ANTECEDENTES : GENTEMAR_OBSERVACIONES
    {
        public long id_antecedente { get; set; }
    }
}
