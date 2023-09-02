using System;

namespace GenteMarCore.Entities.Models
{
    public partial class SGDEA_PREVISTAS : ICloneable
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
