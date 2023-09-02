using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Linq;


namespace DIMARCore.Repositories.Repository
{
    public class LimitacionRepository : GenericRepository<GENTEMAR_LIMITACION>
    {
        /// <summary>
        /// Lista de Limitaciones
        /// </summary>
        /// <returns>Lista de Limitaciones</returns>
        /// <tabla>GENTEMAR_LIMITACION</tabla>
        public IList<GENTEMAR_LIMITACION> GetLimitaciones()
        {

            var resultado = (from a in this.Table
                             select a
                             ).OrderBy(p => p.limitaciones).ToList();
            return resultado;

        }


        /// <summary>
        /// Limitaciones dado el Id
        /// </summary>
        /// <param name="id">Id de la Limitacion</param>
        /// <returns>LIMITACIONES</returns>
        /// <tabla>GENTEMAR_LIMITACION</tabla>
        public GENTEMAR_LIMITACION GetLimitacion(int id)
        {
            var resultado = (from c in this.Table
                             where c.id_limitacion == id
                             select c
                            ).FirstOrDefault();

            return resultado;
        }
    }

}
