using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
        public async Task<IList<GENTEMAR_ESTADO>> GetEstadosAsync()
        {
            var resultado = await (from a in _context.GENTEMAR_ESTADO
                                   select a).OrderBy(p => p.descripcion).AsNoTracking().ToListAsync();
            return resultado;
        }
    }
}
