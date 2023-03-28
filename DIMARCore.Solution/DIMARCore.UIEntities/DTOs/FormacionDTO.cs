using GenteMarCore.Entities.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIMARCore.UIEntities.DTOs
{
    public class FormacionDTO
    {
        public int? id_formacion { get; set; }
        [JsonIgnore]
        private string _Formacion;
        public string formacion
        {
            get => _Formacion?.ToUpper();
            set => _Formacion = value;
        }
        public bool? activo { get; set; }
        public List<GradoDTO> listaGrado { get; set; }
        public int id_formacion_grado { get; set; }
    }
}
