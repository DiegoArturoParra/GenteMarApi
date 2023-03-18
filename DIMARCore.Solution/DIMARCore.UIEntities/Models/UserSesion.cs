using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIMARCore.UIEntities.Models
{
    /// <summary>
    /// Clase para definir el usuario autenticado
    /// </summary>
    public class UserSesion
    {
        public AplicacionSession Aplicacion { get; set; }
        public int CategoriaId => Capitania != null ? Capitania.Categoria : 0;
        public List<Rol> Roles { get; set; }
        public string NombresUsuario { get; set; }
        public string LoginName { get; set; }
        public string ApellidosUsuario { get; set; }
        public string NombreCompletoUsuario => $"{NombresUsuario} {ApellidosUsuario}";
        public string Email { get; set; }
        public CapitaniaSession Capitania { get; set; }
        public string Identificacion { get; set; }
        public int? EstadoId { get; set; }

    }
    public class Rol
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
    }

    public class AplicacionSession
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}
