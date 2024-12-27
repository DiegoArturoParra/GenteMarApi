using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class IdsLlaveCompuestaDTO
    {
        [Required(ErrorMessage = "El id de regla es requerido.")]
        public int ReglaId { get; set; }
        [Required(ErrorMessage = "El id de cargo es requerido.")]
        public int CargoTituloId { get; set; }
        [Required(ErrorMessage = "El activo es requerido.")]
        public bool IsActive { get; set; }
    }
}
