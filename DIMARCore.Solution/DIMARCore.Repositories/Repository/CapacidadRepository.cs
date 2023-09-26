using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class CapacidadRepository : GenericRepository<GENTEMAR_CAPACIDAD>
    {
        public async Task<IEnumerable<GENTEMAR_REGLAS_CARGO>> CapacidadByReglaCargo(IdsLlaveCompuestaDTO items)
        {
            return await _context.GENTEMAR_REGLAS_CARGO.Include(x => x.GENTEMAR_CAPACIDAD)
                .Where(x => x.id_regla == items.ReglaId && x.id_cargo_titulo == items.CargoTituloId && x.GENTEMAR_CAPACIDAD.activo == true).ToListAsync();
        }
    }
}
