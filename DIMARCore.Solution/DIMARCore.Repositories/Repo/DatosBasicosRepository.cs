using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repo
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
                         join pais in _context.t_nav_band on datosBasicos.cod_pais equals pais.cod_pais


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
            var listado = query.OrderBy(x => x.datosBasicos.apellidos).Select(m => new ListadoDatosBasicosDTO
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
                //MunicipioExpedicion = m.subMunicipioExpedicion == null ? string.Empty : m.subMunicipioExpedicion.NOMBRE_MUNICIPIO,
                MunicipioNacimiento = m.municipioNacimento.NOMBRE_MUNICIPIO,
                MunicipioResidencia = m.municipioRecidencia.NOMBRE_MUNICIPIO,
                Nombres = m.datosBasicos.nombres,
                NumeroMovil = m.datosBasicos.numero_movil,
                Pais = m.pais.des_pais,
                Telefono = m.datosBasicos.telefono,
                TipoDocumento = m.tipoDocumento.DESCRIPCION,
                Activo = m.datosBasicos.activo
            });
            return listado;
        }


        /// <summary>
        /// Metodo para crear los datos basicos junto con la asignacion de las imagenes
        /// </summary>
        /// <param name="entidad"></param>
        /// <param name="fotografia"></param>
        /// <returns></returns>
        public async Task CrearDatosBasicos(GENTEMAR_DATOSBASICOS entidad, REPOSITORIO_ARCHIVOS archivo)
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
        /// Metodo para actualizar los datos basicos y la foto de los usaurios de gente de mar  
        /// </summary>
        /// <param name="entidad"></param>
        /// <param name="fotografia"></param>
        /// <returns></returns>
        public async Task ActualizarDatosBasicos(GENTEMAR_DATOSBASICOS entidad, REPOSITORIO_ARCHIVOS archivo)
        {
            using (_context)
            {
                using (var trassaction = _context.Database.BeginTransaction())

                    //await Create(entidad);
                    try
                    {
                        _context.GENTEMAR_DATOSBASICOS.Attach(entidad);
                        var entry = _context.Entry(entidad);
                        entry.State = EntityState.Modified;
                        await SaveAllAsync();

                        if (archivo != null)
                        {
                            archivo.IdModulo = entidad.id_gentemar.ToString();
                            archivo.IdUsuarioUltimaActualizacion = entidad.usuario_actualiza_registro;
                            archivo.IdUsuarioCreador = entidad.usuario_actualiza_registro;
                            archivo.FechaCargue = (DateTime)entidad.fecha_hora_actualizacion;
                            archivo.FechaHoraUltimaActualizacion = (DateTime)entidad.fecha_hora_actualizacion;
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
        public async Task<UsuarioDTO> GetPersonaByIdentificacionOrId(string identificacionConPuntos, int id = 0)
        {
            UsuarioDTO usuario = new UsuarioDTO();
            if (id == 0)
            {
                var existeIdentificacion = await ExistePersonaByIdentificacion(identificacionConPuntos);
                if (existeIdentificacion)
                {
                    usuario = await _context.GENTEMAR_DATOSBASICOS.Where(x => x.documento_identificacion.Equals(identificacionConPuntos))
                        .Select(x => new UsuarioDTO()
                        {
                            Nombre = x.nombres,
                            DocumentoIdentificacion = x.documento_identificacion,
                            Id = x.id_gentemar,
                            Apellido = x.apellidos
                        }).FirstOrDefaultAsync();
                }
                else
                {
                    usuario = null;
                }
            }
            else
            {
                var existeId = await ExisteById(id);
                if (existeId)
                {
                    usuario = await _context.GENTEMAR_DATOSBASICOS.Where(x => x.id_gentemar == id)
                        .Select(x => new UsuarioDTO()
                        {
                            Nombre = x.nombres,
                            DocumentoIdentificacion = x.documento_identificacion,
                            Id = x.id_gentemar,
                            Apellido = x.apellidos
                        }).FirstOrDefaultAsync();
                }
                else
                {
                    usuario = null;
                }
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
            //query.Where(x => x.subFoto.TipoDocumento == Constantes.CARPETA_IMAGENES);
            var query = (from datosBasicos in _context.GENTEMAR_DATOSBASICOS
                         join archivo in _context.GENTEMAR_REPOSITORIO_ARCHIVOS on datosBasicos.id_gentemar.ToString() equals archivo.IdModulo
                         into fo
                         from subFoto in fo.DefaultIfEmpty()
                         join formacionGrado in _context.GENTEMAR_FORMACION_GRADO on datosBasicos.id_formacion_grado equals formacionGrado.id_formacion_grado
                         where datosBasicos.id_gentemar == id && subFoto != null ? subFoto.TipoDocumento == Constantes.CARPETA_IMAGENES : subFoto.TipoDocumento == null
                         select new
                         {
                             datosBasicos,
                             subFoto,
                             formacionGrado,

                         }).OrderByDescending(x => x.subFoto.FechaCargue).Select(x => new DatosBasicosDTO
                         {
                             Apellidos = x.datosBasicos.apellidos,
                             CarpetaFisico = x.datosBasicos.carpeta_fisico,
                             CodPais = x.datosBasicos.cod_pais,
                             CorreoElectronico = x.datosBasicos.correo_electronico,
                             Direccion = x.datosBasicos.direccion,
                             DocumentoIdentificacion = x.datosBasicos.documento_identificacion,
                             FechaExpedicion = x.datosBasicos.fecha_expedicion,
                             FechaNacimiento = x.datosBasicos.fecha_nacimiento,
                             FechaVencimiento = x.datosBasicos.fecha_vencimiento,
                             Formacion = x.formacionGrado.id_formacion,
                             UrlArchivo = x.subFoto.RutaArchivo,
                             IdEstado = x.datosBasicos.id_estado,
                             IdFormacionGrado = x.datosBasicos.id_formacion_grado,
                             IdGenero = x.datosBasicos.id_genero,
                             IdGentemar = x.datosBasicos.id_gentemar,
                             IdMunicipioExpedicion = x.datosBasicos.id_municipio_expedicion,
                             IdMunicipioNacimiento = x.datosBasicos.id_municipio_nacimiento,
                             IdMunicipioResidencia = x.datosBasicos.id_municipio_residencia,
                             IdTipoDocumento = x.datosBasicos.id_tipo_documento,
                             Nombres = x.datosBasicos.nombres,
                             NumeroMovil = x.datosBasicos.numero_movil,
                             Telefono = x.datosBasicos.telefono,
                             Activo = x.datosBasicos.activo,
                             Antecedente = x.datosBasicos.antecedente
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
    }

}

