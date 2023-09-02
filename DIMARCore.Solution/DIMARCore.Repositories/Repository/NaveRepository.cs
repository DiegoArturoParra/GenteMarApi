using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class NaveRepository : GenericRepository<NAVES_BASE>
    {

        /// <summary>
        /// Obtiene el listado de naves 
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<NAVES_BASE>> GetNaves()
        {
            // Obtiene la lista
            return await Table.Where(x => x.cod_pais == Constantes.COLOMBIA_CODIGO && x.activa == Constantes.NAVEACTIVA)
                .OrderBy(x => x.nom_naves).ToListAsync();

        }
    }
}
