using System.Collections.Generic;

namespace DIMARCore.UIEntities.DTOs
{
    public class UserTokenDTO
    {
        public string Token { get; set; }
        public string LoginName { get; set; }
        public string NombreCompleto { get; set; }
        public string Capitania { get; set; }
        public List<string> Roles { get; set; }
        public string Aplicacion { get; set; }
    }
}
