using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class TipoSolicitudDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Descripcion Requerida.")]
        public string Descripcion { get; set; }
    }
}
