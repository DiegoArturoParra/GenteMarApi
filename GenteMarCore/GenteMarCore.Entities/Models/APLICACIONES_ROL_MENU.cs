namespace GenteMarCore.Entities.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("APLICACIONES_ROL_MENU", Schema = "DBA")]
    public partial class APLICACIONES_ROL_MENU
    {
        [Key]
        public int ID_ROL_MENU { get; set; }

        public int ID_ROL { get; set; }

        public int ID_MENU { get; set; }

        public virtual APLICACIONES_MENU APLICACIONES_MENU { get; set; }
    }
}
