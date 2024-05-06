using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class LimitanteRepository : GenericRepository<GENTEMAR_LIMITANTE>
    {
        /// <summary>
        /// Lista de Limitante
        /// </summary>
        /// <returns>Lista de Limitante</returns>
        /// <tabla>GENTEMAR_LIMITACION</tabla>
        public async Task<IList<GENTEMAR_LIMITANTE>> GetLimitantesAsync()
        {

            var resultado = await (from a in Table select a).OrderBy(p => p.descripcion).AsNoTracking().ToListAsync();
            return resultado;
        }
    }
}
