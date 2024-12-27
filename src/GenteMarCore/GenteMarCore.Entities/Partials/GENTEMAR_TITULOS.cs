using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.Models
{
    public partial class GENTEMAR_TITULOS
    {
        [NotMapped]
        public GENTEMAR_OBSERVACIONES_TITULOS Observacion { get; set; }
        [NotMapped]
        public List<CargosTitulo> Cargos { get; set; }
    }
    public class CargosTitulo
    {
        [NotMapped]
        [Required(ErrorMessage = "id tablas requeridos requeridas.")]
        public IdsRelacionCargoRegla IdsRelacion { get; set; }
        [NotMapped]
        public int CargoReglaId { get; set; }
        [NotMapped]
        public long TituloCargoReglaId { get; set; }
        [NotMapped]
        public List<int> HabilitacionesId { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Se requiere por lo menos una función.")]
        public List<int> FuncionesId { get; set; }
    }

    public class IdsRelacionCargoRegla
    {
        [Required(ErrorMessage = "El id de nivel es requerido.")]
        public int NivelId { get; set; }
        [Required(ErrorMessage = "El id de regla es requerido.")]
        public int ReglaId { get; set; }
        [Required(ErrorMessage = "El id de cargo es requerido.")]
        public int CargoId { get; set; }
        [Required(ErrorMessage = "El id de cargo es requerido.")]
        public int CapacidadId { get; set; }
    }
}
