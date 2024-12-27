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
        public async Task<IEnumerable<NivelDTO>> GetNivelesForReglaCargo(IdsTablasForaneasDTO idsTablas)
        {
            return await _context.GENTEMAR_REGLAS_CARGO
                .Where(x => x.id_regla == idsTablas.ReglaId
                       && x.id_cargo_titulo == idsTablas.CargoId
                       && x.id_capacidad == idsTablas.CapacidadId).Include(x => x.GENTEMAR_NIVEL)
                .Select(x => new NivelDTO
                {
                    Id = x.GENTEMAR_NIVEL.id_nivel,
                    Descripcion = x.GENTEMAR_NIVEL.nivel
                }).AsNoTracking().ToListAsync();
        }
    }
}
