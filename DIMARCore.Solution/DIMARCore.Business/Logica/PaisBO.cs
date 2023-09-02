using DIMARCore.Repositories.Repository;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Helpers;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DIMARCore.Business
{
    public class PaisBO
    {
        /// <summary>
        /// Obtiene un País dado su id
        /// </summary>
        /// <param name="codigoPais">Identificador (código del País)</param>
        /// <returns>Una país</returns>
        public PAISES Get(string codigoPais)
        {
            return new PaisRepository().Find(codigoPais);
        }

        /// <summary>
        /// Obtiene listado de paises/banderas
        /// </summary>
        /// <returns>Lista de paises/banderas</returns>
        /// <tabla>t_nav_band</tabla>
        public async Task<IList<PAISES>> GetPaises()
        {
            // Obtiene la lista de paises
            return await new PaisRepository().GetPaises();
        }

   

        public async Task<IList<PAISES>> GetPaisColombia()
        {
            // obtiene colombia 
            return await new PaisRepository().GetPaisColombia(Constantes.COLOMBIA_CODIGO);
        }
    }
}
