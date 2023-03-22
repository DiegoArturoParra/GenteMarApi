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

        /// <summary>
        /// Obtiene listado de paises/banderas extranjeros - se excluye Colombia
        /// </summary>
        /// <returns>Lista de paises/banderas</returns>
        /// <tabla>t_nav_band</tabla>
        public IList<PAISES> GetPaisesExtranjeros()
        {
            // Obtiene la lista de paises extranjeros - se excluye Colombia
            return new PaisRepository().GetPaisesExtranjeros(Constantes.COLOMBIA_CODIGO);
        }

        /// <summary>
        /// Obtiene todos las banderas del sistema con los datos básicos
        /// </summary>
        /// <returns>Lista de banderas con datos básicos</returns>
        public IList<BanderaBasic> GetBanderasBasic()
        {
            // Obtiene la lista
            return new PaisRepository().GetBanderasBasic().OrderBy(x => x.Descripcion).ToList();
        }

        public async Task<IList<PAISES>> GetPaisColombia()
        {
            // obtiene colombia 
            return await new PaisRepository().GetPaisColombia(Constantes.COLOMBIA_CODIGO);
        }

        /// <summary>
        /// Autor: Harold Andres Zamora
        /// Correo: hamz42@hotmail.com
        /// Fecha: 12 de Febrero de 2021
        /// Descripción: Obtiene la lista de banderas dada un condición
        /// </summary>
        /// <param name="whereCondition">Condición de búsqueda</param>
        /// <returns>Lista de banderas que cumplen con la condición</returns>
        public IList<PAISES> GetAll(Expression<Func<PAISES, bool>> whereCondition)
        {
            try
            {
                return new PaisRepository().GetAll(whereCondition).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
