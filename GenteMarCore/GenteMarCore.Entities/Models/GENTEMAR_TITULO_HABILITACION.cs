using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.Models
{
    [Table("GENTEMAR_TITULO_HABILITACION", Schema = "DBA")]
    public partial class GENTEMAR_TITULO_HABILITACION
    {
        public long id_titulo { get; set; }
        public int id_habilitacion { get; set; }
    }
}
