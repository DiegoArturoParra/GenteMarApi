using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class CapacidadRepository : GenericRepository<GENTEMAR_CAPACIDAD>
    {
        public async Task<IEnumerable<CapacidadDTO>> CapacidadesByReglaCargo(IdsLlaveCompuestaDTO items)
        {
            var query = from capacidad in _context.GENTEMAR_CAPACIDAD
                        join reglaCargo in _context.GENTEMAR_REGLAS_CARGO
                        on capacidad.id_capacidad equals reglaCargo.id_capacidad
                        where reglaCargo.id_regla == items.ReglaId && reglaCargo.id_cargo_titulo == items.CargoTituloId
                        group capacidad by new { capacidad.id_capacidad, capacidad.capacidad, capacidad.activo } into grupo
                        select new CapacidadDTO
                        {
                            Id = grupo.Key.id_capacidad,
                            Descripcion = grupo.Key.capacidad,
                            IsActive = grupo.Key.activo
                        };

            query = items.IsActive
                ? query.Where(capacidad => capacidad.IsActive == Constantes.ACTIVO)
                : query;

            return await query.AsNoTracking().ToListAsync();
        }
    }
}
