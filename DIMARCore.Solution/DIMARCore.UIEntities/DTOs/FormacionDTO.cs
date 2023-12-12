using Newtonsoft.Json;
using System.Collections.Generic;

namespace DIMARCore.UIEntities.DTOs
{
    public class FormacionDTO
    {
        public int? id_formacion { get; set; }
        [JsonIgnore]
        private string _Formacion;
        public string formacion
        {
            get => _Formacion?.Trim().ToUpper();
            set => _Formacion = value;
        }
        public bool? activo { get; set; }
        public List<GradoInfoDTO> listaGrado { get; set; }
        public int id_formacion_grado { get; set; }
    }
}
