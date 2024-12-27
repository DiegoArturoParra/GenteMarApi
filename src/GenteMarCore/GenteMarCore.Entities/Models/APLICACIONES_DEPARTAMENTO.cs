namespace GenteMarCore.Entities.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("APLICACIONES_DEPARTAMENTO", Schema = "DBA")]
    public partial class APLICACIONES_DEPARTAMENTO
    {
        [Key]
        public int ID_DEPARTAMENTO { get; set; }

        public int CODIGO_DEPARTAMENTO { get; set; }

        [Required]
        [StringLength(30)]
        public string NOMBRE_DEPARTAMENTO { get; set; }
    }
}
