using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class IdsTablasForaneasDTO
    {

        [Required(ErrorMessage = "El id de nivel es requerido.")]
        public int NivelId { get; set; }
        [Required(ErrorMessage = "El id de regla es requerido.")]
        public int ReglaId { get; set; }
        [Required(ErrorMessage = "El id de cargo es requerido.")]
        public int CargoId { get; set; }
        [Required(ErrorMessage = "El id de cargo es requerido.")]
        public int CapacidadId { get; set; }
    }

    public class IdsLlaveCompuestaDTO
    {
        [Required(ErrorMessage = "El id de regla es requerido.")]
        public int ReglaId { get; set; }
        [Required(ErrorMessage = "El id de cargo es requerido.")]
        public int CargoTituloId { get; set; }
    }

}
