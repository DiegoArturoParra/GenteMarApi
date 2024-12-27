using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class RangoRepository : GenericRepository<APLICACIONES_RANGO>
    {
        /// <summary>
        /// Lista de grados si el estado es true  trae todos los activos si es false devuelve todos los registros 
        /// </summary>
        /// <returns>Lista de los tipos de grados</returns>
        public async Task<IList<APLICACIONES_RANGO>> GetRangosAsync(bool estado)
        {
            var resultado = (from a in _context.APLICACIONES_RANGO
                             select a);
            if (estado)
            {
                resultado = resultado.Where(x => x.activo == estado);
            }
            var data = await resultado.OrderBy(p => p.rango).AsNoTracking().ToListAsync();
            return data;
        }
    }
}
