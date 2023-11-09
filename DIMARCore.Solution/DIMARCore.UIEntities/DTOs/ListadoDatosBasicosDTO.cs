using System;

namespace DIMARCore.UIEntities.DTOs
{
    public class ListadoDatosBasicosDTO
    {
        public long IdGentemar { get; set; }
        public string DocumentoIdentificacion { get; set; }
        public string TipoDocumento { get; set; }
        public string MunicipioExpedicion { get; set; }
        public string Pais { get; set; }
        public DateTime? FechaExpedicion { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Genero { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string MunicipioNacimiento { get; set; }
        public string Direccion { get; set; }
        public string MunicipioResidencia { get; set; }
        public string Telefono { get; set; }
        public string CorreoElectronico { get; set; }
        public string NumeroMovil { get; set; }
        public string Estado { get; set; }
        public long IdEstado { get; set; }
        public bool? Activo { get; set; }
        public string FormacionGrado { get; set; }
        public string CarpetaFisico { get; set; }
        public DateTime FechaRegistro { get; set; }

        public String FechaExpedicionFormato
        {
            get
            {
                return this.FechaExpedicion.HasValue ? string.Format("{0:dd/MM/yyyy}", this.FechaExpedicion.Value) : "";
            }
        }

        public String FechaVencimientoFormato
        {
            get
            {
                return this.FechaVencimiento.HasValue ? string.Format("{0:dd/MM/yyyy}", this.FechaVencimiento.Value) : "";
            }
        }
        public String FechaNacimientoFormato
        {
            get
            {
                return this.FechaNacimiento.HasValue ? string.Format("{0:dd/MM/yyyy}", this.FechaNacimiento.Value) : "";
            }
        }
        public String NombreActivo
        {
            get
            {
                return this.Activo.HasValue ? " Inactivo" : "Activo";
            }
        }
    }
}
