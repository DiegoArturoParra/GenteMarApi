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
            return await AnyWithCondition(x => x.id_regla == reglaId);
        }

        public async Task<IEnumerable<ReglaDTO>> GetReglasByCargoTitulo(int cargoId)
        {

            var query = await (from reglaCargo in _context.GENTEMAR_REGLAS_CARGO
                               join regla in _context.GENTEMAR_REGLAS on reglaCargo.id_regla
                               equals regla.id_regla

                               where reglaCargo.id_cargo_titulo == cargoId
                               group regla by new { regla.id_regla, regla.nombre_regla } into objeto
                               select new ReglaDTO
                               {
                                   Id = objeto.Key.id_regla,
                                   Descripcion = objeto.Key.nombre_regla
                               }).ToListAsync();
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
