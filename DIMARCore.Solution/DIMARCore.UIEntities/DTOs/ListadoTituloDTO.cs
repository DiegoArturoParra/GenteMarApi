using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DIMARCore.UIEntities.DTOs
{
    public class ListadoTituloDTO
    {
        public long Id { get; set; }
        public string CapitaniaFirma { get; set; }
        public string CapitaniaFirmante { get; set; }
        public bool ContienePrevista { get; set; }
        public List<InfoCargosDTO> Cargos { get; set; }
        public string Solicitud { get; set; }
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

    public class InfoCargosDTO
    {
        public string Regla { get; set; }
        public string CargoTitulo { get; set; }
        public string Nivel { get; set; }
    }
}
