using DIMARCore.Utilities.Core.ValidAttributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
namespace DIMARCore.UIEntities.DTOs
{
    public class ObservacionDTO
    {
        [Required(ErrorMessage = "Observación requerida.")]
        [StringLength(2000, ErrorMessage = "Debe tener una longitud mínima de {2} y una longitud máxima de {1}.", MinimumLength = 10)]
        public string Observacion { get; set; }
        //[Required(ErrorMessage = "Id de tabla requerido.")]
        public long? IdTablaRelacion { get; set; }
        public ArchivoBaseDTO ArchivoBase { get; set; }
        public DateTime FechaHoraCreacion { get; set; }
        [ValidExtensionFile("JPG", "JPEG", "PNG", "PDF", "DOC", "DOCX", "XLSX")]
        [ValidSizeFile(MaxFileSizeMB = 10)] // Utiliza un tamaño máximo de 10 MB
        public HttpPostedFile Archivo { get; set; }
    }
}
