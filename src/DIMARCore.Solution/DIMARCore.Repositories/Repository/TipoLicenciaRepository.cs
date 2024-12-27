using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;


namespace DIMARCore.Repositories.Repository
{
    public class TipoLicenciaRepository : GenericRepository<GENTEMAR_TIPO_LICENCIA>
    {
        /// <summary>
        /// Lista de TipoLicencia
        /// </summary>
        /// <returns>Lista de Tipo Licencia</returns>
        /// <tabla>GENTEMAR_TIPO_LICENCIA</tabla>
        public async Task<IEnumerable<GENTEMAR_TIPO_LICENCIA>> GetTipoLicencias()
        {
            var resultado = (from a in _context.GENTEMAR_TIPO_LICENCIA
                             select a).OrderBy(p => p.tipo_licencia);
            return await resultado.ToListAsync();
        }


        /// <summary>
        /// tipo licencia dado el Id
        /// </summary>
        /// <param name="id">Id de la tipo licencia</param>
        /// <returns>TIPO LICENCIA</returns>
        /// <tabla>GENTEMAR_TIPO_LICENCIA</tabla>
        public GENTEMAR_TIPO_LICENCIA GetTipoLicencia(int id)
        {

            var resultado = (from c in _context.GENTEMAR_TIPO_LICENCIA
                             where c.id_tipo_licencia == id
                             select c).FirstOrDefault();

            return resultado;
        }

    }

}
