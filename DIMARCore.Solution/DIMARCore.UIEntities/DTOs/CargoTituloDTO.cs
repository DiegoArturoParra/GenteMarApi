using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class CargoTituloDTO : CamposTablasMaestrasDTO
    {
        public int ClaseId { get; set; }
        public int SeccionId { get; set; }
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

    public class ListadoCargoTituloDTO : CargoTituloDTO
    {
        public string Seccion { get; set; }
        public string Clase { get; set; }
    }
}
