using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;

namespace DIMARCore.UIEntities.DTOs
{
    public class PlantillaLicenciaDTO
    {
        public string Foto { get; set; }
        public GENTEMAR_TIPO_LICENCIA TipoLicencia { get; set; }
        public GENTEMAR_ACTIVIDAD ActividadLicencia { get; set; }
        public long NumeroLicencia { get; set; }
        public string NombreCompleto { get; set; }
        public string Documento { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string CiudadExpedicion { get; set; }
        public string NombreLicencia { get; set; }
        public string CapitaniaFirmante { get; set; }
        public string ParametroDinamico { get; set; } = "";
        public List<string> Limitantes { get; set; }
        public List<string> Limitacion { get; set; }
        public List<NavesImpDocDTO> Naves { get; set; }
        public DateTime FechaExpedicion { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal Radicado { get; set; }
        public string TipoDocumento { get; set; }
        public string Genero { get; set; }
        public long? IdLicenciaTituloPep { get; set; }
        public string LicenciaTituloPep { get; set; }
        public long IdGentemar { get; set; }


    }
}
