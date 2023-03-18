using GenteMarCore.Entities;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class MunicipiosRepository
    {
        GenteDeMarCoreContext contexto = new GenteDeMarCoreContext();

        /// <summary>
        /// Obtiene la lista de municipios/banderas
        /// </summary>
        /// <returns>Lista de paises/banderas</returns>
        /// <tabla>t_nav_band</tabla>
        public async Task<IList<APLICACIONES_MUNICIPIO>> GetMunicipios()
        {

            // Obtiene la lista
            return await this.contexto.APLICACIONES_MUNICIPIO.OrderBy(x => x.NOMBRE_MUNICIPIO).ToListAsync();
        }
    }
}
