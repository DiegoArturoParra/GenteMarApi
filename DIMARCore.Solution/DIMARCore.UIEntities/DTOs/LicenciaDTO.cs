using System;
using System.Collections.Generic;

namespace DIMARCore.UIEntities.DTOs
{
    public class LicenciaDTO
    {
        public long IdLicencia { get; set; }
        public long? IdGentemar { get; set; }
        public long? IdCargoLicencia { get; set; }
        public DateTime? FechaExpedicion { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public int? IdCapitania { get; set; }
        public int? IdEstadoLicencia { get; set; }
        public decimal Radicado { get; set; }
        public int? IdCapitaniaFirmante { get; set; }
        public bool? Activo { get; set; }
        public CargoInfoLicenciaDTO CargoLicencia { get; set; }
        public CapitaniaDTO Capitania { get; set; }
        public EstadoLicenciaDTO Estado { get; set; }
        public CapitaniaDTO CapitaniaFirmante { get; set; }
        public UsuarioGenteMarDTO Usuario { get; set; }
        public ObservacionDTO Observacion { get; set; }
        public List<string> ListaNaves { get; set; }
        public DateTime MaxDateFechaVencimiento { get; set; }
        public bool ContienePrevista { get; set; }
    }
}
