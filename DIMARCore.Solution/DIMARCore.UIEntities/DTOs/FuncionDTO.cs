using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class FuncionDTO : CamposTablasMaestrasDTO
    {
        [Required(ErrorMessage = "Limitación requeridas")]
        public string Limitacion { get; set; }
    }
}
