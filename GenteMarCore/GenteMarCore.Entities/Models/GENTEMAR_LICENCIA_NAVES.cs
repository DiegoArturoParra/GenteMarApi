using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenteMarCore.Entities.Models
{
    [Table("GENTEMAR_LICENCIA_NAVES", Schema = "DBA")]
    public partial class GENTEMAR_LICENCIA_NAVES
    {
        [Key]
        public long id_licencia_nave { get; set; }
        public long id_licencia { get; set; }
        public string identi { get; set; }
    }
}
