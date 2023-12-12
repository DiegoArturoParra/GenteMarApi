using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class EstadoLicenciaRepository : GenericRepository<GENTEMAR_ESTADO_LICENCIA>
    {
        /// <summary>
        /// Lista de estados
        /// </summary>
        /// <returns>Lista de estados</returns>
        /// <tabla>GENTEMAR_ESTADO</tabla>
        public async Task<IEnumerable<GENTEMAR_ESTADO_LICENCIA>> GetEstados()
        {
            var resultado = await (from a in Table select a)
                .OrderBy(p => p.descripcion_estado).ToListAsync();
            return resultado;
        }
    }
}
