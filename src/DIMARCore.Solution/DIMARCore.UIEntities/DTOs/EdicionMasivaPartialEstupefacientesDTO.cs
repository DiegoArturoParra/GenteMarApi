using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class EdicionMasivaPartialEstupefacientesDTO
    {
        [Required(ErrorMessage = "ids de estupefacientes requerido.")]
        public IList<long> EstupefacientesId { get; set; }
        [Required(ErrorMessage = "Estado id requerido.")]
        public int EstadoAntecedenteId { get; set; }
        [Required(ErrorMessage = "Consolidado id requerido.")]
        public int ConsolidadoId { get; set; }
        [Required(ErrorMessage = "Observación de la entidad requerida.")]
        public ObservacionesEntidadDTO ObservacionEntidad { get; set; }
    }
}
