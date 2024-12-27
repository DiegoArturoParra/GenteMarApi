using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class UsuariogdmDTO
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

}
