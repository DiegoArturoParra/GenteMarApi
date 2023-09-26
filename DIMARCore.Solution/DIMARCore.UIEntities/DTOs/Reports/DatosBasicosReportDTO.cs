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
        public short Edad => CalcularEdad();
        [Name("Número de contacto")]
        public string NumeroContacto { get; set; }
        [Name("Correo electrónico")]
        public string CorreoElectronico { get; set; }
        public string Estado { get; set; }
        [Name("País de nacimiento")]
        public string PaisNacimiento { get; set; }
        [Name("País de residencia")]
        public string PaisResidencia { get; set; }
        [Name("Fecha de creación")]
        public DateTime FechaCreacion { get; set; }

        public short CalcularEdad()
        {
            DateTime fechaActual = DateTime.Now;
            short edad = (short)(fechaActual.Year - FechaNacimiento.Year);

            // Ajusta la edad si aún no ha llegado el día de su cumpleaños este año
            if (FechaNacimiento.Date > fechaActual.AddYears(-edad))
            {
                edad--;
            }

            return edad;
        }

    }
}
