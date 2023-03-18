namespace GenteMarCore.Entities.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_CARGO_HABILITACION", Schema = "DBA")]
    public partial class GENTEMAR_CARGO_HABILITACION
    {
        public int id_habilitacion { get; set; }

        public int id_cargo_regla { get; set; }

        public GENTEMAR_HABILITACION GENTEMAR_HABILITACIONES { get; set; }
    }
}
