using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class ExpedienteRepository : GenericRepository<GENTEMAR_EXPEDIENTE>
    {
        public async Task<ExpedienteDTO> GetExpedientePorConsolidadoEntidad(ExpedienteFilter filter)
        {
            return await (from relacion in _context.GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES
                          join expediente in _context.GENTEMAR_EXPEDIENTE on relacion.id_expediente equals expediente.id_expediente
                          where relacion.id_consolidado == filter.ConsolidadoId && relacion.id_entidad == filter.EntidadId
                          group relacion by new { expediente.id_expediente, expediente.numero_expediente } into g
                          select new ExpedienteDTO
                          {
                              Id = g.Key.id_expediente,
                              Descripcion = g.Key.numero_expediente.ToString(),
                          }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ExpedienteDTO>> GetExpedientesPorConsolidado(int consolidadoId)
        {
            return await (from relacion in _context.GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES
                          join expediente in _context.GENTEMAR_EXPEDIENTE on relacion.id_expediente equals expediente.id_expediente
                          where relacion.id_consolidado == consolidadoId
                          group relacion by new { expediente.id_expediente, expediente.numero_expediente } into g
                          select new ExpedienteDTO
                          {
                              Id = g.Key.id_expediente,
                              Descripcion = g.Key.numero_expediente.ToString(),
                          }).ToListAsync();
        }
    }
}
