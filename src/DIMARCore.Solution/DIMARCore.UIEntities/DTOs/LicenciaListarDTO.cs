using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DIMARCore.UIEntities.DTOs
{
    public class LicenciaListarDTO
    {
        public string CargoLicencia { get; set; }
        public string NombreUsuario { get; set; }
        public string DocumentoIdentificacion { get; set; }
        public string EstadoTramite { get; set; }
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
        public string Radicado { get; set; }

    }
    public class TituloListarDTO
    {
        public List<InfoCargosDTO> Cargos { get; set; }
        public string NombreUsuario { get; set; }
        public string DocumentoIdentificacion { get; set; }
        public string EstadoTramite { get; set; }
        [JsonIgnore]
        public DateTime? FechaVencimiento { get; set; }
        [JsonIgnore]
        public DateTime? FechaExpedicion { get; set; }
        public string Radicado { get; set; }
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
