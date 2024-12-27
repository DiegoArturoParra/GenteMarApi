using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class ObservacionesEntidadVciteEdicionMasivaDTO
    {
        [Required(ErrorMessage = "Antecedente id requerida.")]
        public long AntecedenteId { get; set; }
        public IList<ObservacionEntidadVciteDTO> ObservacionesPorEntidad { get; set; }
    }
}
