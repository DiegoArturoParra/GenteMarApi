using DIMARCore.UIEntities.DTOs.Reports;
using DIMARCore.UIEntities.QueryFilters.Reports;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class ReportesLicenciaRepository : GenericRepository<GENTEMAR_LICENCIAS>
    {
        public async Task<IEnumerable<LicenciasReportDTO>> GetDataByReportCsv(LicenciasReportFilter reportFilter, CancellationTokenSource tokenSource)
        {
            var query = _context.VIEW_REPORTE_LICENCIAS.AsNoTracking().AsQueryable();

            if (reportFilter.EstadosTramiteId.Any())
            {
                query = query.Where(y => reportFilter.EstadosTramiteId.Contains(y.EstadoTramiteId));
            }
            if (reportFilter.GeneroId.HasValue)
            {
                query = query.Where(y => y.GeneroId == reportFilter.GeneroId.Value);
            }
            if (reportFilter.CapitaniasId.Any())
            {
                query = query.Where(y => reportFilter.CapitaniasId.Contains(y.CapitaniaId));
            }
            if (reportFilter.TiposDeLicenciaId.Any())
            {
                query = query.Where(y => reportFilter.TiposDeLicenciaId.Contains(y.TipoLicenciaId));
            }
            if (reportFilter.SeccionesId.Any())
            {
                query = query.Where(y => reportFilter.SeccionesId.Contains(y.SeccionId));
            }
            if (reportFilter.ClasesId.Any())
            {
                query = query.Where(y => reportFilter.ClasesId.Contains(y.ClaseId));
            }

            if (reportFilter.ActividadesId.Any())
            {
                query = query.Where(y => reportFilter.ActividadesId.Contains(y.ActividadId));
            }

            if (reportFilter.CargosLicenciaId.Any())
            {
                query = query.Where(y => reportFilter.CargosLicenciaId.Contains(y.CargoLicenciaId));
            }


            if (reportFilter.FechaExpedicionInicial.HasValue && reportFilter.FechaExpedicionFinal.HasValue)
            {
                var (DateInitial, DateEnd) = Reutilizables.FormatDatesByRange(reportFilter.FechaExpedicionInicial.Value, reportFilter.FechaExpedicionFinal.Value);
                query = query.Where(x => x.FechaExpedicion >= DateInitial && x.FechaExpedicion <= DateEnd);
            }
            else
            {
                DateTime fechaActual = DateTime.Now;
                // Establecer el mes y el día a 01
                DateTime fechaDeseada = new DateTime(fechaActual.Year, 1, 1);
                var (DateInitial, DateEnd) = Reutilizables.FormatDatesByRange(fechaDeseada, fechaActual);
                query = query.Where(x => x.FechaExpedicion >= DateInitial && x.FechaExpedicion <= DateEnd);
            }


            if (reportFilter.FechaVencimientoInicial.HasValue && reportFilter.FechaVencimientoFinal.HasValue)
            {
                var (DateInitial, DateEnd) = Reutilizables.FormatDatesByRange(reportFilter.FechaVencimientoInicial.Value, reportFilter.FechaVencimientoFinal.Value);
                query = query.Where(x => x.FechaVencimiento >= DateInitial && x.FechaVencimiento <= DateEnd);
            }

            query = query.OrderByDescending(x => x.FechaExpedicion);
            return await query.Select(x => new LicenciasReportDTO
            {
                Nombres = x.Nombres,
                Apellidos = x.Apellidos,
                TipoDocumento = x.TipoDocumento,
                DocumentoIdentificacion = x.DocumentoIdentificacion,
                FechaNacimiento = x.FechaNacimiento,
                Genero = x.Genero,
                EstadoTramite = x.EstadoTramite,
                FechaExpedicion = x.FechaExpedicion,
                FechaVencimiento = x.FechaVencimiento,
                Capitania = x.Capitania,
                Radicado = x.Radicado,
                CodigoLicencia = x.CodigoLicencia,
                TipoLicencia = x.TipoLicencia,
                Seccion = x.Seccion,
                Actividad = x.Actividad,
                Categoria = x.Categoria,
                CargoLicencia = x.CargoLicencia
            }).AsNoTracking().ToListAsync(tokenSource.Token);
        }


    }
}
