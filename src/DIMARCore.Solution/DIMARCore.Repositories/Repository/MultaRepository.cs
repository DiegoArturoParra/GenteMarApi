using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Enums;
using GenteMarCore.Entities.ViewsSQL;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class MultaRepository : GenericRepository<ViewGenteMarMultasPorUsuario>
    {

        public async Task<IEnumerable<MultaDTO>> GetMultasPorUsuario(string identificacion)
        {
            string estadoAnulado = EnumConfig.GetDescription(EstadoMultaEnum.Anulado);
            string estadoTerminado = EnumConfig.GetDescription(EstadoMultaEnum.Terminado);
            var results = await _context.VIEW_MULTAS_USUARIO.Where(x => x.NumDocumento == identificacion
                                                            && x.EstadoFinal != estadoAnulado
                                                            && x.EstadoFinal != estadoTerminado)
                .Select(x => new MultaDTO
                {
                    FechaRegistro = x.FechaRegistro,
                    Observacion = x.Observacion,
                    TipoMulta = x.TipoMulta,
                }).OrderByDescending(x => x.FechaRegistro).AsNoTracking().ToListAsync();
            return results;
        }
    }
}
