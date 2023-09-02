using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace DIMARCore.Repositories.Repository
{
    public class DimRepository : GenericRepository<DIM_IMPRESION>
    {
        public List<DIM_IMPRESION> GetDimImpresionId(string id)
        {
            return _context.TABLA_DIM_IMPRESION.Where(x => x.cedula.Equals(id)).ToList();
        }
    }
}
