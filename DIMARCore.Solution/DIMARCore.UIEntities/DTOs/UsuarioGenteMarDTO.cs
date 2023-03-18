using DIMARCore.Utilities.Enums;
using System;

namespace DIMARCore.UIEntities.DTOs
{
    public class UsuarioGenteMarDTO
    {
        public long Id { get; set; }
        public string DocumentoIdentificacion { get; set; }
        public string Nombres { get; set; }
        public int Estado { get; set; }
        public int IdTipoDocumento { get; set; }
        public string NombreEstado { get; set; }
        public bool IsCreateTituloOrLicencia => ValidarCreacionTituloOLicencia();
        public string Apellidos { get; set; }
        public string NombreCompleto => $"{Nombres} {Apellidos}";
        public DateTime? FechaNacimiento { get; set; }
        public bool ExisteEnDatosBasicosEstupefaciente { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        private bool ValidarCreacionTituloOLicencia()
        {
            bool validar;
            switch (Estado)
            {
                case (int)EstadoGenteMarEnum.ACTIVO:
                    validar = true;
                    break;
                case (int)EstadoGenteMarEnum.INACTIVO:

                    validar = false;
                    break;
                case (int)EstadoGenteMarEnum.FALLECIDO:

                    validar = false;
                    break;
                case (int)EstadoGenteMarEnum.ANTECEDENTE:
                    validar = false;
                    break;
                case (int)EstadoGenteMarEnum.ENPROCESO:
                    validar = true;
                    break;
                default:
                    validar = false;
                    break;
            }
            return validar;
        }
    }
}
