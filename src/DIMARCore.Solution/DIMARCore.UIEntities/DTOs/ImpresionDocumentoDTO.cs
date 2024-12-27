namespace DIMARCore.UIEntities.DTOs
{
    public class ImpresionDocumentoDTO
    {
        public bool Impreso { get; set; }
        public string PdfBase64 { get; set; }
        public string NombrePDF { get; set; }
        public decimal Radicado { get; set; }
        public string Tramite { get; set; }
    }
}
