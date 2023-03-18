using GenteMarCore.Entities;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class LimitanteRepository : GenericRepository<GENTEMAR_LIMITANTE>
    {
        GenteDeMarCoreContext contexto = new GenteDeMarCoreContext();

        /// <summary>
        /// Lista de Limitante
        /// </summary>
        /// <returns>Lista de Limitante</returns>
        /// <tabla>GENTEMAR_LIMITACION</tabla>
        public IList<GENTEMAR_LIMITANTE> GetLimitantes()
        {
            try
            {
                var resultado = (from a in this.contexto.GENTEMAR_LIMITANTE
                                 select a
                                 ).OrderBy(p => p.descripcion).ToList();

                //var resultado = contexto.GENTEMAR_LIMITACION.OrderBy(e => e.limitaciones).ToList();

                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Limitante dado el Id
        /// </summary>
        /// <param name="id">Id de la Limitacion</param>
        /// <returns>LIMITACIONES</returns>
        /// <tabla>GENTEMAR_LIMITACION</tabla>
        public GENTEMAR_LIMITANTE GetLimitante(int id)
        {
            try
            {
                var resultado = (from c in this.contexto.GENTEMAR_LIMITANTE
                                 where c.id_limitante == id
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
