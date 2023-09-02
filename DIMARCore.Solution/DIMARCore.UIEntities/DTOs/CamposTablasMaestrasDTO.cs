namespace DIMARCore.UIEntities.DTOs
{
    public class CamposTablasMaestrasDTO
    {
        public int? Id { get; set; }
        public string Descripcion { get; set; }
        public bool IsActive { get; set; } = true;
        public string Estado => IsActive ? "Activo" : "Anulado";
    }
}
