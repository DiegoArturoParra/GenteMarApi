namespace DIMARCore.UIEntities.DTOs
{
    public class CambioEstadoLicenciaDTO
    {
        public long IdLicencia { get; set; }
        public bool? Activo { get; set; }
        public ObservacionDTO Observacion { get; set; }
    }
}
