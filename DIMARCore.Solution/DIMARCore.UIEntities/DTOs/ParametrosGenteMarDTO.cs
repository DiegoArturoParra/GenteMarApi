using DIMARCore.Utilities.Helpers;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class ParametrosGenteMarDTO
    {
        public long Id { get; set; }
        public bool IsEstupefacientes { get; set; }
        public string Identificacion { get; set; }
        [JsonIgnore]
        public string IdentificacionConPuntos
        {
            get => Reutilizables.ConvertirStringApuntosDeMil(Identificacion);
        }
    }

    public class CedulaDTO
    {
        [Required(ErrorMessage = "El documento es requerido.")]
        public string Identificacion { get; set; }
        [JsonIgnore]
        public string IdentificacionConPuntos
        {
            get => Reutilizables.ConvertirStringApuntosDeMil(Identificacion);
        }
    }
}
