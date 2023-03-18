using GenteMarCore.Entities;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace DIMARCore.Repositories.Repository
{
    public class LimitacionRepository : GenericRepository<GENTEMAR_LIMITACION>
    {
        GenteDeMarCoreContext contexto = new GenteDeMarCoreContext();

        /// <summary>
        /// Lista de Limitaciones
        /// </summary>
        /// <returns>Lista de Limitaciones</returns>
        /// <tabla>GENTEMAR_LIMITACION</tabla>
        public IList<GENTEMAR_LIMITACION> GetLimitaciones()
        {
            try
            {
                var resultado = (from a in this.contexto.GENTEMAR_LIMITACION
                                 select a
                                 ).OrderBy(p => p.limitaciones).ToList();

                //var resultado = contexto.GENTEMAR_LIMITACION.OrderBy(e => e.limitaciones).ToList();

                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Limitaciones dado el Id
        /// </summary>
        /// <param name="id">Id de la Limitacion</param>
        /// <returns>LIMITACIONES</returns>
        /// <tabla>GENTEMAR_LIMITACION</tabla>
        public GENTEMAR_LIMITACION GetLimitacion(int id)
        {
            try
            {
                var resultado = (from c in this.contexto.GENTEMAR_LIMITACION
                                 where c.id_limitacion == id
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
