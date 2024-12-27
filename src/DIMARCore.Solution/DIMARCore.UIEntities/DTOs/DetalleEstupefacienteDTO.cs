using System;

namespace DIMARCore.UIEntities.DTOs
{
    public class DetalleEstupefacienteDTO
    {

        public long EstupefacienteId { get; set; }
        public long AntecedenteDatosBasicosId { get; set; }
        public string Radicado { get; set; }
        public int TramiteId { get; set; }
        public string Tramite { get; set; }
        public int EstadoId { get; set; }
        public string Estado { get; set; }
        public int CapitaniaId { get; set; }
        public string Capitania { get; set; }
        public EstupefacienteDatosBasicosDTO DatosBasicos { get; set; }
        public DateTime? FechaRadicadoSgdea { get; set; }
        public DateTime? FechaSolicitudSedeCentral { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public DateTime? FechaVigencia { get; set; }
    }
}
