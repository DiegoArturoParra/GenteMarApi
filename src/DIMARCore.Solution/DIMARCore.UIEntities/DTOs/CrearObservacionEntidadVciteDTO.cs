using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class CrearObservacionEntidadVciteDTO
    {
        [Required(ErrorMessage = "Antecedente id requerida.")]
        public long AntecedenteId { get; set; }
        public ObservacionEntidadVciteDTO ObservacionPorEntidad { get; set; }
    }
}
