using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DIMARCore.UIEntities.DTOs
{
    public class InfoTituloDTO
    {

        public long TituloId { get; set; }
        public int CargoId { get; set; }
        public int SeccionId { get; set; }
        public int ReglaId { get; set; }
        public int CapacidadId { get; set; }
        public int EstadoTramiteId { get; set; }
        public int TipoSolicitudId { get; set; }
        public int CapitaniaId { get; set; }
        public int CapitaniaFirmanteId { get; set; }
        public List<HabilitacionInfoDTO> Habilitaciones { get; set; }
        public List<FuncionesTituloDTO> Funciones { get; set; }
        public string Capacidad { get; set; }
        public string Regla { get; set; }
        public string Seccion { get; set; }
        public string Pais { get; set; }
        public string CargoTitulo { get; set; }
        public string Nivel { get; set; }
        public string Solicitud { get; set; }
        public string NombreUsuario { get; set; }
        public string CapitaniaFirma { get; set; }
        public string CapitaniaFirmante { get; set; }
        public string Tramite { get; set; }
        public string Radicado { get; set; }
        public string DocumentoIdentificacion { get; set; }
        [JsonIgnore]
        public DateTime FechaVencimiento { get; set; }
        [JsonIgnore]
        public DateTime FechaExpedicion { get; set; }
        public string FechaExpedicionFormato => string.Format("{0:dd/MM/yyyy}", this.FechaExpedicion);

        public string FechaVencimientoFormato => string.Format("{0:dd/MM/yyyy}", this.FechaVencimiento);

    }
    public class HabilitacionInfoDTO
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
    }

    public class FuncionesTituloDTO
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string Limitacion { get; set; }
    }
}
