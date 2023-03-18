using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Enums;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class LicenciaRepository : GenericRepository<GENTEMAR_LICENCIAS>
    {
        /// <summary>
        /// Lista de licencias por id 
        /// <param name="id">Id del Actividad</param>
        /// </summary>
        /// <returns>Lista de Actividades</returns>
        /// <tabla>GENTEMAR_ACTIVIDAD</tabla>
        public IList<LicenciaDTO> GetlicenciaIdUsuario(long id)
        {

            var resultado = (from licencia in _context.GENTEMAR_LICENCIAS
                             join cargoLicencia in _context.GENTEMAR_CARGO_LICENCIA on
                             licencia.id_cargo_licencia equals cargoLicencia.id_cargo_licencia
                             join estadoLicencia in _context.GENTEMAR_ESTADO_LICENCIAS on
                             licencia.id_estado_licencia equals estadoLicencia.id_estado_licencias
                             join capitania in _context.APLICACIONES_CAPITANIAS on
                             licencia.id_capitania equals capitania.ID_CAPITANIA
                             join capitaniaFirmante in _context.APLICACIONES_CAPITANIAS on
                             licencia.id_capitania equals capitaniaFirmante.ID_CAPITANIA
                             where licencia.id_gentemar == id
                             select new LicenciaDTO
                             {
                                 Activo = licencia.activo,
                                 Radicado = licencia.radicado,
                                 IdTramite = licencia.id_tramite,
                                 IdLicencia = licencia.id_licencia,
                                 IdGentemar = licencia.id_gentemar,
                                 IdEstadoLicencia = licencia.id_estado_licencia,
                                 IdCargoLicencia = licencia.id_cargo_licencia,
                                 IdCapitaniaFirmante = licencia.id_capitania_firmante,
                                 IdCapitania = licencia.id_capitania,
                                 FechaVencimiento = licencia.fecha_vencimiento,
                                 FechaExpedicion = licencia.fecha_expedicion,
                                 Capitania = new CapitaniaDTO
                                 {
                                     Descripcion = capitania.DESCRIPCION,
                                     Id = capitania.ID_CAPITANIA,
                                     Sigla = capitania.SIGLA_CAPITANIA
                                 },
                                 CapitaniaFirmante = new CapitaniaDTO
                                 {
                                     Descripcion = capitaniaFirmante.DESCRIPCION,
                                     Id = capitaniaFirmante.ID_CAPITANIA,
                                     Sigla = capitaniaFirmante.SIGLA_CAPITANIA
                                 },
                                 CargoLicencia = new CargoLicenciaDTO
                                 {
                                     IdCargoLicencia = cargoLicencia.id_cargo_licencia,
                                     CargoLicencia = cargoLicencia.cargo_licencia,
                                     CodigoLicencia = cargoLicencia.codigo_licencia
                                 },
                                 Estado = new EstadoLicenciaDTO
                                 {
                                     IdEstadoLicencias = estadoLicencia.id_estado_licencias,
                                     DescripcionEstado = estadoLicencia.descripcion_estado
                                 }

                             }).ToList();

            return resultado;


        }

        public LicenciaDTO GetlicenciaId(int id)
        {

            var resultado = (from licencia in _context.GENTEMAR_LICENCIAS
                             join usuario in _context.GENTEMAR_DATOSBASICOS on
                             licencia.id_gentemar equals usuario.id_gentemar
                             where licencia.id_licencia == id
                             select new LicenciaDTO
                             {
                                 Activo = licencia.activo,
                                 Radicado = licencia.radicado,
                                 IdTramite = licencia.id_tramite,
                                 IdLicencia = licencia.id_licencia,
                                 IdGentemar = licencia.id_gentemar,
                                 IdEstadoLicencia = licencia.id_estado_licencia,
                                 IdCargoLicencia = licencia.id_cargo_licencia,
                                 IdCapitaniaFirmante = licencia.id_capitania_firmante,
                                 IdCapitania = licencia.id_capitania,
                                 FechaVencimiento = licencia.fecha_vencimiento,
                                 FechaExpedicion = licencia.fecha_expedicion,
                                 Usuario = new UsuarioGenteMarDTO
                                 {
                                     Id = usuario.id_gentemar,
                                     Nombres = usuario.nombres,
                                     Apellidos = usuario.apellidos,
                                     DocumentoIdentificacion = usuario.documento_identificacion,
                                     FechaNacimiento = usuario.fecha_vencimiento
                                 },


                             }).FirstOrDefault();

            return resultado;
        }

        /// <summary>
        /// Lista la liecncia por id 
        /// <param name="id">Id del la licencia</param>
        /// </summary>
        /// <returns>Lista de Actividades</returns>
        /// <tabla>GENTEMAR_ACTIVIDAD</tabla>
        public LicenciaDTO GetlicenciaIdView(int id)
        {

            var resultado = (from licencia in _context.GENTEMAR_LICENCIAS
                             join cargoLicencia in _context.GENTEMAR_CARGO_LICENCIA on
                             licencia.id_cargo_licencia equals cargoLicencia.id_cargo_licencia
                             join estadoLicencia in _context.GENTEMAR_ESTADO_LICENCIAS on
                             licencia.id_estado_licencia equals estadoLicencia.id_estado_licencias
                             join capitania in _context.APLICACIONES_CAPITANIAS on
                             licencia.id_capitania equals capitania.ID_CAPITANIA
                             join capitaniaFirmante in _context.APLICACIONES_CAPITANIAS on
                             licencia.id_capitania equals capitaniaFirmante.ID_CAPITANIA
                             join usuario in _context.GENTEMAR_DATOSBASICOS on
                             licencia.id_gentemar equals usuario.id_gentemar
                             where licencia.id_licencia == id
                             select new LicenciaDTO
                             {
                                 Activo = licencia.activo,
                                 Radicado = licencia.radicado,
                                 IdTramite = licencia.id_tramite,
                                 IdLicencia = licencia.id_licencia,
                                 IdGentemar = licencia.id_gentemar,
                                 IdEstadoLicencia = licencia.id_estado_licencia,
                                 IdCargoLicencia = licencia.id_cargo_licencia,
                                 IdCapitaniaFirmante = licencia.id_capitania_firmante,
                                 IdCapitania = licencia.id_capitania,
                                 FechaVencimiento = licencia.fecha_vencimiento,
                                 FechaExpedicion = licencia.fecha_expedicion,
                                 Capitania = new CapitaniaDTO
                                 {
                                     Descripcion = capitania.DESCRIPCION,
                                     Id = capitania.ID_CAPITANIA,
                                     Sigla = capitania.SIGLA_CAPITANIA
                                 },
                                 CapitaniaFirmante = new CapitaniaDTO
                                 {
                                     Descripcion = capitaniaFirmante.DESCRIPCION,
                                     Id = capitaniaFirmante.ID_CAPITANIA,
                                     Sigla = capitaniaFirmante.SIGLA_CAPITANIA
                                 },
                                 CargoLicencia = new CargoLicenciaDTO
                                 {
                                     IdCargoLicencia = cargoLicencia.id_cargo_licencia,
                                     CargoLicencia = cargoLicencia.cargo_licencia,
                                     CodigoLicencia = cargoLicencia.codigo_licencia
                                 },
                                 Estado = new EstadoLicenciaDTO
                                 {
                                     IdEstadoLicencias = estadoLicencia.id_estado_licencias,
                                     DescripcionEstado = estadoLicencia.descripcion_estado
                                 },
                                 Usuario = new UsuarioGenteMarDTO
                                 {
                                     Id = usuario.id_gentemar,
                                     Nombres = usuario.nombres,
                                     Apellidos = usuario.apellidos,
                                     DocumentoIdentificacion = usuario.documento_identificacion,
                                     FechaNacimiento = usuario.fecha_vencimiento

                                 },

                             }).FirstOrDefault();

            return resultado;


        }

        public async Task CrearLicencia(GENTEMAR_LICENCIAS datos, GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = null)
        {
            using (_context)
            {
                using (var trassaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        datos.id_estado_licencia = (int)EnumEstados.PROCESO;
                        _context.GENTEMAR_LICENCIAS.Add(datos);
                        await SaveAllAsync();
                        if (datos.Observacion != null)
                        {
                            datos.Observacion.id_licencia = (int)datos.id_licencia;
                            _context.GENTEMAR_OBSERVACIONES_LICENCIAS.Add(datos.Observacion);
                            await SaveAllAsync();
                            if (repositorio != null)
                            {
                                repositorio.IdModulo = datos.Observacion.id_observacion.ToString();
                                repositorio.IdUsuarioCreador = datos.Observacion.usuario_creador_registro;
                                repositorio.FechaHoraCreacion = (DateTime)datos.Observacion.fecha_hora_creacion;
                                _context.GENTEMAR_REPOSITORIO_ARCHIVOS.Add(repositorio);
                                await SaveAllAsync();
                            }
                        }
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

        public async Task ActualizarLicencia(GENTEMAR_LICENCIAS datos, GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = null)
        {
            using (_context)
            {
                using (var trassaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.GENTEMAR_LICENCIAS.Attach(datos);
                        var entry = _context.Entry(datos);
                        entry.State = EntityState.Modified;
                        await SaveAllAsync();
                        if (datos.Observacion != null)
                        {
                            datos.Observacion.id_licencia = (int)datos.id_licencia;
                            _context.GENTEMAR_OBSERVACIONES_LICENCIAS.Add(datos.Observacion);
                            await SaveAllAsync();
                            if (repositorio != null)
                            {
                                repositorio.IdModulo = datos.Observacion.id_observacion.ToString();
                                repositorio.IdUsuarioCreador = datos.Observacion.usuario_creador_registro;
                                repositorio.FechaHoraCreacion = datos.Observacion.fecha_hora_creacion;
                                _context.GENTEMAR_REPOSITORIO_ARCHIVOS.Add(repositorio);
                                await SaveAllAsync();
                            }
                        }
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
    }
}
