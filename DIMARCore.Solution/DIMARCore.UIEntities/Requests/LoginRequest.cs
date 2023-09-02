using System.ComponentModel.DataAnnotations;


namespace DIMARCore.UIEntities.Requests
{
    /// <summary>
    /// Login data
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Nombre de usuario / Identificador
        /// </summary>
        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        [MaxLength(50, ErrorMessage = "La longitud del nombre de usuario no es valida.")]
        public string UserName { get; set; }
        /// <summary>
        /// Contraseña
        /// </summary>
        [Required(ErrorMessage = "La contraseña es un dato requerido.")]
        public string Password { get; set; }
        /// <summary>
        /// Id de la aplicación
        /// </summary>
        [Required(ErrorMessage = "El tipo la aplicación es un dato requerido.")]
        public string Aplicacion { get; set; }

    }
}