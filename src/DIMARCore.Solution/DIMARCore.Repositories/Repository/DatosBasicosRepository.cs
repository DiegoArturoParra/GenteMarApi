using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities;
using GenteMarCore.Entities.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class DatosBasicosRepository : GenericRepository<GENTEMAR_DATOSBASICOS>
    {

        public DatosBasicosRepository()
        {

        }
        public DatosBasicosRepository(GenteDeMarCoreContext context) : base(context) // Llama al constructor de la clase base
        {
        }
        /// <summary>
        /// retorna el listado de los datos basicos de los usuarios de gente de mar por filtro dinamico 
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns>IQueryable<ListadoDatosBasicosDTO></returns>
        public IQueryable<ListarDatosBasicosDTO> GetDatosBasicosQueryable(DatosBasicosQueryFilter filtro)
        {
            var query = from datosBasicos in _context.GENTEMAR_DATOSBASICOS
                        join formacionGrado in _context.GENTEMAR_FORMACION_GRADO on datosBasicos.id_formacion_grado equals formacionGrado.id_formacion_grado
                        join tipoDocumento in _context.APLICACIONES_TIPO_DOCUMENTO on datosBasicos.id_tipo_documento equals tipoDocumento.ID_TIPO_DOCUMENTO
                        join genero in _context.APLICACIONES_GENERO on datosBasicos.id_genero equals genero.ID_GENERO
                        join estado in _context.GENTEMAR_ESTADO on datosBasicos.id_estado equals estado.id_estado
                        join pais in _context.TABLA_NAV_BAND on datosBasicos.cod_pais equals pais.cod_pais
                        select new ListarDatosBasicosDTO
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
                            FechaRegistro = datosBasicos.FechaCreacion,
                        };

            if (!string.IsNullOrWhiteSpace(filtro.DocumentoIdentificacion))
            {
                query = query.Where(x => x.DocumentoIdentificacion == filtro.DocumentoIdentificacion.Trim());
            }

            if (!string.IsNullOrWhiteSpace(filtro.apellidos))
            {
                query = query.Where(x => x.Apellidos.Contains(filtro.apellidos.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(filtro.nombres))
            {
                query = query.Where(x => x.Nombres.Contains(filtro.nombres.Trim()));
            }

            if (filtro.id_estado != null)
            {
                query = query.Where(x => x.IdEstado == filtro.id_estado);
            }

            return query.AsNoTracking();
        }


        public async Task CrearDatosBasicos(GENTEMAR_DATOSBASICOS entidad, GENTEMAR_REPOSITORIO_ARCHIVOS repositorio)
        {
            try
            {
                BeginTransaction();
                _context.GENTEMAR_DATOSBASICOS.Add(entidad);
                await SaveAllAsync();
                if (repositorio != null)
                {
                    repositorio.IdModulo = entidad.id_gentemar.ToString();
                    _context.GENTEMAR_REPOSITORIO_ARCHIVOS.Add(repositorio);
                }
                GuardarObservacionDefault(entidad.id_gentemar, entidad.LoginCreacionId, entidad.LoginModificacionId);
                await SaveAllAsync();
                CommitTransaction();

            }
            catch (Exception ex)
            {
                RollbackTransaction();
                ObtenerException(ex, entidad);
            }

        }

        private void GuardarObservacionDefault(long gentemarId, int loginCreacionId, int loginModificacionId)
        {
            GENTEMAR_OBSERVACIONES_DATOSBASICOS observacion = new GENTEMAR_OBSERVACIONES_DATOSBASICOS
            {
                id_gentemar = gentemarId,
                observacion = "Se creó la persona de gente de mar.",
                LoginCreacionId = loginCreacionId,
                LoginModificacionId = loginModificacionId
            };
            _context.GENTEMAR_OBSERVACIONES_DATOSBASICOS.Add(observacion);
        }


        public async Task ActualizarDatosBasicos(GENTEMAR_DATOSBASICOS entidadActual, GENTEMAR_DATOSBASICOS entidadReemplazo,
                                                GENTEMAR_REPOSITORIO_ARCHIVOS repositorio)
        {

            try
            {
                BeginTransaction();
                if (entidadActual.id_tipo_documento != entidadReemplazo.id_tipo_documento
                    || !entidadActual.documento_identificacion.Equals(entidadReemplazo.documento_identificacion))
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
                AgregarObservacion(entidadActual, entidadReemplazo);
                _context.Entry(entidadActual).CurrentValues.SetValues(entidadReemplazo);
                if (repositorio != null)
                {
                    repositorio.IdModulo = entidadActual.id_gentemar.ToString();
                    _context.GENTEMAR_REPOSITORIO_ARCHIVOS.Add(repositorio);
                }
                await SaveAllAsync();
                CommitTransaction();

            }
            catch (Exception ex)
            {
                RollbackTransaction();
                ObtenerException(ex, entidadReemplazo);
            }
        }

        private void AgregarObservacion(GENTEMAR_DATOSBASICOS entidadActual, GENTEMAR_DATOSBASICOS entidadReemplazo)
        {
            var cambios = new List<string>
                        {
                            CompararCampo(entidadActual.nombres, entidadReemplazo.nombres, "Nombres"),
                            CompararCampo(entidadActual.apellidos, entidadReemplazo.apellidos, "Apellidos"),
                            CompararCampo(entidadActual.correo_electronico, entidadReemplazo.correo_electronico, "Correo"),
                            CompararCampo(entidadActual.numero_movil, entidadReemplazo.numero_movil, "Celular"),
                            CompararCampo(entidadActual.direccion, entidadReemplazo.direccion, "Dirección"),
                            CompararCampo(entidadActual.documento_identificacion, entidadReemplazo.documento_identificacion, "Documento"),
                            CompararCambio(entidadActual.id_pais_residencia, entidadReemplazo.id_pais_residencia, "Se cambió el país de residencia"),
                            CompararCambio(entidadActual.id_pais_nacimiento, entidadReemplazo.id_pais_nacimiento, "Se cambió el país de nacimiento"),
                            CompararCambio(entidadActual.id_genero.ToString(), entidadReemplazo.id_genero.ToString(), "Se cambió el género"),
                            CompararCambio(entidadActual.id_tipo_documento.ToString(), entidadReemplazo.id_tipo_documento.ToString(), "Se cambió el tipo de documento"),
                            CompararCambio(entidadActual.id_formacion_grado.ToString(), entidadReemplazo.id_formacion_grado.ToString(), "Se cambió la formación"),
                            CompararCambio(entidadActual.id_municipio_residencia.ToString(), entidadReemplazo.id_municipio_residencia.ToString(), "Se cambió la ciudad de residencia"),
                            CompararCambio(entidadActual.id_municipio_expedicion.ToString(), entidadReemplazo.id_municipio_expedicion.ToString(), "Se cambió la ciudad de expedición del documento"),
                            CompararFecha(entidadActual.fecha_nacimiento, entidadReemplazo.fecha_nacimiento, "Fecha de nacimiento"),
                            CompararFecha(entidadActual.fecha_expedicion, entidadReemplazo.fecha_expedicion, "Fecha de expedición documento"),
                            CompararFecha(entidadActual.fecha_vencimiento, entidadReemplazo.fecha_vencimiento, "Fecha de vencimiento documento"),
                            CompararEstado(entidadActual.id_estado, entidadReemplazo.id_estado)
                        };

            var observacion = "Se actualizó la información de la persona de gente de mar. " + string.Join(", ", cambios.Where(c => !string.IsNullOrEmpty(c)));

            var entidadObservacion = new GENTEMAR_OBSERVACIONES_DATOSBASICOS
            {
                id_gentemar = entidadActual.id_gentemar,
                observacion = observacion
            };

            _context.GENTEMAR_OBSERVACIONES_DATOSBASICOS.Add(entidadObservacion);
        }

        private string CompararCampo(string valorActual, string valorReemplazo, string campo)
        {
            if ((valorActual ?? string.Empty) != (valorReemplazo ?? string.Empty))
            {
                return $"{campo}: {valorActual} -> {valorReemplazo}.";
            }
            return string.Empty;
        }

        private string CompararFecha(DateTime? fechaActual, DateTime? fechaReemplazo, string campo)
        {
            if (fechaActual.HasValue && fechaReemplazo.HasValue)
            {
                if (fechaActual.Value != fechaReemplazo.Value)
                {
                    return $"{campo}: {fechaActual:dd/MM/yyyy} -> {fechaReemplazo:dd/MM/yyyy}.";
                }
            }

            return string.Empty;
        }

        private string CompararCambio(string valorActual, string valorReemplazo, string mensaje)
        {

            if (!(string.IsNullOrEmpty(valorActual) && (!string.IsNullOrEmpty(valorReemplazo))))
            {
                if (!valorActual.Equals(valorReemplazo))
                {
                    return mensaje;
                }
            }
            return string.Empty;
        }

        private string CompararEstado(int estadoActualId, int estadoReemplazoId)
        {
            if (estadoActualId != estadoReemplazoId)
            {
                var estadoActual = (EstadoGenteMarEnum)estadoActualId;
                var estadoReemplazo = (EstadoGenteMarEnum)estadoReemplazoId;
                return $"Se cambió el estado de la persona de gente de mar de {EnumConfig.GetDescription(estadoActual)} a {EnumConfig.GetDescription(estadoReemplazo)}.";
            }
            return string.Empty;
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
            usuario = await (from GenteMar in _context.GENTEMAR_DATOSBASICOS
                             join tipoDocumento in _context.APLICACIONES_TIPO_DOCUMENTO on GenteMar.id_tipo_documento equals tipoDocumento.ID_TIPO_DOCUMENTO
                             join estado in _context.GENTEMAR_ESTADO on GenteMar.id_estado equals estado.id_estado
                             where GenteMar.documento_identificacion.Equals(identificacionConPuntos) || GenteMar.id_gentemar == id
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
                             }).FirstOrDefaultAsync();
            return usuario;
        }


        /// <summary>
        /// Metodo para obtener datos basicos filtados por id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DatosBasicosDTO> GetDatosBasicosIdAsync(long id)
        {
            var query = await (from datosBasicos in _context.GENTEMAR_DATOSBASICOS
                               join formacionGrado in _context.GENTEMAR_FORMACION_GRADO
                               on datosBasicos.id_formacion_grado equals formacionGrado.id_formacion_grado
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
                                                 && archivo.NombreModulo.Equals(Constantes.CARPETA_MODULO_DATOSBASICOS)
                                                 && archivo.TipoDocumento.Equals(Constantes.CARPETA_IMAGENES)
                                                 select new
                                                 {
                                                     archivo
                                                 }).OrderByDescending(x => x.archivo.FechaCreacion)
                                                 .Select(x => x.archivo.RutaArchivo).FirstOrDefault(),
                                   IdEstado = datosBasicos.id_estado,
                                   IdFormacionGrado = datosBasicos.id_formacion_grado,
                                   IdGenero = datosBasicos.id_genero,
                                   IdGentemar = datosBasicos.id_gentemar,
                                   IdMunicipioExpedicion = datosBasicos.id_municipio_expedicion,
                                   IdPaisResidencia = datosBasicos.id_pais_residencia,
                                   IdPaisNacimiento = datosBasicos.id_pais_nacimiento,
                                   IdMunicipioNacimiento = datosBasicos.id_municipio_nacimiento,
                                   IdMunicipioResidencia = datosBasicos.id_municipio_residencia,
                                   IdTipoDocumento = datosBasicos.id_tipo_documento,
                                   Nombres = datosBasicos.nombres,
                                   NumeroMovil = datosBasicos.numero_movil,
                                   TelefonoMasIndicativo = datosBasicos.telefono,
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

                               }).FirstOrDefaultAsync();
            return query;
        }
        public async Task<bool> ExisteById(long id)
        {
            return await AnyWithConditionAsync(x => x.id_gentemar == id);
        }
        public async Task<bool> ExistePersonaByIdentificacion(string identificacionConPuntos)
        {
            return await AnyWithConditionAsync(x => x.documento_identificacion.Equals(identificacionConPuntos));
        }


        public async Task<LicenciasTitulosDTO> GetlicenciaTituloVigentesPorDocumentoUsuario(string documento, DateTime fechaActual)
        {
            DateTime fechaActualHoraCero = fechaActual.Date; // Esto también devuelve la fecha actual con hora 00:00:00
            var licenciaData = (from licencia in _context.GENTEMAR_LICENCIAS
                                join cargoLicencia in _context.GENTEMAR_CARGO_LICENCIA on
                                licencia.id_cargo_licencia equals cargoLicencia.id_cargo_licencia
                                join estadoLicencia in _context.GENTEMAR_ESTADO_LICENCIAS on
                                licencia.id_estado_licencia equals estadoLicencia.id_estado_licencias
                                join usuario in _context.GENTEMAR_DATOSBASICOS on
                                licencia.id_gentemar equals usuario.id_gentemar
                                where usuario.documento_identificacion.Equals(documento) && licencia.fecha_vencimiento >= fechaActualHoraCero
                                select new LicenciaListarDTO
                                {
                                    CargoLicencia = cargoLicencia.cargo_licencia,
                                    DocumentoIdentificacion = usuario.documento_identificacion,
                                    EstadoTramite = estadoLicencia.descripcion_estado,
                                    FechaExpedicion = licencia.fecha_expedicion,
                                    FechaVencimiento = licencia.fecha_vencimiento,
                                    NombreUsuario = usuario.nombres + " " + usuario.apellidos,
                                    Radicado = licencia.radicado != 0 ? licencia.radicado.ToString() : "No contiene radicado."
                                }).AsNoTracking().ToListAsync();


            var DataTitulo = (from titulo in _context.GENTEMAR_TITULOS
                              join usuario in _context.GENTEMAR_DATOSBASICOS on titulo.id_gentemar equals usuario.id_gentemar
                              join tramite in _context.GENTEMAR_ESTADO_TITULO on titulo.id_estado_tramite equals tramite.id_estado_tramite
                              where usuario.documento_identificacion.Equals(documento) && titulo.fecha_vencimiento >= fechaActualHoraCero
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
                                            where cargoTitulo.id_titulo == titulo.id_titulo && cargoTitulo.es_eliminado == false
                                            select new InfoCargosDTO
                                            {
                                                CargoTitulo = cargo.cargo,
                                                Nivel = nivel.nivel,
                                                Regla = regla.nombre_regla
                                            }).ToList()
                              }).AsNoTracking().ToListAsync();


            var data = new LicenciasTitulosDTO()
            {
                Licencias = await licenciaData,
                Titulos = await DataTitulo
            };

            return data;


        }

        public async Task CambiarEstadoPersonaConTitulosLicencias(IEnumerable<GENTEMAR_LICENCIAS> licencias, IEnumerable<GENTEMAR_TITULOS> titulos,
            GENTEMAR_DATOSBASICOS data)
        {
            _context.Entry(data).State = EntityState.Modified;

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

        public async Task<string> GetNombrePorIdentificacion(string numero_identificacion_usuario)
        {
            return await (from datosBasicos in _context.GENTEMAR_DATOSBASICOS
                          where datosBasicos.documento_identificacion.Equals(numero_identificacion_usuario)
                          select datosBasicos.nombres + " " + datosBasicos.apellidos).FirstOrDefaultAsync();
        }

        public async Task CambiarEstadoPersonaEstaInactiva(long idGenteMar, long estadoLicenciaOTitulo)
        {
            var estadoEnum = (EstadosTituloLicenciaEnum)estadoLicenciaOTitulo;
            if (estadoEnum == EstadosTituloLicenciaEnum.VIGENTE)
            {
                var personaReemplazo = await GetByIdAsync(idGenteMar);

                // Clonar el objeto usando serialización y deserialización
                var personaActual = JsonConvert.DeserializeObject<GENTEMAR_DATOSBASICOS>(JsonConvert.SerializeObject(personaReemplazo));

                // Cambiar el estado de la persona sin afectar a personaActual
                personaReemplazo.id_estado = (int)EstadoGenteMarEnum.ACTIVO;

                _context.Entry(personaReemplazo).State = EntityState.Modified;

                // Agregar la observación comparando el objeto original con el modificado
                AgregarObservacion(personaActual, personaReemplazo);
            }
        }
    }
}

