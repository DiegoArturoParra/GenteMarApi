using GenteMarCore.Entities;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace DIMARCore.Repositories
{
    public class TerritorioRepository : GenericRepository<GENTEMAR_TERRITORIO>
    {
        GenteDeMarCoreContext contexto = new GenteDeMarCoreContext();

        /// <summary>
        /// Lista de Territorios
        /// </summary>
        /// <returns>Lista de territorios</returns>
        /// <tabla>GENTEMAR_TERRITORIO</tabla>
        public IList<GENTEMAR_TERRITORIO> GetTerritorios()
        {
            try
            {
                var resultado = (from a in this.contexto.GENTEMAR_TERRITORIO
                                 select a
                                 ).OrderBy(p => p.territorio).ToList();

                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Territorio dado el Id
        /// </summary>
        /// <param name="id">Id del Territorio</param>
        /// <returns>Territorio</returns>
        /// <tabla>GENTEMAR_TERRITORIO</tabla>
        public GENTEMAR_TERRITORIO GetTerritorio(int id)
        {
            try
            {
                var resultado = (from c in this.contexto.GENTEMAR_TERRITORIO
                                 where c.id_territorio == id
                                 select c
                                ).FirstOrDefault();

                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
