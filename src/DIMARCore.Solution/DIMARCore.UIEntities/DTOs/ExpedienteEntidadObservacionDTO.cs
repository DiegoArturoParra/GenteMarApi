using DIMARCore.Utilities.Helpers;
using Newtonsoft.Json;
using System;

namespace DIMARCore.UIEntities.DTOs
{
    public class ExpedienteEntidadObservacionDTO : ExpedienteEntidadDTO
    {
        public string Observacion { get; set; }
        [JsonIgnore]
        public DateTime? FechaEntidad { get; set; }
        public string FechaRespuestaEntidadFormat => FechaEntidad.HasValue ? string.Format("{0:dd/MM/yyyy}", FechaEntidad.Value) : "N/A";
        public string TextInDataTable => string.IsNullOrWhiteSpace(Observacion)
            ? Constantes.OBSERVACION_PENDIENTE : Observacion.Equals(Constantes.SIN_OBSERVACION) ? Constantes.OBSERVACION_REGISTRADA : Observacion;
    }
}
