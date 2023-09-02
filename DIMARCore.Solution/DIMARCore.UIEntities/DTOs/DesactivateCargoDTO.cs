using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class DesactivateCargoDTO
    {
        [Required(ErrorMessage = "titulo id requerido.")]
        public long TituloId { get; set; }
        [Required(ErrorMessage = "cargo regla id requerido.")]
        public int ReglaCargoId { get; set; }
        [Required(ErrorMessage = "cargo tituloReglaCargo requerido.")]
        public long TituloReglaCargoId { get; set; }
    }
}
