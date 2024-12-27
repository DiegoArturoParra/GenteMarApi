using System;

namespace DIMARCore.UIEntities.DTOs
{
    public class ValidacionVciteGenteMarDTO
    {
        public string NombreCompleto { get; set; }
        public string Identificacion { get; set; }
        public string Estado { get; set; }
        public DateTime? FechaVigencia { get; set; }
    }
}
