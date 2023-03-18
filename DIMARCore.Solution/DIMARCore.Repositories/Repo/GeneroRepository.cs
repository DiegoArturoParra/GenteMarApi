using GenteMarCore.Entities;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DIMARCore.Repositories
{
    public class GeneroRepository
    {
        GenteDeMarCoreContext contexto = new GenteDeMarCoreContext();

        /// <summary>
        /// Lista de TipDocumento
        /// </summary>
        /// <returns>Lista de Tipo Documento</returns>
        /// <tabla>APLICACIONES_TIPO_DOCUMENTO</tabla>
        public IList<APLICACIONES_GENERO> GetGenero()
        {
            try
            {
                var resultado = (from a in contexto.APLICACIONES_GENERO
                                 select a
                                 ).OrderBy(p => p.DESCRIPCION).ToList();

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
