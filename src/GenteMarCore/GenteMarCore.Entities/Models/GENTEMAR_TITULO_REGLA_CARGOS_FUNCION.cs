using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.Models
{
    [Table("GENTEMAR_TITULO_REGLA_CARGOS_FUNCION", Schema = "DBA")]
    public partial class GENTEMAR_TITULO_REGLA_CARGOS_FUNCION
    {
        public long id_titulo_cargo_regla { get; set; }
        public long id_funcion { get; set; }
    }
}
