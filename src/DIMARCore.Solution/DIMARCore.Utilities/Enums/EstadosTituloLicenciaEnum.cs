using System.ComponentModel;

namespace DIMARCore.Utilities.Enums
{
    public enum EstadosTituloLicenciaEnum
    {
        [Description("VIGENTE")]
        VIGENTE = 1,
        [Description("NO VIGENTE")]
        NOVIGENTE = 2,
        [Description("EN PROCESO")]
        PROCESO = 3,
        [Description("CANCELADO")]
        CANCELADO = 4,
        [Description("SUSPENDIDA")]
        SUSPENDIDA = 5
    }
}
