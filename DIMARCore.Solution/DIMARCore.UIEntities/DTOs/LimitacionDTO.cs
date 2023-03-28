using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
