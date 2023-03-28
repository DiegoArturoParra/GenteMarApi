using DIMARCore.Utilities.Helpers;

namespace DIMARCore.UIEntities.DTOs
{
    public class RadicadoDTO
    {
        public string Radicado { get; set; }
        public int Conteo { get; set; }
        public string RadicadoPuntos => Reutilizables.ConvertirStringApuntosDeMil(this.Radicado);
    }

}