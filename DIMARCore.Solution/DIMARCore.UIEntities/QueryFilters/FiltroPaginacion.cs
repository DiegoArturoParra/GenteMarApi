using DIMARCore.Utilities.Helpers;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.QueryFilters
{
    /// <summary>
    /// Clase para filtro por numero de identificacion
    /// </summary>
    public class FiltroPaginacionByIdentificacion
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




    /// <summary>
    /// 
    /// </summary>
    public class FiltroPaginacionById
    {
        /// <summary>
        /// propiedad para el id
        /// </summary>
        [Required(ErrorMessage = "Id requerido.")]
        public string Id { get; set; }

    }
}
