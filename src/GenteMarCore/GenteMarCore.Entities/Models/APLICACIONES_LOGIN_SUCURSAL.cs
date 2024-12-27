namespace GenteMarCore.Entities.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("APLICACIONES_LOGIN_SUCURSAL", Schema = "DBA")]
    public partial class APLICACIONES_LOGIN_SUCURSAL
    {
        [Key]
        public int ID_LOGIN_SUCURSAL { get; set; }

        public int ID_LOGIN { get; set; }

        public int ID_SUCURSAL { get; set; }

        public int ID_APLICACION { get; set; }
    }
}
