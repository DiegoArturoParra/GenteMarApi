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
        public async Task<IEnumerable<ReportPieChartVciteDTO>> GetDataByPieChartEstadosEstupefaciente(ReportPieChartVciteFilter reportPieChartFilter)
        {

            var fechaActual = DateTime.Now;
            var (DateInitial, DateEnd) = Reutilizables.FormatDatesByRange(reportPieChartFilter.FechaSolicitudInicial, reportPieChartFilter.FechaSolicitudFinal);

            return await (from antecedente in _context.GENTEMAR_ANTECEDENTES
                          join estado in _context.GENTEMAR_ESTADO_ANTECEDENTES on antecedente.id_estado_antecedente equals estado.id_estado_antecedente
                          where estado.activo == true
                          && antecedente.fecha_vigencia >= fechaActual
                          && antecedente.fecha_solicitud_sede_central >= DateInitial
                          && antecedente.fecha_solicitud_sede_central <= DateEnd
                          group estado by estado.id_estado_antecedente into grupo
                          select new ReportPieChartVciteDTO
                          {
                              CantidadDeDatosPorEstado = grupo.Count(),
                              Estado = grupo.Max(e => e.descripcion_estado_antecedente),
                              IsVigente = "VIGENTE"
                          })
                         .Union
                         (from antecedente in _context.GENTEMAR_ANTECEDENTES
                          join estado in _context.GENTEMAR_ESTADO_ANTECEDENTES on antecedente.id_estado_antecedente equals estado.id_estado_antecedente
                          where estado.activo == true
                          && antecedente.fecha_vigencia <= fechaActual
                          && antecedente.fecha_solicitud_sede_central >= DateInitial
                          && antecedente.fecha_solicitud_sede_central <= DateEnd
                          group estado by estado.id_estado_antecedente into grupo
                          select new ReportPieChartVciteDTO
                          {
                              CantidadDeDatosPorEstado = grupo.Count(),
                              Estado = grupo.Max(e => e.descripcion_estado_antecedente),
                              IsVigente = "NO VIGENTE"
                          })
                         .Union
                         (from antecedente in _context.GENTEMAR_ANTECEDENTES
                          join estado in _context.GENTEMAR_ESTADO_ANTECEDENTES on antecedente.id_estado_antecedente equals estado.id_estado_antecedente
                          where estado.activo == true &&
                          antecedente.fecha_vigencia == null
                          && antecedente.fecha_solicitud_sede_central >= DateInitial
                          && antecedente.fecha_solicitud_sede_central <= DateEnd
                          group estado by estado.id_estado_antecedente into grupo
                          select new ReportPieChartVciteDTO
                          {
                              CantidadDeDatosPorEstado = grupo.Count(),
                              Estado = grupo.Max(e => e.descripcion_estado_antecedente),
                              IsVigente = "NO CONTIENE FECHA DE VIGENCIA."
                          }).ToListAsync();
        }

        public async Task<IEnumerable<VciteReportDTO>> GetDataByReportEstupefacientes(EstupefacientesReportFilter reportFilter, CancellationTokenSource tokenSource)
        {
            var fechaActual = DateTime.Now;
            var query = (from antecedente in _context.GENTEMAR_ANTECEDENTES
                         join usuario in _context.GENTEMAR_ANTECEDENTES_DATOSBASICOS on antecedente.id_gentemar_antecedente equals usuario.id_gentemar_antecedente
                         join tipoDocumento in _context.APLICACIONES_TIPO_DOCUMENTO on usuario.id_tipo_documento equals tipoDocumento.ID_TIPO_DOCUMENTO
                         join estado in _context.GENTEMAR_ESTADO_ANTECEDENTES on antecedente.id_estado_antecedente equals estado.id_estado_antecedente
                         join tramite in _context.GENTEMAR_TRAMITE_ANTECEDENTE on antecedente.id_tipo_tramite equals tramite.id_tipo_tramite
                         join expedienteAntecedente in _context.GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES on antecedente.id_antecedente
                         equals expedienteAntecedente.id_antecedente into eAGroup
                         from expedienteAntecedentes in eAGroup.DefaultIfEmpty()
                         join consolidado in _context.GENTEMAR_CONSOLIDADO on expedienteAntecedentes.id_consolidado
                         equals consolidado.id_consolidado into cGroup
                         from consolidadoDefault in cGroup.DefaultIfEmpty()
                         group new { antecedente, usuario, tipoDocumento, tramite, estado, expedienteAntecedentes, consolidadoDefault }
                         by antecedente.id_antecedente into grupo
                         select new
                         {
                             grupo.Key,
                             grupo.FirstOrDefault().usuario,
                             grupo.FirstOrDefault().tipoDocumento,
                             grupo.FirstOrDefault().antecedente,
                             grupo.FirstOrDefault().tramite,
                             grupo.FirstOrDefault().estado,
                             grupo.FirstOrDefault().expedienteAntecedentes,
                             grupo.FirstOrDefault().consolidadoDefault
                         });

            if (reportFilter.EstadosId.Any())
            {
                query = query.Where(y => reportFilter.EstadosId.Contains(y.estado.id_estado_antecedente));
            }
            if (reportFilter.ConsolidadosId.Any())
            {
                query = query.Where(y => reportFilter.ConsolidadosId.Contains(y.consolidadoDefault.id_consolidado));
            }
            if (reportFilter.IsVigente.HasValue)
            {

                if (reportFilter.IsVigente.Value)
                {
                    query = query.Where(y => y.antecedente.fecha_vigencia >= fechaActual);
                }
                else
                {
                    query = query.Where(y => y.antecedente.fecha_vigencia <= fechaActual);
                }
            }

            if (reportFilter.FechaCreacionInicial.HasValue && reportFilter.FechaCreacionFinal.HasValue)
            {
                var (DateInitial, DateEnd) = Reutilizables.FormatDatesByRange(reportFilter.FechaCreacionInicial.Value, reportFilter.FechaCreacionFinal.Value);
                reportFilter.FechaCreacionInicial = DateInitial;
                reportFilter.FechaCreacionFinal = DateEnd;
                query = query.Where(x => x.antecedente.fecha_hora_creacion >= reportFilter.FechaCreacionInicial
                && x.antecedente.fecha_hora_creacion <= reportFilter.FechaCreacionFinal);
            }
            else
            {
                // Establecer el mes y el día a 01
                DateTime fechaDeseada = new DateTime(fechaActual.Year, 1, 1);
                var (DateInitial, DateEnd) = Reutilizables.FormatDatesByRange(fechaDeseada, fechaActual);
                query = query.Where(x => x.antecedente.fecha_hora_creacion >= DateInitial
                                                             && x.antecedente.fecha_hora_creacion <= DateEnd);
            }
            return await query.Select(x => new VciteReportDTO
            {
                Apellidos = x.usuario.apellidos,
                Nombres = x.usuario.nombres,
                TipoDocumento = x.tipoDocumento.DESCRIPCION,
                Documento = x.usuario.identificacion,
                FechaCreacion = x.antecedente.fecha_hora_creacion,
                FechaVigencia = x.antecedente.fecha_vigencia,
                Estado = x.estado.descripcion_estado_antecedente,
                EsVigente = x.antecedente.fecha_vigencia.HasValue ? x.antecedente.fecha_vigencia.Value >= fechaActual ? "Si" : "No" : "No contiene fecha aún.",
                FechaNacimiento = x.usuario.fecha_nacimiento,
                FechaAprobacion = x.antecedente.fecha_aprobacion,
                FechaRadicadoSGDEA = x.antecedente.fecha_sgdea.Value,
                FechaSolicitud = x.antecedente.fecha_solicitud_sede_central,
                TipoTramite = x.tramite.descripcion_tipo_tramite,
                NumeroRadicadoSGDEA = x.antecedente.numero_sgdea
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
                              FechaRadicadoSGDEA = antecedente.fecha_sgdea.Value,
                              TipoTramite = tramite.descripcion_tipo_tramite,
                              FechaSolicitud = antecedente.fecha_solicitud_sede_central,
                              FechaAprobacion = antecedente.fecha_aprobacion,
                              FechaVigencia = antecedente.fecha_vigencia,
                              EsVigente = antecedente.fecha_vigencia.HasValue ? antecedente.fecha_vigencia.Value >= fechaActual ? "Si" : "No" : "No contiene fecha aún.",
                              FechaCreacion = antecedente.fecha_hora_creacion
                          }).ToListAsync();

        }
    }
}
