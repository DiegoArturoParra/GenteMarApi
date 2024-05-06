using GenteMarCore.Entities.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class TituloReglaCargosRepository : GenericRepository<GENTEMAR_TITULO_REGLA_CARGOS>
    {
        public async Task DesactivarCargoDelTitulo(GENTEMAR_TITULO_REGLA_CARGOS data)
        {
            try
            {
                BeginTransaction();
                var dataHabilitaciones = await _context.GENTEMAR_TITULO_REGLA_CARGOS_HABILITACION.Where(x => x.id_titulo_cargo_regla == data.id_titulo_cargo_regla).ToListAsync();
                var dataFunciones = await _context.GENTEMAR_TITULO_REGLA_CARGOS_FUNCION.Where(x => x.id_titulo_cargo_regla == data.id_titulo_cargo_regla).ToListAsync();
                if (dataHabilitaciones.Any())
                {
                    _context.GENTEMAR_TITULO_REGLA_CARGOS_HABILITACION.RemoveRange(dataHabilitaciones);
                }
                if (dataFunciones.Any())
                {
                    _context.GENTEMAR_TITULO_REGLA_CARGOS_FUNCION.RemoveRange(dataFunciones);
                }
                await Update(data);

                var dataCargo = await GetNombreCargo(data.id_cargo_regla);
                var radicado = await GetRadicado(data.id_titulo);
                var observacion = new GENTEMAR_OBSERVACIONES_TITULOS
                {
                    id_titulo = data.id_titulo,
                    observacion = $"Se desactivó el cargo {dataCargo} del título con radicado {radicado}",
                };
                _context.GENTEMAR_OBSERVACIONES_TITULOS.Add(observacion);
                await SaveAllAsync();
                CommitTransaction();
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                ObtenerException(ex, data);
            }
        }

        private async Task<string> GetNombreCargo(int id_cargo_regla)
        {
            return await _context.GENTEMAR_REGLAS_CARGO.Where(x => x.id_cargo_regla == id_cargo_regla)
                 .Include(x => x.GENTEMAR_CARGO_TITULO).Select(x => x.GENTEMAR_CARGO_TITULO.cargo).FirstOrDefaultAsync();
        }

        private async Task<string> GetRadicado(long id_titulo)
        {
            return await _context.GENTEMAR_TITULOS.Where(x => x.id_titulo == id_titulo).Select(x => x.radicado).FirstOrDefaultAsync();
        }
    }
}
