using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class AplicacionCapitaniaRepository : GenericRepository<APLICACIONES_CAPITANIAS>
    {
        public async Task<IEnumerable<CapitaniaDTO>> GetCapitanias()
        {
            return await _context.APLICACIONES_CAPITANIAS.Select(x => new CapitaniaDTO
            {
                Id = x.ID_CAPITANIA,
                Sigla = x.SIGLA_CAPITANIA,
                Descripcion = x.DESCRIPCION,
            }).OrderBy(x => x.Sigla)
            .AsNoTracking().ToListAsync();
        }
    }
}
