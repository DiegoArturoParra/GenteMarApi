using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;


namespace DIMARCore.Repositories.Repository
{
    public class LimitacionRepository : GenericRepository<GENTEMAR_LIMITACION>
    {
        /// <summary>
        /// Lista de Limitaciones
        /// </summary>
        /// <returns>Lista de Limitaciones</returns>
        /// <tabla>GENTEMAR_LIMITACION</tabla>
        public async Task<IList<GENTEMAR_LIMITACION>> GetLimitaciones()
        {

            var resultado = await (from a in Table
                                   select a
                             ).OrderBy(p => p.limitaciones).AsNoTracking().ToListAsync();
            return resultado;

        }    
    }

}
