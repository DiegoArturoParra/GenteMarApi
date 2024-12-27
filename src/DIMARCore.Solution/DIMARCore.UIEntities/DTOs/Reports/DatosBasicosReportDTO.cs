using CsvHelper.Configuration.Attributes;
using System;

namespace DIMARCore.UIEntities.DTOs.Reports
{
    public class DatosBasicosReportDTO
    {
        [Name("Documento de identificación")]
        public string DocumentoIdentificacion { get; set; }
        [Name("Tipo de documento")]
        public string TipoDocumento { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        [Name("Género")]
        public string Genero { get; set; }
        [Ignore]
        public DateTime FechaNacimiento { get; set; }
        public short Edad { get; set; }
        [Name("Número de contacto")]
        public string NumeroContacto { get; set; }
        [Name("Correo electrónico")]
        public string CorreoElectronico { get; set; }
        public string Estado { get; set; }
        [Name("País de nacimiento")]
        public string PaisNacimiento { get; set; }
        [Name("País de residencia")]
        public string PaisResidencia { get; set; }
        [Name("Nivel de formación")]
        public string Formacion { get; set; }
        [Name("Nivel de grado")]
        public string Grado { get; set; }
        [Name("Fecha de creación")]
        public DateTime FechaCreacion { get; set; }
    }
}
