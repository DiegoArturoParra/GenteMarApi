using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repo
{
    public class CargoTituloRepository : GenericRepository<GENTEMAR_CARGO_TITULO>
    {
        public async Task<bool> ExisteCargoTituloById(int cargoId)
        {
            return await AnyWithCondition(x => x.id_cargo_titulo == cargoId);
        }

        public IEnumerable<ListadoCargoTituloDTO> GetCargosTitulos(CargoTituloFilter Filtro)
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
            var listado = query.Select(m => new ListadoCargoTituloDTO
            {
                Id = m.cargo.id_cargo_titulo,
                Clase = m.clase.descripcion_clase,
                ClaseId = m.clase.id_clase,
                Descripcion = m.cargo.cargo,
                Seccion = m.seccion.actividad_a_bordo,
                SeccionId = m.seccion.id_seccion,
                IsActive = m.cargo.activo
            });
            var data = listado.ToList();
            return data;
        }
    }
}
