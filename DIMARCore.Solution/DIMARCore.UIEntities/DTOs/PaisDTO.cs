using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIMARCore.UIEntities.DTOs
{
    public class PaisDTO
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Sigla { get; set; }
        public string Sigla2 { get; set; }
        public string DescripcionCompleta => $"{Codigo} - {Descripcion}";
    }
}
