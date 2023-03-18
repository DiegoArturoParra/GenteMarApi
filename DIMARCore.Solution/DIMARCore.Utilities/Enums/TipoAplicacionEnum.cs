using System.ComponentModel;

namespace DIMARCore.Utilities.Enums
{
    /// <summary>
    /// Enumerador de id tipo aplicaciones
    /// </summary>
    public enum TipoAplicacionEnum
    {
        [Description("Web")]
        Web = 1,
        [Description("Aplicación Movil")]
        AplicacionMovil = 2,
        [Description("Servicio Externo")]
        ServicioExterno = 3,
        [Description("Servicio local")]
        ServicioLocal = 4,
        [Description("Gente De Mar")]
        GenteDeMar = 10
    };
}
