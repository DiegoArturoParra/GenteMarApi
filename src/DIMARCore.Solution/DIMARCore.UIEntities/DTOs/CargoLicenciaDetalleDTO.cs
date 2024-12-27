using System.Collections.Generic;

namespace DIMARCore.UIEntities.DTOs
{
    public class CargoLicenciaDetalleDTO : CargoInfoLicenciaDTO
    {
        public List<LimitanteDTO> Limitante { get; set; }
        public List<LimitacionDTO> Limitacion { get; set; }
        public List<CategoriaDTO> Categoria { get; set; }
        public ClaseDTO Clase { get; set; }
        public SeccionDTO Seccion { get; set; }
        public ActividadTipoLicenciaDTO Actividad { get; set; }
        public TipoLicenciaDTO TipoLicencia { get; set; }
    }
}
