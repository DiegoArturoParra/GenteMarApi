using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using System.Collections.Generic;

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
