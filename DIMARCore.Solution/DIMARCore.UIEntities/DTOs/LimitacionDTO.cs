using Newtonsoft.Json;

namespace DIMARCore.UIEntities.DTOs
{
    public class LimitacionDTO
    {
        public int? IdLimitacion { get; set; }
        [JsonIgnore]
        private string _Limitaciones;
        public string Limitaciones
        {
            get => _Limitaciones?.ToUpper();
            set => _Limitaciones = value;
        }
        public bool? Activo { get; set; }
    }
}
