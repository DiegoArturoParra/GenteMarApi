using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repo
{
    public class FuncionRepository : GenericRepository<GENTEMAR_FUNCIONES>
    {
        public async Task<IEnumerable<GENTEMAR_REGLA_FUNCION>> GetFuncionesByRegla(int reglaId)
        {
            _context.Configuration.LazyLoadingEnabled = true;
            return await _context.GENTEMAR_REGLA_FUNCION.Include(x => x.GENTEMAR_FUNCIONES)
                .Where(x => x.id_regla == reglaId).ToListAsync();
        }
    }
}
