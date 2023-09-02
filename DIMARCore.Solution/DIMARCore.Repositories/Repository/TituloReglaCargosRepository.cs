using GenteMarCore.Entities.Models;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class TituloReglaCargosRepository : GenericRepository<GENTEMAR_TITULO_REGLA_CARGOS>
    {
        public async Task DesactivarCargoDelTitulo(GENTEMAR_TITULO_REGLA_CARGOS data)
        {
            var dataHabilitaciones = await _context.GENTEMAR_TITULO_CARGO_HABILITACION.Where(x => x.id_titulo_cargo_regla == data.id_titulo_cargo_regla).ToListAsync();
            var dataFunciones = await _context.GENTEMAR_TITULO_CARGO_FUNCION.Where(x => x.id_titulo_cargo_regla == data.id_titulo_cargo_regla).ToListAsync();
            if (dataHabilitaciones.Any())
            {
                _context.GENTEMAR_TITULO_CARGO_HABILITACION.RemoveRange(dataHabilitaciones);
            }
            if (dataFunciones.Any())
            {
                _context.GENTEMAR_TITULO_CARGO_FUNCION.RemoveRange(dataFunciones);
            }
            await Update(data);
        }
    }
}
