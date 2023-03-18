using GenteMarCore.Entities;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repo
{
    public class ServiciosAplicacionesRepository<T> : IDisposable where T : class
    {
        protected readonly GenteDeMarCoreContext _context;
        private DbSet<T> _entities;

        public ServiciosAplicacionesRepository()
        {
            _context = new GenteDeMarCoreContext();
            _entities = _context.Set<T>();
        }

        #region Limpiar objetos
        public void Dispose()
        {
            _context?.Dispose();
        }
        #endregion
        public IQueryable<T> GetAll()
        {
            return _entities.AsNoTracking();
        }

        public IEnumerable<APLICACIONES_CAPITANIAS> GetCapitaniasFirmante()
        {
            var listado =  _context.APLICACIONES_CAPITANIAS.Where(x => x.SIGLA_CAPITANIA.Equals("DIMAR")).ToList();
            return listado;
        }
    }
}
