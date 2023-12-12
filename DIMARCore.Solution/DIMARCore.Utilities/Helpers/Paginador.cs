using System;
using System.Collections.Generic;
using System.Linq;

namespace DIMARCore.Utilities.Helpers
{
    public class Paginador<T>
    {
        public ParametrosPaginacion paginacion { get; set; } = new ParametrosPaginacion();
        public int TotalPages { get; set; }
        public int TotalDatos { get; set; }
        public List<T> Data { get; set; } = new List<T>();

        public Paginador()
        {

        }
        public Paginador(List<T> items, int count, ParametrosPaginacion paginacion)
        {
            this.paginacion = paginacion;
            this.TotalDatos = count;
            TotalPages = (int)Math.Ceiling(count / (double)paginacion.PageSize);
            this.Data.AddRange(items);

        }

        public bool HasPreviousPage
        {
            get
            {
                return (this.paginacion.Page > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (this.paginacion.Page < TotalPages);
            }
        }

        public static Paginador<T> CrearPaginador(int count, IEnumerable<T> source, ParametrosPaginacion paginacion)
        {
            var items = source.ToList();
            return new Paginador<T>(items, count, paginacion);
        }
    }
}
