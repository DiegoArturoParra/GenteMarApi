using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenteMarCore.Entities.Models
{
    [Table("GENTEMAR_SECCION_CLASE", Schema = "DBA")]
    public partial class GENTEMAR_SECCION_CLASE
    {
        [Key]
        public int id_seccion_clase { get; set; }
        public int id_clase { get; set; }
        public int id_seccion { get; set; }
        public virtual GENTEMAR_CLASE_LICENCIAS GENTEMAR_CLASE_LICENCIAS { get; set; }
        public virtual GENTEMAR_SECCION_LICENCIAS GENTEMAR_SECCION_LICENCIAS { get; set; }
    }
}
