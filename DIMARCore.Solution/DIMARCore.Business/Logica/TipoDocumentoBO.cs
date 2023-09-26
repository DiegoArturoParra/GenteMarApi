using DIMARCore.Repositories.Repository;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DIMARCore.Business
{
    public class TipoDocumentoBO
    {
        public async Task<IList<APLICACIONES_TIPO_DOCUMENTO>> GetTiposDocumento()
        {
            // Obtiene la lista
            return await new TipoDocumentoRepository().GetTiposDocumento();
        }
    }
}
