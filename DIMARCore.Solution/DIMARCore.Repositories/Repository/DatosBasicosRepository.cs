using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class DatosBasicosRepository : GenericRepository<GENTEMAR_DATOSBASICOS>
    {
        /// <summary>
        /// retorna el listado de los datos basicos de los usuarios de gente de mar por filtro dinamico 
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns>IQueryable<ListadoDatosBasicosDTO></returns>
        public IQueryable<ListadoDatosBasicosDTO> GetDatosBasicosQueryable(DatosBasicosQueryFilter filtro)
        {
            var query = (from datosBasicos in _context.GENTEMAR_DATOSBASICOS
                         join formacionGrado in _context.GENTEMAR_FORMACION_GRADO on datosBasicos.id_formacion_grado equals formacionGrado.id_formacion_grado
                         join tipoDocumento in _context.APLICACIONES_TIPO_DOCUMENTO on datosBasicos.id_tipo_documento equals tipoDocumento.ID_TIPO_DOCUMENTO
                         join genero in _context.APLICACIONES_GENERO on datosBasicos.id_genero equals genero.ID_GENERO
                         join estado in _context.GENTEMAR_ESTADO on datosBasicos.id_estado equals estado.id_estado
                         join municipioExpedicion in _context.APLICACIONES_MUNICIPIO on datosBasicos.id_municipio_expedicion equals municipioExpedicion.ID_MUNICIPIO
                          into ma
                         from subMunicipioExpedicion in ma.DefaultIfEmpty()
                         join municipioNacimento in _context.APLICACIONES_MUNICIPIO on datosBasicos.id_municipio_nacimiento equals municipioNacimento.ID_MUNICIPIO
                         join municipioRecidencia in _context.APLICACIONES_MUNICIPIO on datosBasicos.id_municipio_residencia equals municipioRecidencia.ID_MUNICIPIO
                         join pais in _context.TABLA_NAV_BAND on datosBasicos.cod_pais equals pais.cod_pais
                         select new
                         {
                             datosBasicos,
                             formacionGrado,
                             tipoDocumento,
                             genero,
                             estado,
                             subMunicipioExpedicion,
                             municipioNacimento,
                             municipioRecidencia,
                             pais
                         });

            if (!string.IsNullOrWhiteSpace(filtro.IdentificacionConPuntos))
            {
                query = query.Where(x => x.datosBasicos.documento_identificacion.Equals(filtro.IdentificacionConPuntos));
            }
            if (!string.IsNullOrWhiteSpace(filtro.apellidos))
            {
                query = query.Where(x => x.datosBasicos.apellidos.Contains(filtro.apellidos));
            }
            if (!string.IsNullOrWhiteSpace(filtro.nombres))
            {
                query = query.Where(x => x.datosBasicos.nombres.Contains(filtro.nombres));
            }
            if (filtro.id_estado != null)
            {
                query = query.Where(x => x.datosBasicos.id_estado == filtro.id_estado);
            }
            var listado = query.OrderBy(x => x.datosBasicos.documento_identificacion).Select(m => new ListadoDatosBasicosDTO
            {
                Apellidos = m.datosBasicos.apellidos,
                CorreoElectronico = m.datosBasicos.correo_electronico,
                Direccion = m.datosBasicos.direccion,
                DocumentoIdentificacion = m.datosBasicos.documento_identificacion,
                Estado = m.estado.descripcion,
                IdEstado = m.estado.id_estado,
                FechaExpedicion = m.datosBasicos.fecha_expedicion,
                FechaNacimiento = m.datosBasicos.fecha_nacimiento,
                FechaVencimiento = m.datosBasicos.fecha_vencimiento,
                FormacionGrado = m.formacionGrado.GENTEMAR_FORMACION.formacion,
                Genero = m.genero.DESCRIPCION,
                IdGentemar = m.datosBasicos.id_gentemar,
                MunicipioNacimiento = m.municipioNacimento.NOMBRE_MUNICIPIO,
                MunicipioResidencia = m.municipioRecidencia.NOMBRE_MUNICIPIO,
                Nombres = m.datosBasicos.nombres,
                NumeroMovil = m.datosBasicos.numero_movil,
                Pais = m.pais.des_pais,
                Telefono = m.datosBasicos.telefono,
                TipoDocumento = m.tipoDocumento.DESCRIPCION
            });
            return listado;
        }


        /// <summary>
        /// Metodo para crear los datos basicos junto con la asignacion de las imagenes
        /// </summary>
        /// <param name="entidad"></param>
        /// <param name="fotografia"></param>
        /// <returns></returns>
        public async Task CrearDatosBasicos(GENTEMAR_DATOSBASICOS entidad, GENTEMAR_REPOSITORIO_ARCHIVOS archivo)
        {
            using (_context)
            {
                using (var trassaction = _context.Database.BeginTransaction())
                {
                    //await Create(entidad);
                    try
                    {
                        _context.GENTEMAR_DATOSBASICOS.Add(entidad);
                        await SaveAllAsync();
                        archivo.IdModulo = entidad.id_gentemar.ToString();
                        archivo.IdUsuarioCreador = entidad.usuario_creador_registro;
                        archivo.FechaCargue = (DateTime)entidad.fecha_hora_creacion;
                        archivo.FechaHoraUltimaActualizacion = (DateTime)entidad.fecha_hora_creacion;
                        _context.GENTEMAR_REPOSITORIO_ARCHIVOS.Add(archivo);
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
        /// <summary>
        /// Metodo para actualizar los datos basicos y la foto de los usuarios de gente de mar  
        /// </summary>
        /// <param name="entidad"></param>
        /// <param name="fotografia"></param>
        /// <returns></returns>
        public async Task ActualizarDatosBasicos(GENTEMAR_DATOSBASICOS entidadActual, GENTEMAR_DATOSBASICOS entidad, GENTEMAR_REPOSITORIO_ARCHIVOS archivo)
        {
            using (_context)
            {
                using (var trassaction = _context.Database.BeginTransaction())
                    try
                    {
                        if (entidadActual.id_tipo_documento != entidad.id_tipo_documento || !entidadActual.documento_identificacion.Equals(entidad.documento_identificacion))
                        {
                            var historial = new GENTEMAR_HISTORIAL_DOCUMENTO()
                            {
                                documento_identificacion = entidadActual.documento_identificacion,
                                id_gentemar = entidadActual.id_gentemar,
                                id_tipo_documento = entidadActual.id_tipo_documento
                            };
                            _context.GENTEMAR_HISTORIAL_DOCUMENTO.Add(historial);
                            await SaveAllAsync();
                        }

                        entidad.usuario_creador_registro = entidadActual.usuario_creador_registro;
                        _context.Entry(entidadActual).CurrentValues.SetValues(entidad);
                        await SaveAllAsync();

                        if (archivo != null)
                        {
                            archivo.IdModulo = entidadActual.id_gentemar.ToString();
                            archivo.IdUsuarioUltimaActualizacion = entidadActual.usuario_actualiza_registro;
                            archivo.IdUsuarioCreador = entidadActual.usuario_actualiza_registro;
                            archivo.FechaCargue = (DateTime)entidadActual.fecha_hora_actualizacion;
                            archivo.FechaHoraUltimaActualizacion = (DateTime)entidadActual.fecha_hora_actualizacion;
                            _context.GENTEMAR_REPOSITORIO_ARCHIVOS.Add(archivo);
                            await SaveAllAsync();
                        }

                        trassaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        trassaction.Rollback();
                        ObtenerException(ex, entidad);
                    }
            }
        }


        public async Task ActualizarDatosBasicosSinFoto(GENTEMAR_DATOSBASICOS entidad)//GENTEMAR_TITULOS titulos
        {
            try
            {
                _context.GENTEMAR_DATOSBASICOS.Attach(entidad);
                var entry = _context.Entry(entidad);
                entry.State = EntityState.Modified;
                await SaveAllAsync();
            }
            catch (Exception ex)
            {
                ObtenerException(ex, entidad);
            }
        }


        /// <summary>
        /// Obtiene los datos basicos de un usuario filtrado por numero de identificacion 
        /// </summary>
        /// <param name="identificacionConPuntos"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UsuarioGenteMarDTO> GetPersonaByIdentificacionOrId(string identificacionConPuntos, long id = 0)
        {
            UsuarioGenteMarDTO usuario = new UsuarioGenteMarDTO();
            if (id == 0)
            {
                usuario = await (from GenteMar in _context.GENTEMAR_DATOSBASICOS
                                 join tipoDocumento in _context.APLICACIONES_TIPO_DOCUMENTO on GenteMar.id_tipo_documento equals tipoDocumento.ID_TIPO_DOCUMENTO
                                 join estado in _context.GENTEMAR_ESTADO on GenteMar.id_estado equals estado.id_estado
                                 where GenteMar.documento_identificacion.Equals(identificacionConPuntos)
                                 select new
                                 {
                                     GenteMar,
                                     tipoDocumento,
                                     estado
                                 }).Select(x => new UsuarioGenteMarDTO()
                                 {
                                     NombreEstado = x.estado.descripcion,
                                     Estado = x.estado.id_estado,
                                     Nombres = x.GenteMar.nombres,
                                     Apellidos = x.GenteMar.apellidos,
                                     DocumentoIdentificacion = x.GenteMar.documento_identificacion,
                                     Id = x.GenteMar.id_gentemar,
                                     IdTipoDocumento = x.tipoDocumento.ID_TIPO_DOCUMENTO,
                                     FechaNacimiento = x.GenteMar.fecha_nacimiento,
                                     FechaVencimiento = x.GenteMar.fecha_vencimiento
                                 }).FirstOrDefaultAsync();
            }
            else
            {

                usuario = await (from GenteMar in _context.GENTEMAR_DATOSBASICOS
                                 join tipoDocumento in _context.APLICACIONES_TIPO_DOCUMENTO on GenteMar.id_tipo_documento
                                 equals tipoDocumento.ID_TIPO_DOCUMENTO
                                 join estado in _context.GENTEMAR_ESTADO on GenteMar.id_estado equals estado.id_estado
                                 where GenteMar.id_gentemar == id
                                 select new
                                 {
                                     GenteMar,
                                     tipoDocumento,
                                     estado

                                 }).Select(x => new UsuarioGenteMarDTO()
                                 {
                                     NombreEstado = x.estado.descripcion,
                                     Estado = x.estado.id_estado,
                                     Nombres = x.GenteMar.nombres,
                                     Apellidos = x.GenteMar.apellidos,
                                     DocumentoIdentificacion = x.GenteMar.documento_identificacion,
                                     Id = x.GenteMar.id_gentemar,
                                     IdTipoDocumento = x.tipoDocumento.ID_TIPO_DOCUMENTO,
                                     FechaNacimiento = x.GenteMar.fecha_nacimiento,
                                     FechaVencimiento = x.GenteMar.fecha_vencimiento
                                 }).FirstOrDefaultAsync();
            }
            return usuario;
        }


        /// <summary>
        /// Metodo para obtener datos basicos filtados por id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DatosBasicosDTO GetDatosBasicosId(long id)
        {
            var temp = (from datosBasicos in _context.GENTEMAR_DATOSBASICOS
                        join formacionGrado in _context.GENTEMAR_FORMACION_GRADO on datosBasicos.id_formacion_grado equals formacionGrado.id_formacion_grado
                        where datosBasicos.id_gentemar == id
                        select new
                        {
                            datosBasicos,
                            formacionGrado,

                        });
            var query = (from subDatosBasicos in temp
                         join archivo in _context.GENTEMAR_REPOSITORIO_ARCHIVOS on subDatosBasicos.datosBasicos.id_gentemar.ToString() equals archivo.IdModulo
                         into fo
                         from subFoto in fo.DefaultIfEmpty()
                         select new
                         {
                             subDatosBasicos,
                             subFoto,

                         }).Select(x => new DatosBasicosDTO
                         {
                             Apellidos = x.subDatosBasicos.datosBasicos.apellidos,
                             CodPais = x.subDatosBasicos.datosBasicos.cod_pais,
                             CorreoElectronico = x.subDatosBasicos.datosBasicos.correo_electronico,
                             Direccion = x.subDatosBasicos.datosBasicos.direccion,
                             DocumentoIdentificacion = x.subDatosBasicos.datosBasicos.documento_identificacion,
                             FechaExpedicion = x.subDatosBasicos.datosBasicos.fecha_expedicion,
                             FechaNacimiento = x.subDatosBasicos.datosBasicos.fecha_nacimiento,
                             FechaVencimiento = x.subDatosBasicos.datosBasicos.fecha_vencimiento,
                             Formacion = x.subDatosBasicos.formacionGrado.id_formacion,
                             UrlArchivo = x.subFoto.RutaArchivo,
                             IdEstado = x.subDatosBasicos.datosBasicos.id_estado,
                             IdFormacionGrado = x.subDatosBasicos.datosBasicos.id_formacion_grado,
                             IdGenero = x.subDatosBasicos.datosBasicos.id_genero,
                             IdGentemar = x.subDatosBasicos.datosBasicos.id_gentemar,
                             IdMunicipioExpedicion = x.subDatosBasicos.datosBasicos.id_municipio_expedicion,
                             IdMunicipioNacimiento = x.subDatosBasicos.datosBasicos.id_municipio_nacimiento,
                             IdMunicipioResidencia = x.subDatosBasicos.datosBasicos.id_municipio_residencia,
                             IdTipoDocumento = x.subDatosBasicos.datosBasicos.id_tipo_documento,
                             Nombres = x.subDatosBasicos.datosBasicos.nombres,
                             NumeroMovil = x.subDatosBasicos.datosBasicos.numero_movil,
                             Telefono = x.subDatosBasicos.datosBasicos.telefono,
                             HistorialDocumento = (from hd in _context.GENTEMAR_HISTORIAL_DOCUMENTO
                                                   join td in _context.APLICACIONES_TIPO_DOCUMENTO on hd.id_tipo_documento equals td.ID_TIPO_DOCUMENTO
                                                   where hd.id_gentemar == id
                                                   select new HistorialDocumentoDTO
                                                   {
                                                       DocumentoIdentificacion = hd.documento_identificacion,
                                                       IdHistorialDocumento = hd.id_historial_documento,
                                                       IdTipoDocumento = hd.id_tipo_documento,
                                                       NombreTipoDocumento = td.DESCRIPCION,
                                                       FechaCambio = hd.fecha_cambio
                                                   }).ToList()
        }).FirstOrDefault();

            return query;
        }
        public async Task<bool> ExisteById(long id)
        {
            return await AnyWithCondition(x => x.id_gentemar == id);
        }
        public async Task<bool> ExistePersonaByIdentificacion(string identificacionConPuntos)
        {
            return await AnyWithCondition(x => x.documento_identificacion.Equals(identificacionConPuntos));
        }


        /// <summary>
        /// Lista de licencias por id 
        /// <param name="id">Id del Actividad</param>
        /// </summary>
        /// <returns>Lista de Actividades</returns>
        /// <tabla>GENTEMAR_ACTIVIDAD</tabla>
        public LicenciasTitulosDTO GetlicenciaTituloDocumentoUsuario(string documento)
        {

            var licenciaData = (from licencia in _context.GENTEMAR_LICENCIAS
                                join cargoLicencia in _context.GENTEMAR_CARGO_LICENCIA on
                                licencia.id_cargo_licencia equals cargoLicencia.id_cargo_licencia
                                join estadoLicencia in _context.GENTEMAR_ESTADO_LICENCIAS on
                                licencia.id_estado_licencia equals estadoLicencia.id_estado_licencias
                                join usuario in _context.GENTEMAR_DATOSBASICOS on
                                licencia.id_gentemar equals usuario.id_gentemar
                                where usuario.documento_identificacion == documento && licencia.fecha_vencimiento >= DateTime.Now
                                select new LicenciaListarDTO
                                {
                                    CargoLicencia = cargoLicencia.cargo_licencia,
                                    DocumentoIdentificacion = usuario.documento_identificacion,
                                    EstadoTramite = estadoLicencia.descripcion_estado,
                                    FechaExpedicion = licencia.fecha_expedicion,
                                    FechaVencimiento = licencia.fecha_vencimiento,
                                    Id = licencia.id_licencia,
                                    NombreUsuario = usuario.nombres + " " + usuario.apellidos,

                                }).ToList();


            var DataTitulo = (from titulo in _context.GENTEMAR_TITULOS
                              join usuario in _context.GENTEMAR_DATOSBASICOS on titulo.id_gentemar equals usuario.id_gentemar
                              join cargoRegla in _context.GENTEMAR_REGLAS_CARGO on titulo.id_cargo_regla equals cargoRegla.id_cargo_regla
                              join reglas in _context.GENTEMAR_REGLAS on cargoRegla.id_regla equals reglas.id_regla
                              join cargoTitulo in _context.GENTEMAR_CARGO_TITULO on cargoRegla.id_cargo_titulo equals cargoTitulo.id_cargo_titulo
                              join tramite in _context.APLICACIONES_ESTADO_TRAMITE on titulo.id_estado_tramite equals tramite.id_estado_tramite
                              where usuario.documento_identificacion == documento && titulo.fecha_vencimiento >= DateTime.Now
                              select new ListadoTituloDTO
                              {
                                  FechaExpedicion = titulo.fecha_expedicion,
                                  NombreUsuario = usuario.nombres + " " + usuario.apellidos,
                                  DocumentoIdentificacion = usuario.documento_identificacion,
                                  CargoTitulo = cargoTitulo.cargo,
                                  Regla = reglas.Regla,
                                  EstadoTramite = tramite.descripcion_tramite,
                                  FechaVencimiento = titulo.fecha_vencimiento,
                                  Id = titulo.id_titulo,
                              }).ToList();


            var data = new LicenciasTitulosDTO()
            {
                Licencias = licenciaData,
                Titulos = DataTitulo

            };

            return data;


        }
    }

}

