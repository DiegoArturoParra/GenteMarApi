using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class RelacionCargosReglaDTO
    {
        [Required(ErrorMessage = "id tablas requeridos requeridas.")]
        public IdsTablasForaneasDTO IdsLlaveCompuesta { get; set; }
        public int CargoReglaId { get; set; }
        public long TituloCargoReglaId { get; set; }
        public List<int> HabilitacionesId { get; set; }
        [Required(ErrorMessage = "Se requiere por lo menos una función.")]
        public List<int> FuncionesId { get; set; }
    }
}
