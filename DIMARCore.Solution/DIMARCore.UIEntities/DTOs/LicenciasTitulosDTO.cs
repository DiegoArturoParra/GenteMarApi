using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIMARCore.UIEntities.DTOs
{
    public class LicenciasTitulosDTO
    {
        public List<LicenciaListarDTO> Licencias { get; set; }
        public List<ListadoTituloDTO> Titulos { get; set; }

    }
}
