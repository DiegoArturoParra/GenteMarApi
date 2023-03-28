using DIMARCore.Repositories.Repository;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;

namespace DIMARCore.Business
{
    public class TipoDocumentoBO
    {
        /// <summary>
        /// Lista de Tipo Licencias
        /// </summary>
        /// <returns>Lista de Tipo Licenciass</returns>
        /// <entidad>GENTEMAR_TIPO_LICENCIA</entidad>
        /// <tabla>GENTEMAR_TIPO_LICENCIA</tabla>
        public IList<APLICACIONES_TIPO_DOCUMENTO> GetTipoDocumento()
        {
            // Obtiene la lista
            var lista = new TipoDocumentoRepository().GetTipoDocumento();
            return lista;
        }
    }
}
