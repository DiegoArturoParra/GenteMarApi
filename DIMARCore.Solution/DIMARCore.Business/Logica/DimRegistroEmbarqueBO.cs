using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Middleware;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class DimRegistroEmbarqueBO
    {
        public async Task<IEnumerable<DimRegistroEmbarqueDTO>> GetDimRegistroEmbarqueAsync(long usuarioId)
        {
            var data = await new DatosBasicosRepository().GetWithCondition(y => y.id_gentemar == usuarioId);
            return data == null
                ? throw new HttpStatusCodeException(System.Net.HttpStatusCode.NotFound, "No se encontraron datos del usuario.")
                : await new DimRegistroEmbarqueRepository().GetDimRegistroEmbarque(data.documento_identificacion);
        }
    }
}
