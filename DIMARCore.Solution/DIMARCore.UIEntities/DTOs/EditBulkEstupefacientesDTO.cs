using DIMARCore.Utilities.Core.ValidAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class EditBulkEstupefacientesDTO
    {
        [Required(ErrorMessage = "ids requeridos")]
        public IList<long> EstupefacientesId { get; set; }
        [Required(ErrorMessage = "Estado id requerida.")]
        public int EstadoAntecedenteId { get; set; }
        [Required(ErrorMessage = "Consolidado id requerido.")]
        public int ConsolidadoId { get; set; }
        public IList<ObservacionesEntidadDTO> ObservacionesPorEntidad { get; set; }
    }

    public class EditBulkPartialEstupefacientesDTO
    {
        [Required(ErrorMessage = "ids de estupefacientes requerido.")]
        public IList<long> EstupefacientesId { get; set; }
        [Required(ErrorMessage = "Estado id requerido.")]
        public int EstadoAntecedenteId { get; set; }
        [Required(ErrorMessage = "Consolidado id requerido.")]
        public int ConsolidadoId { get; set; }
        [Required(ErrorMessage = "Observación de la entidad requerida.")]
        public ObservacionesEntidadDTO ObservacionEntidad { get; set; }
    }

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
