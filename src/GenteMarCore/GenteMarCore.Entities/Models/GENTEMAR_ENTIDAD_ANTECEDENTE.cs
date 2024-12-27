namespace GenteMarCore.Entities.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_ENTIDAD_ANTECEDENTE", Schema = "DBA")]
    public partial class GENTEMAR_ENTIDAD_ANTECEDENTE
    {
        [Key]
        public int id_entidad { get; set; }

        [StringLength(200)]
        public string entidad { get; set; }

        public bool activo { get; set; }
    }
}
