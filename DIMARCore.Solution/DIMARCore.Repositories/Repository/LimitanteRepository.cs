using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace DIMARCore.Repositories.Repository
{
    public class LimitanteRepository : GenericRepository<GENTEMAR_LIMITANTE>
    {
        /// <summary>
        /// Lista de Limitante
        /// </summary>
        /// <returns>Lista de Limitante</returns>
        /// <tabla>GENTEMAR_LIMITACION</tabla>
        public IList<GENTEMAR_LIMITANTE> GetLimitantes()
        {

            var resultado = (from a in this.Table
                             select a
                             ).OrderBy(p => p.descripcion).ToList();
            return resultado;

        }


        /// <summary>
        /// Limitante dado el Id
        /// </summary>
        /// <param name="id">Id de la Limitacion</param>
        /// <returns>LIMITACIONES</returns>
        /// <tabla>GENTEMAR_LIMITACION</tabla>
        public GENTEMAR_LIMITANTE GetLimitante(int id)
        {

            var resultado = (from c in this.Table
                             where c.id_limitante == id
                             select c
                            ).FirstOrDefault();

            return resultado;
        }
    }
}
