using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class FuncionRepository : GenericRepository<GENTEMAR_FUNCIONES>
    {
        public async Task<IEnumerable<FuncionDTO>> GetFuncionesByReglaId(int reglaId, bool isShowAll)
        {
            var data = await (from funcionRegla in _context.GENTEMAR_REGLA_FUNCION
                              join funcion in _context.GENTEMAR_FUNCIONES on funcionRegla.id_funcion equals funcion.id_funcion
                              where funcionRegla.id_regla == reglaId && (isShowAll ? true : funcion.activo)
                              select new FuncionDTO
                              {
                                  Id = funcion.id_funcion,
                                  Descripcion = funcion.funcion,
                                  IsActive = funcion.activo,
                              }).AsNoTracking().ToListAsync();
            return data;
        }
    }
}
