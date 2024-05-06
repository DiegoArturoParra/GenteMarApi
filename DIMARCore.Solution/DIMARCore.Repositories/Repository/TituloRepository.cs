using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Config;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class TituloRepository : GenericRepository<GENTEMAR_TITULOS>
    {
        private IQueryable<ListadoTituloDTO> FiltroTitulos(string Identificacion, long IdGenteMar = 0)
        {
            var query = (from titulo in _context.GENTEMAR_TITULOS
                         join usuario in _context.GENTEMAR_DATOSBASICOS on titulo.id_gentemar equals usuario.id_gentemar
                         join capitaniaFirma in _context.APLICACIONES_CAPITANIAS on titulo.id_capitania equals capitaniaFirma.ID_CAPITANIA
                         join capitaniaFirmante in _context.APLICACIONES_CAPITANIAS on titulo.id_capitania_firmante equals capitaniaFirmante.ID_CAPITANIA
                         join tramite in _context.GENTEMAR_ESTADO_TITULO on titulo.id_estado_tramite equals tramite.id_estado_tramite
                         join solicitud in _context.APLICACIONES_TIPO_SOLICITUD on titulo.id_tipo_solicitud equals solicitud.ID_TIPO_SOLICITUD
                         where titulo.id_gentemar == IdGenteMar || usuario.documento_identificacion.Equals(Identificacion)
                         select new
                         {
                             titulo,
                             usuario,
                             tramite,
                             capitaniaFirma,
                             capitaniaFirmante,
                             solicitud,
                         });

            var listado = query.OrderBy(x => x.titulo.id_titulo).Select(m => new ListadoTituloDTO
            {
                FechaExpedicion = m.titulo.fecha_expedicion,
                NombreUsuario = m.usuario.nombres + " " + m.usuario.apellidos,
                DocumentoIdentificacion = m.usuario.documento_identificacion,
                CapitaniaFirma = m.capitaniaFirma.SIGLA_CAPITANIA + " " + m.capitaniaFirma.DESCRIPCION,
                CapitaniaFirmante = m.capitaniaFirmante.SIGLA_CAPITANIA + " " + m.capitaniaFirmante.DESCRIPCION,
                Solicitud = m.solicitud.DESCRIPCION,
                EstadoTramite = m.tramite.descripcion_tramite,
                FechaVencimiento = m.titulo.fecha_vencimiento,
                Id = m.titulo.id_titulo,
                Radicado = m.titulo.radicado,
                ContienePrevista = _context.TABLA_SGDEA_PREVISTAS.Any(x => x.radicado.ToString().Equals(m.titulo.radicado) && x.estado == Constantes.PREVISTAGENERADA),
                Cargos = (from tituloCargoRegla in _context.GENTEMAR_TITULO_REGLA_CARGOS
                          join reglaCargo in _context.GENTEMAR_REGLAS_CARGO on tituloCargoRegla.id_cargo_regla equals reglaCargo.id_cargo_regla
                          join regla in _context.GENTEMAR_REGLAS on reglaCargo.id_regla equals regla.id_regla
                          join cargo in _context.GENTEMAR_CARGO_TITULO on reglaCargo.id_cargo_titulo equals cargo.id_cargo_titulo
                          join nivel in _context.GENTEMAR_NIVEL on reglaCargo.id_nivel equals nivel.id_nivel
                          where tituloCargoRegla.id_titulo == m.titulo.id_titulo && tituloCargoRegla.es_eliminado == false
                          select new
                          {
                              tituloCargoRegla,
                              reglaCargo,
                              regla,
                              cargo,
                              nivel
                          }).Select(c => new InfoCargosDTO
                          {
                              Nivel = c.nivel.nivel,
                              CargoTitulo = c.cargo.cargo,
                              Regla = c.regla.nombre_regla
                          }).ToList()
            });
            return listado.AsNoTracking();
        }

        public async Task CrearTitulo(GENTEMAR_TITULOS datos, GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = null)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.GENTEMAR_TITULOS.Add(datos);
                    await SaveAllAsync();
                    if (datos.Observacion != null)
                    {
                        datos.Observacion.id_titulo = datos.id_titulo;
                        _context.GENTEMAR_OBSERVACIONES_TITULOS.Add(datos.Observacion);
                        await SaveAllAsync();
                    }

                    foreach (var cargo in datos.Cargos)
                    {
                        var jsons = await GetJsonHabilitacionesyFuncionesDeUnCargo(cargo.FuncionesId, cargo.HabilitacionesId);

                        GENTEMAR_TITULO_REGLA_CARGOS tituloCargoRegla = new GENTEMAR_TITULO_REGLA_CARGOS
                        {
                            id_titulo = datos.id_titulo,
                            es_eliminado = false,
                            id_cargo_regla = cargo.CargoReglaId,
                            habilitaciones_json = jsons.jsonHabilitaciones,
                            funciones_json = jsons.jsonFunciones,
                        };
                        _context.GENTEMAR_TITULO_REGLA_CARGOS.Add(tituloCargoRegla);
                        await SaveAllAsync();
                        await AgregarHabiltacionesyFuncionesDeUnCargo(tituloCargoRegla.id_titulo_cargo_regla, cargo.HabilitacionesId, cargo.FuncionesId);
                    }
                    if (repositorio != null)
                    {
                        repositorio.IdModulo = datos.Observacion.id_observacion.ToString();
                        _context.GENTEMAR_REPOSITORIO_ARCHIVOS.Add(repositorio);
                    }
                    await SaveAllAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ObtenerException(ex, datos);
                }
            }
        }

        private async Task<(string jsonHabilitaciones, string jsonFunciones)> GetJsonHabilitacionesyFuncionesDeUnCargo(List<int> funcionesId, List<int> habilitacionesId)
        {
            var dataHabilitaciones = new List<HabilitacionCargoDTO>();
            var dataFunciones = new List<FuncionCargoDTO>();
            if (habilitacionesId.Any())
            {
                dataHabilitaciones = await (from habilitacion in _context.GENTEMAR_HABILITACION
                                            join habilitacionId in habilitacionesId on habilitacion.id_habilitacion equals habilitacionId
                                            select new HabilitacionCargoDTO
                                            {
                                                HabilitacionId = habilitacion.id_habilitacion,
                                                Descripcion = habilitacion.habilitacion
                                            }).ToListAsync();
            }
            if (funcionesId.Any())
            {
                dataFunciones = await (from funcion in _context.GENTEMAR_FUNCIONES
                                       join funcionId in funcionesId on funcion.id_funcion equals funcionId
                                       select new FuncionCargoDTO
                                       {
                                           FuncionId = funcion.id_funcion,
                                           Descripcion = funcion.funcion,
                                           Limitacion = funcion.limitacion_funcion
                                       }).ToListAsync();
            }

            return (JsonConvert.SerializeObject(dataHabilitaciones), JsonConvert.SerializeObject(dataFunciones));
        }

        public async Task ActualizarTitulo(GENTEMAR_TITULOS datos, GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = null)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    datos.Observacion.id_titulo = datos.id_titulo;
                    _context.GENTEMAR_OBSERVACIONES_TITULOS.Add(datos.Observacion);
                    await SaveAllAsync();
                    var cargos = await _context.GENTEMAR_TITULO_REGLA_CARGOS.Where(x => x.id_titulo == datos.id_titulo && x.es_eliminado == false).ToListAsync();
                    if (datos.Cargos.Any())
                    {
                        foreach (var cargo in datos.Cargos)
                        {
                            if (cargo.TituloCargoReglaId == 0)
                            {
                                var jsons = await GetJsonHabilitacionesyFuncionesDeUnCargo(cargo.FuncionesId, cargo.HabilitacionesId);
                                GENTEMAR_TITULO_REGLA_CARGOS tituloCargoRegla = new GENTEMAR_TITULO_REGLA_CARGOS
                                {
                                    id_titulo = datos.id_titulo,
                                    es_eliminado = false,
                                    id_cargo_regla = cargo.CargoReglaId,
                                    habilitaciones_json = jsons.jsonHabilitaciones,
                                    funciones_json = jsons.jsonFunciones,
                                };
                                _context.GENTEMAR_TITULO_REGLA_CARGOS.Add(tituloCargoRegla);
                                await SaveAllAsync();
                                await AgregarHabiltacionesyFuncionesDeUnCargo(tituloCargoRegla.id_titulo_cargo_regla, cargo.HabilitacionesId, cargo.FuncionesId);
                            }
                            else
                            {
                                var TitulocargoRegla = cargos.FirstOrDefault(x => x.id_titulo_cargo_regla == cargo.TituloCargoReglaId);
                                var jsons = await GetJsonHabilitacionesyFuncionesDeUnCargo(cargo.FuncionesId, cargo.HabilitacionesId);
                                TitulocargoRegla.habilitaciones_json = jsons.jsonHabilitaciones;
                                TitulocargoRegla.funciones_json = jsons.jsonFunciones;
                                TitulocargoRegla.id_cargo_regla = cargo.CargoReglaId;
                                await SaveAllAsync();
                                await AgregarHabiltacionesyFuncionesDeUnCargo(TitulocargoRegla.id_titulo_cargo_regla, cargo.HabilitacionesId, cargo.FuncionesId, true);
                            }
                        }
                    }

                    if (repositorio != null)
                    {
                        repositorio.IdModulo = datos.Observacion.id_observacion.ToString();
                        _context.GENTEMAR_REPOSITORIO_ARCHIVOS.Add(repositorio);
                    }
                    await Update(datos);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ObtenerException(ex, datos);
                }
            }
        }

        private async Task AgregarHabiltacionesyFuncionesDeUnCargo(long tituloCargoReglaId, List<int> habilitacionesId, List<int> funcionesId, bool IsEdit = false)
        {
            if (IsEdit)
            {
                var dataHabilitaciones = await _context.GENTEMAR_TITULO_REGLA_CARGOS_HABILITACION
                    .Where(x => x.id_titulo_cargo_regla == tituloCargoReglaId).ToListAsync();

                var dataFunciones = await _context.GENTEMAR_TITULO_REGLA_CARGOS_FUNCION
                    .Where(x => x.id_titulo_cargo_regla == tituloCargoReglaId).ToListAsync();

                if (dataHabilitaciones.Any())
                {
                    _context.GENTEMAR_TITULO_REGLA_CARGOS_HABILITACION.RemoveRange(dataHabilitaciones);
                }
                if (dataFunciones.Any())
                {
                    _context.GENTEMAR_TITULO_REGLA_CARGOS_FUNCION.RemoveRange(dataFunciones);
                }
            }
            if (habilitacionesId.Any())
            {
                foreach (var habilitacion in habilitacionesId)
                {
                    GENTEMAR_TITULO_REGLA_CARGOS_HABILITACION tituloCargoHabilitacion = new GENTEMAR_TITULO_REGLA_CARGOS_HABILITACION
                    {
                        id_habilitacion = habilitacion,
                        id_titulo_cargo_regla = tituloCargoReglaId
                    };
                    _context.GENTEMAR_TITULO_REGLA_CARGOS_HABILITACION.Add(tituloCargoHabilitacion);
                }

            }
            if (funcionesId.Any())
            {
                foreach (var funcion in funcionesId)
                {
                    GENTEMAR_TITULO_REGLA_CARGOS_FUNCION tituloCargoFuncion = new GENTEMAR_TITULO_REGLA_CARGOS_FUNCION
                    {
                        id_funcion = funcion,
                        id_titulo_cargo_regla = tituloCargoReglaId
                    };
                    _context.GENTEMAR_TITULO_REGLA_CARGOS_FUNCION.Add(tituloCargoFuncion);
                }
            }
        }

        public async Task<InfoTituloDTO> GetTituloById(long id)
        {
            var query = (from titulo in _context.GENTEMAR_TITULOS
                         join usuario in _context.GENTEMAR_DATOSBASICOS on titulo.id_gentemar equals usuario.id_gentemar
                         join capitaniaFirma in _context.APLICACIONES_CAPITANIAS on titulo.id_capitania equals capitaniaFirma.ID_CAPITANIA
                         join capitaniaFirmante in _context.APLICACIONES_CAPITANIAS on titulo.id_capitania_firmante equals capitaniaFirmante.ID_CAPITANIA
                         join tramite in _context.GENTEMAR_ESTADO_TITULO on titulo.id_estado_tramite equals tramite.id_estado_tramite
                         join solicitud in _context.APLICACIONES_TIPO_SOLICITUD on titulo.id_tipo_solicitud equals solicitud.ID_TIPO_SOLICITUD
                         join refrendo in _context.APLICACIONES_TIPO_REFRENDO on titulo.id_tipo_refrendo equals refrendo.ID_TIPO_CERTIFICADO
                         where titulo.id_titulo == id
                         select new
                         {
                             titulo,
                             usuario,
                             tramite,
                             capitaniaFirma,
                             capitaniaFirmante,
                             solicitud,
                             refrendo
                         });


            var tituloNavegacion = await query.OrderBy(x => x.titulo.id_titulo).Select(m => new InfoTituloDTO
            {
                TituloId = m.titulo.id_titulo,
                FechaExpedicion = m.titulo.fecha_expedicion,
                NombreUsuario = m.usuario.nombres + " " + m.usuario.apellidos,
                DocumentoIdentificacion = m.usuario.documento_identificacion,
                CapitaniaFirmanteId = m.capitaniaFirmante.ID_CAPITANIA,
                EstadoTramiteId = m.tramite.id_estado_tramite,
                TipoSolicitudId = m.solicitud.ID_TIPO_SOLICITUD,
                CapitaniaFirma = m.capitaniaFirma.SIGLA_CAPITANIA + " " + m.capitaniaFirma.DESCRIPCION,
                CapitaniaFirmante = m.capitaniaFirmante.SIGLA_CAPITANIA + " " + m.capitaniaFirmante.DESCRIPCION,
                Solicitud = m.solicitud.DESCRIPCION,
                Pais = (from pais in _context.TABLA_NAV_BAND
                        where m.titulo.cod_pais.Equals(pais.cod_pais)
                        select pais.cod_pais + " " + pais.des_pais).FirstOrDefault(),
                Tramite = m.tramite.descripcion_tramite,
                FechaVencimiento = m.titulo.fecha_vencimiento,
                Radicado = m.titulo.radicado,
                TipoRefrendoId = m.refrendo.ID_TIPO_CERTIFICADO,
                Cargos = (from tituloCargoRegla in _context.GENTEMAR_TITULO_REGLA_CARGOS
                          join reglaCargo in _context.GENTEMAR_REGLAS_CARGO on tituloCargoRegla.id_cargo_regla equals reglaCargo.id_cargo_regla
                          join regla in _context.GENTEMAR_REGLAS on reglaCargo.id_regla equals regla.id_regla
                          join cargo in _context.GENTEMAR_CARGO_TITULO on reglaCargo.id_cargo_titulo equals cargo.id_cargo_titulo
                          join seccion in _context.GENTEMAR_SECCION_TITULOS on cargo.id_seccion equals seccion.id_seccion
                          join capacidad in _context.GENTEMAR_CAPACIDAD on reglaCargo.id_capacidad equals capacidad.id_capacidad
                          join nivel in _context.GENTEMAR_NIVEL on reglaCargo.id_nivel equals nivel.id_nivel
                          where tituloCargoRegla.id_titulo == id && tituloCargoRegla.es_eliminado == false
                          select new
                          {
                              tituloCargoRegla,
                              reglaCargo,
                              regla,
                              cargo,
                              seccion,
                              capacidad,
                              nivel

                          }).Select(c => new ListarCargosTituloDTO
                          {
                              TituloCargoReglaId = c.tituloCargoRegla.id_titulo_cargo_regla,
                              ReglaCargoId = c.tituloCargoRegla.id_cargo_regla,
                              CapacidadId = c.capacidad.id_capacidad,
                              Capacidad = c.capacidad.capacidad,
                              Nivel = c.nivel.nivel,
                              NivelId = c.nivel.id_nivel,
                              CargoId = c.cargo.id_cargo_titulo,
                              CargoTitulo = c.cargo.cargo,
                              SeccionId = c.seccion.id_seccion,
                              Seccion = c.seccion.actividad_a_bordo,
                              ReglaId = c.regla.id_regla,
                              Regla = c.regla.nombre_regla,
                              HabilitacionesJson = c.tituloCargoRegla.habilitaciones_json,
                              FuncionesJson = c.tituloCargoRegla.funciones_json,
                              Habilitaciones = (from tituloHabilitacion in _context.GENTEMAR_TITULO_REGLA_CARGOS_HABILITACION
                                                join habilitacion in _context.GENTEMAR_HABILITACION on tituloHabilitacion.id_habilitacion
                                                equals habilitacion.id_habilitacion
                                                where tituloHabilitacion.id_titulo_cargo_regla == c.tituloCargoRegla.id_titulo_cargo_regla
                                                select new HabilitacionCargoDTO
                                                {
                                                    HabilitacionId = habilitacion.id_habilitacion,
                                                    Descripcion = habilitacion.habilitacion
                                                }).ToList(),
                              Funciones = (from tituloFuncion in _context.GENTEMAR_TITULO_REGLA_CARGOS_FUNCION
                                           join funcion in _context.GENTEMAR_FUNCIONES on tituloFuncion.id_funcion equals funcion.id_funcion
                                           where tituloFuncion.id_titulo_cargo_regla == c.tituloCargoRegla.id_titulo_cargo_regla
                                           select new FuncionCargoDTO
                                           {
                                               FuncionId = funcion.id_funcion,
                                               Descripcion = funcion.funcion,
                                               Limitacion = funcion.limitacion_funcion
                                           }).ToList()
                          }).ToList()
            }).FirstOrDefaultAsync();
            return tituloNavegacion;
        }

        public async Task<(FechasRadioOperadoresDTO fechas, bool HayTitulosPorSeccionPuente)> GetFechasRadioOperadores(long idGenteMar)
        {
            bool hay = false;
            FechasRadioOperadoresDTO fechas = new FechasRadioOperadoresDTO();
            string seccionPuente = "SECCIÓN DE PUENTE";
            var idSeccionPuente = await _context.GENTEMAR_SECCION_TITULOS.Where(x => x.actividad_a_bordo.ToUpper().Equals(seccionPuente.ToUpper())).
                Select(x => x.id_seccion).FirstOrDefaultAsync();
            if (idSeccionPuente > 0)
            {
                fechas = await (from titulo in _context.GENTEMAR_TITULOS
                                join tituloReglaCargo in _context.GENTEMAR_TITULO_REGLA_CARGOS on titulo.id_titulo equals tituloReglaCargo.id_titulo
                                join cargoRegla in _context.GENTEMAR_REGLAS_CARGO on tituloReglaCargo.id_cargo_regla equals cargoRegla.id_cargo_regla
                                join cargoTitulo in _context.GENTEMAR_CARGO_TITULO on cargoRegla.id_cargo_titulo equals cargoTitulo.id_cargo_titulo
                                join seccion in _context.GENTEMAR_SECCION_TITULOS on cargoTitulo.id_seccion equals seccion.id_seccion
                                where titulo.id_gentemar == idGenteMar && seccion.id_seccion == idSeccionPuente
                                orderby titulo.fecha_vencimiento descending
                                select new FechasRadioOperadoresDTO
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


        /// <summary>
        /// metodo para obtener los datos del usuario y del titulo para la plantilla
        /// por id de la licencia.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PlantillaTituloDTO> GetPlantillaTitulo(long id)
        {
            var query = await (from titulo in _context.GENTEMAR_TITULOS
                               join datosBasicos in _context.GENTEMAR_DATOSBASICOS on titulo.id_gentemar equals datosBasicos.id_gentemar
                               join ciuExpDoc in _context.TABLA_NAV_BAND on datosBasicos.cod_pais equals ciuExpDoc.cod_pais
                               join capitaniaFirmante in _context.APLICACIONES_CAPITANIAS on titulo.id_capitania_firmante equals capitaniaFirmante.ID_CAPITANIA
                               where titulo.id_titulo == id
                               select new PlantillaTituloDTO
                               {
                                   Foto = (from archivo in _context.GENTEMAR_REPOSITORIO_ARCHIVOS
                                           where archivo.IdModulo == datosBasicos.id_gentemar.ToString()
                                           select new
                                           {
                                               archivo
                                           }).OrderByDescending(x => x.archivo.FechaCreacion).Select(x => x.archivo.RutaArchivo).FirstOrDefault(),
                                   NombreCompleto = datosBasicos.nombres + " " + datosBasicos.apellidos,
                                   Documento = datosBasicos.documento_identificacion,
                                   FechaNacimiento = datosBasicos.fecha_nacimiento,
                                   CiudadExpedicion = datosBasicos.id_municipio_expedicion.HasValue
                                                        ? (from munExpDoc in _context.APLICACIONES_MUNICIPIO
                                                           where munExpDoc.ID_MUNICIPIO == datosBasicos.id_municipio_expedicion
                                                           select munExpDoc.NOMBRE_MUNICIPIO + " " + ciuExpDoc.des_pais).FirstOrDefault() : string.Empty,
                                   FechaExpedicion = titulo.fecha_expedicion,
                                   FechaVencimiento = titulo.fecha_vencimiento,
                                   NumeroTitulo = titulo.id_titulo,
                                   Radicado = titulo.radicado,
                                   CapitaniaFirmante = capitaniaFirmante.DESCRIPCION,
                                   FirmaBase64 = (from dimFirma in _context.TABLA_DIM_PERSONAS
                                                  where dimFirma.cedula.Equals(datosBasicos.documento_identificacion)
                                                  select dimFirma != null ? dimFirma.FirmaBin : ""
                                                  ).FirstOrDefault(),
                                   Reglas = (from tituloReglaCargos in _context.GENTEMAR_TITULO_REGLA_CARGOS
                                             join reglaCargo in _context.GENTEMAR_REGLAS_CARGO on tituloReglaCargos.id_cargo_regla equals reglaCargo.id_cargo_regla
                                             join regla in _context.GENTEMAR_REGLAS on reglaCargo.id_regla equals regla.id_regla
                                             where tituloReglaCargos.id_titulo == titulo.id_titulo && tituloReglaCargos.es_eliminado == false
                                             select new
                                             {
                                                 regla
                                             }).Select(x => x.regla.nombre_regla).ToList(),
                                   Funcion = (from funciones in _context.GENTEMAR_FUNCIONES
                                              join reglaFuncion in _context.GENTEMAR_REGLA_FUNCION on funciones.id_funcion equals reglaFuncion.id_funcion
                                              join regla in _context.GENTEMAR_REGLAS on reglaFuncion.id_regla equals regla.id_regla
                                              join reglaCargo in _context.GENTEMAR_REGLAS_CARGO on regla.id_regla equals reglaCargo.id_regla
                                              join tituloReglaCargo in _context.GENTEMAR_TITULO_REGLA_CARGOS on reglaCargo.id_cargo_regla equals tituloReglaCargo.id_cargo_regla
                                              where tituloReglaCargo.id_titulo == titulo.id_titulo && tituloReglaCargo.es_eliminado == false
                                              group new { funciones, reglaCargo } by new { funciones.id_funcion, reglaCargo.id_nivel } into grupo
                                              select new
                                              {
                                                  Descripcion = grupo.FirstOrDefault().funciones.funcion,
                                                  Nivel = (from nivel in _context.GENTEMAR_NIVEL
                                                           where nivel.id_nivel == grupo.FirstOrDefault().reglaCargo.id_nivel
                                                           select nivel.nivel).FirstOrDefault(),
                                                  Limitacion = grupo.FirstOrDefault().funciones.limitacion_funcion
                                              }).ToList(),
                                   Cargo = (from cargo in _context.GENTEMAR_CARGO_TITULO
                                            join reglaCargo in _context.GENTEMAR_REGLAS_CARGO on cargo.id_cargo_titulo equals reglaCargo.id_cargo_titulo
                                            join tituloReglaCargo in _context.GENTEMAR_TITULO_REGLA_CARGOS on reglaCargo.id_cargo_regla equals tituloReglaCargo.id_cargo_regla
                                            where tituloReglaCargo.id_titulo == titulo.id_titulo && tituloReglaCargo.es_eliminado == false
                                            select new
                                            {
                                                Cargo = cargo.cargo,
                                                Capacidad = (from capacidad in _context.GENTEMAR_CAPACIDAD
                                                             where capacidad.id_capacidad == reglaCargo.id_capacidad
                                                             select new
                                                             {
                                                                 capacidad
                                                             }).Select(x => x.capacidad.capacidad).FirstOrDefault()
                                            }).ToList(),
                               }).FirstOrDefaultAsync();

            return query;
        }

    }
}
