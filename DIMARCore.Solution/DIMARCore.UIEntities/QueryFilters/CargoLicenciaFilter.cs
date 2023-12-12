using System.Collections.Generic;

namespace DIMARCore.UIEntities.QueryFilters
{
    public class CargoLicenciaFilter
    {
        public List<int> SeccionesLicenciaId { get; set; } = new List<int>();
        public List<int> ActividadesId { get; set; } = new List<int>();
        public List<int> ClasesLicenciaId { get; set; } = new List<int>();
        public List<int> TiposDeLicenciaId { get; set; } = new List<int>();
    }
}
