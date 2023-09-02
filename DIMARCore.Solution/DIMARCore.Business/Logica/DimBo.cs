using DIMARCore.Repositories.Repository;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;

namespace DIMARCore.Business.Logica
{
    public class DimBO
    {
        public List<DIM_IMPRESION> GetDimImpresionId(string id)
        {
            return new DimRepository().GetDimImpresionId(id);
        }

    }
}
