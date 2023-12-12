namespace GenteMarCore.Entities.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_CAPACIDAD", Schema = "DBA")]
    public partial class GENTEMAR_CAPACIDAD
    {
        [Key]
        public int id_capacidad { get; set; }

        public string capacidad { get; set; }

        public bool activo { get; set; } = true;
    }
}
