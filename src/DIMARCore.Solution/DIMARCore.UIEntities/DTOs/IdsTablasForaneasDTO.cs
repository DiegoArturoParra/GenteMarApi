using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class IdsTablasForaneasDTO
    {
        public IdsTablasForaneasDTO()
        {

        }
        public IdsTablasForaneasDTO(int nivelId, int reglaId, int cargoId, int capacidadId)
        {
            NivelId = nivelId;
            ReglaId = reglaId;
            CargoId = cargoId;
            CapacidadId = capacidadId;
        }

        [Required(ErrorMessage = "El id de nivel es requerido.")]
        public int NivelId { get; set; }
        [Required(ErrorMessage = "El id de regla es requerido.")]
        public int ReglaId { get; set; }
        [Required(ErrorMessage = "El id de cargo es requerido.")]
        public int CargoId { get; set; }
        [Required(ErrorMessage = "El id de capacidad es requerido.")]
        public int CapacidadId { get; set; }
    }
}
