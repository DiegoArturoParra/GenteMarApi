using System.ComponentModel;

namespace DIMARCore.Utilities.Enums
{
    public enum EstadoGenteMarEnum
    {
        [Description("ACTIVO")]
        ACTIVO = 1,
        [Description("INACTIVO")]
        INACTIVO = 2,
        [Description("FALLECIDO")]
        FALLECIDO = 3,
        [Description("EN PROCESO")]
        ENPROCESO = 4
    };
}
