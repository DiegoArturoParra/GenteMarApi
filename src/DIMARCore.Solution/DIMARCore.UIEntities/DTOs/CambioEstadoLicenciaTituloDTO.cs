using GenteMarCore.Entities.Models;
using System.Collections.Generic;

namespace DIMARCore.UIEntities.DTOs
{
    public class CambioEstadoLicenciaTituloDTO
    {
        public string Estado { get; set; }
        public bool IsSendEmail { get; set; }
        public string Mensaje { get; set; }
        public IEnumerable<GENTEMAR_LICENCIAS> Licencias { get; set; }
        public IEnumerable<GENTEMAR_TITULOS> Titulos { get; set; }
    }
}
