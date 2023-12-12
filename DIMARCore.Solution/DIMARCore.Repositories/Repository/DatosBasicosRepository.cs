using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.Utilities.Config;
using DIMARCore.Utilities.Enums;
using GenteMarCore.Entities.Models;
using LinqKit;
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
            var predicate = PredicateBuilder.True<ListadoDatosBasicosDTO>();

            if (!string.IsNullOrWhiteSpace(filtro.DocumentoIdentificacion))
            {
                predicate = predicate.And(x => x.DocumentoIdentificacion.Equals(filtro.DocumentoIdentificacion));
            }
            if (!string.IsNullOrWhiteSpace(filtro.apellidos))
            {
                predicate = predicate.And(x => x.Apellidos.Contains(filtro.apellidos));
            }
            if (!string.IsNullOrWhiteSpace(filtro.nombres))
            {
                predicate = predicate.And(x => x.Nombres.Contains(filtro.nombres));
            }
            if (filtro.id_estado != null)
            {
                predicate = predicate.And(x => x.IdEstado == filtro.id_estado);
            }
            var query = (from datosBasicos in _context.GENTEMAR_DATOSBASICOS
                         join formacionGrado in _context.GENTEMAR_FORMACION_GRADO on datosBasicos.id_formacion_grado equals formacionGrado.id_formacion_grado
                         join tipoDocumento in _context.APLICACIONES_TIPO_DOCUMENTO on datosBasicos.id_tipo_documento equals tipoDocumento.ID_TIPO_DOCUMENTO
                         join genero in _context.APLICACIONES_GENERO on datosBasicos.id_genero equals genero.ID_GENERO
                         join estado in _context.GENTEMAR_ESTADO on datosBasicos.id_estado equals estado.id_estado
                         join pais in _context.TABLA_NAV_BAND on datosBasicos.cod_pais equals pais.cod_pais
                         select new ListadoDatosBasicosDTO
                         {
                             Apellidos = datosBasicos.apellidos,
                             CorreoElectronico = datosBasicos.correo_electronico,
                             Direccion = datosBasicos.direccion,
                             DocumentoIdentificacion = datosBasicos.documento_identificacion,
                             Estado = estado.descripcion,
                             IdEstado = estado.id_estado,
                             FechaExpedicion = datosBasicos.fecha_expedicion,
                             FechaNacimiento = datosBasicos.fecha_nacimiento,
                             FechaVencimiento = datosBasicos.fecha_vencimiento,
                             FormacionGrado = formacionGrado.GENTEMAR_FORMACION.formacion,
                             Genero = genero.DESCRIPCION,
                             IdGentemar = datosBasicos.id_gentemar,
                             Nombres = datosBasicos.nombres,
                             NumeroMovil = datosBasicos.numero_movil,
                             Pais = pais.des_pais,
                             Telefono = datosBasicos.telefono,
                             TipoDocumento = tipoDocumento.DESCRIPCION,
                             FechaRegistro = datosBasicos.fecha_hora_creacion,
                         }).Where(predicate).AsNoTracking();
            return query;
        }


        /// <summary>
        /// Metodo para crear los datos basicos junto con la asignacion de las imagenes
        /// </summary>
        /// <param name="entidad"></param>
        /// <param name="fotografia"></param>
        /// <returns></returns>
        public async Task CrearDatosBasicos(GENTEMAR_DATOSBASICOS entidad, GENTEMAR_REPOSITORIO_ARCHIVOS repositorio)
        {

            using (var transaction = _context.Database.BeginTransaction())
            {
                //await Create(entidad);
                try
                {
                    _context.GENTEMAR_DATOSBASICOS.Add(entidad);
                    await SaveAllAsync();
                    repositorio.IdModulo = entidad.id_gentemar.ToString();
                    repositorio.IdUsuarioCreador = ClaimsHelper.GetLoginName();
                    repositorio.FechaHoraCreacion = DateTime.Now;
                    _context.GENTEMAR_REPOSITORIO_ARCHIVOS.Add(repositorio);
                    await SaveAllAsync();
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ObtenerException(ex, entidad);
                }
            }
        }
        /// <summary>
        /// Metodo para actualizar los datos basicos y la foto de los usuarios de gente de mar  
        /// </summary>
        /// <param name="entidad"></param>
        /// <param name="fotografia"></param>
        /// <returns></returns>
        public async Task ActualizarDatosBasicos(GENTEMAR_DATOSBASICOS entidadActual, GENTEMAR_DATOSBASICOS entidad, GENTEMAR_REPOSITORIO_ARCHIVOS repositorio)
        {
            using (var transaction = _context.Database.BeginTransaction())
                try
                {
                    if (entidadActual.id_tipo_documento != entidad.id_tipo_documento
                        || !entidadActual.documento_identificacion.Equals(entidad.documento_identificacion))
                    {
                        var historial = new GENTEMAR_HISTORIAL_DOCUMENTO()
                        {
                            documento_identificacion = entidadActual.documento_identificacion,
                            id_gentemar = entidadActual.id_gentemar,
                            id_tipo_documento = entidadActual.id_tipo_documento,
                            fecha_cambio = DateTime.Now
                        };
                        _context.GENTEMAR_HISTORIAL_DOCUMENTO.Add(historial);
                        await SaveAllAsync();
                    }

                    entidad.usuario_creador_registro = entidadActual.usuario_creador_registro;
                    _context.Entry(entidadActual).CurrentValues.SetValues(entidad);
                    await SaveAllAsync();

                    if (repositorio != null)
                    {
                        repositorio.IdModulo = entidadActual.id_gentemar.ToString();
                        repositorio.IdUsuarioCreador = ClaimsHelper.GetLoginName();
                        repositorio.FechaHoraCreacion = DateTime.Now;
                        _context.GENTEMAR_REPOSITORIO_ARCHIVOS.Add(repositorio);
                        await SaveAllAsync();
                    }

                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ObtenerException(ex, entidad);
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
        /// Obtiene los datos basicos de un usuario filtrado por numero de identificacion y verifica si contiene estupefaciente la persona
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
                                     FechaVencimiento = x.GenteMar.fecha_vencimiento,
                                     ContieneEstupefacienteVigente = (from datosBasicosEstupefaciente in _context.GENTEMAR_ANTECEDENTES_DATOSBASICOS
                                                                      join estupefaciente in _context.GENTEMAR_ANTECEDENTES on datosBasicosEstupefaciente.id_gentemar_antecedente
                                                                      equals estupefaciente.id_gentemar_antecedente
                                                                      where datosBasicosEstupefaciente.identificacion == x.GenteMar.documento_identificacion
                                                                      && estupefaciente.id_estado_antecedente != (int)EstadoEstupefacienteEnum.Exitosa
                                                                      && estupefaciente.fecha_vigencia.HasValue ? estupefaciente.fecha_vigencia.Value >= DateTime.Now : false
                                                                      select estupefaciente).Any(vcite => vcite.id_antecedente > 0)
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
                                     FechaVencimiento = x.GenteMar.fecha_vencimiento,
                                     ContieneEstupefacienteVigente = (from datosBasicosEstupefaciente in _context.GENTEMAR_ANTECEDENTES_DATOSBASICOS
                                                                      join estupefaciente in _context.GENTEMAR_ANTECEDENTES on datosBasicosEstupefaciente.id_gentemar_antecedente
                                                                      equals estupefaciente.id_gentemar_antecedente
                                                                      where datosBasicosEstupefaciente.identificacion == x.GenteMar.documento_identificacion
                                                                      && estupefaciente.id_estado_antecedente != (int)EstadoEstupefacienteEnum.Exitosa
                                                                      && estupefaciente.fecha_vigencia.HasValue ? estupefaciente.fecha_vigencia.Value >= DateTime.Now : false
                                                                      select estupefaciente).Any(vcite => vcite.id_antecedente > 0)
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
            var query = (from datosBasicos in _context.GENTEMAR_DATOSBASICOS
                         join formacionGrado in _context.GENTEMAR_FORMACION_GRADO on datosBasicos.id_formacion_grado equals formacionGrado.id_formacion_grado
                         where datosBasicos.id_gentemar == id
                         select new DatosBasicosDTO
                         {
                             Apellidos = datosBasicos.apellidos,
                             CodPais = datosBasicos.cod_pais,
                             CorreoElectronico = datosBasicos.correo_electronico,
                             Direccion = datosBasicos.direccion,
                             DocumentoIdentificacion = datosBasicos.documento_identificacion,
                             FechaExpedicion = datosBasicos.fecha_expedicion,
                             FechaNacimiento = datosBasicos.fecha_nacimiento,
                             FechaVencimiento = datosBasicos.fecha_vencimiento,
                             Formacion = formacionGrado.id_formacion,
                             UrlArchivo = (from archivo in _context.GENTEMAR_REPOSITORIO_ARCHIVOS
                                           where archivo.IdModulo == datosBasicos.id_gentemar.ToString()
                                           select new
                                           {
                                               archivo
                                           }).OrderByDescending(x => x.archivo.FechaHoraCreacion).Select(x => x.archivo.RutaArchivo).FirstOrDefault(),
                             IdEstado = datosBasicos.id_estado,
                             IdFormacionGrado = datosBasicos.id_formacion_grado,
                             IdGenero = datosBasicos.id_genero,
                             IdGentemar = datosBasicos.id_gentemar,
                             IdMunicipioExpedicion = datosBasicos.id_municipio_expedicion,
                             IdPaisResidencia = datosBasicos.id_pais_residencia,
                             IdPaisNacimiento = datosBasicos.id_pais_nacimiento,
                             IdMunicipioResidencia = datosBasicos.id_municipio_residencia,
                             IdTipoDocumento = datosBasicos.id_tipo_documento,
                             Nombres = datosBasicos.nombres,
                             NumeroMovil = datosBasicos.numero_movil,
                             Telefono = datosBasicos.telefono,
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
        public async Task<LicenciasTitulosDTO> GetlicenciaTituloVigentesPorDocumentoUsuario(string documento)
        {
            var fechaActual = DateTime.Now;

            var licenciaData = (from licencia in _context.GENTEMAR_LICENCIAS
                                join cargoLicencia in _context.GENTEMAR_CARGO_LICENCIA on
                                licencia.id_cargo_licencia equals cargoLicencia.id_cargo_licencia
                                join estadoLicencia in _context.GENTEMAR_ESTADO_LICENCIAS on
                                licencia.id_estado_licencia equals estadoLicencia.id_estado_licencias
                                join usuario in _context.GENTEMAR_DATOSBASICOS on
                                licencia.id_gentemar equals usuario.id_gentemar
                                where usuario.documento_identificacion.Equals(documento) && licencia.fecha_vencimiento >= fechaActual
                                select new LicenciaListarDTO
                                {
                                    CargoLicencia = cargoLicencia.cargo_licencia,
                                    DocumentoIdentificacion = usuario.documento_identificacion,
                                    EstadoTramite = estadoLicencia.descripcion_estado,
                                    FechaExpedicion = licencia.fecha_expedicion,
                                    FechaVencimiento = licencia.fecha_vencimiento,
                                    NombreUsuario = usuario.nombres + " " + usuario.apellidos,
                                    Radicado = licencia.radicado != 0 ? licencia.radicado.ToString() : "No contiene radicado."
                                }).ToListAsync();


            var DataTitulo = (from titulo in _context.GENTEMAR_TITULOS
                              join usuario in _context.GENTEMAR_DATOSBASICOS on titulo.id_gentemar equals usuario.id_gentemar
                              join tramite in _context.GENTEMAR_ESTADO_TITULO on titulo.id_estado_tramite equals tramite.id_estado_tramite
                              where usuario.documento_identificacion.Equals(documento) && titulo.fecha_vencimiento >= fechaActual
                              select new TituloListarDTO
                              {
                                  FechaExpedicion = titulo.fecha_expedicion,
                                  NombreUsuario = usuario.nombres + " " + usuario.apellidos,
                                  DocumentoIdentificacion = usuario.documento_identificacion,
                                  EstadoTramite = tramite.descripcion_tramite,
                                  FechaVencimiento = titulo.fecha_vencimiento,
                                  Radicado = titulo.radicado,
                                  Cargos = (from cargoTitulo in _context.GENTEMAR_TITULO_REGLA_CARGOS
                                            join reglaCargo in _context.GENTEMAR_REGLAS_CARGO on cargoTitulo.id_cargo_regla equals reglaCargo.id_cargo_regla
                                            join cargo in _context.GENTEMAR_CARGO_TITULO on reglaCargo.id_cargo_titulo equals cargo.id_cargo_titulo
                                            join nivel in _context.GENTEMAR_NIVEL on reglaCargo.id_nivel equals nivel.id_nivel
                                            join regla in _context.GENTEMAR_REGLAS on reglaCargo.id_regla equals regla.id_regla
                                            where cargoTitulo.id_titulo == titulo.id_titulo

                                            select new InfoCargosDTO
                                            {
                                                CargoTitulo = cargo.cargo,
                                                Nivel = nivel.nivel,
                                                Regla = regla.nombre_regla
                                            }).ToList()
                              }).ToListAsync();


            var data = new LicenciasTitulosDTO()
            {
                Licencias = await licenciaData,
                Titulos = await DataTitulo
            };

            return data;


        }

        public async Task CambiarEstadoTitulosLicenciasByEstadoPersona(IEnumerable<GENTEMAR_LICENCIAS> licencias, IEnumerable<GENTEMAR_TITULOS> titulos)
        {
            if (licencias.Any())
            {
                foreach (var licencia in licencias)
                {
                    _context.Entry(licencia).State = EntityState.Modified;
                }
            }
            if (titulos.Any())
            {
                foreach (var titulo in titulos)
                {
                    _context.Entry(titulo).State = EntityState.Modified;
                }
            }
            await SaveAllAsync();
        }
    }

}

