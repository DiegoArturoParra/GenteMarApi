using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class AclaracionEstupefacienteDTO
    {
        public long AclaracionId { get; set; }
        [Required(ErrorMessage = "Entidad id requerida.")]
        public int EntidadId { get; set; }
        [StringLength(5000, ErrorMessage = "Debe tener una longitud máxima de {1} caracteres.")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "Fecha respuesta requerida")]
        public DateTime FechaRespuestaEntidad { get; set; }
        [Required(ErrorMessage = "verificación requerida")]
        public bool VerificacionExitosa { get; set; }
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        [StringLength(12, ErrorMessage = "Longitud mínima de {1} caracteres y máximo de {2} caracteres.", MinimumLength = 2)]
        [Required(ErrorMessage = "Número de expediente requerido.")]
        public string NumeroDeExpediente { get; set; }
    }

    public class AclaracionDTO
    {
        [Required(ErrorMessage = "Antecedente id requerida.")]
        public long AntecedenteId { get; set; }
        public AclaracionEstupefacienteDTO Aclaracion { get; set; }
    }

    public class AclaracionesBulkDTO
    {
        [Required(ErrorMessage = "Antecedente id requerida.")]
        public long AntecedenteId { get; set; }
        public IList<AclaracionEstupefacienteDTO> Aclaraciones { get; set; }
    }

    public class AclaracionEditDTO
    {
        [Required(ErrorMessage = "Antecedente id requerida.")]
        public long AntecedenteId { get; set; }
        public IList<AclaracionEstupefacienteDTO> Aclaraciones { get; set; }
        public ObservacionDTO Observacion { get; set; }
    }

    public class DetalleAclaracionesEstupefacienteDTO : AclaracionEstupefacienteDTO
    {
        public string Entidad { get; set; }
        public long AntecedenteId { get; set; }
        public string Radicado { get; set; }
    }
}
