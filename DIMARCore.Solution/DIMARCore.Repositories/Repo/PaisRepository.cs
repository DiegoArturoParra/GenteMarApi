using GenteMarCore.Entities;
using GenteMarCore.Entities.Helpers;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DIMARCore.Repositories
{
    public class PaisRepository
    {
        GenteDeMarCoreContext contexto = new GenteDeMarCoreContext();

        /// <summary>
        /// Obtiene una bandera de la base de datos
        /// </summary>
        /// <param name="codigoPais">Identificador (código del País)</param>
        /// <returns>Una bandera</returns>
        public PAISES Find(string codigoPais)
        {
            try
            {
                return this.contexto.t_nav_band.Find(codigoPais);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Obtiene la lista de paises/banderas
        /// </summary>
        /// <returns>Lista de paises/banderas</returns>
        /// <tabla>t_nav_band</tabla>
        public async Task<IList<PAISES>> GetPaises()
        {
            try
            {    // Obtiene la lista
                return await this.contexto.t_nav_band.OrderBy(x => x.des_pais).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Obtiene la lista de paises/banderas
        /// </summary>
        /// <returns>Lista de paises/banderas</returns>
        /// <tabla>t_nav_band</tabla>
        public IList<PAISES> GetPaisesExtranjeros(string codigoPaisColombia)
        {
            try
            {    // Obtiene la lista
                return this.contexto.t_nav_band.Where(x => x.cod_pais != codigoPaisColombia).OrderBy(x => x.des_pais).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IList<PAISES>> GetPaisColombia(string COLOMBIA_CODIGO)
        {
            return await this.contexto.t_nav_band.Where(x => x.cod_pais.Equals(COLOMBIA_CODIGO)).ToListAsync();
        }

        /// <summary>
        /// Obtiene la lista de banderas de la base de datos con los datos básicos
        /// </summary>
        /// <returns>Lista de banderas</returns>
        public IList<BanderaBasic> GetBanderasBasic()
        {
            try
            {
                var banderas = this.contexto.t_nav_band.ToList();
                if (banderas == null)
                {
                    banderas = new List<PAISES>();
                }

                var resultado = (from b in banderas
                                 select new BanderaBasic
                                 {
                                     Id = b.cod_pais,
                                     Descripcion = b.des_pais,
                                 }).ToList();

                // retorna la lista
                return resultado;
            }
            catch (Exception)
            {
                throw;
            }
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
            IList<PAISES> model;
            using (var contexto = new GenteDeMarCoreContext())
            {
                model = contexto.t_nav_band.Where(whereCondition).ToList<PAISES>();
            }
            return model;
        }

    }

}
