namespace GenteMarCore.Entities.Models
{
    using GenteMarCore.Entities.Helpers;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_OBSERVACIONES_LICENCIAS", Schema = "DBA")]
    public partial class GENTEMAR_OBSERVACIONES_LICENCIAS : GENTEMAR_OBSERVACIONES
    {
        public long id_licencia { get; set; }
    }
}
