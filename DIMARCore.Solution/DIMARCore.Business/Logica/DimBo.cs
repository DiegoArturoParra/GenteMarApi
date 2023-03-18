using DIMARCore.Repositories.Repository;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
