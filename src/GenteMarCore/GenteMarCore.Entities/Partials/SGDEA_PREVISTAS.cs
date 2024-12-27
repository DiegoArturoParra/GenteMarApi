using System;
using System.ComponentModel.DataAnnotations.Schema;

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
