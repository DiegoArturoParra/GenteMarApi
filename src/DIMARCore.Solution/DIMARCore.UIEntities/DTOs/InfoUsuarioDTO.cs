using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DIMARCore.UIEntities.DTOs
{
    public class InfoUsuarioDTO
    {
        public CapitaniaSessionDTO Capitania { get; set; }
        public long LoginId { get; set; }
        public string Nombres { get; set; }
        [JsonIgnore]
        public bool IsActive { get; set; }
        public string Apellidos { get; set; }
        public string LoginName { get; set; }
        public string Correo { get; set; }
        public string NombreCompleto => $"{Nombres} {Apellidos}";
        public IEnumerable<RolSessionDTO> Roles { get; set; }
        public bool IsActiveOrInactive => IsActive && (Roles.Any());
        [JsonIgnore]
        public DateTime FechaCreacion { get; set; }
        public bool IsNew => FechaCreacion.Date == DateTime.Now.Date;

    }
}
