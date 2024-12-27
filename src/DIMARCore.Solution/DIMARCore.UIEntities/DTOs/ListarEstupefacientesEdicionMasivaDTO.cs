using System.Collections.Generic;

namespace DIMARCore.UIEntities.DTOs
{
    public class ListarEstupefacientesEdicionMasivaDTO
    {
        public long EstupefacienteId { get; set; }
        public string Radicado { get; set; }
        public string Documento { get; set; }
        public string NombreCompleto { get; set; }
        public string Estado { get; set; }
        public int CountObservaciones { get; set; }
        public List<ExpedienteEntidadObservacionDTO> ObservacionExpedientePorEntidad { get; set; }
    }
}
