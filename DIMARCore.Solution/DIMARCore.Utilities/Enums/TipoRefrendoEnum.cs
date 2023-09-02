using System.ComponentModel;

namespace DIMARCore.Utilities.Enums
{
    public enum TipoRefrendoEnum
    {
        [Description("REFRENDO NACIONAL")]
        RefrendoNacional = 1,
        [Description("REFRENDO INTERNACIONAL")]
        RefrendoInternacional = 2,
        [Description("TITULO")]
        Titulo = 3
    }
}
