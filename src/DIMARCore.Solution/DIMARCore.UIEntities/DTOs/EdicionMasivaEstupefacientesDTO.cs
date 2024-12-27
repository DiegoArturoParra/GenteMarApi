using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class EdicionMasivaEstupefacientesDTO
    {
        [Required(ErrorMessage = "ids requeridos")]
        public IList<long> EstupefacientesId { get; set; }
        [Required(ErrorMessage = "Estado id requerida.")]
        public int EstadoAntecedenteId { get; set; }
        [Required(ErrorMessage = "Consolidado id requerido.")]
        public int ConsolidadoId { get; set; }
        public IList<ObservacionesEntidadDTO> ObservacionesPorEntidad { get; set; }
    }
}
