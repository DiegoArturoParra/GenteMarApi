using System.ComponentModel;

namespace DIMARCore.Utilities.Enums
{
    public enum EnvironmentEnum
    {
        [Description("GenteMarContext")]
        Development = 0,
        [Description("GenteMarContextTesting")]
        Testing = 1,
        [Description("GenteMarContextProduction")]
        Production = 2
    }
    public enum EnvironmentDapperEnum
    {
        [Description("GenteMarContextDapper")]
        Development = 0,
        [Description("GenteMarContextDapperTesting")]
        Testing = 1,
        [Description("GenteMarContextDapperProduction")]
        Production = 2
    }
}
