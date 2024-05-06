using Dapper;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Enums;
using GenteMarCore.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class MultaRepository
    {
        private readonly CoreDapperContext _coreContextDapper;

        public MultaRepository()
        {
            _coreContextDapper = new CoreDapperContext();
        }
        public async Task<IEnumerable<MultaDTO>> GetMultasPorUsuario(string identificacion)
        {
            try
            {
                using (IDbConnection db = _coreContextDapper.Context)
                {
                    string estadoAnulado = EnumConfig.GetDescription(EstadoMultaEnum.Anulado);
                    string estadoTerminado = EnumConfig.GetDescription(EstadoMultaEnum.Terminado);
                    const string sqlQuery = @"SELECT * FROM DBA.VwGenteMarMultasPorUsuario WHERE NumDocumento = ? AND EstadoFinal <> ? AND EstadoFinal <> ?";
                    var results = await db.QueryAsync<MultaDTO>(sqlQuery, new { identificacion, estadoAnulado, estadoTerminado });
                    return results;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _coreContextDapper.CloseConnection();
            }
        }
    }
}
