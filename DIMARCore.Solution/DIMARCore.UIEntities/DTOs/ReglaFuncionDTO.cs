using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class ReglaFuncionDTO
    {
        [Required(ErrorMessage = "Id regla requerido.")]
        public int ReglaId { get; set; }
        [Required(ErrorMessage = "Se requiere una función.")]
        public List<int> Funciones { get; set; }
    }
}
