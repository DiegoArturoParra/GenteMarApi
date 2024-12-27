using System.Collections.Generic;

namespace DIMARCore.UIEntities.DTOs
{
    public class RegistrosActualizadosDTO
    {
        public List<DatosBasicosUsuarioDTO> UsuarioActualizado { get; set; }
        public int TotalRegistros { get; set; }
    }
}
