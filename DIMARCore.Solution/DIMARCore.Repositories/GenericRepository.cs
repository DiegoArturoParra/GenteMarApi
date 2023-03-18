using GenteMarCore.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DIMARCore.Repositories
{
    public class GenericRepository<T> : IDisposable, IGenericRepository<T> where T : class
    {
        protected readonly GenteDeMarCoreContext _context;
        private DbSet<T> _entities;

        public GenericRepository()
        {
            _context = new GenteDeMarCoreContext();
            _entities = _context.Set<T>();
        }

        #region Listado IEnumerable In BD
        public IEnumerable<T> GetAll()
        {
            return _entities.ToList();
        }

        #endregion

        #region get de un objeto con condiciones 
        public async Task<T> GetWithCondition(Expression<Func<T, bool>> whereCondition)
        {
            return await _entities.Where(whereCondition).FirstOrDefaultAsync();
        }
        #endregion

        #region existe por condiciones 
        public async Task<bool> AnyWithCondition(Expression<Func<T, bool>> whereCondition)
        {
            return await _entities.AnyAsync(whereCondition);
        }
        #endregion

        #region listado de un objeto con condiciones async
        public async Task<IEnumerable<T>> GetAllWithConditionAsync(Expression<Func<T, bool>> whereCondition)
        {
            return await _entities.Where(whereCondition).ToListAsync();
        }
        #endregion

        #region listado de un objeto con condiciones 
        public IEnumerable<T> GetAllWithCondition(Expression<Func<T, bool>> whereCondition)
        {
            return _entities.Where(whereCondition).ToList();
        }
        #endregion

        #region Listado IQueryable para hacer consultas al servidor optimizado
        public IQueryable<T> GetAllAsQueryable()
        {
            return _entities.AsNoTracking();
        }

        #endregion

        #region Get by Id In BD
        public async Task<T> GetById(object id)
        {
            return await _entities.FindAsync(id);
        }
        #endregion

        #region Create In BD
        public async Task Create(T entidad)
        {
            if (entidad == null) throw new ArgumentNullException($"{nameof(entidad)} no debe ser nula");
            try
            {
                _context.Entry(entidad).State = EntityState.Added;
                await SaveAllAsync();
            }
            catch (Exception ex)
            {
                ObtenerException(ex, entidad);
            }
        }
        #endregion

        #region Update In BD
        public async Task Update(T entidad)
        {
            if (entidad == null) throw new ArgumentNullException($"{nameof(entidad)} no debe ser nula");
            try
            {             
                _context.Entry(entidad).State = EntityState.Modified;
                await SaveAllAsync();
            }
            catch (Exception ex)
            {
                ObtenerException(ex, entidad);
            }
        }
        #endregion

        #region Save In BD
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        #endregion

        #region Exception Generic
        public static TException GetInnerException<TException>(Exception exception)
          where TException : Exception
        {
            Exception innerException = exception;
            while (innerException != null)
            {
                if (innerException is TException result)
                {
                    return result;
                }
                innerException = innerException.InnerException;

            }
            return null;
        }

        public void ObtenerException(Exception ex, T entidad)
        {
            string Mensaje = string.Empty;
            var udpateException = GetInnerException<DbUpdateException>(ex);
            var DbEntityException = GetInnerException<DbEntityValidationException>(ex);

            if (udpateException != null)
            {
                if (DbEntityException != null)
                {
                    Mensaje = obtenerErroresDeAtributos(DbEntityException);
                    throw new Exception($"{entidad.GetType().FullName} no se ha podido crear/actualizar: {udpateException.InnerException.Message}\n, {Mensaje}");
                }
                else
                {
                    throw new Exception($"{entidad.GetType().FullName} no se ha podido crear/actualizar ERROR: {udpateException.Message}, \nERROR ESPECIFICO: {udpateException.InnerException.InnerException.Message}");
                }
            }
            else if (DbEntityException != null)
            {
                Mensaje = obtenerErroresDeAtributos(DbEntityException);
                throw new Exception($"{Mensaje}");
            }
            else
            {
                throw new Exception($"{entidad}: {ex.Message}");
            }
        }
        private string obtenerErroresDeAtributos(DbEntityValidationException dbEntityException)
        {
            string mensaje = string.Empty;
            foreach (var eve in dbEntityException.EntityValidationErrors)
            {
                mensaje += $"Entidad de tipo {eve.Entry.Entity.GetType().Name} en el estado {eve.Entry.State} tiene los siguientes errores de validación:";
                Console.WriteLine("Entidad de tipo\"{0}\" en el estado \"{1}\" tiene los siguientes errores de validación:",
                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                foreach (var ve in eve.ValidationErrors)
                {
                    mensaje += $"\nPropiedad: {ve.PropertyName}, Error: {ve.ErrorMessage}";
                    Console.WriteLine("- Propiedad: \"{0}\", Error: \"{1}\"",
                        ve.PropertyName, ve.ErrorMessage);
                }
            }
            return mensaje;
        }
        #endregion

        #region Limpiar objetos
        public void Dispose()
        {
            _context?.Dispose();
        }
        #endregion
    }
}
