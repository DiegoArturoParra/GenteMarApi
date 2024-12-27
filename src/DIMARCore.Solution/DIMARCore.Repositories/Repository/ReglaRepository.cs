using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class ReglaRepository : GenericRepository<GENTEMAR_REGLAS>
    {
        public async Task<bool> ExisteRegla(int reglaId)
        {
            return await AnyWithConditionAsync(x => x.id_regla == reglaId);
        }

        public async Task<IEnumerable<ReglaDTO>> GetReglasByCargoTitulo(int cargoId, bool isShowAll)
        {
            var query = await (from reglaCargo in _context.GENTEMAR_REGLAS_CARGO
                               join regla in _context.GENTEMAR_REGLAS on reglaCargo.id_regla equals regla.id_regla
                               where reglaCargo.id_cargo_titulo == cargoId && (isShowAll ? true : regla.activo)
                               group regla by new { regla.id_regla, regla.nombre_regla, regla.activo } into grupo
                               select new ReglaDTO
                               {
                                   Id = grupo.Key.id_regla,
                                   Descripcion = grupo.Key.nombre_regla,
                                   IsActive = grupo.Key.activo,
                               }).AsNoTracking().ToListAsync();
            return query;
        }

        public async Task Actualizar(GENTEMAR_REGLAS objeto)
        {
            _context.GENTEMAR_REGLAS.Attach(objeto);
            _context.Entry(objeto).State = EntityState.Modified;
            await SaveAllAsync();
        }
    }
}
