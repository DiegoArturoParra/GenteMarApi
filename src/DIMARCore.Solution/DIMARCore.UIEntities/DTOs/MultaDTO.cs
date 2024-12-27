using Newtonsoft.Json;
using System;

namespace DIMARCore.UIEntities.DTOs
{
    public class MultaDTO
    {
        public string TipoMulta { get; set; }
        public string Observacion { get; set; }
        [JsonIgnore]
        public DateTime FechaRegistro { get; set; }
        public string FechaRegistroFormat
        {
            get
            {
                return FechaRegistro.ToString("dd/MM/yyyy");
            }
        }
    }
}
