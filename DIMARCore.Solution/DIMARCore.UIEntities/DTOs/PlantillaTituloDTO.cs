using System;
using System.Collections.Generic;

namespace DIMARCore.UIEntities.DTOs
{
    public class PlantillaTituloDTO
    {
        public string Foto { get; set; }
        public long NumeroTitulo { get; set; }
        public string NombreCompleto { get; set; }
        public string Documento { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string CiudadExpedicion { get; set; }
        public string CapitaniaFirmante { get; set; }
        public string TableFunciones { get; set; } = "";
        public string TableCargos { get; set; } = "";
        public DateTime FechaExpedicion { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string Radicado { get; set; }
        public List<string> Reglas { get; set; }
        public dynamic Cargo { get; set; }
        public dynamic Funcion { get; set; }
        public string FirmaBase64 { get; set; }
    }
}
