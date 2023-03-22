using DIMARCore.Repositories;
using DIMARCore.Repositories.Repository;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;

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
        public IList<APLICACIONES_GENERO> GetGenero()
        {
            // Obtiene la lista
            return new GeneroRepository().GetGenero();
        }
    }
}
