using DIMARCore.Repositories;
using DIMARCore.Repositories.Repository;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DIMARCore.Business
{
    public class MunicipiosBO
    {
        /// <summary>
        /// Obtiene listado de Municipios
        /// </summary>
        /// <returns>Lista de municipios</returns>
        /// <tabla>APLICACIONES_MUNICIPIO</tabla>
        public async Task<IList<APLICACIONES_MUNICIPIO>> GetMunicipios()
        {
            // Obtiene la lista de Municipios
            return await new MunicipiosRepository().GetMunicipios();
        }

    }
}
