using DIMARCore.Business.Helpers;
using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.Utilities.CorreoSMTP;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
namespace DIMARCore.Business.Logica
{
    public class DatosBasicosBO
    {
        private readonly EstupefacienteDatosBasicosRepository _estupefacienteDatosBasicosRepository;
        private readonly DatosBasicosRepository _datosBasicosRepository;
        private readonly LicenciaRepository _licenciaRepository;
        private readonly TituloRepository _tituloRepository;
        public DatosBasicosBO()
        {
            _estupefacienteDatosBasicosRepository = new EstupefacienteDatosBasicosRepository();
            _datosBasicosRepository = new DatosBasicosRepository();
            _licenciaRepository = new LicenciaRepository();
            _tituloRepository = new TituloRepository();
        }
        public DatosBasicosBO(DatosBasicosRepository datosBasicosRepository, EstupefacienteDatosBasicosRepository estupefacienteDatosBasicosRepository,
            LicenciaRepository licenciaRepository, TituloRepository tituloRepository)
        {
            _datosBasicosRepository = datosBasicosRepository;
            _estupefacienteDatosBasicosRepository = estupefacienteDatosBasicosRepository;
            _licenciaRepository = licenciaRepository;
            _tituloRepository = tituloRepository;
        }

        #region  Metodo que crea un usuario con sus datos basicos 
        public async Task<Respuesta> CrearAsync(GENTEMAR_DATOSBASICOS entidad, string rutaInicial)
        {
            Respuesta respuesta = new Respuesta();

            var validate = await _datosBasicosRepository.AnyWithConditionAsync(x => x.documento_identificacion.Equals(entidad.documento_identificacion));
            if (validate)
                throw new HttpStatusCodeException(Responses.SetConflictResponse($"El usuario con # de identificación: {entidad.documento_identificacion} ya se encuentra registrado."));

            if (!entidad.IncludePhoto)
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "Debe adjuntar una foto de la persona.");

            try
            {
                entidad.id_estado = (int)EstadoGenteMarEnum.ENPROCESO;
                entidad.telefono = entidad.telefono.Trim();
                entidad.numero_movil = entidad.numero_movil.Trim();
                if (entidad.Archivo != null)
                {
                    string path = $"{Constantes.CARPETA_MODULO_DATOSBASICOS}\\{Constantes.CARPETA_IMAGENES}";
                    var nombreArchivo = $"{Guid.NewGuid()}_{entidad.Archivo.FileName}";
                    respuesta = Reutilizables.GuardarArchivo(entidad.Archivo, rutaInicial, path, nombreArchivo);
                    if (respuesta.Estado)
                    {
                        var archivo = (Archivo)respuesta.Data;
                        if (archivo != null)
                        {
                            GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = new GENTEMAR_REPOSITORIO_ARCHIVOS()
                            {
                                IdAplicacion = Constantes.ID_APLICACION,
                                NombreModulo = Constantes.CARPETA_MODULO_DATOSBASICOS,
                                TipoDocumento = Constantes.CARPETA_IMAGENES,
                                FechaCargue = DateTime.Now,
                                NombreArchivo = entidad.Archivo.FileName,
                                Nombre = entidad.Archivo.FileName,
                                RutaArchivo = archivo.PathArchivo,
                                DescripcionDocumento = Reutilizables.DescribirDocumento(Path.GetExtension(archivo.NombreArchivo))
                            };
                            await _datosBasicosRepository.CrearDatosBasicos(entidad, repositorio);

                        }
                    }
                }
                else
                {
                    await _datosBasicosRepository.CrearDatosBasicos(entidad, null);
                }
                respuesta = Responses.SetCreatedResponse(new
                {
                    UsuarioGenteMarId = entidad.id_gentemar,
                    DocumentoIdentificacion = entidad.documento_identificacion,
                    NombreCompleto = $"{entidad.nombres} {entidad.apellidos}"
                });
            }
            catch (Exception ex)
            {
                var archivo = (Archivo)respuesta.Data;
                if (archivo != null)
                {
                    Reutilizables.EliminarArchivo(rutaInicial, archivo.PathArchivo);
                }
                respuesta = Responses.SetInternalServerErrorResponse(ex);
                _ = new DbLoggerHelper().InsertLogToDatabase(respuesta);
            }
            return respuesta;
        }
        #endregion

        #region metodo que actualiza los datos basicos de una persona
        public async Task<Respuesta> ActualizarAsync(GENTEMAR_DATOSBASICOS dataReemplazo, string rutaInicial)
        {
            Respuesta respuesta = new Respuesta();
            var existeDocumento = await _datosBasicosRepository.AnyWithConditionAsync(x => x.documento_identificacion.Equals(dataReemplazo.documento_identificacion)
            && x.id_gentemar != dataReemplazo.id_gentemar);
            if (existeDocumento)
                throw new HttpStatusCodeException(HttpStatusCode.Conflict, "Ya se encuentra registrada una persona con el documento digitado.");

            var userGenteDeMar = await _datosBasicosRepository.GetWithConditionAsync(x => x.id_gentemar == dataReemplazo.id_gentemar);
            if (userGenteDeMar == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encuentra la persona registrada.");
            try
            {
                if (dataReemplazo.Archivo == null)
                {
                    var reemplazo = GENTEMAR_DATOSBASICOS.UpdateId(userGenteDeMar.id_gentemar, userGenteDeMar.id_estado, userGenteDeMar.FechaCreacion,
                                                         userGenteDeMar.LoginCreacionId, dataReemplazo);
                    await _datosBasicosRepository.ActualizarDatosBasicos(userGenteDeMar, reemplazo, null);
                    return Responses.SetUpdatedResponse(reemplazo);
                }
                string path = $"{Constantes.CARPETA_MODULO_DATOSBASICOS}\\{Constantes.CARPETA_IMAGENES}";
                var nombreArchivo = $"{Guid.NewGuid()}_{dataReemplazo.Archivo.FileName}";
                respuesta = Reutilizables.GuardarArchivo(dataReemplazo.Archivo, rutaInicial, path, nombreArchivo);
                if (respuesta.Estado)
                {
                    var archivo = (Archivo)respuesta.Data;
                    if (archivo != null)
                    {
                        GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = new GENTEMAR_REPOSITORIO_ARCHIVOS()
                        {
                            IdAplicacion = Constantes.ID_APLICACION,
                            NombreModulo = Constantes.CARPETA_MODULO_DATOSBASICOS,
                            TipoDocumento = Constantes.CARPETA_IMAGENES,
                            FechaCargue = DateTime.Now,
                            NombreArchivo = dataReemplazo.Archivo.FileName,
                            RutaArchivo = archivo.PathArchivo,
                        };
                        var reemplazo = GENTEMAR_DATOSBASICOS.UpdateId(userGenteDeMar.id_gentemar, userGenteDeMar.id_estado, userGenteDeMar.FechaCreacion,
                                                         userGenteDeMar.LoginCreacionId, dataReemplazo);
                        await _datosBasicosRepository.ActualizarDatosBasicos(userGenteDeMar, reemplazo, repositorio);
                        return Responses.SetUpdatedResponse(reemplazo);
                    }
                }
            }
            catch (Exception ex)
            {
                var archivo = (Archivo)respuesta.Data;
                if (archivo != null)
                {
                    Reutilizables.EliminarArchivo(rutaInicial, archivo.PathArchivo);
                }
                respuesta = Responses.SetInternalServerErrorResponse(ex);
                _ = new DbLoggerHelper().InsertLogToDatabase(respuesta);
            }
            return respuesta;
        }
        #endregion

        #region metodo que actualiza el estado del usuario y dependiendo el estado actualiza el estado de los titulos y licencias
        public async Task<Respuesta> CambiarEstadoPersona(GENTEMAR_DATOSBASICOS datos, string rutaInicial)
        {

            var data = await _datosBasicosRepository.GetWithConditionAsync(x => x.id_gentemar == datos.id_gentemar);
            var estadoActual = data.id_estado;
            if (data == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encuentra registrado el usuario.");

            data.observacion = datos.observacion;
            var obser = await new ObservacionesBO().CrearObservacionesDatosBasicos(datos.observacion, rutaInicial);
            if (!obser.Estado)
                return obser;

            data.id_estado = datos.id_estado;

            var cambio = await CambiarEstadoTitulosLicenciasByEstadoPersona(estadoActual, datos.id_estado, data.id_gentemar);
            if (cambio == null)
            {
                await _datosBasicosRepository.ActualizarDatosBasicosSinFoto(data);
                return Responses.SetUpdatedResponse();
            }
            await _datosBasicosRepository.CambiarEstadoPersonaConTitulosLicencias(cambio.Licencias, cambio.Titulos, data);
            if (cambio.IsSendEmail)
                await EnviarNotificacion(cambio.Mensaje, cambio.Estado, $"{data.nombres} {data.apellidos}");
            return Responses.SetUpdatedResponse();
        }


        #endregion

        public async Task EnviarNotificacion(string mensaje, string estado, string nombreCompleto)
        {
            String[] emails = await new UsuarioRepository().GetEmailsAdministradores();
            SendEmailRequest request = new SendEmailRequest
            {
                CorreosDestino = emails,
                Asunto = $"Actualización estado {mensaje} persona {nombreCompleto} - Hora: {DateTime.Now:dd/MM/yyyy hh: mm:ss tt}",
                CuerpoDelMensaje = $"Se actualizaron {mensaje} a {estado} de la persona {nombreCompleto}."
            };
            await new EnvioNotificacionesHelper().SendNotificationByEmail(request);
        }

        #region  Metodo que cambia el estado de los titulos y licencias de una persona a cancelado cuando fallece o se inactiva
        public async Task<CambioEstadoLicenciaTituloDTO> CambiarEstadoTitulosLicenciasByEstadoPersona(int estadoGenteDeMarActualId, int estadoGenteDeMarId, long gentemarId)
        {
            var cambioEstado = new CambioEstadoLicenciaTituloDTO();

            var date = Reutilizables.FormatDatesByRange(DateTime.Now, DateTime.Now);
            IEnumerable<GENTEMAR_LICENCIAS> licencias = new List<GENTEMAR_LICENCIAS>();
            IEnumerable<GENTEMAR_TITULOS> titulos = new List<GENTEMAR_TITULOS>();
            string estadoTituloLicencia = string.Empty;

            var EsFallecidoOInactivo = estadoGenteDeMarId == (int)EstadoGenteMarEnum.FALLECIDO || estadoGenteDeMarId == (int)EstadoGenteMarEnum.INACTIVO;
            var cambioAActivo = estadoGenteDeMarId == (int)EstadoGenteMarEnum.ACTIVO &&
                (estadoGenteDeMarActualId == (int)EstadoGenteMarEnum.FALLECIDO || estadoGenteDeMarActualId == (int)EstadoGenteMarEnum.INACTIVO);

            if (!EsFallecidoOInactivo && !(cambioAActivo))
                return null;

            if (EsFallecidoOInactivo)
            {
                licencias = await _licenciaRepository.GetAllWithConditionAsync(x => x.id_gentemar == gentemarId
                                                                 && x.id_estado_licencia != (int)EstadosTituloLicenciaEnum.CANCELADO);

                titulos = await _tituloRepository.GetAllWithConditionAsync(x => x.id_gentemar == gentemarId
                                                                    && x.id_estado_tramite != (int)EstadosTituloLicenciaEnum.CANCELADO);

                GetLicenciasTitulosPorEstado(ref licencias, ref titulos, EstadosTituloLicenciaEnum.CANCELADO);

                cambioEstado.Estado = $"estado {EstadosTituloLicenciaEnum.CANCELADO}";
            }

            else if (cambioAActivo)
            {
                licencias = await _licenciaRepository.GetAllWithConditionAsync(x => x.id_gentemar == gentemarId
                                                                  && x.id_estado_licencia == (int)EstadosTituloLicenciaEnum.CANCELADO
                                                                  && x.fecha_vencimiento >= date.DateEnd);

                titulos = await _tituloRepository.GetAllWithConditionAsync(x => x.id_gentemar == gentemarId
                                                                   && x.id_estado_tramite == (int)EstadosTituloLicenciaEnum.CANCELADO
                                                                   && x.fecha_vencimiento >= date.DateEnd);

                GetLicenciasTitulosPorEstado(ref licencias, ref titulos, EstadosTituloLicenciaEnum.VIGENTE);
                cambioEstado.Estado = $"estado {EstadosTituloLicenciaEnum.VIGENTE}";
            }

            if (licencias.Any())
            {
                cambioEstado.Mensaje = $"{licencias.Count()} licencias";
                cambioEstado.IsSendEmail = true;
            }
            if (titulos.Any())
            {
                cambioEstado.Mensaje += $" {titulos.Count()} títulos";
                cambioEstado.IsSendEmail = true;
            }
            cambioEstado.Licencias = licencias;
            cambioEstado.Titulos = titulos;
            return cambioEstado;
        }

        private static void GetLicenciasTitulosPorEstado(ref IEnumerable<GENTEMAR_LICENCIAS> licencias, ref IEnumerable<GENTEMAR_TITULOS> titulos,
            EstadosTituloLicenciaEnum estado)
        {
            licencias = licencias.Select(entidad =>
            {
                // Realizar los cambios necesarios en cada objeto de la lista se cambia el estado
                entidad.id_estado_licencia = (int)estado;
                return entidad;
            }).ToList();

            titulos = titulos.Select(entidad =>
            {
                // Realizar los cambios necesarios en cada objeto de la lista se cambia el estado
                entidad.id_estado_tramite = (int)estado;
                return entidad;
            }).ToList();
        }
        #endregion


        public async Task<DatosBasicosDTO> GetDatosBasicosIdAsync(long id, string rutaInicial)
        {
            var data = await _datosBasicosRepository.GetDatosBasicosIdAsync(id);
            if (data == null)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encuentra registrada la persona."));

            if (!string.IsNullOrWhiteSpace(data.UrlArchivo))
            {
                var rutaArchivo = $@"{rutaInicial}\{data.UrlArchivo}";
                string archivoBase64 = null;

                var respuestaBuscarArchivo = Reutilizables.DescargarArchivo(rutaArchivo, out archivoBase64);
                if (respuestaBuscarArchivo != null && respuestaBuscarArchivo.Estado && !string.IsNullOrEmpty(archivoBase64))
                {
                    data.FotoBase64 = archivoBase64;
                }
                else
                {
                    _ = new DbLoggerHelper().InsertLogToDatabase(respuestaBuscarArchivo);
                }
            }
            return data;
        }

        public IQueryable<ListarDatosBasicosDTO> GetDatosBasicosQueryable(DatosBasicosQueryFilter filtro)
        {
            var datos = _datosBasicosRepository.GetDatosBasicosQueryable(filtro).OrderByDescending(x => x.FechaRegistro);
            if (!datos.Any())
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encontraron registros."));
            return datos;
        }

        public async Task<Respuesta> ExisteById(long id)
        {
            var existe = await _datosBasicosRepository.ExisteById(id);
            if (!existe)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encuentra registrada la persona."));
            return Responses.SetOkResponse();
        }


        public async Task<LicenciasTitulosDTO> GetlicenciaTituloVigentesPorDocumentoUsuario(string documento, DateTime fechaActual)
        {
            var documentoPuntos = Reutilizables.ConvertirStringApuntosDeMil(documento);
            var data = await _datosBasicosRepository.GetlicenciaTituloVigentesPorDocumentoUsuario(documentoPuntos, fechaActual);
            return data;
        }


        public async Task<Respuesta> GetDatosBasicosValidacionEstadoyVcitePersona(ParametrosGenteMarDTO parametrosGenteMar)
        {
            var dataPerson = await GetDatosBasicosValidacionEstadoPersona(parametrosGenteMar);
            await ValidarVcitePersona(dataPerson.DocumentoIdentificacion, DateTime.Now);
            return Responses.SetOkResponse(dataPerson);
        }

        public async Task<Respuesta> GetDatosBasicosValidacionPersona(ParametrosGenteMarDTO parametrosGenteMar)
        {
            var dataPerson = await GetDatosBasicosValidacionEstadoPersona(parametrosGenteMar);
            return Responses.SetOkResponse(dataPerson);
        }

        public async Task ValidarVcitePersona(string documentoIdentificacion, DateTime fechaActual)
        {
            var contieneEstupefacienteVigente = await _estupefacienteDatosBasicosRepository.ValidarEstupefacienteVigentePersona(documentoIdentificacion, fechaActual);
            var contieneEstupefacienteNegativo = await _estupefacienteDatosBasicosRepository.ValidarEstupefacienteNegativoPersona(documentoIdentificacion);
            if (!contieneEstupefacienteVigente && contieneEstupefacienteNegativo)
            {
                throw new HttpStatusCodeException(HttpStatusCode.Conflict,
                                                 @"El usuario contiene verificación negativa en carencia de informe por Trafico de Estupefacientes (VCITE),
                                                           por lo tanto no puede generar títulos y licencias de navegación.");
            }
            if (!contieneEstupefacienteVigente)
            {
                throw new HttpStatusCodeException(HttpStatusCode.Conflict,
                                                  @"El usuario no se le ha realizado la consulta o la verificación 
                                                        de carencia de informe por Trafico de Estupefacientes (VCITE),
                                                        por lo tanto no puede generar títulos y licencias de navegación.");
            }
        }
        public async Task<UsuarioGenteMarDTO> GetDatosBasicosValidacionEstadoPersona(ParametrosGenteMarDTO parametrosGenteMar)
        {
            var datos = await _datosBasicosRepository
                .GetPersonaByIdentificacionOrId(parametrosGenteMar.Identificacion, parametrosGenteMar.Id)
                ?? throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encuentra registrada la persona en Datos Básicos.");

            if (!datos.IsCreateTituloOrLicencia)
                throw new HttpStatusCodeException(HttpStatusCode.Conflict, $"El usuario está en estado {datos.NombreEstado}, " +
                                                                           $"por lo tanto no puede generar títulos y licencias de navegación.");
            return datos;
        }

        public async Task<Respuesta> GetlicenciaTituloActivoIdUsuario(long id)
        {
            var licencias = (await new LicenciaRepository().GetlicenciasPorUsuarioId(id))
                .Where(x => x.IdEstadoLicencia == (int)EstadosTituloLicenciaEnum.VIGENTE)
                .Select(x => new { Id = x.IdLicencia, Nombre = x.CargoLicencia.CargoLicencia }).ToList();

            var titulos = (await new TituloRepository().GetTitulosFiltro("", id))
                .Where(x => x.IdEstadoTramite == (int)EstadosTituloLicenciaEnum.VIGENTE)
                .Select(x => new { Id = x.Id, Nombre = string.Join(",", x.Cargos.Select(y => y.CargoTitulo)) }).ToList();
            var listaCombinada = licencias.Concat(titulos).ToList();
            return Responses.SetOkResponse(listaCombinada);
        }
    }
}
