using Newtonsoft.Json;
using System;

namespace DIMARCore.UIEntities.DTOs
{
    public class FechasRadioOperadoresDTO
    {
        [JsonIgnore]
        public DateTime? FechaVencimiento { get; set; }
        [JsonIgnore]
        public DateTime? FechaExpedicion { get; set; }
        public String FechaExpedicionFormato
        {
            get
            {
                return this.FechaExpedicion.HasValue ? string.Format("{0:dd/MM/yyyy}", this.FechaExpedicion.Value) : "";
            }
        }

        public String FechaVencimientoFormato
        {
            get
            {
                return this.FechaVencimiento.HasValue ? string.Format("{0:dd/MM/yyyy}", this.FechaVencimiento.Value) : "";
            }
        }
    }
}
