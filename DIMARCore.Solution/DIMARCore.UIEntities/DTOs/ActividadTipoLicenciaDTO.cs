using Newtonsoft.Json;

namespace DIMARCore.UIEntities.DTOs
{
    public class ActividadTipoLicenciaDTO
    {
        public int? IdActividad { get; set; }
        [JsonIgnore]
        private string _Actividad;
        public string Actividad
        {
            get => _Actividad?.Trim().ToUpper();
            set => _Actividad = value;
        }
        public bool? Activo { get; set; }
        public int IdTipoLicencia { get; set; }
        public string TipoLicencia { get; set; }
    }

}
