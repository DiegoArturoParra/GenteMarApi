namespace GenteMarCore.Entities.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("APLICACIONES_ESTADO", Schema = "DBA")]

    public partial class APLICACIONES_ESTADO
    {
        public APLICACIONES_ESTADO()
        {
            APLICACIONES_ROLES = new HashSet<APLICACIONES_ROLES>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte ID_ESTADO { get; set; }

        [Required]
        [StringLength(30)]
        public string DESCRIPCION { get; set; }

        public virtual ICollection<APLICACIONES_ROLES> APLICACIONES_ROLES { get; set; }
    }
}
