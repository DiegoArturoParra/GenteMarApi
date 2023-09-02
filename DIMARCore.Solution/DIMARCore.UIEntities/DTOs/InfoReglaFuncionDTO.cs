using System.Collections.Generic;

namespace DIMARCore.UIEntities.DTOs
{
    public class InfoReglaFuncionDTO
    {
        public int ReglaId { get; set; }
        public string Regla { get; set; }
        public List<InfoFuncionDTO> Funciones { get; set; }
    }
    public class InfoFuncionDTO
    {
        public int TablaIntermediaId { get; set; }
        public string Descripcion { get; set; }
        public bool IsActive { get; set; }
        public int Id { get; set; }
    }
}
