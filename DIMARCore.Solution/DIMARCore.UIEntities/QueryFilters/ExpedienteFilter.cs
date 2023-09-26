using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.QueryFilters
{
    public class ExpedienteFilter
    {
        [Required(ErrorMessage = "Consolidado id requerido.")]
        public int ConsolidadoId { get; set; }
        [Required(ErrorMessage = "Entidad id requerido.")]
        public int EntidadId { get; set; }
    }
}
