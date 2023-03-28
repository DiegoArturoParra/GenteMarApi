using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class DimRegistroEmbarqueBO
    {
        public List<DimRegistroEmbarqueDTO> GetDimRegistroEmbarque(string id)
        {

            return new DimRegistroEmbarqueRepository().GetDimRegistroEmbarque(id);

        }
    }
}
