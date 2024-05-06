using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
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
            var hayExpedientesPorConsolidado = await new ExpedienteObservacionEstupefacienteRepository().AnyWithConditionAsync(x => x.id_consolidado == consolidadoId);
            if (!hayExpedientesPorConsolidado)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No hay expedientes aún del número de consolidado."));

            return await new ExpedienteRepository().GetExpedientesPorConsolidado(consolidadoId);
        }

        public async Task<Respuesta> GetExpedientePorConsolidadoEntidad(ExpedienteFilter filter)
        {
            bool existeConsolidado = await new ConsolidadoEstupefacienteRepository().AnyWithConditionAsync(x => x.id_consolidado == filter.ConsolidadoId);
            if (!existeConsolidado)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No existe el consolidado."));

            bool existeEntidad = await new EntidadEstupefacienteRepository().AnyWithConditionAsync(x => x.id_entidad == filter.EntidadId);
            if (!existeEntidad)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No existe la entidad."));

            var data = await new ExpedienteRepository().GetExpedientePorConsolidadoEntidad(filter);
            return Responses.SetOkResponse(data);
        }
    }
}
