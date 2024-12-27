using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class PaisRepository : GenericRepository<PAISES>
    {

        /// <summary>
        /// Obtiene una bandera de la base de datos
        /// </summary>
        /// <param name="codigoPais">Identificador (código del País)</param>
        /// <returns>Una bandera</returns>
        public PAISES Find(string codigoPais)
        {
            return _context.TABLA_NAV_BAND.Find(codigoPais);
        }

        /// <summary>
        /// Obtiene la lista de paises/banderas
        /// </summary>
        /// <returns>Lista de paises/banderas</returns>
        /// <tabla>t_nav_band</tabla>
        public async Task<IList<PAISES>> GetPaises()
        {
            // Obtiene la lista
            return await Table.OrderBy(x => x.des_pais).ToListAsync();

        }

        /// <summary>
        /// Obtiene la lista de paises/banderas
        /// </summary>
        /// <returns>Lista de paises/banderas</returns>
        /// <tabla>t_nav_band</tabla>
        public IList<PAISES> GetPaisesExtranjeros(string codigoPaisColombia)
        {
            // Obtiene la lista
            return Table.Where(x => x.cod_pais != codigoPaisColombia).OrderBy(x => x.des_pais).ToList();
        }

        public async Task<IList<PAISES>> GetPaisColombia(string COLOMBIA_CODIGO)
        {
            return await Table.Where(x => x.cod_pais.Equals(COLOMBIA_CODIGO)).ToListAsync();
        }
    }

}
