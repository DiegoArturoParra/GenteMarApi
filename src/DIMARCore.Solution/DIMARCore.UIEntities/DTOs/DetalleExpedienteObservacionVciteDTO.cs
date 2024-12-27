using System;

namespace DIMARCore.UIEntities.DTOs
{
    public class DetalleExpedienteObservacionVciteDTO
    {
        public long ExpedienteObservacionId { get; set; }
        public int EntidadId { get; set; }
        public string DetalleObservacion { get; set; }
        public DateTime? FechaRespuestaEntidad { get; set; }
        public bool VerificacionExitosa { get; set; }
        public string Entidad { get; set; }
        public long AntecedenteId { get; set; }
        public string Radicado { get; set; }
        public string NumeroDeExpediente { get; set; }
    }
}
