using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repo
{
    public class DimRepository : GenericRepository<DIM_IMPRESION>
    {
        public List<DIM_IMPRESION> GetDimImpresionId(string id)
        {
            return _context.DimImpresion.Where(x => x.cedula.Equals(id)).ToList();
        }
    }
}
