using DIMARCore.Utilities.Enums;
using GenteMarCore.Entities;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
namespace DIMARCore.Repositories
{
    public class TipoDocumentoRepository
    {
        GenteDeMarCoreContext contexto = new GenteDeMarCoreContext();

        /// <summary>
        /// Lista de TipDocumento
        /// </summary>
        /// <returns>Lista de Tipo Documento</returns>
        /// <tabla>APLICACIONES_TIPO_DOCUMENTO</tabla>
        public IList<APLICACIONES_TIPO_DOCUMENTO> GetTipoDocumento()
        {
            try
            {
                var resultado = (from a in contexto.APLICACIONES_TIPO_DOCUMENTO
                                 select a
                                 ).Where(x=> x.ID_TIPO_DOCUMENTO != (int)TipoDocumentoEnum.ID && x.ID_TIPO_DOCUMENTO != (int)TipoDocumentoEnum.NIT).OrderBy(p => p.DESCRIPCION).ToList();

                //var resultado = contexto.GENTEMAR_TIPO_LICENCIA.OrderBy(e => e.tipo_licencia).ToList();

                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
