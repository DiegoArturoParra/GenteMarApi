namespace GenteMarCore.Entities.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("APLICACIONES_MUNICIPIO", Schema = "DBA")]
    public partial class APLICACIONES_MUNICIPIO
    {
        [Key]
        public int ID_MUNICIPIO { get; set; }
        public int CODIGO_MUNICIPIO { get; set; }
        public string NOMBRE_MUNICIPIO { get; set; }
        public int ID_DEPARTAMENTO { get; set; }
    }
}
