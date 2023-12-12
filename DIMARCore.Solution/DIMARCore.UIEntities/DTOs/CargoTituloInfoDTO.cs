using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class CargoTituloInfoDTO : CamposTablasMaestrasDTO
    {
        public int ClaseId { get; set; }
        public int SeccionId { get; set; }
    }

    public class CargoTituloDTO : CamposTablasMaestrasDTO
    {

    }

    public class CreatedUpdateCargoTituloDTO
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "El campo Descripción es requerido.")]
        [MaxLength(500, ErrorMessage = "El campo Descripción debe tener máximo 500 caracteres.")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "Clase id es requerido.")]
        public int ClaseId { get; set; }
        [Required(ErrorMessage = "Sección id es requerido.")]
        public int SeccionId { get; set; }
    }

    public class ListadoCargoTituloDTO : CargoTituloInfoDTO
    {
        public string Seccion { get; set; }
        public string Clase { get; set; }
    }
}
