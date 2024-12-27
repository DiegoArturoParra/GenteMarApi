using System;

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
