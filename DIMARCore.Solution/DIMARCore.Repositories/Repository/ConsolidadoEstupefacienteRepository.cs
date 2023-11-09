using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class ConsolidadoEstupefacienteRepository : GenericRepository<GENTEMAR_CONSOLIDADO>
    {
        public async Task<IEnumerable<ConsolidadoDTO>> GetConsolidadosEnUso()
        {
            return await (from expedienteObservacion in _context.GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES
                          join consolidado in Table on expedienteObservacion.id_consolidado equals consolidado.id_consolidado
                          group expedienteObservacion by new { expedienteObservacion.id_consolidado, consolidado.numero_consolidado } into grupo
                          select new ConsolidadoDTO
                          {
                              Id = grupo.Key.id_consolidado,
                              Descripcion = grupo.Key.numero_consolidado,
                          }).ToListAsync();
        }

        public async Task<IEnumerable<long>> GetAllIdsEstupefacienteByConsolidado(int consolidadoId)
        {
            return await (from expedienteObservacion in _context.GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES
                          join consolidado in Table on expedienteObservacion.id_consolidado equals consolidado.id_consolidado
                          where expedienteObservacion.id_consolidado == consolidadoId
                          group expedienteObservacion by new { expedienteObservacion.id_antecedente } into grupo
                          select grupo.Key.id_antecedente).ToListAsync();
        }

        public async Task<string> GetLastConsolidado()
        {
            return await Table.OrderByDescending(x => x.id_consolidado).Select(x => x.numero_consolidado).FirstOrDefaultAsync();
        }
    }
}
