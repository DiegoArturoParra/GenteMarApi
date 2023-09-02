using System.ComponentModel;

namespace DIMARCore.Utilities.Enums
{
    public enum EstadosTituloLicenciaEnum
    {
        [Description("Vigente")]
        VIGENTE = 1,
        [Description("NoVigente")]
        NOVIGENTE = 2,
        [Description("Proceso")]
        PROCESO = 3,
        [Description("Cancelado")]
        CANCELADO = 4,
    }
}
