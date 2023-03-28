using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIMARCore.UIEntities.DTOs
{
    public class RangoDTO
    {
        public int? id_rango { get; set; }
        private string _rango;
        public string rango
        {
            get => _rango?.ToUpper();
            set => _rango = value;
        }
        public bool? activo { get; set; }
    }
}
