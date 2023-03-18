using System.ComponentModel;

namespace DIMARCore.Utilities.Enums
{
    public enum EstadoGenteMarEnum
    {
        [Description("Activo")]
        ACTIVO = 1,
        [Description("Inactivo")]
        INACTIVO = 2,
        [Description("Fallecido")]
        FALLECIDO = 3,
        [Description("Antecedente")]
        ANTECEDENTE = 4,
        [Description("EN PROCESO")]
        ENPROCESO = 5
    };
}
