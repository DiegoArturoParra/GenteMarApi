using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class NivelTituloRepository : GenericRepository<GENTEMAR_NIVEL>
    {
        public async Task<IEnumerable<NivelDTO>> GetNivelesByReglaId(int reglaId)
        {
            _context.Configuration.LazyLoadingEnabled = true;
            var listaAgrupada = _context.GENTEMAR_REGLAS_CARGO.Where(x => x.id_regla == reglaId)
                .Include(x => x.GENTEMAR_NIVEL).GroupBy(p => new { p.id_nivel });

            var query = await listaAgrupada.Select(x => new NivelDTO
            {
                Id = x.Key.id_nivel,
                Descripcion = x.Select(o=>o.GENTEMAR_NIVEL.nivel).FirstOrDefault()

            }).ToListAsync();
            return query;
        }
    }
}
