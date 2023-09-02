using Newtonsoft.Json;

namespace DIMARCore.UIEntities.DTOs
{
    public class TerritorioDTO
    {
        public int? IdTerritorio { get; set; }
        [JsonIgnore]
        private string _Territorio;
        public string Territorio
        {
            get => _Territorio?.ToUpper();
            set => _Territorio = value;
        }
        public bool? Activo { get; set; }
    }
}
