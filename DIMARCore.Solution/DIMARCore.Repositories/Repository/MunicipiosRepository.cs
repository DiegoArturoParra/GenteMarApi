using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class MunicipiosRepository : GenericRepository<APLICACIONES_MUNICIPIO>
    {
        /// <summary>
        /// Obtiene la lista de municipios/banderas
        /// </summary>
        public async Task<IList<APLICACIONES_MUNICIPIO>> GetMunicipios()
        {
            // Obtiene la lista
            return await Table.OrderBy(x => x.NOMBRE_MUNICIPIO).AsNoTracking().ToListAsync();
        }
    }
}
