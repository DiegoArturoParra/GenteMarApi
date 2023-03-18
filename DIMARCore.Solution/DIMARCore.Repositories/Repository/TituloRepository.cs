using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class TituloRepository : GenericRepository<GENTEMAR_TITULOS>
    {
        private IQueryable<ListadoTituloDTO> FiltroTitulos(string Identificacion, long Id = 0)
        {
            var query = (from titulo in _context.GENTEMAR_TITULOS
                         join usuario in _context.GENTEMAR_DATOSBASICOS on titulo.id_gentemar equals usuario.id_gentemar
                         join cargoRegla in _context.GENTEMAR_REGLAS_CARGO on titulo.id_cargo_regla equals cargoRegla.id_cargo_regla
                         join reglas in _context.GENTEMAR_REGLAS on cargoRegla.id_regla equals reglas.id_regla
                         join cargoTitulo in _context.GENTEMAR_CARGO_TITULO on cargoRegla.id_cargo_titulo equals cargoTitulo.id_cargo_titulo
                         join capitaniaFirma in _context.APLICACIONES_CAPITANIAS on titulo.id_capitania equals capitaniaFirma.ID_CAPITANIA
                         join capitaniaFirmante in _context.APLICACIONES_CAPITANIAS on titulo.id_capitania_firmante equals capitaniaFirmante.ID_CAPITANIA
                         join pais in _context.TABLA_NAV_BAND on titulo.cod_pais equals pais.cod_pais
                         join tramite in _context.APLICACIONES_ESTADO_TRAMITE on titulo.id_estado_tramite equals tramite.id_estado_tramite
                         join solicitud in _context.APLICACIONES_TIPO_SOLICITUD on titulo.id_tipo_solicitud equals solicitud.ID_TIPO_SOLICITUD
                         join nivel in _context.GENTEMAR_NIVEL on cargoRegla.id_nivel equals nivel.id_nivel
                         select new
                         {
                             titulo,
                             usuario,
                             cargoRegla,
                             cargoTitulo,
                             pais,
                             tramite,
                             capitaniaFirma,
                             capitaniaFirmante,
                             solicitud,
                             reglas,
                             nivel,
                         });

            if (!string.IsNullOrWhiteSpace(Identificacion))
            {
                query = query.Where(x => x.usuario.documento_identificacion.Equals(Identificacion));
            }
            else if (Id > 0)
            {
                query = query.Where(x => x.titulo.id_gentemar == Id);
            }
            var listado = query.OrderBy(x => x.titulo.id_titulo).Select(m => new ListadoTituloDTO
            {
                FechaExpedicion = m.titulo.fecha_expedicion,
                NombreUsuario = m.usuario.nombres + " " + m.usuario.apellidos,
                DocumentoIdentificacion = m.usuario.documento_identificacion,
                CargoTitulo = m.cargoTitulo.cargo,
                CapitaniaFirma = m.capitaniaFirma.SIGLA_CAPITANIA + " " + m.capitaniaFirma.DESCRIPCION,
                CapitaniaFirmante = m.capitaniaFirmante.SIGLA_CAPITANIA + " " + m.capitaniaFirmante.DESCRIPCION,
                Solicitud = m.solicitud.DESCRIPCION,
                Nivel = m.nivel.nivel,
                Regla = m.reglas.Regla,
                EstadoTramite = m.tramite.descripcion_tramite,
                FechaVencimiento = m.titulo.fecha_vencimiento,
                Id = m.titulo.id_titulo,
                Radicado = m.titulo.radicado
            });
            return listado;
        }

        public async Task CrearTitulo(GENTEMAR_TITULOS datos, GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = null)
        {
            using (_context)
            {
                using (var trassaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.GENTEMAR_TITULOS.Add(datos);
                        await SaveAllAsync();
                        if (datos.Observacion != null)
                        {
                            datos.Observacion.id_titulo = datos.id_titulo;
                            _context.GENTEMAR_OBSERVACIONES_TITULOS.Add(datos.Observacion);
                            if (repositorio != null)
                            {
                                repositorio.IdModulo = datos.Observacion.id_observacion.ToString();
                                repositorio.IdUsuarioCreador = datos.Observacion.usuario_creador_registro;
                                repositorio.FechaHoraCreacion = datos.Observacion.fecha_hora_creacion;
                                _context.GENTEMAR_REPOSITORIO_ARCHIVOS.Add(repositorio);
                            }
                        }
                        if (datos.HabilitacionesId.Count > 0)
                        {
                            foreach (var item in datos.HabilitacionesId)
                            {
                                GENTEMAR_TITULO_HABILITACION habiltacionByTitulo = new GENTEMAR_TITULO_HABILITACION
                                {
                                    id_habilitacion = item,
                                    id_titulo = datos.id_titulo
                                };
                                _context.GENTEMAR_TITULO_HABILITACION.Add(habiltacionByTitulo);
                            }
                        }
                        if (datos.FuncionesId.Count > 0)
                        {
                            foreach (var item in datos.FuncionesId)
                            {
                                GENTEMAR_TITULO_FUNCION funcionByTitulo = new GENTEMAR_TITULO_FUNCION
                                {
                                    id_funcion = item,
                                    id_titulo = datos.id_titulo
                                };
                                _context.GENTEMAR_TITULO_FUNCION.Add(funcionByTitulo);
                            }
                        }
                        await SaveAllAsync();
                        trassaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        trassaction.Rollback();
                        ObtenerException(ex, datos);
                    }
                }
            }
        }
        public async Task ActualizarTitulo(GENTEMAR_TITULOS datos, GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = null)
        {
            using (_context)
            {
                using (var trassaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        datos.Observacion.id_titulo = datos.id_titulo;
                        _context.GENTEMAR_OBSERVACIONES_TITULOS.Add(datos.Observacion);
                        await SaveAllAsync();
                        _context.GENTEMAR_TITULO_HABILITACION.RemoveRange(_context.GENTEMAR_TITULO_HABILITACION.Where(x => x.id_titulo == datos.id_titulo));
                        _context.GENTEMAR_TITULO_FUNCION.RemoveRange(_context.GENTEMAR_TITULO_FUNCION.Where(x => x.id_titulo == datos.id_titulo));
                        if (datos.HabilitacionesId.Count > 0)
                        {
                            foreach (var item in datos.HabilitacionesId)
                            {
                                GENTEMAR_TITULO_HABILITACION habiltacionByTitulo = new GENTEMAR_TITULO_HABILITACION
                                {
                                    id_habilitacion = item,
                                    id_titulo = datos.id_titulo
                                };
                                _context.GENTEMAR_TITULO_HABILITACION.Add(habiltacionByTitulo);
                            }
                        }
                        if (datos.FuncionesId.Count > 0)
                        {
                            foreach (var item in datos.FuncionesId)
                            {
                                GENTEMAR_TITULO_FUNCION funcionByTitulo = new GENTEMAR_TITULO_FUNCION
                                {
                                    id_funcion = item,
                                    id_titulo = datos.id_titulo
                                };
                                _context.GENTEMAR_TITULO_FUNCION.Add(funcionByTitulo);
                            }
                        }
                        if (repositorio != null)
                        {
                            repositorio.IdModulo = datos.Observacion.id_observacion.ToString();
                            repositorio.IdUsuarioCreador = datos.Observacion.usuario_creador_registro;
                            repositorio.FechaHoraCreacion = datos.Observacion.fecha_hora_creacion;
                            _context.GENTEMAR_REPOSITORIO_ARCHIVOS.Add(repositorio);
                        }

                        await Update(datos);
                        trassaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        trassaction.Rollback();
                        ObtenerException(ex, datos);
                    }
                }
            }
        }
        public async Task<InfoTituloDTO> GetTituloById(long id)
        {
            var query = (from titulo in _context.GENTEMAR_TITULOS
                         join usuario in _context.GENTEMAR_DATOSBASICOS on titulo.id_gentemar equals usuario.id_gentemar
                         join cargoRegla in _context.GENTEMAR_REGLAS_CARGO on titulo.id_cargo_regla equals cargoRegla.id_cargo_regla
                         join regla in _context.GENTEMAR_REGLAS on cargoRegla.id_regla equals regla.id_regla
                         join capacidad in _context.GENTEMAR_CAPACIDAD on cargoRegla.id_capacidad equals capacidad.id_capacidad
                         join cargoTitulo in _context.GENTEMAR_CARGO_TITULO on cargoRegla.id_cargo_titulo equals cargoTitulo.id_cargo_titulo
                         join seccion in _context.GENTEMAR_SECCION_TITULOS on cargoTitulo.id_seccion equals seccion.id_seccion
                         join capitaniaFirma in _context.APLICACIONES_CAPITANIAS on titulo.id_capitania equals capitaniaFirma.ID_CAPITANIA
                         join capitaniaFirmante in _context.APLICACIONES_CAPITANIAS on titulo.id_capitania_firmante equals capitaniaFirmante.ID_CAPITANIA
                         join pais in _context.TABLA_NAV_BAND on titulo.cod_pais equals pais.cod_pais
                         join tramite in _context.APLICACIONES_ESTADO_TRAMITE on titulo.id_estado_tramite equals tramite.id_estado_tramite
                         join solicitud in _context.APLICACIONES_TIPO_SOLICITUD on titulo.id_tipo_solicitud equals solicitud.ID_TIPO_SOLICITUD
                         join nivel in _context.GENTEMAR_NIVEL on cargoRegla.id_nivel equals nivel.id_nivel
                         where titulo.id_titulo == id
                         select new
                         {
                             titulo,
                             usuario,
                             cargoRegla,
                             cargoTitulo,
                             seccion,
                             pais,
                             tramite,
                             capitaniaFirma,
                             capitaniaFirmante,
                             solicitud,
                             regla,
                             nivel,
                             capacidad
                         });


            var listado = await query.OrderBy(x => x.titulo.id_titulo).Select(m => new InfoTituloDTO
            {
                TituloId = m.titulo.id_titulo,
                FechaExpedicion = m.titulo.fecha_expedicion,
                NombreUsuario = m.usuario.nombres + " " + m.usuario.apellidos,
                DocumentoIdentificacion = m.usuario.documento_identificacion,
                CargoTitulo = m.cargoTitulo.cargo,
                CargoId = m.cargoTitulo.id_cargo_titulo,
                CapitaniaFirmanteId = m.capitaniaFirmante.ID_CAPITANIA,
                EstadoTramiteId = m.tramite.id_estado_tramite,
                ReglaId = m.regla.id_regla,
                SeccionId = m.seccion.id_seccion,
                TipoSolicitudId = m.solicitud.ID_TIPO_SOLICITUD,
                CapitaniaFirma = m.capitaniaFirma.SIGLA_CAPITANIA + " " + m.capitaniaFirma.DESCRIPCION,
                CapitaniaFirmante = m.capitaniaFirmante.SIGLA_CAPITANIA + " " + m.capitaniaFirmante.DESCRIPCION,
                Solicitud = m.solicitud.DESCRIPCION,
                Nivel = m.nivel.nivel,
                Pais = m.pais.cod_pais + " " + m.pais.des_pais,
                Tramite = m.tramite.descripcion_tramite,
                FechaVencimiento = m.titulo.fecha_vencimiento,
                Radicado = m.titulo.radicado,
                Seccion = m.seccion.actividad_a_bordo,
                Habilitaciones = (from tituloHabilitacion in _context.GENTEMAR_TITULO_HABILITACION
                                  join habilitacion in _context.GENTEMAR_HABILITACION on tituloHabilitacion.id_habilitacion
                                  equals habilitacion.id_habilitacion
                                  where tituloHabilitacion.id_titulo == id
                                  select new HabilitacionInfoDTO
                                  {
                                      Id = habilitacion.id_habilitacion,
                                      Descripcion = habilitacion.habilitacion
                                  }).ToList(),
                Regla = m.regla.Regla,
                Capacidad = m.capacidad.capacidad,
                CapacidadId = m.capacidad.id_capacidad,
                Funciones = (from tituloFuncion in _context.GENTEMAR_TITULO_FUNCION
                             join funcion in _context.GENTEMAR_FUNCIONES on tituloFuncion.id_funcion equals funcion.id_funcion
                             where tituloFuncion.id_titulo == m.titulo.id_titulo
                             select new FuncionesTituloDTO
                             {
                                 Id = funcion.id_funcion,
                                 Descripcion = funcion.funcion,
                                 Limitacion = funcion.limitacion_funcion == string.Empty ? "Ninguna" : funcion.limitacion_funcion
                             }).ToList(),

            }).FirstOrDefaultAsync();

            return listado;
        }

        public async Task<(FechasDTO fechas, bool HayTitulosPorSeccionPuente)> GetFechasRadioOperadores(long idGenteMar)
        {
            bool hay = false;
            FechasDTO fechas = new FechasDTO();
            string seccionPuente = "SECCIÓN DE PUENTE";
            var idSeccionPuente = await _context.GENTEMAR_SECCION_TITULOS.Where(x => x.actividad_a_bordo.ToUpper().Equals(seccionPuente.ToUpper())).
                Select(x => x.id_seccion).FirstOrDefaultAsync();
            if (idSeccionPuente > 0)
            {
                fechas = await (from titulo in _context.GENTEMAR_TITULOS
                                join cargoRegla in _context.GENTEMAR_REGLAS_CARGO on titulo.id_cargo_regla equals cargoRegla.id_cargo_regla
                                join cargoTitulo in _context.GENTEMAR_CARGO_TITULO on cargoRegla.id_cargo_titulo equals cargoTitulo.id_cargo_titulo
                                join seccion in _context.GENTEMAR_SECCION_TITULOS on cargoTitulo.id_seccion equals seccion.id_seccion
                                where titulo.id_gentemar == idGenteMar && seccion.id_seccion == idSeccionPuente
                                orderby titulo.fecha_vencimiento descending
                                select new FechasDTO
                                {
                                    FechaExpedicion = titulo.fecha_expedicion,
                                    FechaVencimiento = titulo.fecha_vencimiento
                                }).FirstOrDefaultAsync();
                if (fechas != null)
                {
                    hay = true;
                }
            }
            return (fechas, hay);
        }

        public IQueryable<ListadoTituloDTO> GetTitulosQueryable()
        {
            var listado = FiltroTitulos(string.Empty);
            return listado;
        }

        public async Task<IEnumerable<ListadoTituloDTO>> GetTitulosFiltro(string identificacion, long id = 0)
        {
            return await FiltroTitulos(identificacion, id).ToListAsync();
        }
    }
}
