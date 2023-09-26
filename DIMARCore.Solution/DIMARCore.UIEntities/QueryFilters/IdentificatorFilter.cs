using System.ComponentModel.DataAnnotations;
namespace DIMARCore.UIEntities.QueryFilters
{
    /// <summary>
    /// Clase para filtrar por identificador id
    /// </summary>
    public class IdentificatorFilter
    {
        /// <summary>
        /// propiedad para el id
        /// </summary>
        [Required(ErrorMessage = "Id requerido.")]
        public string Id { get; set; }
    }
}
