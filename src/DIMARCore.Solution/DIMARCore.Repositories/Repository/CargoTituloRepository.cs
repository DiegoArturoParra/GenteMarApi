using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class CargoTituloRepository : GenericRepository<GENTEMAR_CARGO_TITULO>
    {
        public async Task<bool> ExisteCargoTituloById(int cargoId)
        {
            return await AnyWithConditionAsync(x => x.id_cargo_titulo == cargoId);
        }

        public async Task<IEnumerable<CargoTituloDTO>> GetCargosActivos()
        {
            return await Table.Where(x => x.activo == Constantes.ACTIVO).Select(x => new CargoTituloDTO
            {
                Id = x.id_cargo_titulo,
                Descripcion = x.cargo,
            }).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<CargoTituloDetalleDTO>> GetCargosTituloBySeccionId(int seccionId, bool showAll)
        {
            var data = await (from cargo in _context.GENTEMAR_CARGO_TITULO
                              join clase in _context.GENTEMAR_CLASE_TITULOS on cargo.id_clase equals clase.id_clase
                              where cargo.id_seccion == seccionId && (showAll || cargo.activo)
                              select new CargoTituloDetalleDTO
                              {
                                  Id = cargo.id_cargo_titulo,
                                  Descripcion = cargo.cargo,
                                  SeccionId = cargo.id_seccion,
                                  ClaseId = clase.id_clase,
                                  IsActive = cargo.activo
                              }).AsNoTracking().ToListAsync();
            return data;
        }

        public async Task<IEnumerable<ListarCargoTituloDTO>> GetCargosTitulos(CargoTituloFilter Filtro)
        {
            var query = (from cargo in _context.GENTEMAR_CARGO_TITULO
                         join seccion in _context.GENTEMAR_SECCION_TITULOS on cargo.id_seccion equals seccion.id_seccion
                         join clase in _context.GENTEMAR_CLASE_TITULOS on cargo.id_clase equals clase.id_clase
                         select new
                         {
                             cargo,
                             seccion,
                             clase,
                         });

            if (Filtro != null)
            {
                if (Filtro.SeccionId != null && Filtro.SeccionId > 0)
                {
                    query = query.Where(x => x.cargo.id_seccion == Filtro.SeccionId);
                }
                if (Filtro.Activo != null)
                {
                    query = query.Where(x => x.cargo.activo == Filtro.Activo);
                }
                if (Filtro.ClaseId != null && Filtro.ClaseId > 0)
                {
                    query = query.Where(x => x.cargo.id_clase == Filtro.ClaseId);
                }

            }
            var data = await query.OrderByDescending(x => x.cargo.id_cargo_titulo).Select(m => new ListarCargoTituloDTO
            {
                Id = m.cargo.id_cargo_titulo,
                Clase = m.clase.descripcion_clase,
                ClaseId = m.clase.id_clase,
                Descripcion = m.cargo.cargo,
                Seccion = m.seccion.actividad_a_bordo,
                SeccionId = m.seccion.id_seccion,
                IsActive = m.cargo.activo
            }).AsNoTracking().ToListAsync();
            return data;
        }

        public async Task<IEnumerable<CargoTituloDTO>> GetCargoTitulosBySeccionesIds(List<int> seccionesIds)
        {
            return await Table.Where(x => x.activo == Constantes.ACTIVO && seccionesIds.Contains(x.id_seccion))
                .Select(x => new CargoTituloDTO
                {
                    Id = x.id_cargo_titulo,
                    Descripcion = x.cargo,
                }).AsNoTracking().ToListAsync();
        }
    }
}
