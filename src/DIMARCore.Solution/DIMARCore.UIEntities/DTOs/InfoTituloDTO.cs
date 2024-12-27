using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DIMARCore.UIEntities.DTOs
{
    public class InfoTituloDTO
    {
        public long TituloId { get; set; }
        public int EstadoTramiteId { get; set; }
        public int TipoSolicitudId { get; set; }
        public int TipoRefrendoId { get; set; }
        public int CapitaniaId { get; set; }
        public int CapitaniaFirmanteId { get; set; }
        public List<ListarCargosDeTitulosPorPersonaDTO> Cargos { get; set; }
        public string Pais { get; set; }
        public string Solicitud { get; set; }
        public string NombreUsuario { get; set; }
        public string CapitaniaFirma { get; set; }
        public string CapitaniaFirmante { get; set; }
        public string Tramite { get; set; }
        public string Radicado { get; set; }
        public string DocumentoIdentificacion { get; set; }
        [JsonIgnore]
        public DateTime FechaVencimiento { get; set; }
        [JsonIgnore]
        public DateTime FechaExpedicion { get; set; }
        public string FechaExpedicionFormato => string.Format("{0:dd/MM/yyyy}", this.FechaExpedicion);

        public string FechaVencimientoFormato => string.Format("{0:dd/MM/yyyy}", this.FechaVencimiento);

    }
}
