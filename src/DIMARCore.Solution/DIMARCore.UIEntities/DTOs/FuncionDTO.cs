using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class FuncionDTO : CamposTablasMaestrasDTO
    {
        [Required(ErrorMessage = "Limitación requerida.")]
        public string Limitacion { get; set; }
    }
    public class FuncionCargoDTO
    {
        public int FuncionId { get; set; }
        public string Descripcion { get; set; }
        public string Limitacion { get; set; }

    }
}
