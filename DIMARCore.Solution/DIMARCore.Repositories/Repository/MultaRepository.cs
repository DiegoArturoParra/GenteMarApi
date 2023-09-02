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
        private readonly CoreContextDapper _coreContextDapper;
        public MultaRepository()
        {
            _coreContextDapper = new CoreContextDapper();
        }
        public async Task<IEnumerable<MultaDTO>> GetMultasPorUsuario(string identificacion)
        {
            try
            {
                using (IDbConnection db = _coreContextDapper.Context)
                {
                    string estadoAnulado = EnumConfig.GetDescription(EstadoMultaEnum.Anulado);
                    string estadoTerminado = EnumConfig.GetDescription(EstadoMultaEnum.Terminado);
                    const string sqlQuery = @"SELECT multas_info.OBSERVACION as Observacion, multas_info.FECHA_REGISTRO as FechaRegistro, 
                                          tipo_multa.NOMBRE_MULTA as TipoMulta FROM DBA.MULTAS_INFORMACION multas_info
                                          JOIN DBA.MULTAS_TIPO_MULTA tipo_multa ON tipo_multa.ID_TIPO_MULTA = multas_info.TIPO_DE_MULTA 
                                          JOIN DBA.MULTAS_DEUDORES_MULTA multa_deudor_multa ON multas_info.ID_MULTA = multa_deudor_multa.ID_MULTA
                                          JOIN DBA.MULTAS_DEUDORES mdeudor ON multa_deudor_multa.ID_DEUDOR = mdeudor.ID_DEUDOR
                                          WHERE mdeudor.NUM_DOCUMENTO = ? AND multas_info.ESTADO_FINAL <> ? AND multas_info.ESTADO_FINAL <> ?";

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
