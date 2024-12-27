using DIMARCore.UIEntities.DTOs.Reports;
using DIMARCore.UIEntities.QueryFilters.Reports;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class ReportesTituloRepository : GenericRepository<GENTEMAR_TITULOS>
    {

        public async Task<IEnumerable<TitulosReportDTO>> GetDataByReportCsv(TitulosReportFilter reportFilter, CancellationTokenSource tokenSource)
        {

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

            var query = _context.VIEW_REPORTE_TITULOS.AsNoTracking().AsQueryable();

            query = query
                  .Where(x => !reportFilter.GeneroId.HasValue || x.GeneroId == reportFilter.GeneroId.Value)
                  .Where(x => !reportFilter.CapitaniasId.Any() || reportFilter.CapitaniasId.Contains(x.CapitaniaId))
                  .Where(x => !reportFilter.EstadosTramiteId.Any() || reportFilter.EstadosTramiteId.Contains(x.EstadoTramiteId))
                  .Where(x => !reportFilter.SeccionesId.Any() || reportFilter.SeccionesId.Contains(x.SeccionId))
                  .Where(x => !reportFilter.CargosTituloId.Any() || reportFilter.CargosTituloId.Contains(x.CargoTituloId));

            query = query.Where(x => x.FechaExpedicion >= reportFilter.FechaExpedicionInicial
                                && x.FechaExpedicion <= reportFilter.FechaExpedicionFinal);

            if (reportFilter.FechaVencimientoInicial.HasValue && reportFilter.FechaVencimientoFinal.HasValue)
            {
                query = query.Where(x => x.FechaVencimiento >= reportFilter.FechaVencimientoInicial
                                    && x.FechaVencimiento <= reportFilter.FechaVencimientoFinal);
            }

            query = query.OrderByDescending(x => x.FechaExpedicion);
            var results = await query
                .Select(x => new TitulosReportDTO
                {
                    Nombres = x.Nombres,
                    Apellidos = x.Apellidos,
                    Edad = x.Edad,
                    Genero = x.Genero,
                    FechaNacimiento = x.FechaNacimiento,
                    TipoDocumento = x.TipoDocumento,
                    DocumentoIdentificacion = x.DocumentoIdentificacion,
                    FechaExpedicion = x.FechaExpedicion,
                    FechaVencimiento = x.FechaVencimiento,
                    Capitania = x.Capitania,
                    EstadoTramite = x.EstadoTramite,
                    Radicado = x.Radicado,
                    Seccion = x.Seccion,
                    Cargos = x.Cargos,
                    Reglas = x.Reglas
                }).AsNoTracking().ToListAsync(tokenSource.Token);

            return results;
        }
    }
}
