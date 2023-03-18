using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIMARCore.UIEntities.DTOs
{
    public class DatosBasicosUsuarioDTO
    {
        public long Id { get; set; }
        public string DocumentoIdentificacion { get; set; }
        public string NombreEstado { get; set; }
        public string NombreCompleto { get; set; }
        public long IdLicencia { get; set; }
    }
}
