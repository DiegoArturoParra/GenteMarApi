﻿using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class EstupefacienteRepository : GenericRepository<GENTEMAR_ANTECEDENTES>
    {
        public EstupefacienteRepository()
        {
        }
        public EstupefacienteRepository(GenteDeMarCoreContext context) : base(context)
        {
        }

        public IQueryable<ListarEstupefacientesDTO> GetEstupefacientesByFiltro(EstupefacientesFilter filtro)
        {
            var query = _context.VIEW_REPORTE_ANTECEDENTES.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro.Identificacion))
            {
                query = query.Where(x => x.Identificacion.Equals(filtro.Identificacion));
            }

            if (filtro.FechaInicial.HasValue && filtro.FechaFinal.HasValue)
            {
                var (DateInitial, DateEnd) = Reutilizables.FormatDatesByRange(filtro.FechaInicial.Value, filtro.FechaFinal.Value);
                query = query.Where(x => x.FechaSolicitud >= DateInitial && x.FechaSolicitud <= DateEnd);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(filtro.Identificacion) && string.IsNullOrWhiteSpace(filtro.Radicado) && filtro.ConsolidadoId == 0)
                {
                    var fechaActual = DateTime.Now;
                    var (DateInitial, DateEnd) = Reutilizables.FormatDatesByRange(fechaActual, fechaActual);
                    query = query.Where(x => x.FechaSolicitud >= DateInitial && x.FechaSolicitud <= DateEnd);
                }

            }

            if (filtro.EstadosId.Any())
            {
                query = query.Where(x => filtro.EstadosId.Contains(x.EstadoId));
            }
            else if (!filtro.FechaInicial.HasValue && !filtro.FechaFinal.HasValue &&
                string.IsNullOrWhiteSpace(filtro.Identificacion)
                && string.IsNullOrWhiteSpace(filtro.Radicado)
                && filtro.ConsolidadoId == 0)
            {
                List<int> containsEstado = new List<int> { (int)EstadoEstupefacienteEnum.ParaEnviar, (int)EstadoEstupefacienteEnum.Consulta };
                query = query.Where(x => containsEstado.Contains(x.EstadoId));
            }

            if (filtro.ConsolidadoId > 0)
            {
                query = query.Where(x => x.ConsolidadoId == filtro.ConsolidadoId);
            }

            if (filtro.TramiteId > 0)
            {
                query = query.Where(x => x.TramiteId == filtro.TramiteId);
            }
            if (!string.IsNullOrWhiteSpace(filtro.Radicado))
            {
                query = query.Where(x => x.NumeroRadicadoSgdea.Contains(filtro.Radicado.Trim()));
            }

            query = query.Where(x => x.EstadoEsActivo == true);
            var intermediateQuery = query.Select(grupo => new ListarEstupefacientesDTO
            {
                Id = grupo.AntecedenteId,
                Nombre = grupo.Nombres,
                Apellido = grupo.Apellidos,
                Radicado = grupo.NumeroRadicadoSgdea,
                Tramite = grupo.TipoTramite,
                Capitania = grupo.SiglaCapitania + "-" + grupo.NombreCapitania,
                Documento = grupo.Identificacion,
                Estado = grupo.Estado,
                Vigencia = grupo.EsVigente,
                FechaAprobacion = grupo.FechaAprobacion,
                FechaSolicitudSedeCentral = grupo.FechaSolicitud,
                FechaVigencia = grupo.FechaVigencia,
                NumeroConsolidado = grupo.NumeroConsolidado,
                FechaCreacion = grupo.FechaCreacion,
                UsuarioCreacion = grupo.UsuarioCreador,
                FechaModificacion = grupo.FechaModificacion,
                UsuarioModificacion = grupo.UsuarioModificador,
                ExpedientesPorEntidad = (from relacion in _context.GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES
                                         join expediente in _context.GENTEMAR_EXPEDIENTE on relacion.id_expediente equals expediente.id_expediente
                                         join entidad in _context.GENTEMAR_ENTIDAD_ANTECEDENTE on relacion.id_entidad equals entidad.id_entidad
                                         where relacion.id_antecedente == grupo.AntecedenteId
                                         select new ExpedienteEntidadDTO
                                         {
                                             Entidad = entidad.entidad,
                                             NumeroDeExpediente = expediente.numero_expediente,
                                         }).ToList()
            });
            return intermediateQuery.AsNoTracking();
        }


        public async Task<IEnumerable<string>> GetRadicadosDeEstupefacientes(string identificacion)
        {
            return await (from antecedente in _context.GENTEMAR_ANTECEDENTES
                          join usuario in _context.GENTEMAR_ANTECEDENTES_DATOSBASICOS on antecedente.id_gentemar_antecedente equals usuario.id_gentemar_antecedente
                          where usuario.identificacion.Equals(identificacion)
                          select antecedente.numero_sgdea).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<string>> GetRadicadosTitulosByDocumento(string identificacion)
        {
            var list = await GetRadicadosDeEstupefacientes(identificacion);

            return await (from titulo in _context.GENTEMAR_TITULOS
                          join usuario in _context.GENTEMAR_DATOSBASICOS on titulo.id_gentemar equals usuario.id_gentemar
                          where usuario.documento_identificacion.Equals(identificacion) && !list.Contains(titulo.radicado)
                          select titulo.radicado).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<decimal>> GetRadicadosLicenciasByDocumento(string identificacion)
        {
            var list = await GetRadicadosDeEstupefacientes(identificacion);
            return await (from titulo in _context.GENTEMAR_LICENCIAS
                          join usuario in _context.GENTEMAR_DATOSBASICOS on titulo.id_gentemar equals usuario.id_gentemar
                          where usuario.documento_identificacion.Equals(identificacion) && !list.Contains(titulo.radicado.ToString())
                          select titulo.radicado).ToListAsync();
        }

        public async Task<UsuarioGenteMarDTO> GetDatosGenteMarEstupefaciente(string identificacionConPuntos)
        {
            return await (from GenteMar in _context.GENTEMAR_ANTECEDENTES_DATOSBASICOS
                          join tipoDocumento in _context.APLICACIONES_TIPO_DOCUMENTO on GenteMar.id_tipo_documento
                          equals tipoDocumento.ID_TIPO_DOCUMENTO
                          where GenteMar.identificacion.Equals(identificacionConPuntos)
                          select new
                          {
                              GenteMar,
                              tipoDocumento,
                          }).Select(x => new UsuarioGenteMarDTO()
                          {
                              Nombres = x.GenteMar.nombres,
                              Apellidos = x.GenteMar.apellidos,
                              DocumentoIdentificacion = x.GenteMar.identificacion,
                              Id = x.GenteMar.id_gentemar_antecedente,
                              IdTipoDocumento = x.tipoDocumento.ID_TIPO_DOCUMENTO,
                              FechaNacimiento = x.GenteMar.fecha_nacimiento
                          }).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task CreateWithPersonGenteMar(GENTEMAR_ANTECEDENTES entidad, GENTEMAR_ANTECEDENTES_DATOSBASICOS datosBasicosAntecedentes)
        {
            try
            {
                BeginTransaction();
                _context.GENTEMAR_ANTECEDENTES_DATOSBASICOS.Add(datosBasicosAntecedentes);
                await SaveAllAsync();
                entidad.id_gentemar_antecedente = datosBasicosAntecedentes.id_gentemar_antecedente;
                _context.GENTEMAR_ANTECEDENTES.Add(entidad);
                await SaveAllAsync();
                CommitTransaction();
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                ObtenerException(ex, entidad);
            }
        }
        public async Task<DetalleEstupefacienteDTO> GetDetallePersonaEstupefaciente(long id)
        {
            var query = await (from antecedenteGenteMar in _context.GENTEMAR_ANTECEDENTES_DATOSBASICOS
                               join antecedente in _context.GENTEMAR_ANTECEDENTES on antecedenteGenteMar.id_gentemar_antecedente equals antecedente.id_gentemar_antecedente
                               join estado in _context.GENTEMAR_ESTADO_ANTECEDENTES on antecedente.id_estado_antecedente equals estado.id_estado_antecedente
                               join tramite in _context.GENTEMAR_TRAMITE_ANTECEDENTE on antecedente.id_tipo_tramite equals tramite.id_tipo_tramite
                               join capitania in _context.APLICACIONES_CAPITANIAS on antecedente.id_capitania equals capitania.ID_CAPITANIA
                               join tipoDocumento in _context.APLICACIONES_TIPO_DOCUMENTO on antecedenteGenteMar.id_tipo_documento equals tipoDocumento.ID_TIPO_DOCUMENTO
                               where antecedente.id_antecedente == id
                               select new
                               {
                                   antecedenteGenteMar,
                                   tipoDocumento,
                                   antecedente,
                                   estado,
                                   tramite,
                                   capitania
                               }).Select(x => new DetalleEstupefacienteDTO()
                               {
                                   AntecedenteDatosBasicosId = x.antecedenteGenteMar.id_gentemar_antecedente,
                                   EstupefacienteId = x.antecedente.id_antecedente,
                                   CapitaniaId = x.capitania.ID_CAPITANIA,
                                   EstadoId = x.estado.id_estado_antecedente,
                                   TramiteId = x.tramite.id_tipo_tramite,
                                   DatosBasicos = new EstupefacienteDatosBasicosDTO
                                   {
                                       Nombres = x.antecedenteGenteMar.nombres,
                                       Apellidos = x.antecedenteGenteMar.apellidos,
                                       Identificacion = x.antecedenteGenteMar.identificacion,
                                       FechaNacimiento = x.antecedenteGenteMar.fecha_nacimiento,
                                       TipoDocumentoId = x.tipoDocumento.ID_TIPO_DOCUMENTO,
                                   },
                                   Estado = x.estado.descripcion_estado_antecedente,
                                   FechaAprobacion = x.antecedente.fecha_aprobacion,
                                   FechaRadicadoSgdea = x.antecedente.fecha_sgdea,
                                   FechaSolicitudSedeCentral = x.antecedente.fecha_solicitud_sede_central,
                                   Radicado = x.antecedente.numero_sgdea,
                                   FechaVigencia = x.antecedente.fecha_vigencia,
                                   Tramite = x.tramite.descripcion_tipo_tramite,
                                   Capitania = x.capitania.SIGLA_CAPITANIA + " - " + x.capitania.DESCRIPCION
                               }).AsNoTracking().FirstOrDefaultAsync();

            return query;
        }

        public async Task EditBulk(IList<GENTEMAR_ANTECEDENTES> estupefacientes, int numeroDeLotes)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    for (int i = 0; i < estupefacientes.Count; i++)
                    {
                        _context.Entry(estupefacientes[i]).State = EntityState.Modified;
                        if ((i + 1) % numeroDeLotes == 0 || i == estupefacientes.Count - 1)
                        {
                            await SaveAllAsync();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ObtenerException(ex, estupefacientes[0]);
                }
            }
        }

        public async Task<List<EstupefacientesExcelDTO>> GetEstupefacientesEstadoInicial()
        {
            var query = (from antecedente in _context.GENTEMAR_ANTECEDENTES
                         join usuario in _context.GENTEMAR_ANTECEDENTES_DATOSBASICOS on antecedente.id_gentemar_antecedente equals usuario.id_gentemar_antecedente
                         join documento in _context.APLICACIONES_TIPO_DOCUMENTO on usuario.id_tipo_documento equals documento.ID_TIPO_DOCUMENTO
                         join tramite in _context.GENTEMAR_TRAMITE_ANTECEDENTE on antecedente.id_tipo_tramite equals tramite.id_tipo_tramite
                         join capitaniaFirma in _context.APLICACIONES_CAPITANIAS on antecedente.id_capitania equals capitaniaFirma.ID_CAPITANIA
                         where antecedente.id_estado_antecedente == (int)EstadoEstupefacienteEnum.ParaEnviar
                         select new
                         {
                             antecedente,
                             usuario,
                             tramite,
                             capitaniaFirma,
                             documento
                         });

            return await query.OrderBy(x => x.antecedente.id_estado_antecedente).Select(m => new EstupefacientesExcelDTO
            {
                EstupefacienteId = m.antecedente.id_antecedente,
                Nombres = m.usuario.nombres,
                Apellidos = m.usuario.apellidos,
                Radicado = m.antecedente.numero_sgdea,
                TipoTramite = m.tramite.descripcion_tipo_tramite,
                LugarTramiteCapitania = m.capitaniaFirma.SIGLA_CAPITANIA + "-" + m.capitaniaFirma.DESCRIPCION,
                Documento = m.usuario.identificacion,
                FechaSolicitudSedeCentral = m.antecedente.fecha_solicitud_sede_central,
                FechaRadicado = m.antecedente.fecha_sgdea,
                FechaNacimiento = m.usuario.fecha_nacimiento,
                TipoDocumento = m.documento.DESCRIPCION
            }).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<ListarEstupefacientesEdicionMasivaDTO>> GetEstupefacientesSinObservaciones(IList<long> listIds)
        {
            var query = (from antecedenteGenteMar in _context.GENTEMAR_ANTECEDENTES_DATOSBASICOS
                         join antecedente in _context.GENTEMAR_ANTECEDENTES on antecedenteGenteMar.id_gentemar_antecedente equals antecedente.id_gentemar_antecedente
                         join tipoDocumento in _context.APLICACIONES_TIPO_DOCUMENTO on antecedenteGenteMar.id_tipo_documento equals tipoDocumento.ID_TIPO_DOCUMENTO
                         join estado in _context.GENTEMAR_ESTADO_ANTECEDENTES on antecedente.id_estado_antecedente equals estado.id_estado_antecedente
                         where antecedente.id_estado_antecedente == (int)EstadoEstupefacienteEnum.Consulta && listIds.Contains(antecedente.id_antecedente)
                         select new
                         {
                             antecedenteGenteMar,
                             tipoDocumento,
                             estado,
                             antecedente
                         });

            var data = await query.Select(x => new ListarEstupefacientesEdicionMasivaDTO()
            {
                Documento = x.antecedenteGenteMar.identificacion,
                EstupefacienteId = x.antecedente.id_antecedente,
                NombreCompleto = x.antecedenteGenteMar.nombres + " " + x.antecedenteGenteMar.apellidos,
                Radicado = x.antecedente.numero_sgdea,
                Estado = x.estado.descripcion_estado_antecedente,
                CountObservaciones = _context.GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES
                .Where(y => y.id_antecedente == x.antecedente.id_antecedente && (y.descripcion_observacion == null
                            || y.descripcion_observacion.ToUpper().Equals(Constantes.SIN_OBSERVACION))).Count(),
                ObservacionExpedientePorEntidad = (from relacion in _context.GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES
                                                   join expediente in _context.GENTEMAR_EXPEDIENTE on relacion.id_expediente equals expediente.id_expediente
                                                   join entidad in _context.GENTEMAR_ENTIDAD_ANTECEDENTE on relacion.id_entidad equals entidad.id_entidad
                                                   where relacion.id_antecedente == x.antecedente.id_antecedente
                                                   select new ExpedienteEntidadObservacionDTO
                                                   {
                                                       Entidad = entidad.entidad,
                                                       NumeroDeExpediente = expediente.numero_expediente,
                                                       Observacion = relacion.descripcion_observacion,
                                                       FechaEntidad = relacion.fecha_respuesta_entidad
                                                   }).ToList()
            }).Where(f => f.CountObservaciones == _context.GENTEMAR_ENTIDAD_ANTECEDENTE.Where(e => e.activo == Constantes.ACTIVO).Count())
            .AsNoTracking().ToListAsync();
            return data;
        }

        public async Task EditBulkWithconsolidated(IList<GENTEMAR_ANTECEDENTES> estupefacientes, int numeroDeLotes,
            string consolidadoOrId, List<CrearExpedienteEntidadDTO> arrayExpedientesEntidad, bool isNew)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    GENTEMAR_CONSOLIDADO newProcedding = new GENTEMAR_CONSOLIDADO();
                    if (isNew)
                    {
                        newProcedding.numero_consolidado = consolidadoOrId;
                        _context.GENTEMAR_CONSOLIDADO.Add(newProcedding);
                        await SaveAllAsync();
                    }

                    var consolidadoId = isNew ? newProcedding.id_consolidado : int.Parse(consolidadoOrId);
                    foreach (var item in arrayExpedientesEntidad)
                    {
                        GENTEMAR_EXPEDIENTE expediente = await _context.GENTEMAR_EXPEDIENTE
                            .FirstOrDefaultAsync(x => x.numero_expediente == item.NumeroExpediente);

                        if (expediente == null)
                        {
                            expediente = new GENTEMAR_EXPEDIENTE
                            {
                                numero_expediente = item.NumeroExpediente,
                            };
                            _context.GENTEMAR_EXPEDIENTE.Add(expediente);
                            await SaveAllAsync();
                        }
                        for (int i = 0; i < estupefacientes.Count; i++)
                        {
                            GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES relacion = new GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES
                            {
                                id_antecedente = estupefacientes[i].id_antecedente,
                                id_expediente = expediente.id_expediente,
                                id_consolidado = consolidadoId,
                                id_entidad = item.EntidadId,
                            };
                            _context.GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES.Add(relacion);

                            _context.Entry(estupefacientes[i]).State = EntityState.Modified;
                            if ((i + 1) % numeroDeLotes == 0 || i == estupefacientes.Count - 1)
                            {
                                await SaveAllAsync();
                            }
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ObtenerException(ex, estupefacientes[0]);
                }
            }
        }

        public async Task ActualizarAntecedenteWithGenteDeMar(GENTEMAR_ANTECEDENTES dataEstupefaciente,
            GENTEMAR_ANTECEDENTES_DATOSBASICOS estupefacienteDatosBasicosActual, GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = null)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    dataEstupefaciente.Observacion.id_antecedente = dataEstupefaciente.id_antecedente;
                    _context.GENTEMAR_OBSERVACIONES_ANTECEDENTES.Add(dataEstupefaciente.Observacion);
                    await SaveAllAsync();
                    if (repositorio != null)
                    {
                        repositorio.IdModulo = dataEstupefaciente.Observacion.id_observacion.ToString();
                        _context.GENTEMAR_REPOSITORIO_ARCHIVOS.Add(repositorio);
                    }
                    _context.Entry(dataEstupefaciente).State = EntityState.Modified;
                    _context.Entry(estupefacienteDatosBasicosActual).State = EntityState.Modified;
                    await SaveAllAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ObtenerException(ex, dataEstupefaciente);
                }
            }
        }

        public async Task<ValidacionVciteGenteMarDTO> ContieneEstupefacienteVigentePorEstado(long gentemarAntecedenteId)
        {
            var fechaActual = DateTime.Now;
            var idsEstado = new List<int> { (int)EstadoEstupefacienteEnum.ParaEnviar, (int)EstadoEstupefacienteEnum.Consulta };
            var query = await (from antecedente in _context.GENTEMAR_ANTECEDENTES
                               join persona in _context.GENTEMAR_ANTECEDENTES_DATOSBASICOS
                               on antecedente.id_gentemar_antecedente equals persona.id_gentemar_antecedente
                               join estado in _context.GENTEMAR_ESTADO_ANTECEDENTES
                               on antecedente.id_estado_antecedente equals estado.id_estado_antecedente
                               where antecedente.id_gentemar_antecedente == gentemarAntecedenteId
                               && idsEstado.Contains(antecedente.id_estado_antecedente)
                               || (antecedente.id_gentemar_antecedente == gentemarAntecedenteId
                                   && antecedente.id_estado_antecedente == (int)EstadoEstupefacienteEnum.Exitosa
                                   && antecedente.fecha_vigencia >= fechaActual)
                               orderby antecedente.fecha_solicitud_sede_central descending
                               select new ValidacionVciteGenteMarDTO
                               {
                                   NombreCompleto = persona.nombres + " " + persona.apellidos,
                                   Identificacion = persona.identificacion,
                                   Estado = estado.descripcion_estado_antecedente,
                                   FechaVigencia = antecedente.fecha_vigencia
                               }).ToListAsync();

            return query.FirstOrDefault();
        }

        public async Task ChangeNarcoticStateIfAllVerifications(long idAntecedente)
        {
            var entidades = await _context.GENTEMAR_ENTIDAD_ANTECEDENTE.Where(y => y.activo == Constantes.ACTIVO).Select(x => x.id_entidad).ToListAsync();

            var hayNegativos = await _context.GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES
                                 .Where(y => y.id_antecedente == idAntecedente
                                 && entidades.Contains(y.id_entidad)
                                 && y.verificacion_exitosa == false).AnyAsync();
            if (hayNegativos)
            {
                var antecedente = await _context.GENTEMAR_ANTECEDENTES.Where(y => y.id_antecedente == idAntecedente).FirstOrDefaultAsync();
                antecedente.id_estado_antecedente = (int)EstadoEstupefacienteEnum.Negativa;
                await Update(antecedente);
                return;
            }


            var observacionesVerificacion = await _context.GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES
                        .Where(y => y.id_antecedente == idAntecedente
                        && entidades.Contains(y.id_entidad)
                        && y.verificacion_exitosa == Constantes.ACTIVO).CountAsync();

            if (observacionesVerificacion == entidades.Count)
            {
                var antecedente = await _context.GENTEMAR_ANTECEDENTES.Where(y => y.id_antecedente == idAntecedente).FirstOrDefaultAsync();
                antecedente.id_estado_antecedente = (int)EstadoEstupefacienteEnum.Exitosa;
                antecedente.fecha_aprobacion = DateTime.Now;
                antecedente.fecha_vigencia = DateTime.Now.AddYears(5);
                await Update(antecedente);
            }
        }

        public async Task<bool> ChangeNarcoticsStateIfAllVerificationsAreTrue(IList<GENTEMAR_ANTECEDENTES> antecedentes)
        {
            var countEntidades = await _context.GENTEMAR_ENTIDAD_ANTECEDENTE.Where(y => y.activo == true).CountAsync();
            var antecedenteIds = antecedentes.Select(a => a.id_antecedente).ToList();

            var counts = await _context.GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES
                .Where(eoa => antecedenteIds.Contains(eoa.id_antecedente) && eoa.verificacion_exitosa == true)
                .GroupBy(eoa => eoa.id_antecedente)
                .Select(group => new
                {
                    AntecedenteId = group.Key,
                    CountObservaciones = group.Count()
                })
                .ToListAsync();
            var antecedenteCounts = counts.ToDictionary(item => item.AntecedenteId, item => item.CountObservaciones);
            var isChange = false;
            foreach (var antecedente in antecedentes)
            {
                if (antecedenteCounts.ContainsKey(antecedente.id_antecedente) && antecedenteCounts[antecedente.id_antecedente] == countEntidades)
                {
                    // Cambiar el estado del estupefaciente en el antecedente
                    antecedente.id_estado_antecedente = (int)EstadoEstupefacienteEnum.Exitosa;
                    _context.Entry(antecedente).State = EntityState.Modified;
                    isChange = true;
                }
            }
            return isChange;
        }
    }
}
