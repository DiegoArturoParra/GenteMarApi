using System.ComponentModel;

namespace DIMARCore.Utilities.Enums
{
    public enum RolesEnum
    {
        [Description("Administrador")]
        Administrador = 29,
        [Description("Gestor sede Central")]
        GestorSedeCentral = 30,
        [Description("Gestor Capitanía")]
        Capitania = 31,
        [Description("Consultas")]
        Consultas = 32,
        [Description("Administrador Estupefacientes")]
        AdministradorEstupefacientes = 33,
        [Description("Gestor Estupefacientes")]
        GestorEstupefacientes = 34,
        [Description("Consultas Estupefacientes")]
        ConsultasEstupefacientes = 35,
        [Description("Jurídica Estupefacientes")]
        JuridicaEstupefacientes = 36,
        [Description("ASEPAC")]
        ASEPAC = 37,
    }
}
