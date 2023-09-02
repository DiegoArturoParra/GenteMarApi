namespace GenteMarCore.Entities.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_CARGO_LICENCIA", Schema = "DBA")]
    public partial class GENTEMAR_CARGO_LICENCIA
    {
        [Key]
        public int id_cargo_licencia { get; set; }
        public string cargo_licencia { get; set; }
        public string codigo_licencia { get; set; }
        public decimal vigencia { get; set; }
        public bool activo { get; set; }
        public int id_actividad_seccion_licencia { get; set; }
        public int id_seccion_clase { get; set; }
        public bool nave { get; set; }
    }
}
