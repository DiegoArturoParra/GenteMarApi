using DIMARCore.UIEntities.DTOs;

namespace DIMARCore.UIEntities.QueryFilters
{
    public class CargoTituloFilter : ActivoDTO
    {
        public int? SeccionId { get; set; }
        public int? ClaseId { get; set; }
    }
}
