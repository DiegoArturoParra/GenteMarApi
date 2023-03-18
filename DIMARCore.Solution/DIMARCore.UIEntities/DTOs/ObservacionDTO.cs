using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
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
        public ArchivoBase ArchivoBase { get; set; }            
        public DateTime FechaHoraCreacion { get; set; }

        [ValidFileAttribute(ErrorMessage = "Es incorrecto el formato, solo se permiten extensiones JPG,JPEG,PNG,PDF,DOC,DOCX,XLSX")]
        [ValidSizeFileAttribute(ErrorMessage = "Es incorrecto el tamaño, solo se permite 5 mb por el archivo.")]
        public HttpPostedFile Archivo { get; set; }
        public class ValidFileAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if (value == null)
                    return true;

                string[] _validExtensions = { "JPG", "JPEG", "PNG", "PDF", "DOC", "DOCX", "XLSX" };

                var file = (HttpPostedFile)value;

                var ext = Path.GetExtension(file.FileName).ToUpper().Replace(".", "");
                return _validExtensions.Contains(ext);
            }
        }
        public class ValidSizeFileAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if (value == null)
                    return true;

                var file = (HttpPostedFile)value;

                if (file.ContentLength > 5242880)
                {
                    return false;
                }
                return true;
            }
        }
    }

   

    public class ArchivoBase
    {
        public string ArchivoBase64 { get; set; }
        public string RutaArchivo { get; set; }
        public string Extension => Path.GetExtension(RutaArchivo);
    }
}
