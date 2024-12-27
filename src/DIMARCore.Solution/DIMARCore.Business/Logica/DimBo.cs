using DIMARCore.Repositories.Repository;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class DimBO
    {
        public async Task<List<DIM_IMPRESION>> GetDimImpresionIdAsync(string id)
        {
            return await new DimRepository().GetDimImpresionIdAsync(id);
        }

    }
}
