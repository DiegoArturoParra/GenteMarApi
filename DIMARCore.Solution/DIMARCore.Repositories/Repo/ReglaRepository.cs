using GenteMarCore.Entities;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repo
{
    public class ReglaRepository : GenericRepository<GENTEMAR_REGLAS>
    {
        public async Task<bool> ExisteRegla(int reglaId)
        {
            return await AnyWithCondition(x => x.id_regla == reglaId);
        }

        public async Task<IEnumerable<GENTEMAR_REGLAS_CARGO>> GetReglasByCargoTitulo(int cargoId)
        {
            _context.Configuration.LazyLoadingEnabled = true;
            var query = await _context.GENTEMAR_REGLAS_CARGO.Include(x => x.GENTEMAR_REGLAS)
                .Where(x => x.id_cargo_titulo == cargoId && x.GENTEMAR_REGLAS.activo == true).ToListAsync();
            return query;
        }

        public async Task Actualizar(GENTEMAR_REGLAS objeto)
        {
            using (var db = new GenteDeMarCoreContext())
            {
                db.GENTEMAR_REGLAS.Attach(objeto);
                db.Entry(objeto).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
        }

        public async Task<GENTEMAR_REGLAS> GetByIdLazy(int id)
        {
            using (_context)
            {
                _context.Configuration.LazyLoadingEnabled = false;
                return await GetById(id);
            }
        }
    }
}
