using System.Collections.Generic;

namespace DIMARCore.UIEntities.DTOs
{
    /// <summary>
    /// Clase para definir el usuario autenticado
    /// </summary>
    public class UserSesionDTO
    {
        public int LoginId { get; set; }
        public AplicacionSessionDTO Aplicacion { get; set; }
        public int CategoriaId => Capitania != null ? Capitania.Categoria : 0;
        public List<RolSessionDTO> Roles { get; set; }
        public string NombresUsuario { get; set; }
        public string LoginName { get; set; }
        public string ApellidosUsuario { get; set; }
        public string NombreCompletoUsuario => $"{NombresUsuario} {ApellidosUsuario}";
        public string Email { get; set; }
        public CapitaniaSessionDTO Capitania { get; set; }
        public string Identificacion { get; set; }
        public int? EstadoId { get; set; }

    }
}
