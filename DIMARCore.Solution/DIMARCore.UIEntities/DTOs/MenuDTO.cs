using System.Collections.Generic;

namespace DIMARCore.UIEntities.DTOs
{
    public class MenuDTO
    {
        public int MenuId { get; set; }
        public string Vista { get; set; }
        public string Controlador { get; set; }
        public string Nombre { get; set; }
        public int PadreId { get; set; }
        public int AplicacionId { get; set; }
    }
}
