using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.Models
{
    [Table("GENTEMAR_TITULO_FUNCION", Schema = "DBA")]
    public partial class GENTEMAR_TITULO_FUNCION
    {
        public long id_titulo { get; set; }
        public int id_funcion { get; set; }
    }
}
