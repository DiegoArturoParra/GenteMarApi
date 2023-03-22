using System.ComponentModel.DataAnnotations;

namespace GenteMarCore.Entities.Helpers
{
    /// <summary>
    /// Datos básicos de las banderas
    /// </summary>
    public class BanderaBasic
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required]
        [Display(Name = "Id", Description = "Identificador", ShortName = "Id")]
        public string Id { get; set; }
        /// <summary>
        /// Descripción
        /// </summary>
        [Display(Name = "Nombre", Description = "Nombre", ShortName = "Nombre")]
        public string Descripcion { get; set; }
    }
}
