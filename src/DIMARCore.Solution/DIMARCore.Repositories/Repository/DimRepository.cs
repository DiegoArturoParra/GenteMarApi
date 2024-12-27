using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class DimRepository : GenericRepository<DIM_IMPRESION>
    {
        public async Task<List<DIM_IMPRESION>> GetDimImpresionIdAsync(string id)
        {
            return await Table.Where(x => x.cedula.Equals(id)).AsNoTracking().ToListAsync();
        }
    }
}
