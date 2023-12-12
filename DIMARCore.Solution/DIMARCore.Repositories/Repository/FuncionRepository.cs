using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class FuncionRepository : GenericRepository<GENTEMAR_FUNCIONES>
    {
        public async Task<IEnumerable<GENTEMAR_REGLA_FUNCION>> GetFuncionesByReglaActivas(int reglaId)
        {
            return await _context.GENTEMAR_REGLA_FUNCION.Include(x => x.GENTEMAR_FUNCIONES).Where(x => x.id_regla == reglaId).ToListAsync();
        }
    }
}
