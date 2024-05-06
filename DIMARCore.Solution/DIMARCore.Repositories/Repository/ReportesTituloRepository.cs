using Dapper;
using DIMARCore.UIEntities.DTOs.Reports;
using DIMARCore.UIEntities.QueryFilters.Reports;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using GenteMarCore.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class ReportesTituloRepository
    {

        private readonly CoreDapperContext _coreContextDapper;
        public ReportesTituloRepository()
        {
            _coreContextDapper = new CoreDapperContext();
        }

        public async Task<IEnumerable<TitulosReportDTO>> GetDataByReportCsv(TitulosReportFilter reportFilter, CancellationTokenSource tokenSource)
        {
            try
            {
                using (IDbConnection db = _coreContextDapper.Context)
                {

                    const string nameProcedure = "DBA.PrGenteMarFiltrarReporteTitulos ?, ?, ?, ?, ?, ?, ?, ?, ?";
                    if (reportFilter.FechaExpedicionInicial.HasValue && reportFilter.FechaExpedicionFinal.HasValue)
                    {
                        var (DateInitial, DateEnd) = Reutilizables.FormatDatesByRange(reportFilter.FechaExpedicionInicial.Value, reportFilter.FechaExpedicionFinal.Value);
                        reportFilter.FechaExpedicionInicial = DateInitial;
                        reportFilter.FechaExpedicionFinal = DateEnd;
                    }
                    else
                    {
                        DateTime fechaActual = DateTime.Now;
                        // Establecer el mes y el día a 01
                        DateTime fechaDeseada = new DateTime(fechaActual.Year, 1, 1);
                        var (DateInitial, DateEnd) = Reutilizables.FormatDatesByRange(fechaDeseada, fechaActual);
                        reportFilter.FechaExpedicionInicial = DateInitial;
                        reportFilter.FechaExpedicionFinal = DateEnd;
                    }

                    if (reportFilter.FechaVencimientoInicial.HasValue && reportFilter.FechaVencimientoFinal.HasValue)
                    {
                        var (DateInitial, DateEnd) = Reutilizables.FormatDatesByRange(reportFilter.FechaVencimientoInicial.Value, reportFilter.FechaVencimientoFinal.Value);
                        reportFilter.FechaVencimientoInicial = DateInitial;
                        reportFilter.FechaVencimientoFinal = DateEnd;
                    }

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@GeneroId", reportFilter.GeneroId);
                    queryParameters.Add("@FechaExpedicionInicio", reportFilter.FechaExpedicionInicial);
                    queryParameters.Add("@FechaExpedicionFin", reportFilter.FechaExpedicionFinal);
                    queryParameters.Add("@FechaVencimientoInicio", reportFilter.FechaVencimientoInicial);
                    queryParameters.Add("@FechaVencimientoFin", reportFilter.FechaVencimientoFinal);
                    queryParameters.Add("@CapitaniasIdArrayIn", reportFilter.CapitaniasIdArrayIn);
                    queryParameters.Add("@EstadosTramiteIdArrayIn", reportFilter.EstadosTramiteIdArrayIn);
                    queryParameters.Add("@SeccionesIdArrayIn", reportFilter.SeccionesIdArrayIn);
                    queryParameters.Add("@CargosTituloIdArrayIn", reportFilter.CargosTituloIdArrayIn);


                    var data = await db.QueryAsync<TitulosReportDTO>(
                       new CommandDefinition(nameProcedure, queryParameters, commandType: CommandType.StoredProcedure, cancellationToken: tokenSource.Token));

                    if (tokenSource.Token.IsCancellationRequested)
                    {
                        throw new OperationCanceledException();
                    }
                    return data;
                }
            }
            catch (OperationCanceledException ex)
            {
                // Handle the cancellation
                // Aquí puedes realizar las acciones necesarias si se ha solicitado la cancelación
                throw new HttpStatusCodeException(Responses.SetRequestCanceledResponse(ex, "La operación ha sido cancelada,GetDataByReportTitulosDapper"));
            }
            catch (Exception ex)
            {
                throw new HttpStatusCodeException(System.Net.HttpStatusCode.InternalServerError, ex);
            }
            finally
            {
                _coreContextDapper.CloseConnection();
            }
        }
    }
}
