namespace GenteMarCore.Entities.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("APLICACIONES_MENU", Schema = "DBA")]
    public partial class APLICACIONES_MENU
    {
        public APLICACIONES_MENU()
        {
            APLICACIONES_ROL_MENU = new HashSet<APLICACIONES_ROL_MENU>();
        }

        [Key]
        public int ID_MENU { get; set; }

        [Required]
        [StringLength(50)]
        public string VISTA { get; set; }

        [Required]
        [StringLength(50)]
        public string CONTROLADOR { get; set; }

        [Required]
        [StringLength(50)]
        public string NOMBRE { get; set; }

        public int ID_PADRE { get; set; }

        public int ID_APLICACION { get; set; }

        public virtual ICollection<APLICACIONES_ROL_MENU> APLICACIONES_ROL_MENU { get; set; }
    }
}
