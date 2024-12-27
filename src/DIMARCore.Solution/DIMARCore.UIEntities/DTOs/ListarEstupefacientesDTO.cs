using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DIMARCore.UIEntities.DTOs
{

    public class ListarEstupefacientesDTO: AuditoriaDTO
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Radicado { get; set; }
        public string Capitania { get; set; }
        public string Tramite { get; set; }
        public string Documento { get; set; }
        public string Estado { get; set; }
        [JsonIgnore]
        public string Vigencia { get; set; }

        public VciteVigenciaDTO DetalleVigencia => new VciteVigenciaDTO(this.Vigencia, this.FechaVigencia);
        public string NumeroConsolidado { get; set; }
        [JsonIgnore]
        public DateTime? FechaSolicitudSedeCentral { get; set; }
        [JsonIgnore]
        public DateTime? FechaAprobacion { get; set; }
        [JsonIgnore]
        public DateTime? FechaVigencia { get; set; }
        public List<ExpedienteEntidadDTO> ExpedientesPorEntidad { get; set; }

        public String FechaSolicitudSedeCentralFormato
        {
            get
            {
                return this.FechaSolicitudSedeCentral.HasValue ? string.Format("{0:dd/MM/yyyy}", this.FechaSolicitudSedeCentral.Value) : "";
            }
        }

        public String FechaAprobacionFormato
        {
            get
            {
                return this.FechaAprobacion.HasValue ? string.Format("{0:dd/MM/yyyy}", this.FechaAprobacion.Value) : "Ninguna";
            }
        }
        public String FechaVigenciaFormato
        {
            get
            {
                return this.FechaVigencia.HasValue ? string.Format("{0:dd/MM/yyyy}", this.FechaVigencia.Value) : "Ninguna";
            }
        }
    }

    public class VciteVigenciaDTO
    {
        string Vigencia;
        DateTime? FechaVigencia;
        public VciteVigenciaDTO()
        {

        }
        public VciteVigenciaDTO(string vigencia, DateTime? fechaVigencia)
        {
            this.Vigencia = vigencia;
            this.FechaVigencia = fechaVigencia;
        }
        public bool EsVigente => Vigencia.ToUpper().Equals("SI");
        public bool ContieneFechaVigencia => FechaVigencia.HasValue;
    }
}
