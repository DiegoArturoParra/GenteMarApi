using DIMARCore.Utilities.Helpers;
using Newtonsoft.Json;

namespace DIMARCore.UIEntities.QueryFilters
{
    public class DatosBasicosQueryFilter
    {
        public ParametrosPaginacion Paginacion { get; set; }
        public string DocumentoIdentificacion { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public int? id_estado { get; set; }
    }
}
