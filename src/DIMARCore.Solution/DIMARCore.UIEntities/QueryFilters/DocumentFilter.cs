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
        [StringLength(22, ErrorMessage = "Debe tener una longitud mínima de {2} y una longitud máxima de {1}.", MinimumLength = 4)]
        public string Identificacion { get; set; }
    }


}
