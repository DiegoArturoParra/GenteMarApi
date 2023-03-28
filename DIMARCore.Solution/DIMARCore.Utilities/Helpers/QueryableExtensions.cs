using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIMARCore.Utilities.Helpers
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Metodo para paginar una lista
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="paginacion"></param>
        /// <returns></returns>
        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, ParametrosPaginacion paginacion)
        {
            var query = queryable
                .Skip((paginacion.Page - 1) * paginacion.PageSize)
                .Take(paginacion.PageSize);
            return query;
        }
    }
}
