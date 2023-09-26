using DIMARCore.Repositories;
using DIMARCore.Repositories.Repository;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DIMARCore.Business
{
    public class GeneroBO
    {
        /// <summary>
        /// Lista de generos
        /// </summary>
        /// <returns>Lista de Genero</returns>
        /// <entidad>APLICACIONES_GENERO</entidad>
        /// <tabla>APLICACIONES_GENERO</tabla>
        public async Task<IList<APLICACIONES_GENERO>> GetGeneros()
        {
            // Obtiene la lista
            return await new GeneroRepository().GetGeneros();
        }
    }
}
