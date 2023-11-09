using System.ComponentModel;

namespace DIMARCore.Utilities.Enums
{
    public enum EstadoEstupefacienteEnum
    {
        [Description("VERIFICACIÓN EXITOSA")]
        Exitosa = 1,
        [Description("EN CONSULTA")]
        Consulta = 2,
        [Description("VERIFICACIÓN NEGATIVA")]
        Negativa = 3,
        [Description("POR ENVIAR")]
        ParaEnviar = 4,
        [Description("N/A")]
        Ninguno = 5,
        [Description("VCITE POSTERIOR")]
        VcitePosterior = 6,
        [Description("AMPLIACIÓN ACLARADA")]
        AmpliacionAclarada = 7
    }
}
