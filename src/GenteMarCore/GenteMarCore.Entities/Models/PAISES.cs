using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.Models
{
    [Table("t_nav_band", Schema = "DBA")]
    public partial class PAISES
    {
        [Key]
        public string cod_pais { get; set; }
        public string des_pais { get; set; }
        public string sigla { get; set; }
        public string sigla_2 { get; set; }
        public string cod_continente { get; set; }
        public string nacionalidad { get; set; }
        public string cod_andino { get; set; }
        public string emailsNotificacion { get; set; }
        public bool? esComunidadAndina { get; set; }
        public bool? tieneConvenioNotificacion { get; set; }
    }
}
