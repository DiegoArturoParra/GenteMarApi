using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class CargoLicenciaDTO
    {
        public int? IdCargoLicencia { get; set; }
        [JsonIgnore]
        private string _CargoLicencia;
        public string CargoLicencia
        {
            get => _CargoLicencia?.ToUpper();
            set => _CargoLicencia = value;
        }
        public string CodigoLicencia;
        public int IdTipoLicencia { get; set; }
        public int IdActividad { get; set; }
        public int IdSeccion { get; set; }
        public int IdClase { get; set; }
        public int IdActividadSeccion { get; set; }
        public int IdSeccionClase { get; set; }
        [Range(182, 3650, ErrorMessage = "La vigencia debe tener más de {1} y menos de {2} días.")]
        public decimal? Vigencia { get; set; }
        public bool? Activo { get; set; }
        public bool? Nave { get; set; }
        public List<int> IdLimitacion { get; set; }
        public List<int> IdCategoria { get; set; }
        public List<int> IdLimitante { get; set; }


    }
}
