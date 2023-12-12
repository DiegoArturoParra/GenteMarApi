using Newtonsoft.Json;

namespace DIMARCore.UIEntities.DTOs
{
    public class GradoInfoDTO
    {
        public int? id_grado { get; set; }
        [JsonIgnore]
        private string _grado;
        public string grado
        {
            get => _grado?.ToUpper();
            set => _grado = value;
        }
        public int? id_rango { get; set; }
        [JsonIgnore]
        private string _nombreRango;
        public string nombreRango
        {
            get => _nombreRango != null ? _nombreRango?.ToUpper() : _nombreRango;
            set => _nombreRango = value;
        }
        [JsonIgnore]
        private string _sigla;
        public string sigla
        {
            get => _sigla?.ToUpper();
            set => _sigla = value;
        }
        public int? id_formacion_grado { get; set; }
        public bool activo { get; set; }
        public FormacionDTO formacion { get; set; }

    }
    public class GradoDTO : CamposTablasMaestrasDTO
    {

    }
}
