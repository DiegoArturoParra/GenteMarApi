using Newtonsoft.Json;
using System;

namespace DIMARCore.UIEntities.DTOs
{
    public class HistorialAclaracionDTO
    {
        [JsonIgnore]
        public string ObservacionAnteriorJson { get; set; }
        public ObservacionAnteriorDTO ObservacionAnterior => !string.IsNullOrWhiteSpace(ObservacionAnteriorJson) ?
                                                            JsonConvert.DeserializeObject<ObservacionAnteriorDTO>(ObservacionAnteriorJson) : null;
        public string DetalleAclaracion { get; set; }
        public ArchivoBaseDTO ArchivoBase { get; set; }
        public DateTime FechaHoraCreacion { get; set; }
        public string Entidad { get; set; }
        public string UsuarioCreador { get; set; }

    }
}
