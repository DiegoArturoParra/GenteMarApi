using DIMARCore.Utilities.Enums;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class TipoDocumentoRepository : GenericRepository<APLICACIONES_TIPO_DOCUMENTO>
    {
        /// <summary>
        /// Lista de TipDocumento
        /// </summary>
        /// <returns>Lista de Tipo Documento</returns>
        /// <tabla>APLICACIONES_TIPO_DOCUMENTO</tabla>
        public async Task<IList<APLICACIONES_TIPO_DOCUMENTO>> GetTiposDocumento()
        {
            return await (from documento in Table select documento).Where(x => x.ID_TIPO_DOCUMENTO
                             != (int)TipoDocumentoEnum.ID && x.ID_TIPO_DOCUMENTO != (int)TipoDocumentoEnum.NIT)
                             .OrderBy(p => p.DESCRIPCION).AsNoTracking().ToListAsync();

        }
    }
}
