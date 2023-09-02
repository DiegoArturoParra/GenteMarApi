using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Seguridad;
using GenteMarCore.Entities;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class ServiciosAplicacionesRepository<T> : IDisposable where T : class
    {
        protected readonly GenteDeMarCoreContext _context;
        private DbSet<T> _entities;
        private bool _isDisposed;
        private readonly string _isEnvironment;

        public bool IsDisposed { get => _isDisposed; set => _isDisposed = value; }

        public ServiciosAplicacionesRepository()
        {
            _isEnvironment = ConfigurationManager.AppSettings[Constantes.KEY_ENVIRONMENT];
            _context = GetContext(_isEnvironment);
            IsDisposed = false;
        }

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


        public async Task<bool> AnyWithCondition(Expression<Func<T, bool>> whereCondition)
        {
            return await Entities.AnyAsync(whereCondition);
        }

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
