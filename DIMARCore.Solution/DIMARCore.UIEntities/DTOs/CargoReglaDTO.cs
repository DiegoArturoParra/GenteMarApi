using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class CargoReglaDTO : IdsTablasForaneasDTO
    {
        public int CargoReglaId { get; set; }
        public int SeccionId { get; set; }
        public List<int> HabilitacionesId { get; set; }
    }
}
