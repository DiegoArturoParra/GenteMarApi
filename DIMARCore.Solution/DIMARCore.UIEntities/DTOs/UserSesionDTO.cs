using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    /// <summary>
    /// Clase para definir el usuario autenticado
    /// </summary>
    public class UserSesionDTO
    {
        public int LoginId { get; set; }
        public AplicacionSession Aplicacion { get; set; }
        public int CategoriaId => Capitania != null ? Capitania.Categoria : 0;
        public List<RolSession> Roles { get; set; }
        public string NombresUsuario { get; set; }
        public string LoginName { get; set; }
        public string ApellidosUsuario { get; set; }
        public string NombreCompletoUsuario => $"{NombresUsuario} {ApellidosUsuario}";
        public string Email { get; set; }
        public CapitaniaSession Capitania { get; set; }
        public string Identificacion { get; set; }
        public int? EstadoId { get; set; }

    }
    public class UsuarioGDMDTO
    {
        public long? LoginId { get; set; }
        [Required(ErrorMessage = "El nombre es requerido.")]
        [StringLength(100, ErrorMessage = "Debe tener una longitud máxima de {1} caracteres.")]
        public string Nombres { get; set; }
        [Required(ErrorMessage = "El apellido es requerido.")]
        [StringLength(100, ErrorMessage = "Debe tener una longitud máxima de {1} caracteres.")]
        public string Apellidos { get; set; }
        [Required(ErrorMessage = "El user name es requerido.")]
        [StringLength(50, ErrorMessage = "Debe tener una longitud máxima de {1} caracteres.")]
        public string LoginName { get; set; }
        [Required(ErrorMessage = "El correo es requerido.")]
        [StringLength(50, ErrorMessage = "Debe tener una longitud máxima de {1} caracteres.")]
        public string Correo { get; set; }
        [Required(ErrorMessage = "Se requiere por lo menos un rol.")]
        public List<int> RolesId { get; set; }
        [Required(ErrorMessage = "la capitania es requerido.")]
        public int CapitaniaId { get; set; }
    }

    public class RolSession
    {
        public int Id { get; set; }
        public string NombreRol { get; set; }
        public int EstadoId { get; set; }

    }
    public class CapitaniaSession
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string Sigla { get; set; }
        public int Categoria { get; set; }
        public string DescripcionCompleta => $"{Descripcion} - {Sigla}";
    }

    public class AplicacionSession
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}
