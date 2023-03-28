using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIMARCore.UIEntities.DTOs
{
    public class HistorialDocumentoDTO
    {
        public int IdHistorialDocumento { get; set; }
        public string DocumentoIdentificacion { get; set; }
        public int IdTipoDocumento { get; set; }
        public string NombreTipoDocumento { get; set; }
        public DateTime FechaCambio { get; set; }
    }
}
