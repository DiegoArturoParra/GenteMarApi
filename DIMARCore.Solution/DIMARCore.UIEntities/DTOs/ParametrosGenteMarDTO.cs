using DIMARCore.Utilities.Helpers;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class ParametrosGenteMarDTO
    {
        public long Id { get; set; }
        public bool IsModuleEstupefacientes { get; set; }
        public string Identificacion { get; set; }
    }

    public class CedulaDTO
    {
        [Required(ErrorMessage = "El documento es requerido.")]
        public string Identificacion { get; set; }
    }
}
