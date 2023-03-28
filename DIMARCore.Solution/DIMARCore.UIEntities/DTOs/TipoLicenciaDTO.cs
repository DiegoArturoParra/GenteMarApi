using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
