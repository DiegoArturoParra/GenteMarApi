using System;

namespace DIMARCore.UIEntities.DTOs
{
    public class RadicadoInfoDTO
    {
        public string Radicado { get; set; }
        public int Conteo { get; set; }
        public DateTime FechaRadicado { get; set; }
        public string NumeroIdentificacionSGDEA { get; set; }
        public string FechaRadicadoFormat => FechaRadicado.ToString("dd/MM/yyyy");
        public string Identificacion { get; set; }
        public string NombreCompleto => !string.IsNullOrWhiteSpace(this.Nombres) ? $"{Nombres} {Apellidos}" : string.Empty;
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int TipoDocumentoId { get; set; }
        public string TipoDocumento { get; set; }
        public string FechaNacimientoFormat => FechaNacimiento.HasValue ? FechaNacimiento.Value.ToString("dd/MM/yyyy") : null;
    }
}
