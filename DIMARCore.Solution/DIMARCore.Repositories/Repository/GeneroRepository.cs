using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace DIMARCore.Repositories.Repository
{
    public class GeneroRepository : GenericRepository<APLICACIONES_GENERO>
    {
        public IList<APLICACIONES_GENERO> GetGenero()
        {
            var resultado = (from a in this.Table
                             select a
                             ).OrderBy(p => p.DESCRIPCION).ToList();

            return resultado;
        }
    }
}
