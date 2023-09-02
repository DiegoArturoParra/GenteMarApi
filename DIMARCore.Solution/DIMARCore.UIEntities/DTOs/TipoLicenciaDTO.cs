using Newtonsoft.Json;

namespace DIMARCore.UIEntities.DTOs
{
    public class TipoLicenciaDTO
    {

        public int? IdTipoLicencia { get; set; }
        [JsonIgnore]
        public string _TipoLicencia;
        public string TipoLicencia
        {
            get => _TipoLicencia?.ToUpper();
            set => _TipoLicencia = value;
        }

        public bool? Activo { get; set; }
    }
}
