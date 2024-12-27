using Newtonsoft.Json;
using System;

namespace DIMARCore.UIEntities.DTOs
{
    public class AuditoriaDTO
    {
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        [JsonIgnore]
        public DateTime FechaCreacion { get; set; }

        public string FechaCreacionFormato
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy hh:mm:ss tt}", this.FechaCreacion);
            }
        }

        [JsonIgnore]
        public DateTime? FechaModificacion { get; set; }


        public string FechaModificacionFormato
        {
            get
            {
                return this.FechaModificacion.HasValue ? string.Format("{0:dd/MM/yyyy hh:mm:ss tt}", this.FechaModificacion.Value) : "N/A";
            }
        }
    }
}