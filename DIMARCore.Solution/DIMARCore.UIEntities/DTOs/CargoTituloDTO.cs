namespace DIMARCore.UIEntities.DTOs
{
    public class CargoTituloDTO : CamposTablasMaestrasDTO
    {
        public int ClaseId { get; set; }
        public int SeccionId { get; set; }
    }

    public class ListadoCargoTituloDTO : CargoTituloDTO
    {
        public string Seccion { get; set; }
        public string Clase { get; set; }
    }
}
