using DIMARCore.Utilities.Helpers;
using System.Collections.Generic;

namespace DIMARCore.UIEntities.QueryFilters
{
    public class CargoLicenciaFilter
    {
        public ParametrosPaginacion Paginacion { get; set; }
        public string CargoLicencia { get; set; }
        public string CodigoLicencia { get; set; }
        public bool? Activo { get; set; }
    }
}
