using DIMARCore.Utilities.Core.ValidAttributes;
using DIMARCore.Utilities.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web;
namespace DIMARCore.UIEntities.DTOs
{
    public class CrearDatosBasicosDTO
    {
        public long IdGentemar { get; set; }
        [StringLength(19, MinimumLength = 4, ErrorMessage = "El campo debe tener entre 4 y 19 caracteres.")]
        [RegularExpression(@"^\d{1,3}(.\d{3})*$", ErrorMessage = "El campo debe tener el formato de números con puntos de mil.")]
        public string DocumentoIdentificacion { get; set; }
        public byte? IdTipoDocumento { get; set; }
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
        public int? IdMunicipioNacimiento { get; set; }
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
        public string FotoBase64 { get; set; }
        [JsonIgnore]
        public string UrlArchivo { get; set; }
        public List<HistorialDocumentoDTO> HistorialDocumento { get; set; }
        [ValidExtensionFile("JPG", "JPEG", "PNG")]
        [ValidSizeFile(MaxFileSizeMB = 5)] // Utiliza un tamaño máximo de 5 MB
        public HttpPostedFile Archivo { get; set; }
        public ObservacionDTO Observacion { get; set; }
    }
    public class DatosBasicosDTO
    {
        public long IdGentemar { get; set; }
        public string DocumentoIdentificacion { get; set; }
        public byte? IdTipoDocumento { get; set; }
        public int? IdMunicipioExpedicion { get; set; }
        public string CodPais { get; set; }
        public DateTime? FechaExpedicion { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public int? IdGenero { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string IdPaisNacimiento { get; set; }
        public int? IdMunicipioNacimiento { get; set; }
        public string IdPaisResidencia { get; set; }
        public string Direccion { get; set; }
        public int? IdMunicipioResidencia { get; set; }
        [JsonIgnore]
        public string TelefonoMasIndicativo { get; set; }
        public string IndicativoTelefono => GetIndicativo();     
        public string Telefono => GetTelefono();
        public string CorreoElectronico { get; set; }
        public string NumeroMovil { get; set; }
        public int? IdEstado { get; set; }
        public bool? Activo => this.IdEstado.HasValue ? this.IdEstado == (int)EstadoGenteMarEnum.ACTIVO : (bool?)null;
        public int? IdFormacionGrado { get; set; }
        public int? Formacion { get; set; }
        public string FotoBase64 { get; set; }
        [JsonIgnore]
        public string UrlArchivo { get; set; }
        public List<HistorialDocumentoDTO> HistorialDocumento { get; set; }
        private string GetIndicativo()
        {
            // Definir la expresión regular para capturar el indicativo
            string patron = @"^(\d+)\+";

            if (string.IsNullOrWhiteSpace(this.TelefonoMasIndicativo))
                return "";

            // Buscar coincidencias en la cadena
            var coincidencia = Regex.Match(this.TelefonoMasIndicativo, patron);

            // Si hay coincidencias, extraer el indicativo
            if (coincidencia.Success)
            {
                return coincidencia.Groups[1].Value;
            }
            else
            {
                return "";
            }
        }
        private string GetTelefono()
        {
            string patron = @"\+(\d+)$";

            if (string.IsNullOrWhiteSpace(this.TelefonoMasIndicativo))
                return "";
            // Buscar coincidencias en la cadena
            var coincidencia = Regex.Match(this.TelefonoMasIndicativo, patron);

            // Si hay coincidencias, extraer el teléfono
            if (coincidencia.Success)
            {
                return coincidencia.Groups[1].Value;
            }
            else
            {
                return null;
            }
        }
    }
}
