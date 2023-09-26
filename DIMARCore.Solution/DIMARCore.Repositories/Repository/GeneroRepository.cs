using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class GeneroRepository : GenericRepository<APLICACIONES_GENERO>
    {
        public async Task<IList<APLICACIONES_GENERO>> GetGeneros()
        {
            return await (from genero in Table select genero).OrderBy(p => p.DESCRIPCION).ToListAsync();
        }
    }
}
