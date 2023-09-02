using System.IO;

namespace DIMARCore.UIEntities.DTOs
{
    public class ArchivoBaseDTO
    {
        public string ArchivoBase64 { get; set; }
        public string RutaArchivo { get; set; }
        public string Extension => Path.GetExtension(RutaArchivo);
    }
}
