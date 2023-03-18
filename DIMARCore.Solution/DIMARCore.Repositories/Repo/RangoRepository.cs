using GenteMarCore.Entities;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repo
{
    public class RangoRepository : GenericRepository<APLICACIONES_RANGO>
    {
        GenteDeMarCoreContext contexto = new GenteDeMarCoreContext();
        /// <summary>
        /// Lista de formacion si el estado es true  trae todos los activos si es false devuelve todos los registros 
        /// </summary>
        /// <returns>Lista de los tipos de formacion</returns>
        public IList<APLICACIONES_RANGO> GetRango(bool estado)
        {
            try
            {
                var resultado = (from a in contexto.APLICACIONES_RANGO
                                 select a
                                   );
                if (estado)
                {
                    resultado = resultado.Where(x => x.activo == estado);
                }
                var data  = resultado.OrderBy(p => p.rango).ToList();

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
