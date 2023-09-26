using DIMARCore.Utilities.Helpers;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.QueryFilters
{
    /// <summary>
    /// Clase para filtrar titulos por numero de identificación
    /// </summary>
    public class DocumentFilter
    {

        /// <summary>
        /// propiedad para la identificacion
        /// </summary>
        [Required(ErrorMessage = "Identificación requerida.")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        [StringLength(16, ErrorMessage = "Debe tener una longitud mínima de {2} y una longitud máxima de {1}.", MinimumLength = 4)]
        public string Identificacion { get; set; }
        /// <summary>
        /// propiedad que devuelve la identificacion con puntos
        /// </summary>
        [JsonIgnore]
        public string IdentificacionConPuntos
        {
            get => Reutilizables.ConvertirStringApuntosDeMil(Identificacion);
        }
    }

  
}
