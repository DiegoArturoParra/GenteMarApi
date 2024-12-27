using System;
using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class ObservacionEntidadVciteDTO
    {
        [Required(ErrorMessage = "Entidad id requerida.")]
        public int EntidadId { get; set; }
        [StringLength(5000, ErrorMessage = "Debe tener una longitud máxima de {1} caracteres.")]
        public string DetalleObservacion { get; set; }
        [Required(ErrorMessage = "Fecha respuesta requerida")]
        public DateTime? FechaRespuestaEntidad { get; set; }
        [Required(ErrorMessage = "verificación requerida")]
        public bool? VerificacionExitosa { get; set; }
    }
}
