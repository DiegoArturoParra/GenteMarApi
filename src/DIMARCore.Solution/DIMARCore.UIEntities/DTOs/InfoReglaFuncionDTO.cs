using System.Collections.Generic;

namespace DIMARCore.UIEntities.DTOs
{
    public class InfoReglaFuncionDTO
    {
        public int ReglaId { get; set; }
        public string Regla { get; set; }
        public List<InfoFuncionDTO> Funciones { get; set; }
    }

}
