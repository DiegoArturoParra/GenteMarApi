using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class CapitaniaDTO
    {
        public int Id { get; set; }
        public string Sigla { get; set; }
        [Required(ErrorMessage = "Descripcion Requerida.")]
        public string Descripcion { get; set; }
        public string DescripcionCompleta => $"{Sigla} - {Descripcion}";
    }
}
