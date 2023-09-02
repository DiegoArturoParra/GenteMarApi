using System.ComponentModel;

namespace DIMARCore.Utilities.Enums
{
    public enum EstadoMultaEnum
    {
        [Description("PENDIENTE")]
        pendiente = 1,
        [Description("VIGENTE")]
        Vigente = 2,
        [Description("TERMINADO")]
        Terminado = 3,
        [Description("ANULADO")]
        Anulado = 4,
    }
}
