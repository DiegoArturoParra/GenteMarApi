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
    public class ReportesVciteRepository : GenericRepository<GENTEMAR_ANTECEDENTES>
    {
        public async Task<IEnumerable<DataPieChartVciteDTO>> GetDataByPieChartEstadosEstupefaciente(ReportPieChartVciteFilter reportPieChartFilter,
            CancellationTokenSource tokenSource)
        {

            var fechaActual = DateTime.Now;
            var (DateInitial, DateEnd) = Reutilizables.FormatDatesByRange(reportPieChartFilter.FechaSolicitudInicial, reportPieChartFilter.FechaSolicitudFinal);

            return await (from antecedente in _context.GENTEMAR_ANTECEDENTES
                          join estado in _context.GENTEMAR_ESTADO_ANTECEDENTES on antecedente.id_estado_antecedente equals estado.id_estado_antecedente
                          where estado.activo == Constantes.ACTIVO
                          && antecedente.fecha_vigencia >= fechaActual
                          && antecedente.fecha_solicitud_sede_central >= DateInitial
                          && antecedente.fecha_solicitud_sede_central <= DateEnd
                          group estado by estado.id_estado_antecedente into grupo
                          select new DataPieChartVciteDTO
                          {
                              data = grupo.Count(),
                              Estado = grupo.Max(e => e.descripcion_estado_antecedente),
                              IsVigente = "VIGENTE"
                          })
                         .Union
                         (from antecedente in _context.GENTEMAR_ANTECEDENTES
                          join estado in _context.GENTEMAR_ESTADO_ANTECEDENTES on antecedente.id_estado_antecedente equals estado.id_estado_antecedente
                          where estado.activo == Constantes.ACTIVO
                          && antecedente.fecha_vigencia <= fechaActual
                          && antecedente.fecha_solicitud_sede_central >= DateInitial
                          && antecedente.fecha_solicitud_sede_central <= DateEnd
                          group estado by estado.id_estado_antecedente into grupo
                          select new DataPieChartVciteDTO
                          {
                              data = grupo.Count(),
                              Estado = grupo.Max(e => e.descripcion_estado_antecedente),
                              IsVigente = "NO VIGENTE"
                          })
                         .Union
                         (from antecedente in _context.GENTEMAR_ANTECEDENTES
                          join estado in _context.GENTEMAR_ESTADO_ANTECEDENTES on antecedente.id_estado_antecedente equals estado.id_estado_antecedente
                          where estado.activo == Constantes.ACTIVO &&
                          antecedente.fecha_vigencia == null
                          && antecedente.fecha_solicitud_sede_central >= DateInitial
                          && antecedente.fecha_solicitud_sede_central <= DateEnd
                          group estado by estado.id_estado_antecedente into grupo
                          select new DataPieChartVciteDTO
                          {
                              data = grupo.Count(),
                              Estado = grupo.Max(e => e.descripcion_estado_antecedente),
                              IsVigente = "NO CONTIENE FECHA DE VIGENCIA."
                          }).AsNoTracking().ToListAsync(tokenSource.Token);
        }

        public async Task<IEnumerable<VciteReportDTO>> GetDataByReportEstupefacientes(EstupefacientesReportFilter reportFilter, CancellationTokenSource tokenSource)
        {
            var fechaActual = DateTime.Now;
            var query = _context.VIEW_REPORTE_ANTECEDENTES.AsNoTracking().AsQueryable();

            if (reportFilter.EstadosId.Any())
            {
                query = query.Where(y => reportFilter.EstadosId.Contains(y.EstadoId));
            }
            if (reportFilter.ConsolidadosId.Any())
            {
                query = query.Where(y => reportFilter.ConsolidadosId.Contains(y.ConsolidadoId));
            }
            if (reportFilter.IsVigente.HasValue)
            {

                if (reportFilter.IsVigente.Value)
                {
                    query = query.Where(y => y.FechaVigencia >= fechaActual);
                }
                else
                {
                    query = query.Where(y => y.FechaVigencia <= fechaActual);
                }
            }

            if (reportFilter.FechaCreacionInicial.HasValue && reportFilter.FechaCreacionFinal.HasValue)
            {
                var (DateInitial, DateEnd) = Reutilizables.FormatDatesByRange(reportFilter.FechaCreacionInicial.Value, reportFilter.FechaCreacionFinal.Value);
                reportFilter.FechaCreacionInicial = DateInitial;
                reportFilter.FechaCreacionFinal = DateEnd;
                query = query.Where(x => x.FechaCreacion >= reportFilter.FechaCreacionInicial
                && x.FechaCreacion <= reportFilter.FechaCreacionFinal);
            }
            else
            {
                // Establecer el mes y el día a 01
                DateTime fechaDeseada = new DateTime(fechaActual.Year, 1, 1);
                var (DateInitial, DateEnd) = Reutilizables.FormatDatesByRange(fechaDeseada, fechaActual);
                query = query.Where(x => x.FechaCreacion >= DateInitial
                                                             && x.FechaCreacion <= DateEnd);
            }
            return await query.Select(x => new VciteReportDTO
            {
                Nombres = x.Nombres,
                Apellidos = x.Apellidos,
                TipoDocumento = x.TipoDocumento,
                Documento = x.Identificacion,
                FechaCreacion = x.FechaCreacion,
                FechaVigencia = x.FechaVigencia,
                Estado = x.Estado,
                EsVigente = x.EsVigente,
                FechaNacimiento = x.FechaNacimiento,
                FechaAprobacion = x.FechaAprobacion,
                FechaRadicadoSGDEA = x.FechaRadicadoSgdea,
                FechaSolicitud = x.FechaSolicitud,
                TipoTramite = x.TipoTramite,
                NumeroRadicadoSGDEA = x.NumeroRadicadoSgdea
            }).AsNoTracking().ToListAsync(tokenSource.Token);
        }

        public async Task<IEnumerable<VciteInfoHistoricoPersonaDTO>> GetHistoricoByPersonaIdentificacion(long idGenteDeMar)
        {
            var fechaActual = DateTime.Now;
            return await (from antecedente in _context.GENTEMAR_ANTECEDENTES
                          join estado in _context.GENTEMAR_ESTADO_ANTECEDENTES on antecedente.id_estado_antecedente equals estado.id_estado_antecedente
                          join tramite in _context.GENTEMAR_TRAMITE_ANTECEDENTE on antecedente.id_tipo_tramite equals tramite.id_tipo_tramite
                          where antecedente.id_gentemar_antecedente == idGenteDeMar
                          select new VciteInfoHistoricoPersonaDTO
                          {
                              Estado = estado.descripcion_estado_antecedente,
                              NumeroRadicadoSGDEA = antecedente.numero_sgdea,
                              FechaRadicadoSGDEA = antecedente.fecha_sgdea,
                              TipoTramite = tramite.descripcion_tipo_tramite,
                              FechaSolicitud = antecedente.fecha_solicitud_sede_central,
                              FechaAprobacion = antecedente.fecha_aprobacion,
                              FechaVigencia = antecedente.fecha_vigencia,
                              EsVigente = antecedente.fecha_vigencia.HasValue ? antecedente.fecha_vigencia.Value >= fechaActual ? "Si" : "No" : "No contiene fecha aún.",
                              FechaCreacion = antecedente.FechaCreacion
                          }).AsNoTracking().ToListAsync();
        }
    }
}
