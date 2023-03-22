using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIMARCore.Utilities.Enums
{
    public enum EnumEstados
    {
        [Description("Vigente")]
        VIGENTE = 1,
        [Description("NoVigente")]
        NOVIGENTE = 2,
        [Description("Proceso")]
        PROCESO = 3,
        [Description("Cancelado")]
        CANCELADO = 4,
    }
}
