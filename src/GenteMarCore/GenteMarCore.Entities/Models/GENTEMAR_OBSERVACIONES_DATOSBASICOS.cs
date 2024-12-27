namespace GenteMarCore.Entities.Models
{
    using GenteMarCore.Entities.Helpers;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_OBSERVACIONES_DATOSBASICOS", Schema = "DBA")]
    public partial class GENTEMAR_OBSERVACIONES_DATOSBASICOS : GENTEMAR_OBSERVACIONES
    {
        public long id_gentemar { get; set; }
    }
}
