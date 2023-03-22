using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIMARCore.UIEntities.DTOs
{
    public class RegistrosActualizadosDTO
    {
        public List<DatosBasicosUsuarioDTO> UsuarioActualizado { get; set; }
        public int TotalRegistros { get; set; }
    }
}
