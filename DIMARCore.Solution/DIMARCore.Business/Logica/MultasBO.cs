using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class MultasBO
    {
        public async Task<IEnumerable<MultaDTO>> GetMultasPorUsuario(string identificacion)
        {
            return await new MultaRepository().GetMultasPorUsuario(identificacion);
        }
    }
}
