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
        [Required(ErrorMessage = "Fecha de aprobación requerida.")]
        public DateTime FechaAprobacion { get; set; }
        [Required(ErrorMessage = "Fecha de vigencia requerida.")]
        public DateTime FechaVigencia { get; set; }
        public IList<ObservacionesEntidadDTO> ObservacionesPorEntidad { get; set; }
    }
    public class ObservacionesEntidadDTO
    {
        [Required(ErrorMessage = "Entidad id requerida.")]
        public int EntidadId { get; set; }
        [Required(ErrorMessage = "Observación requerida.")]
        [StringLength(5000, ErrorMessage = "Debe tener una longitud máxima de {1} caracteres.")]
        public string Observacion { get; set; }
        [ValidFileInBytesType(".pdf", ".docx", MaxFileSizeMB = 10)]
        public byte[] Archivo { get; set; }
        [Required(ErrorMessage = "Fecha respuesta entidad requerida.")]
        public DateTime FechaRespuestaEntidad { get; set; }
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        [StringLength(12, ErrorMessage = "Longitud mínima de {1} caracteres y máximo de {2} caracteres.", MinimumLength = 2)]
        [Required(ErrorMessage = "Número de expediente requerido.")]
        public string NumeroExpediente { get; set; }
    }
}
