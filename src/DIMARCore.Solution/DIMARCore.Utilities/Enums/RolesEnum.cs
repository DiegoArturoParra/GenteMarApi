using System.ComponentModel;

namespace DIMARCore.Utilities.Enums
{
    public enum RolesEnum
    {
        [Description("Administrador")]
        AdministradorGDM = 29,
        [Description("Gestor sede Central")]
        GestorSedeCentral = 30,
        [Description("Gestor Capitanía")]
        Capitania = 31,
        [Description("Consultas")]
        Consultas = 32,
        [Description("Administrador Estupefacientes")]
        AdministradorVCITE = 33,
        [Description("Gestor Estupefacientes")]
        GestorVCITE = 34,
        [Description("Consultas Estupefacientes")]
        ConsultasVCITE = 35,
        [Description("Jurídica Estupefacientes")]
        JuridicaVCITE = 36,
        [Description("ASEPAC")]
        ASEPAC = 37,
    }
}
