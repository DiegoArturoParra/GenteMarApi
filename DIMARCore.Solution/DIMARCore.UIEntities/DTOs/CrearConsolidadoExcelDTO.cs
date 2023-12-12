using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class CrearConsolidadoExcelDTO
    {
        [Required(ErrorMessage = "Consolidado Requerido.")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números.")]
        [StringLength(12, ErrorMessage = "Longitud máxima de {1} caracteres y mínimo de {2} caracteres.", MinimumLength = 1)]
        public string Consolidado { get; set; }
        public bool IsNew { get; set; }
        public List<CrearExpedienteEntidadDTO> ArrayExpedientesEntidad { get; set; }
    }
    public class CrearExpedienteEntidadDTO
    {
        [Required(ErrorMessage = "Consolidado Requerido.")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números.")]
        [StringLength(12, ErrorMessage = "Longitud máxima de {1} caracteres y mínimo de {2} caracteres.", MinimumLength = 4)]
        public string NumeroExpediente { get; set; }
        public int EntidadId { get; set; }
    }
}
