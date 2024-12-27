using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.ViewsSQL
{
    [Table("VwGenteMarReporteAntecedentes", Schema = "DBA")]
    public class ViewGenteMarReporteAntecedentes
    {
        [Key]
        public long AntecedenteId { get; set; }
        public int EstadoId { get; set; }
        public int ConsolidadoId { get; set; }
        public int TramiteId { get; set; }
        public byte TipoDocumentoId { get; set; }
        public string Identificacion { get; set; }
        public string Apellidos { get; set; }
        public string Nombres { get; set; }
        public string TipoDocumento { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreador { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificador { get; set; }
        public DateTime FechaVigencia { get; set; }
        public string Estado { get; set; }
        public bool EstadoEsActivo { get; set; }
        public string EsVigente { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaAprobacion { get; set; }
        public DateTime FechaRadicadoSgdea { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string TipoTramite { get; set; }
        public bool TramiteEsActivo { get; set; }
        public string NumeroRadicadoSgdea { get; set; }
        public string SiglaCapitania { get; set; }
        public string NombreCapitania { get; set; }
        public string NumeroConsolidado { get; set; }
    }
}
