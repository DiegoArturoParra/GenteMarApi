using DIMARCore.Utilities.Core.ValidAttributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
namespace DIMARCore.UIEntities.DTOs
{
    public class DatosBasicosDTO
    {
        public long IdGentemar { get; set; }
        [StringLength(19, MinimumLength = 4, ErrorMessage = "El campo debe tener entre 4 y 19 caracteres.")]
        [RegularExpression(@"^\d{1,3}(.\d{3})*$", ErrorMessage = "El campo debe tener el formato de números con puntos de mil.")]
        public string DocumentoIdentificacion { get; set; }
        public int? IdTipoDocumento { get; set; }
        public int? IdMunicipioExpedicion { get; set; }
        public string CodPais { get; set; }
        public DateTime? FechaExpedicion { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        private string _firstName;
        public string Nombres
        {
            get => _firstName?.ToUpper().Trim();
            set => _firstName = value;
        }
        private string _lastName;
        public string Apellidos
        {
            get => _lastName?.ToUpper().Trim();
            set => _lastName = value;
        }
        public int? IdGenero { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string IdPaisNacimiento { get; set; }
        public string IdPaisResidencia { get; set; }
        private string _direccion;
        public string Direccion
        {
            get => _direccion?.ToUpper().Trim();
            set => _direccion = value;
        }
        public int? IdMunicipioResidencia { get; set; }
        public string Telefono { get; set; }
        private string _correo;
        public string CorreoElectronico
        {
            get => _correo = _correo?.ToUpper().Trim();
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
        [JsonIgnore]
        public string UrlArchivo { get; set; }
        public List<HistorialDocumentoDTO> HistorialDocumento { get; set; }
        [ValidExtensionFile("JPG", "JPEG", "PNG")]
        [ValidSizeFile(MaxFileSizeMB = 5)] // Utiliza un tamaño máximo de 5 MB
        public HttpPostedFile Archivo { get; set; }
        public ObservacionDTO Observacion { get; set; }
    }
}
