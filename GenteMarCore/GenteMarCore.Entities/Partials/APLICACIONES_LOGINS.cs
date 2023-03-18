namespace GenteMarCore.Entities.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class APLICACIONES_LOGINS
    {
        [NotMapped]
        public string NombreCompleto
        {
            get
            {
                var nombres = !string.IsNullOrEmpty(this.NOMBRES) ? this.NOMBRES : string.Empty;
                var apellidos = !string.IsNullOrEmpty(this.APELLIDOS) ? this.APELLIDOS : string.Empty;
                return nombres + " " + apellidos;
            }
        }
    }
}
