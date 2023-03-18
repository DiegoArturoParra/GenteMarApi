using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIMARCore.UIEntities.DTOs
{
    public class EstadoDTO
    {
        public int? id_estado { get; set; }
        [Required(ErrorMessage = "Descripción Requerida.")]
        public string descripcion { get; set; }
        [StringLength(11, ErrorMessage = "Debe tener una longitud mínima de {2} y una longitud máxima de {1}.", MinimumLength = 2)]
        [Required(ErrorMessage = "Sigla Requerida.")]
        public string sigla { get; set; }
        public bool? activo { get; set; }
    }
}
