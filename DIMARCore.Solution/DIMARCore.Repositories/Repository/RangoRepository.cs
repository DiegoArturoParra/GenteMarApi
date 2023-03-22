using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace DIMARCore.Repositories.Repository
{
    public class RangoRepository : GenericRepository<APLICACIONES_RANGO>
    {
        /// <summary>
        /// Lista de formacion si el estado es true  trae todos los activos si es false devuelve todos los registros 
        /// </summary>
        /// <returns>Lista de los tipos de formacion</returns>
        public IList<APLICACIONES_RANGO> GetRango(bool estado)
        {
            var resultado = (from a in _context.APLICACIONES_RANGO
                             select a);
            if (estado)
            {
                resultado = resultado.Where(x => x.activo == estado);
            }
            var data = resultado.OrderBy(p => p.rango).ToList();
            return data;
        }
    }
}
