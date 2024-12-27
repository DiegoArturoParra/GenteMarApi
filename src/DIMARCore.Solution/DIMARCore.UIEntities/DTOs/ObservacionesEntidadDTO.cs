using DIMARCore.Utilities.Core.ValidAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class ObservacionesEntidadDTO
    {
        [Required(ErrorMessage = "Entidad id requerida.")]
        public int EntidadId { get; set; }
        [Required(ErrorMessage = "Observación requerida.")]
        [StringLength(5000, ErrorMessage = "Debe tener una longitud máxima de {1} caracteres.")]
        public string Observacion { get; set; }
        [ValidFileInBytesType(MaxFileSizeMB = 10)]
        public byte[] FileBytes { get; set; }
        [ValidExtension("PDF", "DOCX")]
        public string Extension { get; set; }
        [Required(ErrorMessage = "Fecha respuesta entidad requerida.")]
        public DateTime FechaRespuestaEntidad { get; set; }
        [Required(ErrorMessage = "Número de expediente requerido.")]
        public int ExpedienteId { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public DateTime? FechaVigencia { get; set; }
    }
}
