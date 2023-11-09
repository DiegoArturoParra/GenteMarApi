using System;
using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class EditInfoEstupefacienteDTO
    {
        [Required(ErrorMessage = "Estupefaciente id requerido.")]
        public long EstupefacienteId { get; set; }
        [Required(ErrorMessage = "Gente de mar id requerido.")]
        public long GenteDeMarId { get; set; }
        public int CapitaniaId { get; set; }
        [Required(ErrorMessage = "Estado id requerido.")]
        public int EstadoId { get; set; }
        [Required(ErrorMessage = "Tramite id requerido.")]
        public int TramiteId { get; set; }
        [Required(ErrorMessage = "Fecha sede central requerida.")]
        public DateTime FechaSolicitudSedeCentral { get; set; }
        public EstupefacienteDatosBasicosDTO DatosBasicos { get; set; }
        public DateTime FechaAprobacion { get; set; }
        public DateTime FechaVigencia { get; set; }
        public ObservacionDTO Observacion { get; set; }
    }
}
