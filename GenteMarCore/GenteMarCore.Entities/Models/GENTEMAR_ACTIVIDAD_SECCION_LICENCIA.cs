using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenteMarCore.Entities.Models
{
    [Table("GENTEMAR_ACTIVIDAD_SECCION_LICENCIA", Schema = "DBA")]
    public partial class GENTEMAR_ACTIVIDAD_SECCION_LICENCIA
    {
        [Key]
        public int id_actividad_seccion_licencia { get; set; }
        public int id_actividad { get; set; }
        public int id_seccion { get; set; }
        public virtual GENTEMAR_ACTIVIDAD GENTEMAR_ACTIVIDAD { get; set; }
        public virtual GENTEMAR_SECCION_LICENCIAS GENTEMAR_SECCION_LICENCIAS { get; set; }
    }
}
