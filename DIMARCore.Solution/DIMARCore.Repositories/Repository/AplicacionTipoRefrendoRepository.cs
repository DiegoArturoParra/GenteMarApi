using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class AplicacionTipoRefrendoRepository : GenericRepository<APLICACIONES_TIPO_REFRENDO>
    {
        public async Task<IEnumerable<TipoRefrendoDTO>> GetTiposRefrendo()
        {
            return await _context.APLICACIONES_TIPO_REFRENDO.Select(x => new TipoRefrendoDTO
            {
                Id = x.ID_TIPO_CERTIFICADO,
                Descripcion = x.DESCRIPCION,
            }).OrderBy(x => x.Descripcion).ToListAsync();
        }
    }
}
