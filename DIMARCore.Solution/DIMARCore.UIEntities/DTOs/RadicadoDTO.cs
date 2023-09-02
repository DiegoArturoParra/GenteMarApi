using DIMARCore.Utilities.Helpers;
using System;

namespace DIMARCore.UIEntities.DTOs
{
    public class RadicadoDTO
    {
        public decimal Radicado { get; set; }
        public int Conteo { get; set; }
        public string RadicadoPuntos => Reutilizables.ConvertirStringApuntosDeMil(this.Radicado);

        public string TipoTramite { get; set; }
    }

    public class RadicadoInfoDTO
    {
        public string Radicado { get; set; }
        public int Conteo { get; set; }
        public DateTime FechaRadicado { get; set; }
        public string NumeroIdentificacionSGDEA { get; set; }
        public string FechaRadicadoFormat => FechaRadicado.ToString("dd/MM/yyyy");
        public InfoPersonaDTO Info { get; set; }
    }
    public class InfoPersonaDTO
    {
        public string Identificacion { get; set; }
        public string NombreCompleto => $"{Nombres} {Apellidos}";
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int TipoDocumentoId { get; set; }
        public string TipoDocumento { get; set; }
        public string FechaNacimientoFormat => FechaNacimiento.ToString("dd/MM/yyyy");
    }

}