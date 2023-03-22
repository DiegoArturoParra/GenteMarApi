using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIMARCore.UIEntities.DTOs
{
    public class LicenciaListarDTO
    {
        public long Id { get; set; }
        public string Regla { get; set; }
        public string CargoLicencia { get; set; }
        public string NombreUsuario { get; set; }
        public string DocumentoIdentificacion { get; set; }
        public string EstadoTramite { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public DateTime? FechaExpedicion { get; set; }
        public string Radicado { get; set; }

    }
}
