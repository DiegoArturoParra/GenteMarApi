namespace GenteMarCore.Entities.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_FUNCION", Schema = "DBA")]
    public partial class GENTEMAR_FUNCIONES
    {
        [Key]
        public int id_funcion { get; set; }

        public string funcion { get; set; }

        public string limitacion_funcion { get; set; }

        public bool activo { get; set; } = true;
    }
}
