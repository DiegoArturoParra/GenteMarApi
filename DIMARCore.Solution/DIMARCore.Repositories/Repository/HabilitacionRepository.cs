using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DIMARCore.Repositories.Repository
{
    public class HabilitacionRepository : GenericRepository<GENTEMAR_HABILITACION>
    {
        public IEnumerable<GENTEMAR_CARGO_HABILITACION> GetHabilitacionesByReglaCargoId(int CargoReglaId)
        {
            _context.Configuration.LazyLoadingEnabled = true;
            return _context.GENTEMAR_CARGO_HABILITACION.Include(x => x.GENTEMAR_HABILITACIONES).Where(x => x.id_cargo_regla == CargoReglaId
                                                                 && x.GENTEMAR_HABILITACIONES.activo == true).ToList();
        }
    }
}
