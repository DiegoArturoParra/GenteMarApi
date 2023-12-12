using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Seguridad;
using System;
using System.Configuration;
using System.Data;
using System.Data.Odbc;

namespace GenteMarCore.Entities
{
    public class CoreContextDapper
    {
        private readonly string _isEnvironment;
        private string _connectionString;
        private IDbConnection _context;

        public IDbConnection Context
        {
            get
            {
                return _context;
            }
            set { _context = value; }
        }
        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
            set { _connectionString = value; }
        }
        public CoreContextDapper()
        {
            _isEnvironment = ConfigurationManager.AppSettings[Constantes.KEY_ENVIRONMENT];
            Context = CreateConnection(_isEnvironment);
        }

        private IDbConnection CreateConnection(string isEnvironment)
        {
            try
            {
                string connectionString;
                int environment = Convert.ToInt32(isEnvironment);
                if ((int)EnvironmentEnum.Production == environment)
                {
                    connectionString = ConfigurationManager.ConnectionStrings[EnumConfig.GetDescription(EnvironmentDapperEnum.Production)].ConnectionString;
                    var desencryptConnection = SecurityEncrypt.GenerateDecrypt(connectionString);

                    if (_context == null || _context.State != ConnectionState.Open)
                    {
                        _context = new OdbcConnection(desencryptConnection);
                        _context.Open();
                    }
                }
                else if ((int)EnvironmentEnum.Testing == environment)
                {
                    connectionString = ConfigurationManager.ConnectionStrings[EnumConfig.GetDescription(EnvironmentDapperEnum.Testing)].ConnectionString;
                    var desencryptConnection = SecurityEncrypt.GenerateDecrypt(connectionString);
                    _context = new OdbcConnection(desencryptConnection);
                    _context.Open();
                }
                else
                {
                    connectionString = ConfigurationManager.ConnectionStrings[EnumConfig.GetDescription(EnvironmentDapperEnum.Development)].ConnectionString;
                    _context = new OdbcConnection(connectionString);
                    _context.Open();
                }
            }
            catch (Exception ex)
            {
                Responses.SetInternalServerErrorResponse(ex, "La conexión a la BD ha fallado, verifique la cadena de conexión.");
                throw;
            }
            return _context;
        }
        public void CloseConnection()
        {
            if (_context != null && _context.State == ConnectionState.Open)
            {
                _context.Close();
            }
        }
    }
}
