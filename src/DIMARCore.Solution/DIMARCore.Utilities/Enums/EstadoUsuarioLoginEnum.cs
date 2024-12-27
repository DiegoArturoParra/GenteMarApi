using System.ComponentModel;

namespace DIMARCore.Utilities.Enums
{
    public enum EstadoUsuarioLoginEnum
    {
        [Description("Inactivo")]
        INACTIVO = 0,
        [Description("Activo")]
        ACTIVO = 1,
        [Description("Vacaciones")]
        VACACIONES = 2,
        [Description("Retiro")]
        RETIRO = 3,
        [Description("Comision")]
        COMISION = 4,
        [Description("Licencia Materna")]
        LICENCIAMATERNA = 5,
        [Description(" Licencia Paterna")]
        LICENCIAPATERNA = 6,
        [Description(" Luto")]
        LUTO = 7,
        [Description("Usuario Nuevo")]
        USUARIONUEVO = 8,

    }
}
