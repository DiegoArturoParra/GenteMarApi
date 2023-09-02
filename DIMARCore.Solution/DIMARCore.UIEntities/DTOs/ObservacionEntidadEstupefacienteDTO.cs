using DIMARCore.Utilities.Core.ValidAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class ObservacionEntidadEstupefacienteDTO
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

    public class CrearObservacionEntidadVciteDTO
    {
        [Required(ErrorMessage = "Antecedente id requerida.")]
        public long AntecedenteId { get; set; }
        public ObservacionEntidadEstupefacienteDTO ObservacionPorEntidad { get; set; }
    }

    public class ObservacionesEntidadBulkDTO
    {
        [Required(ErrorMessage = "Antecedente id requerida.")]
        public long AntecedenteId { get; set; }
        public IList<ObservacionEntidadEstupefacienteDTO> ObservacionesPorEntidad { get; set; }
    }

    public class AclaracionEditDTO
    {
        [Required(ErrorMessage = "Antecedente id requerido.")]
        public long AntecedenteId { get; set; }
        [Required(ErrorMessage = "Detalle aclaración requerido.")]
        [StringLength(5000, ErrorMessage = "Debe tener una longitud mínima de {2} y una longitud máxima de {1}.", MinimumLength = 10)]
        public string DetalleAclaracion { get; set; }
        [Required(ErrorMessage = "El archivo es requerido.")]
        [ValidFileInBytesType(".pdf", ".docx", MaxFileSizeMB = 10)]
        public byte[] Archivo { get; set; }
        [Required(ErrorMessage = "expediente observación id requerido.")]
        public long ExpedienteObservacionId { get; set; }
        public string DetalleObservacionNuevo { get; set; }
        public bool VerificacionExitosa { get; set; }
        public string Extension { get; set; }
        public ObservacionAnteriorDTO ObservacionAnterior { get; set; }
    }

    public class ObservacionAnteriorDTO
    {
        public string DetalleAnterior { get; set; }
        public bool VerificacionExitosaAnterior { get; set; }
    }

    public class DetalleExpedienteObservacionEstupefacienteDTO
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
