using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class AplicacionTipoSolicitudRepository : GenericRepository<APLICACIONES_TIPO_SOLICITUD>
    {
        public async Task<IEnumerable<TipoSolicitudDTO>> GetTiposSolicitud(List<string> excepto)
        {
            return await (from tipoSolicitud in _context.APLICACIONES_TIPO_SOLICITUD
                          where !excepto.Contains(tipoSolicitud.DESCRIPCION)
                          select new TipoSolicitudDTO
                          {
                              Id = tipoSolicitud.ID_TIPO_SOLICITUD,
                              Descripcion = tipoSolicitud.DESCRIPCION,
                          }).OrderBy(x => x.Descripcion).ToListAsync();
        }
    }
}
