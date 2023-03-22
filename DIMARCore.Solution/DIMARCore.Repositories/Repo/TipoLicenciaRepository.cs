using GenteMarCore.Entities;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace DIMARCore.Repositories
{
    public class TipoLicenciaRepository: GenericRepository<GENTEMAR_TIPO_LICENCIA>
    {
        GenteDeMarCoreContext contexto = new GenteDeMarCoreContext();
        /// <summary>
        /// Lista de TipoLicencia
        /// </summary>
        /// <returns>Lista de Tipo Licencia</returns>
        /// <tabla>GENTEMAR_TIPO_LICENCIA</tabla>
        public IList<GENTEMAR_TIPO_LICENCIA> GetTipoLicencias()
        {
            try
            {
                var resultado = (from a in this.contexto.GENTEMAR_TIPO_LICENCIA
                                 select a
                                 ).OrderBy(p => p.tipo_licencia).ToList();

                //var resultado = contexto.GENTEMAR_TIPO_LICENCIA.OrderBy(e => e.tipo_licencia).ToList();

                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// tipo licencia dado el Id
        /// </summary>
        /// <param name="id">Id de la tipo licencia</param>
        /// <returns>TIPO LICENCIA</returns>
        /// <tabla>GENTEMAR_TIPO_LICENCIA</tabla>
        public GENTEMAR_TIPO_LICENCIA GetTipoLicencia(int id)
        {
            try
            {
                var resultado = (from c in this.contexto.GENTEMAR_TIPO_LICENCIA
                                 where c.id_tipo_licencia == id
                                 select c
                                ).FirstOrDefault();

                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }       

    }

}
