using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using DIMARCore.Utilities.Seguridad;
using GenteMarCore.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace DIMARCore.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T>, IDisposable where T : class
    {
        protected readonly GenteDeMarCoreContext _context;
        private DbSet<T> _entities;
        private bool _isDisposed;
        private readonly string _isEnvironment;

        public GenericRepository()
        {
            _isEnvironment = ConfigurationManager.AppSettings[Constantes.KEY_ENVIRONMENT];
            _context = GetContext(_isEnvironment);
            IsDisposed = false;
        }
        public bool IsDisposed { get => _isDisposed; set => _isDisposed = value; }

        #region Obtener la cadena de conexión dependiendo el entorno (Production=2,Testing=1,Development=0)
        private GenteDeMarCoreContext GetContext(string _isEnvironment)
        {
            string connectionString;
            if (string.IsNullOrWhiteSpace(_isEnvironment))
                return new GenteDeMarCoreContext();

            int enviroment = Convert.ToInt32(_isEnvironment);
            if ((int)EnvironmentEnum.Production == enviroment)
            {
                connectionString = ConfigurationManager.ConnectionStrings[EnumConfig.GetDescription(EnvironmentEnum.Production)].ConnectionString;
                var desencryptConnection = SecurityEncrypt.GenerateDecrypt(connectionString);
                return new GenteDeMarCoreContext(desencryptConnection);
            }
            else if ((int)EnvironmentEnum.Testing == enviroment)
            {
                connectionString = ConfigurationManager.ConnectionStrings[EnumConfig.GetDescription(EnvironmentEnum.Testing)].ConnectionString;
                var desencryptConnection = SecurityEncrypt.GenerateDecrypt(connectionString);
                return new GenteDeMarCoreContext(desencryptConnection);
            }
            else if ((int)EnvironmentEnum.Development == enviroment)
            {
                return new GenteDeMarCoreContext();
            }

            throw new Exception("No se ha definido el ambiente de trabajo.");

        }
        #endregion

        public void BeginTransaction()
        {
            _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _context.Database.CurrentTransaction?.Commit();
        }

        public void RollbackTransaction()
        {
            _context.Database.CurrentTransaction?.Rollback();
        }

        private DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                {
                    _entities = _context.Set<T>();
                }
                return _entities;
            }
        }
        public virtual IQueryable<T> Table
        {
            get
            {
                return this.Entities;
            }
        }

        #region Listado IEnumerable In BD
        public IEnumerable<T> GetAll()
        {
            return Entities.ToList();
        }
        #endregion

        #region Listado IEnumerable In BD  Async
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Entities.ToListAsync();
        }

        #endregion

        #region get de un objeto con condiciones 
        public async Task<T> GetWithConditionAsync(Expression<Func<T, bool>> whereCondition)
        {
            return await Entities.Where(whereCondition).FirstOrDefaultAsync();
        }
        #endregion

        #region existe por condiciones 
        public async Task<bool> AnyWithConditionAsync(Expression<Func<T, bool>> whereCondition)
        {
            return await Entities.AnyAsync(whereCondition);
        }
        #endregion

        #region listado de un objeto con condiciones async
        public async Task<IEnumerable<T>> GetAllWithConditionAsync(Expression<Func<T, bool>> whereCondition)
        {
            return await Entities.Where(whereCondition).ToListAsync();
        }
        #endregion

        #region listado de un objeto con condiciones 
        public IEnumerable<T> GetAllWithCondition(Expression<Func<T, bool>> whereCondition)
        {
            return Entities.Where(whereCondition).ToList();
        }
        #endregion

        #region Listado IQueryable para hacer consultas al servidor optimizado
        public IQueryable<T> GetAllAsQueryable()
        {
            return Entities.AsNoTracking();
        }

        #endregion

        #region Get by Id Async In BD
        public async Task<T> GetByIdAsync(object id)
        {
            return await Entities.FindAsync(id);
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
            var udpateException = GetInnerException<DbUpdateException>(ex);
            var DbEntityException = GetInnerException<DbEntityValidationException>(ex);

            string Mensaje;
            if (udpateException != null)
            {
                if (DbEntityException != null)
                {
                    Mensaje = ObtenerErroresDeAtributos(DbEntityException);
                    throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, $"{entidad.GetType().FullName} no se ha podido crear/actualizar: {udpateException.InnerException.Message}\n, {Mensaje}");
                }
                else
                {
                    throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, $"{entidad.GetType().FullName} no se ha podido crear/actualizar ERROR: {udpateException.Message}, \nERROR ESPECIFICO: {udpateException.InnerException.InnerException.Message}");
                }
            }
            else if (DbEntityException != null)
            {
                Mensaje = ObtenerErroresDeAtributos(DbEntityException);
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, $"{Mensaje}");
            }
            else
            {
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, $"{entidad}: {ex.Message}");
            }
        }
        private string ObtenerErroresDeAtributos(DbEntityValidationException dbEntityException)
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
            if (_context != null)
                _context.Dispose();
            IsDisposed = true;
        }
        #endregion
    }
}
