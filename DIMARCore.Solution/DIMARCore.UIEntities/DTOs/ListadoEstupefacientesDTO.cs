using Newtonsoft.Json;
using System;

namespace DIMARCore.UIEntities.DTOs
{
    public class ListadoEstupefacientesDTO
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Radicado { get; set; }
        public string Capitania { get; set; }
        public string Tramite { get; set; }
        public string Documento { get; set; }
        public string Estado { get; set; }
        [JsonIgnore]
        public DateTime? FechaSolicitudSedeCentral { get; set; }
        [JsonIgnore]
        public DateTime? FechaAprobacion { get; set; }

        [JsonIgnore]
        public DateTime? FechaVigencia { get; set; }

        public String FechaSolicitudSedeCentralFormato
        {
            get
            {
                return this.FechaSolicitudSedeCentral.HasValue ? string.Format("{0:dd/MM/yyyy}", this.FechaSolicitudSedeCentral.Value) : "";
            }
        }

        public String FechaAprobacionFormato
        {
            get
            {
                return this.FechaAprobacion.HasValue ? string.Format("{0:dd/MM/yyyy}", this.FechaAprobacion.Value) : "Ninguna";
            }
        }
        public String FechaVigenciaFormato
        {
            get
            {
                return this.FechaVigencia.HasValue ? string.Format("{0:dd/MM/yyyy}", this.FechaAprobacion.Value) : "Ninguna";
            }
        }
    }
}
