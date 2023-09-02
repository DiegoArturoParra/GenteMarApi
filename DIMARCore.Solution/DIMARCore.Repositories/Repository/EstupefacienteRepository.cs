using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.Utilities.Config;
using DIMARCore.Utilities.Enums;
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
        public IQueryable<ListadoEstupefacientesDTO> GetAntecedentesByFiltro(EstupefacientesFilter filtro)
        {
            var query = (from antecedente in _context.GENTEMAR_ANTECEDENTES
                         join usuario in _context.GENTEMAR_ANTECEDENTES_DATOSBASICOS on antecedente.id_gentemar_antecedente equals usuario.id_gentemar_antecedente
                         join estado in _context.GENTEMAR_ESTADO_ANTECEDENTES on antecedente.id_estado_antecedente equals estado.id_estado_antecedente
                         join tramite in _context.GENTEMAR_TIPO_TRAMITE on antecedente.id_tipo_tramite equals tramite.id_tipo_tramite
                         join capitaniaFirma in _context.APLICACIONES_CAPITANIAS on antecedente.id_capitania equals capitaniaFirma.ID_CAPITANIA
                         join expedienteAntecedente in _context.GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES on antecedente.id_antecedente equals expedienteAntecedente.id_antecedente into eAGroup
                         from expedienteAntecedentes in eAGroup.DefaultIfEmpty()
                         select new
                         {
                             antecedente,
                             usuario,
                             tramite,
                             estado,
                             capitaniaFirma,
                             expedienteAntecedentes,
                         });

            if (!string.IsNullOrWhiteSpace(filtro.IdentificacionConPuntos))
            {
                query = query.Where(x => x.usuario.identificacion.Equals(filtro.IdentificacionConPuntos));
            }

            if (filtro.FechaInicial != null && filtro.FechaFinal != null)
            {
                filtro.FechaInicial = new DateTime(filtro.FechaInicial.Value.Year, filtro.FechaInicial.Value.Month, filtro.FechaInicial.Value.Day, 0, 0, 0, 0);
                filtro.FechaFinal = new DateTime(filtro.FechaFinal.Value.Year, filtro.FechaFinal.Value.Month, filtro.FechaFinal.Value.Day, 0, 0, 0, 0);
                filtro.FechaFinal = filtro.FechaFinal.Value.AddHours(24).AddSeconds(-1);
                query = query.Where(x => x.antecedente.fecha_solicitud_sede_central >= filtro.FechaInicial && x.antecedente.fecha_solicitud_sede_central <= filtro.FechaFinal);
            }

            if (filtro.EstadoId > 0)
            {
                query = query.Where(x => x.antecedente.id_estado_antecedente == filtro.EstadoId);
            }

            if (filtro.ConsolidadoId > 0)
            {
                query = query.Where(x => x.expedienteAntecedentes.id_consolidado == filtro.ConsolidadoId);
            }

            if (filtro.ExpedienteId > 0)
            {
                query = query.Where(x => x.expedienteAntecedentes.id_expediente == filtro.ExpedienteId);
            }

            if (filtro.TramiteId > 0)
            {
                query = query.Where(x => x.antecedente.id_tipo_tramite == filtro.TramiteId);
            }
            if (!string.IsNullOrWhiteSpace(filtro.Radicado))
            {
                query = query.Where(x => x.antecedente.numero_sgdea.Equals(filtro.Radicado));
            }
            var intermediateQuery = query.Select(m => new
            {
                Id = m.antecedente.id_antecedente,
                Nombre = m.usuario.nombres,
                Apellido = m.usuario.apellidos,
                Radicado = m.antecedente.numero_sgdea,
                Tramite = m.tramite.descripcion_tipo_tramite,
                Capitania = m.capitaniaFirma.SIGLA_CAPITANIA + "-" + m.capitaniaFirma.DESCRIPCION,
                Documento = m.usuario.identificacion,
                Estado = m.estado.descripcion_estado_antecedente,
                FechaAprobacion = m.antecedente.fecha_aprobacion,
                FechaSolicitudSedeCentral = m.antecedente.fecha_solicitud_sede_central,
                FechaVigencia = m.antecedente.fecha_vigencia,
                ConsolidadoId = m.expedienteAntecedentes != null ? m.expedienteAntecedentes.id_consolidado : 0,
            });

            var listado = intermediateQuery
                .GroupBy(m => m.Id).OrderByDescending(y => y.FirstOrDefault().Id)
                .Select(grupo => new ListadoEstupefacientesDTO
                {
                    Id = grupo.Key, // Use the grouping key instead of FirstOrDefault
                    Nombre = grupo.FirstOrDefault().Nombre,
                    Apellido = grupo.FirstOrDefault().Apellido,
                    Radicado = grupo.FirstOrDefault().Radicado,
                    Tramite = grupo.FirstOrDefault().Tramite,
                    Capitania = grupo.FirstOrDefault().Capitania,
                    Documento = grupo.FirstOrDefault().Documento,
                    Estado = grupo.FirstOrDefault().Estado,
                    FechaAprobacion = grupo.FirstOrDefault().FechaAprobacion,
                    FechaSolicitudSedeCentral = grupo.FirstOrDefault().FechaSolicitudSedeCentral,
                    FechaVigencia = grupo.FirstOrDefault().FechaVigencia,
                    NumeroConsolidado = grupo.FirstOrDefault().ConsolidadoId == 0 ? "No contiene consolidado aún." : _context.GENTEMAR_CONSOLIDADO
                        .Where(y => y.id_consolidado == grupo.FirstOrDefault().ConsolidadoId)
                        .Select(y => y.numero_consolidado)
                        .FirstOrDefault(),
                    ExpedientesPorEntidad = (from relacion in _context.GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES
                                             join expediente in _context.GENTEMAR_EXPEDIENTE on relacion.id_expediente equals expediente.id_expediente
                                             join entidad in _context.GENTEMAR_ENTIDAD on relacion.id_entidad equals entidad.id_entidad
                                             where relacion.id_antecedente == grupo.Key
                                             select new ExpedienteEntidadDTO
                                             {
                                                 Entidad = entidad.entidad,
                                                 NumeroDeExpediente = expediente.numero_expediente,
                                             }).ToList()
                });
            return listado;
        }


        public async Task<IEnumerable<string>> GetRadicadosDeEstupefacientes(string identificacion)
        {
            return await (from antecedente in _context.GENTEMAR_ANTECEDENTES
                          join usuario in _context.GENTEMAR_ANTECEDENTES_DATOSBASICOS on antecedente.id_gentemar_antecedente equals usuario.id_gentemar_antecedente
                          where usuario.identificacion.Equals(identificacion)
                          select antecedente.numero_sgdea).ToListAsync();
        }
        public async Task<IEnumerable<string>> GetRadicadosTitulosByDocumento(string identificacion)
        {
            var list = await GetRadicadosDeEstupefacientes(identificacion);

            return await (from titulo in _context.GENTEMAR_TITULOS
                          join usuario in _context.GENTEMAR_DATOSBASICOS on titulo.id_gentemar equals usuario.id_gentemar
                          where usuario.documento_identificacion.Equals(identificacion) && !list.Contains(titulo.radicado)
                          select titulo.radicado).ToListAsync();
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
                              FechaNacimiento = x.GenteMar.fecha_nacimiento,
                          }).FirstOrDefaultAsync();
        }

        public async Task CreateWithPersonGenteMar(GENTEMAR_ANTECEDENTES entidad, GENTEMAR_ANTECEDENTES_DATOSBASICOS datosBasicosAntecedentes)
        {
            using (_context)
            {
                using (var trassaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.GENTEMAR_ANTECEDENTES_DATOSBASICOS.Add(datosBasicosAntecedentes);
                        await SaveAllAsync();
                        entidad.id_gentemar_antecedente = datosBasicosAntecedentes.id_gentemar_antecedente;
                        _context.GENTEMAR_ANTECEDENTES.Add(entidad);
                        await SaveAllAsync();
                        trassaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        trassaction.Rollback();
                        ObtenerException(ex, entidad);
                    }
                }
            }
        }
        public async Task<DetalleEstupefacienteDTO> GetDetallePersonaEstupefaciente(long id)
        {
            var query = await (from antecedenteGenteMar in _context.GENTEMAR_ANTECEDENTES_DATOSBASICOS
                               join antecedente in _context.GENTEMAR_ANTECEDENTES on antecedenteGenteMar.id_gentemar_antecedente equals antecedente.id_gentemar_antecedente
                               join estado in _context.GENTEMAR_ESTADO_ANTECEDENTES on antecedente.id_estado_antecedente equals estado.id_estado_antecedente
                               join tramite in _context.GENTEMAR_TIPO_TRAMITE on antecedente.id_tipo_tramite equals tramite.id_tipo_tramite
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
                               }).FirstOrDefaultAsync();

            return query;
        }

        public async Task EditBulk(IList<GENTEMAR_ANTECEDENTES> estupefacientes, int numeroDeLotes)
        {
            using (_context)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < estupefacientes.Count(); i++)
                        {
                            _context.Entry(estupefacientes[i]).State = EntityState.Modified;
                            if (i % numeroDeLotes == 0)
                            {
                                await SaveAllAsync();
                            }
                        }
                        await SaveAllAsync();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ObtenerException(ex, estupefacientes[0]);
                    }
                }
            }
        }

        public async Task<IList<EstupefacientesExcelDTO>> GetEstupefacientesEstadoInicial()
        {
            var query = (from antecedente in _context.GENTEMAR_ANTECEDENTES
                         join usuario in _context.GENTEMAR_ANTECEDENTES_DATOSBASICOS on antecedente.id_gentemar_antecedente equals usuario.id_gentemar_antecedente
                         join documento in _context.APLICACIONES_TIPO_DOCUMENTO on usuario.id_tipo_documento equals documento.ID_TIPO_DOCUMENTO
                         join tramite in _context.GENTEMAR_TIPO_TRAMITE on antecedente.id_tipo_tramite equals tramite.id_tipo_tramite
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
                TipoDocumento = m.documento.SIGLA
            }).ToListAsync();
        }

        public async Task<IEnumerable<EstupefacientesBulkDTO>> GetEstupefacientesSinObservaciones(IList<long> listIds)
        {
            var query = (from antecedenteGenteMar in _context.GENTEMAR_ANTECEDENTES_DATOSBASICOS
                         join antecedente in _context.GENTEMAR_ANTECEDENTES on antecedenteGenteMar.id_gentemar_antecedente equals antecedente.id_gentemar_antecedente
                         join tipoDocumento in _context.APLICACIONES_TIPO_DOCUMENTO on antecedenteGenteMar.id_tipo_documento equals tipoDocumento.ID_TIPO_DOCUMENTO
                         join estado in _context.GENTEMAR_ESTADO_ANTECEDENTES on antecedente.id_estado_antecedente equals estado.id_estado_antecedente
                         where antecedente.id_estado_antecedente == (int)EstadoEstupefacienteEnum.Consulta
                         select new
                         {
                             antecedenteGenteMar,
                             tipoDocumento,
                             estado,
                             antecedente
                         });

            if (listIds.Any())
            {
                query = query.Where(y => listIds.Contains(y.antecedente.id_antecedente));
            }

            var data = await query.Select(x => new EstupefacientesBulkDTO()
            {
                Documento = x.antecedenteGenteMar.identificacion,
                EstupefacienteId = x.antecedente.id_antecedente,
                NombreCompleto = x.antecedenteGenteMar.nombres + " " + x.antecedenteGenteMar.apellidos,
                Radicado = x.antecedente.numero_sgdea,
                Estado = x.estado.descripcion_estado_antecedente,
                CountObservaciones = _context.GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES.Where(y => y.id_antecedente == x.antecedente.id_antecedente
                && y.fecha_respuesta_entidad == null && y.descripcion == null).Count(),
            }).Where(f => f.CountObservaciones == _context.GENTEMAR_ENTIDAD.Where(e => e.activo == true).Count()).ToListAsync();
            return data;
        }

        public async Task EditBulkWithconsolidated(IList<GENTEMAR_ANTECEDENTES> estupefacientes, int numeroDeLotes,
            string consolidadoOrId, List<CrearExpedienteEntidadDTO> arrayExpedientesEntidad, bool isNew)
        {
            using (_context)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var usuarioCreadorRegistro = ClaimsHelper.GetNombreCompletoUsuario();
                        GENTEMAR_CONSOLIDADO newProcedding = new GENTEMAR_CONSOLIDADO();
                        if (isNew)
                        {
                            newProcedding.fecha_hora_creacion = DateTime.Now;
                            newProcedding.numero_consolidado = consolidadoOrId;
                            newProcedding.usuario_creador_registro = usuarioCreadorRegistro;
                            _context.GENTEMAR_CONSOLIDADO.Add(newProcedding);
                            await SaveAllAsync();
                        }

                        var consolidadoId = isNew ? newProcedding.id_consolidado : int.Parse(consolidadoOrId);


                        foreach (var item in arrayExpedientesEntidad)
                        {
                            GENTEMAR_EXPEDIENTE expediente = new GENTEMAR_EXPEDIENTE
                            {
                                numero_expediente = item.NumeroExpediente,
                                fecha_hora_creacion = DateTime.Now,
                                usuario_creador_registro = usuarioCreadorRegistro
                            };
                            _context.GENTEMAR_EXPEDIENTE.Add(expediente);
                            await SaveAllAsync();
                            for (int i = 0; i < estupefacientes.Count(); i++)
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
                                if (i % numeroDeLotes == 0)
                                {
                                    await SaveAllAsync();
                                }
                            }
                        }

                        await SaveAllAsync();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ObtenerException(ex, estupefacientes[0]);
                    }
                }
            }
        }

        public async Task ActualizarAntecedenteWithGenteDeMar(GENTEMAR_ANTECEDENTES dataEstupefaciente,
            GENTEMAR_ANTECEDENTES_DATOSBASICOS estupefacienteDatosBasicosActual, GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = null)
        {
            using (_context)
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
                            repositorio.IdUsuarioCreador = dataEstupefaciente.Observacion.usuario_creador_registro;
                            repositorio.FechaHoraCreacion = dataEstupefaciente.Observacion.fecha_hora_creacion;
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

        }

        public async Task<(bool isContains, string NombreCompleto, string Identificacion, string Estado)> ContieneAntecedenteVigentePorEstado(long gentemarAntecedenteId)
        {
            var query = await (from antecedente in _context.GENTEMAR_ANTECEDENTES
                               join persona in _context.GENTEMAR_ANTECEDENTES_DATOSBASICOS
                               on antecedente.id_gentemar_antecedente equals persona.id_gentemar_antecedente
                               join estado in _context.GENTEMAR_ESTADO_ANTECEDENTES on antecedente.id_estado_antecedente equals estado.id_estado_antecedente
                               where (antecedente.id_estado_antecedente == (int)EstadoEstupefacienteEnum.Consulta
                               || antecedente.id_estado_antecedente == (int)EstadoEstupefacienteEnum.ParaEnviar)
                               && antecedente.id_gentemar_antecedente == gentemarAntecedenteId

                               select new
                               {
                                   NombreCompleto = persona.nombres + " " + persona.apellidos,
                                   Identificacion = persona.identificacion,
                                   Estado = estado.descripcion_estado_antecedente,
                               }).FirstOrDefaultAsync();

            return (query != null, query?.NombreCompleto, query?.Identificacion, query?.Estado);
        }
    }
}
