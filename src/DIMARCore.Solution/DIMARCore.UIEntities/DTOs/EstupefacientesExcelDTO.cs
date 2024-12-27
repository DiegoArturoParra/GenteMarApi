using Newtonsoft.Json;
using System;

namespace DIMARCore.UIEntities.DTOs
{

    public class EstupefacientesExcelDTO
    {
        public long EstupefacienteId { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Radicado { get; set; }
        public string LugarTramiteCapitania { get; set; }
        public string TipoDocumento { get; set; }
        public string TipoTramite { get; set; }
        public string Documento { get; set; }
        [JsonIgnore]
        public DateTime FechaNacimiento { get; set; }
        [JsonIgnore]
        public DateTime? FechaSolicitudSedeCentral { get; set; }
        [JsonIgnore]
        public DateTime? FechaRadicado { get; set; }

        public String FechaSolicitudSedeCentralFormato
        {
            get
            {
                return this.FechaSolicitudSedeCentral.HasValue ? string.Format("{0:dd/MM/yyyy}", this.FechaSolicitudSedeCentral.Value) : "N/A";
            }
        }

        public String FechaNacimientoFormato
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy}", this.FechaNacimiento);
            }
        }
        public String FechaRadicadoFormato
        {
            get
            {
                return this.FechaRadicado.HasValue ? string.Format("{0:dd/MM/yyyy}", this.FechaRadicado.Value) : "N/A";
            }
        }
    }
}
