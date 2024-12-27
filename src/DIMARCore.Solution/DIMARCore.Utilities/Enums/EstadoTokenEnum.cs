using System.ComponentModel;

namespace DIMARCore.Utilities.Enums
{
    /// <summary>
    /// Enumerador de estados token Estado (0 = Inactivo, 1 = Activo)
    /// </summary>
    public enum EstadoTokenEnum
    {
        [Description("Inactivo")]
        Inactivo = 0,
        [Description("Activo")]
        Activo = 1
    };
}
