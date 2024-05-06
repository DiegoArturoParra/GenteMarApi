using System;
using System.ComponentModel.DataAnnotations;

namespace GenteMarCore.Entities.Helpers
{
    /// <summary>
    /// Campos de auditoria por columnas
    /// </summary>
    public class GENTEMAR_CAMPOS_AUDITORIA
    {
        [Required(ErrorMessage = "El login creación Id es requerido.")]
        public int LoginCreacionId { get; set; }
        [Required(ErrorMessage = "La fecha de creación es requerida.")]
        public DateTime FechaCreacion { get; set; }
        public int LoginModificacionId { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
