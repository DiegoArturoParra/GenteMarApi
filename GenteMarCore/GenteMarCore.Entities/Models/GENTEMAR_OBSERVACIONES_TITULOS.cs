namespace GenteMarCore.Entities.Models
{
    using GenteMarCore.Entities.Helpers;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_OBSERVACIONES_TITULOS", Schema = "DBA")]
    public partial class GENTEMAR_OBSERVACIONES_TITULOS : GENTEMAR_OBSERVACIONES
    {
        public long id_titulo { get; set; }
    }
}
