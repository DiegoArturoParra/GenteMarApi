using System;
using System.ComponentModel.DataAnnotations;

namespace GenteMarCore.Entities.Helpers
{
    public class GENTEMAR_CAMPOS_AUDITORIA
    {
        [Required]
        public string usuario_creador_registro { get; set; }
        public DateTime fecha_hora_creacion { get; set; }
        public string usuario_actualiza_registro { get; set; }
        public DateTime? fecha_hora_actualizacion { get; set; }
    }
}
