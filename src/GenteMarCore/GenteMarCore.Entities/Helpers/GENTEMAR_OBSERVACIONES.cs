using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace GenteMarCore.Entities.Helpers
{
    public class GENTEMAR_OBSERVACIONES : GENTEMAR_CAMPOS_AUDITORIA
    {
        [Key]
        public long id_observacion { get; set; }
        public string observacion { get; set; }

        public string ruta_archivo { get; set; }
        [NotMapped]
        public string ArchivoBase64 { get; set; }

        [NotMapped]
        public HttpPostedFile Archivo { get; set; }
    }
}
