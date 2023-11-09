using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.Models
{
    [Table("GENTEMAR_TITULO_REGLA_CARGOS_HABILITACION", Schema = "DBA")]
    public partial class GENTEMAR_TITULO_REGLA_CARGOS_HABILITACION
    {
        public long id_titulo_cargo_regla { get; set; }
        public long id_habilitacion { get; set; }
    }
}
