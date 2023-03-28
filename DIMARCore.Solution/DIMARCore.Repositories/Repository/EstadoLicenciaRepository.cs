using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
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
        public IList<GENTEMAR_ESTADO_LICENCIA> GetEstado()
        {
            var resultado = (from a in _context.GENTEMAR_ESTADO_LICENCIAS
                             select a).OrderBy(p => p.descripcion_estado).ToList();
            return resultado;
        }
    }
}
