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
        [RegularExpression("(^[0-9.]+$)", ErrorMessage = "Solo se permiten números y punto decimal.")]
        [StringLength(19, ErrorMessage = "Longitud mínima de {1} caracteres y máximo de {2} caracteres.", MinimumLength = 12)]
        [Required(ErrorMessage = "Radicado requerido.")]
        public string Radicado { get; set; }
        public int CapitaniaId { get; set; }
        [Required(ErrorMessage = "Estado id requerido.")]
        public int EstadoId { get; set; }
        [Required(ErrorMessage = "Tramite id requerido.")]
        public int TramiteId { get; set; }
        public string Estado { get; set; }
        public string Tramite { get; set; }
        public string Capitania { get; set; }
        public EstupefacienteDatosBasicosDTO DatosBasicos { get; set; }
        public DateTime FechaRadicadoSgdea { get; set; }
        public DateTime FechaSolicitudSedeCentral { get; set; }
        public DateTime FechaAprobacion { get; set; }
        public DateTime FechaVigencia { get; set; }
    }
}
