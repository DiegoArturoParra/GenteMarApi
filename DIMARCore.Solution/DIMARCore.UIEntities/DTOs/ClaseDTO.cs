using System.Collections.Generic;

namespace DIMARCore.UIEntities.DTOs
{
    public class ClaseDTO : CamposTablasMaestrasDTO
    {
        public string Sigla { get; set; }
        public List<SeccionDTO> Seccion { get; set; }

        public int IdClaseSeccion { get; set; }


    }

}
