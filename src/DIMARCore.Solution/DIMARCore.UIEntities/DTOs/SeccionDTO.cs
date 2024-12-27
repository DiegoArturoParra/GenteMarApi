using System.Collections.Generic;

namespace DIMARCore.UIEntities.DTOs
{
    public class SeccionDTO : CamposTablasMaestrasDTO
    {
        public List<ActividadTipoLicenciaDTO> Actividad { get; set; }

        public int IdActividaSeccion { get; set; }
    }
}
