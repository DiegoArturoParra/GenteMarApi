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
    public class ReportesDatosBasicosRepository : GenericRepository<GENTEMAR_DATOSBASICOS>
    {
        public async Task<IEnumerable<DatosBasicosReportDTO>> GetDataByReportCsv(DatosBasicosReportFilter reportFilter, CancellationTokenSource tokenSource)
        {
            var query = _context.VIEW_REPORTE_DATOSBASICOS.AsNoTracking().AsQueryable();
            if (reportFilter.EstadosId.Any())
            {
                query = query.Where(y => reportFilter.EstadosId.Contains(y.EstadoId));
            }
            if (reportFilter.GeneroId.HasValue)
            {
                query = query.Where(y => y.GeneroId == reportFilter.GeneroId.Value);
            }
            if (reportFilter.FormacionId.HasValue)
            {
                query = query.Where(y => y.FormacionId == reportFilter.FormacionId.Value);
            }
            if (reportFilter.GradosId.Any())
            {
                query = query.Where(y => reportFilter.GradosId.Contains(y.GradoId));
            }

            if (reportFilter.FechaCreacionInicial.HasValue && reportFilter.FechaCreacionFinal.HasValue)
            {
                var (DateInitial, DateEnd) = Reutilizables.FormatDatesByRange(reportFilter.FechaCreacionInicial.Value, reportFilter.FechaCreacionFinal.Value);
                query = query.Where(x => x.FechaCreacion >= DateInitial && x.FechaCreacion <= DateEnd);
            }
            else
            {
                DateTime fechaActual = DateTime.Now;
                // Establecer el mes y el día a 01
                DateTime fechaDeseada = new DateTime(fechaActual.Year, 1, 1);
                var (DateInitial, DateEnd) = Reutilizables.FormatDatesByRange(fechaDeseada, fechaActual);
                query = query.Where(x => x.FechaCreacion >= DateInitial && x.FechaCreacion <= DateEnd);
            }
            return await query.Select(x => new DatosBasicosReportDTO
            {
                Nombres = x.Nombres,
                Apellidos = x.Apellidos,
                TipoDocumento = x.TipoDocumento,
                DocumentoIdentificacion = x.DocumentoIdentificacion,
                FechaNacimiento = x.FechaNacimiento,
                Genero = x.Genero,
                Estado = x.Estado,
                Formacion = x.Formacion,
                Grado = x.Grado,
                PaisNacimiento = x.PaisNacimiento,
                FechaCreacion = x.FechaCreacion,
                CorreoElectronico = x.CorreoElectronico,
                NumeroContacto = x.NumeroContacto,
                PaisResidencia = x.PaisResidencia,
                Edad = x.Edad
            }).OrderByDescending(x => x.FechaCreacion).AsNoTracking().ToListAsync(tokenSource.Token);
        }
    }
}
