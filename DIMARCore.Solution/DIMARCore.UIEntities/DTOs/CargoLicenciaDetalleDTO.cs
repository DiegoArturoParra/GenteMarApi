using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIMARCore.UIEntities.DTOs
{
    public class CargoLicenciaDetalleDTO : CargoLicenciaDTO
    {
        public List<LimitanteDTO> Limitante { get; set; }
        public List<LimitacionDTO> Limitacion { get; set; }
        public List<CategoriaDTO> Categoria { get; set; }
        public ClaseDTO Clase { get; set; }
        public SeccionDTO Seccion { get; set; }
        public ActividadDTO Actividad { get; set; }
        public TipoLicenciaDTO TipoLicencia { get; set; }
    }
}
