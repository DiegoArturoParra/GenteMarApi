using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
namespace DIMARCore.UIEntities.DTOs
{
    public class DatosBasicosDTO
    {
        public long IdGentemar { get; set; }
        public string DocumentoIdentificacion { get; set; }
        public int? IdTipoDocumento { get; set; }
        public int? IdMunicipioExpedicion { get; set; }
        public string CodPais { get; set; }
        public DateTime? FechaExpedicion { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        private string _firstName;
        public string Nombres
        {
            get => _firstName?.ToUpper();
            set => _firstName = value;
        }
        private string _lastName;
        public string Apellidos
        {
            get => _lastName?.ToUpper();
            set => _lastName = value;
        }
        public int? IdGenero { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int? IdMunicipioNacimiento { get; set; }
        private string _direccion;
        public string Direccion
        {
            get => _direccion?.ToUpper();
            set => _direccion = value;
        }
        public int? IdMunicipioResidencia { get; set; }
        public string Telefono { get; set; }
        private string _correo;
        public string CorreoElectronico
        {
            get => _correo = _correo?.ToUpper();
            set => _correo = value;
        }
        public string NumeroMovil { get; set; }
        public int? IdEstado { get; set; }
        public bool? Activo { get; set; }
        public int? IdFormacionGrado { get; set; }
        public int? Formacion { get; set; }
        public string CarpetaFisico { get; set; }
        public bool? Antecedente { get; set; }
        public string FotoBase64 { get; set; }
        public string UrlArchivo { get; set; }
        public List<HistorialDocumentoDTO> HistorialDocumento { get; set; }
        [ValidFileAttribute(ErrorMessage = "Es incorrecto el formato, solo se permiten extensiones JPG,JPEG,PNG")]
        [ValidSizeFileAttribute(ErrorMessage = "Es incorrecto el tamaño, solo se permite 5 mb por el archivo.")]
        public HttpPostedFile Archivo { get; set; }        
        public ObservacionDTO Observacion { get; set; }
        public class ValidFileAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if (value == null)
                    return true;

                string[] _validExtensions = { "JPG", "JPEG", "PNG" };

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
}
