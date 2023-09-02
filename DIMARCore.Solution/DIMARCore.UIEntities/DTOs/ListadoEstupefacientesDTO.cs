using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DIMARCore.UIEntities.DTOs
{

    public class ListadoEstupefacientesDTO
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Radicado { get; set; }
        public string Capitania { get; set; }
        public string Tramite { get; set; }
        public string Documento { get; set; }
        public string Estado { get; set; }
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
                return this.FechaVigencia.HasValue ? string.Format("{0:dd/MM/yyyy}", this.FechaAprobacion.Value) : "Ninguna";
            }
        }
    }

    public class EstupefacientesBulkDTO
    {
        public long EstupefacienteId { get; set; }
        public string Radicado { get; set; }
        public string Documento { get; set; }
        public string NombreCompleto { get; set; }
        public string Estado { get; set; }
        public int CountObservaciones { get; set; }
        public int MyProperty { get; set; }
    }

    public class EstupefacientesExcelDTO
    {
        public long EstupefacienteId { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Radicado { get; set; }
        public string LugarTramiteCapitania { get; set; }
        public string TipoDocumento { get; set; }
        public string TipoTramite { get; set; }
        public string Documento { get; set; }
        [JsonIgnore]
        public DateTime FechaNacimiento { get; set; }
        [JsonIgnore]
        public DateTime? FechaSolicitudSedeCentral { get; set; }
        [JsonIgnore]
        public DateTime? FechaRadicado { get; set; }

        public String FechaSolicitudSedeCentralFormato
        {
            get
            {
                return this.FechaSolicitudSedeCentral.HasValue ? string.Format("{0:dd/MM/yyyy}", this.FechaSolicitudSedeCentral.Value) : "N/A";
            }
        }

        public String FechaNacimientoFormato
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy}", this.FechaNacimiento);
            }
        }
        public String FechaRadicadoFormato
        {
            get
            {
                return this.FechaRadicado.HasValue ? string.Format("{0:dd/MM/yyyy}", this.FechaRadicado.Value) : "N/A";
            }
        }
    }
}
