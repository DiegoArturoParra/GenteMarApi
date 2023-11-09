using System.ComponentModel;

namespace DIMARCore.Utilities.Enums
{
    public enum TipoDocumentoEnum
    {
        [Description("CÉDULA DE CIUDADANIA")]
        CC = 1,
        [Description("CÉDULA DE EXTRANJERÍA")]
        CE = 2,
        [Description("TARJETA DE IDENTIDAD")]
        TI = 3,
        [Description("ID")]
        ID = 4,
        [Description("PASAPORTE")]
        PA = 5,
        [Description("NÚMERO TRIBUTACIÓN UNITARIO")]
        NIT = 6,
        [Description("NÚMERO ÚNICO DE IDENTIFICACIÓN PERSONAL")]
        NUIP = 7,
        [Description("PERMISO POR PROTECCIÓN TEMPORAL")]
        PPT = 8,
        [Description("PERMISO ESPECIAL DE PERMANENCIA")]
        PEP = 9,
        [Description("NO APLICA")]
        N_A = 10
    }
}
