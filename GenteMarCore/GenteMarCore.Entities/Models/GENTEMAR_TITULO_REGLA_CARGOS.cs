using GenteMarCore.Entities.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.Models
{
    [Table("GENTEMAR_TITULO_REGLA_CARGOS", Schema = "DBA")]
    public partial class GENTEMAR_TITULO_REGLA_CARGOS : GENTEMAR_CAMPOS_AUDITORIA
    {
        [Key]
        public long id_titulo_cargo_regla { get; set; }
        public long id_titulo { get; set; }
        public int id_cargo_regla { get; set; }
        public string habilitaciones_json { get; set; }
        public string funciones_json { get; set; }
        public bool es_eliminado { get; set; }
        [NotMapped]
        public List<string> Funciones { get; set; }
    }
}
