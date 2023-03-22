using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class GenteDeMarEstadoRepository : GenericRepository<GENTEMAR_ESTADO>
    {
        /// <summary>
        /// Lista de estados
        /// </summary>
        /// <returns>Lista de estados</returns>
        /// <tabla>GENTEMAR_ESTADO</tabla>
        public IList<GENTEMAR_ESTADO> GetEstado()
        {
            var resultado = (from a in _context.GENTEMAR_ESTADO
                             select a).OrderBy(p => p.descripcion).ToList();
            return resultado;
        }
    }
}
