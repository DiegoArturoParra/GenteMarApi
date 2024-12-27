using DIMARCore.Utilities.Core.ValidAttributes;
using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class AclaracionEditDTO
    {
        [Required(ErrorMessage = "Antecedente id requerido.")]
        public long AntecedenteId { get; set; }
        [Required(ErrorMessage = "Detalle aclaración requerido.")]
        [StringLength(5000, ErrorMessage = "La aclaración debe tener una longitud mínima de {2} y una longitud máxima de {1}.", MinimumLength = 10)]
        public string DetalleAclaracion { get; set; }
        [ValidFileInBytesType(MaxFileSizeMB = 10)]
        public byte[] FileBytes { get; set; }
        [Required(ErrorMessage = "expediente observación id requerido.")]
        public long ExpedienteObservacionId { get; set; }
        public string DetalleObservacionNuevo { get; set; }
        public bool VerificacionExitosa { get; set; }
        [ValidExtension("PDF", "DOCX")]
        public string Extension { get; set; }
        public ObservacionEntidadAnteriorVciteDTO ObservacionAnterior { get; set; }
    }
}
