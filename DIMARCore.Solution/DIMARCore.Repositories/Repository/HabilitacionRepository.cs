using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class HabilitacionRepository : GenericRepository<GENTEMAR_HABILITACION>
    {
        public async Task<IEnumerable<GENTEMAR_REGLA_CARGO_HABILITACION>> GetHabilitacionesActivasByReglaCargoId(int CargoReglaId)
        {
            return await _context.GENTEMAR_REGLA_CARGO_HABILITACION.Include(x => x.GENTEMAR_HABILITACIONES).Where(x => x.id_cargo_regla == CargoReglaId
                                                                 && x.GENTEMAR_HABILITACIONES.activo == Constantes.ACTIVO).AsNoTracking().ToListAsync();
        }
    }
}
