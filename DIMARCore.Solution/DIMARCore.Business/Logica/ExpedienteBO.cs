using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class ExpedienteBO
    {
        public async Task<IEnumerable<GENTEMAR_EXPEDIENTE>> GetExpedientes()
        {

            using (var repository = new ExpedienteRepository())
            {
                return await repository.GetAllAsync();
            }
        }

        public async Task<IEnumerable<ExpedienteDTO>> GetExpedientesPorConsolidado(int consolidadoId)
        {
            var hayExpedientesPorConsolidado = await new ExpedienteObservacionEstupefacienteRepository().AnyWithCondition(x => x.id_consolidado == consolidadoId);
            if (!hayExpedientesPorConsolidado)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No hay expedientes aún del número de consolidado."));

            return await new ExpedienteRepository().GetExpedientesPorConsolidado(consolidadoId);
        }
    }
}
