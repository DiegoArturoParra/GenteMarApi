namespace GenteMarCore.Entities.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("APLICACIONES_GENERO", Schema = "DBA")]
    public partial class APLICACIONES_GENERO
    {
        [Key]
        public int ID_GENERO { get; set; }

        [StringLength(100)]
        public string DESCRIPCION { get; set; }
    }
}
