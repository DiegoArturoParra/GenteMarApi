using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class AclaracionEstupefacienteDTO
    {
        public long AclaracionId { get; set; }
        [Required(ErrorMessage = "Entidad id requerida.")]
        public int EntidadId { get; set; }
        [StringLength(5000, ErrorMessage = "Debe tener una longitud máxima de {1} caracteres.")]
        public string Descripcion { get; set; }
        public DateTime? FechaRespuestaEntidad { get; set; }
    }

    public class AclaracionCreateDTO
    {
        [Required(ErrorMessage = "Antecedente id requerida.")]
        public long AntecedenteId { get; set; }
        public IList<AclaracionEstupefacienteDTO> Aclaraciones { get; set; }
    }

    public class AclaracionEditDTO
    {
        [Required(ErrorMessage = "Antecedente id requerida.")]
        public long AntecedenteId { get; set; }
        public IList<AclaracionEstupefacienteDTO> Aclaraciones { get; set; }
        public ObservacionDTO Observacion { get; set; }
    }

    public class DetalleAclaracionesEstupefacienteDTO : AclaracionEstupefacienteDTO
    {
        public string Entidad { get; set; }
        public long AntecedenteId { get; set; }
        public string Radicado { get; set; }
    }
}
