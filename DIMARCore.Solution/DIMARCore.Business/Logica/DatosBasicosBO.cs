using DIMARCore.Business.Helpers;
using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.Utilities.CorreoSMTP;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using GenteMarCore.Entities.Models;
using log4net;
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
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region  Metodo que crea un usuario con sus datos basicos 
        /// <summary>
        ///  Metodo que crea un usuario con sus datos basicos 
        /// </summary>
        /// <param name="entidad"></param>
        /// <param name="rutaInicial"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusCodeException"></exception>
        public async Task<Respuesta> CrearAsync(GENTEMAR_DATOSBASICOS entidad, string rutaInicial)
        {
            Respuesta respuesta = new Respuesta();
            using (var repo = new DatosBasicosRepository())
            {
                var validate = await repo.AnyWithConditionAsync(x => x.documento_identificacion.Equals(entidad.documento_identificacion));
                if (validate)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse($@"El usuario con # de identificación: 
                                                                                      {entidad.documento_identificacion} ya se encuentra registrado."));
                try
                {
                    entidad.id_estado = (int)EstadoGenteMarEnum.ENPROCESO;
                    entidad.telefono = entidad.telefono.Trim();
                    entidad.numero_movil = entidad.numero_movil.Trim();
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
                            await repo.CrearDatosBasicos(entidad, repositorio);
                            respuesta = Responses.SetCreatedResponse();
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
            }
            return respuesta;
        }
        #endregion

        #region metodo que actualiza los datos basicos de una persona
        /// <summary>
        /// metodo que actualiza los datos basicos de una persona 
        /// </summary>
        /// <param name="dataReemplazo"></param>
        /// <param name="rutaInicial"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusCodeException"></exception>
        public async Task<Respuesta> ActualizarAsync(GENTEMAR_DATOSBASICOS dataReemplazo, string rutaInicial)
        {
            Respuesta respuesta = new Respuesta();
            using (var repo = new DatosBasicosRepository())
            {
                var existeDocumento = await repo.AnyWithConditionAsync(x => x.documento_identificacion.Equals(dataReemplazo.documento_identificacion)
                && x.id_gentemar != dataReemplazo.id_gentemar);
                if (existeDocumento)
                    throw new HttpStatusCodeException(HttpStatusCode.Conflict, "Ya se encuentra registrada una persona con el documento digitado.");

                var userGenteDeMar = await repo.GetWithConditionAsync(x => x.id_gentemar == dataReemplazo.id_gentemar);
                if (userGenteDeMar == null)
                    throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encuentra la persona registrada.");
                try
                {
                    if (dataReemplazo.Archivo == null)
                    {
                        var reemplazo = GENTEMAR_DATOSBASICOS.UpdateId(userGenteDeMar.id_gentemar, userGenteDeMar.id_estado, userGenteDeMar.FechaCreacion,
                                                             userGenteDeMar.LoginCreacionId, dataReemplazo);
                        await repo.ActualizarDatosBasicos(userGenteDeMar, reemplazo, null);
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
                            await repo.ActualizarDatosBasicos(userGenteDeMar, reemplazo, repositorio);
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
            }
            return respuesta;
        }
        #endregion

        #region metodo que actualiza el estado del usuario y dependiendo el estado actualiza el estado de los titulos y licencias
        public async Task<Respuesta> ChangeStatus(GENTEMAR_DATOSBASICOS datos, string rutaInicial)
        {

            using (var repo = new DatosBasicosRepository())
            {
                var data = await repo.GetWithConditionAsync(x => x.id_gentemar == datos.id_gentemar);
                var estadoActual = data.id_estado;
                if (data == null)
                    throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encuentra registrado el usuario.");

                data.observacion = datos.observacion;
                var obser = await new ObservacionesBO().CrearObservacionesDatosBasicos(datos.observacion, rutaInicial);
                if (!obser.Estado)
                    return obser;

                data.id_estado = datos.id_estado;

                var cambio = await CambiarEstadoTitulosLicenciasByEstadoPersona(repo, estadoActual, datos.id_estado, data.id_gentemar);

                await repo.CambiarEstadoPersonaConTitulosLicencias(cambio.Licencias, cambio.Titulos, data);

                if (cambio.IsSendEmail)
                    await EnviarNotificacion(cambio.Mensaje, cambio.Estado, $"{data.nombres} {data.apellidos}");

                return Responses.SetUpdatedResponse();
            }
        }

        private async Task EnviarNotificacion(string mensaje, string estado, string nombreCompleto)
        {
            String[] emails = await new UsuarioRepository().GetEmailsAdministradores();
            SendEmailRequest request = new SendEmailRequest
            {
                CorreosDestino = emails,
                Asunto = $"Actualización estado {mensaje} a la hora {DateTime.Now:dd/MM/yyyy hh: mm:ss tt}",
                CuerpoDelMensaje = $"Se actualizaron {mensaje} a {estado} de la persona {nombreCompleto}."
            };
            await new EnvioNotificacionesHelper().SendNotificationByEmail(request);
        }
        #endregion

        #region  Metodo que cambia el estado de los titulos y licencias de una persona a cancelado cuando fallece o se inactiva
        /// <summary>
        /// Metodo que cambia el estado de los titulos y licencias de una persona a cancelado cuando fallece o se inactiva
        /// </summary>
        /// <param name="repo">obj de repositorio</param>
        /// <param name="estadoGenteDeMarId">parametro id estado de la persona de gente de mar de datos basicos</param>
        /// <param name="gentemarId">parametro id de la persona de gente de mar de datos basicos</param>
        /// <returns></returns>
        private async Task<CambioEstadoLicenciaTituloDTO> CambiarEstadoTitulosLicenciasByEstadoPersona(DatosBasicosRepository repo,
            int estadoGenteDeMarActualId, int estadoGenteDeMarId, long gentemarId)
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
                licencias = await new LicenciaRepository().GetAllWithConditionAsync(x => x.id_gentemar == gentemarId
                                                                 && x.id_estado_licencia != (int)EstadosTituloLicenciaEnum.CANCELADO);

                titulos = await new TituloRepository().GetAllWithConditionAsync(x => x.id_gentemar == gentemarId
                                                                    && x.id_estado_tramite != (int)EstadosTituloLicenciaEnum.CANCELADO);

                GetLicenciasTitulosPorEstado(ref licencias, ref titulos, EstadosTituloLicenciaEnum.CANCELADO);

                cambioEstado.Estado = $"estado {EstadosTituloLicenciaEnum.CANCELADO}";
            }

            else if (cambioAActivo)
            {
                licencias = await new LicenciaRepository().GetAllWithConditionAsync(x => x.id_gentemar == gentemarId
                                                                  && x.id_estado_licencia == (int)EstadosTituloLicenciaEnum.CANCELADO
                                                                  && x.fecha_vencimiento >= date.DateEnd);

                titulos = await new TituloRepository().GetAllWithConditionAsync(x => x.id_gentemar == gentemarId
                                                                   && x.id_estado_tramite == (int)EstadosTituloLicenciaEnum.CANCELADO
                                                                   && x.fecha_vencimiento >= date.DateEnd);

                GetLicenciasTitulosPorEstado(ref licencias, ref titulos, EstadosTituloLicenciaEnum.VIGENTE);
                cambioEstado.Estado = $"estado {EstadosTituloLicenciaEnum.VIGENTE}";
            }

            if (licencias.Any())
            {
                cambioEstado.Mensaje = $"{licencias.Count()} licencias ";
                cambioEstado.IsSendEmail = true;
            }
            if (titulos.Any())
            {
                cambioEstado.Mensaje += $"{titulos.Count()} titulos";
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

        /// <summary>
        /// Metodo que obtiene un usuario en especifico por id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DatosBasicosDTO</returns>
        public async Task<DatosBasicosDTO> GetDatosBasicosIdAsync(long id, string rutaInicial)
        {
            var data = await new DatosBasicosRepository().GetDatosBasicosIdAsync(id);
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

        public IQueryable<ListadoDatosBasicosDTO> GetDatosBasicosQueryable(DatosBasicosQueryFilter filtro)
        {
            var datos = new DatosBasicosRepository().GetDatosBasicosQueryable(filtro).OrderByDescending(x => x.FechaRegistro);
            if (!datos.Any())
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encontraron registros."));
            return datos;
        }

        public async Task<Respuesta> ExisteById(long id)
        {
            var existe = await new DatosBasicosRepository().ExisteById(id);
            if (!existe)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encuentra registrada la persona."));
            return Responses.SetOkResponse();
        }

        /// <summary>
        /// Obtener la licencia dado el documento del usuario.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objeto licencia dado el id de usaurio</returns>
        /// <entidad>GENTEMAR_ACTIVIDAD</entidad>
        /// <tabla>GENTEMAR_ACTIVIDAD</tabla>
        public async Task<LicenciasTitulosDTO> GetlicenciaTituloVigentesPorDocumentoUsuario(string documento)
        {
            var documentoPuntos = Reutilizables.ConvertirStringApuntosDeMil(documento);
            return await new DatosBasicosRepository().GetlicenciaTituloVigentesPorDocumentoUsuario(documentoPuntos);
        }


        /// <summary>
        /// Valida si el usuario existe y si puede crear licencias y titulos dependiendo el estado.
        /// </summary>
        /// <param name="parametrosGenteMar"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusCodeException"></exception>
        public async Task<Respuesta> ValidationsStatusPersona(ParametrosGenteMarDTO parametrosGenteMar)
        {
            var datos = await new DatosBasicosRepository()
                .GetPersonaByIdentificacionOrId(parametrosGenteMar.Identificacion, parametrosGenteMar.Id)
                ?? throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encuentra registrada la persona en Datos Básicos.");

            if (!datos.IsCreateTituloOrLicencia && (!parametrosGenteMar.IsModuleEstupefacientes))
                throw new HttpStatusCodeException(HttpStatusCode.Conflict, $"El usuario está en estado {datos.NombreEstado}, " +
                                                                           $"por lo tanto no puede generar títulos y licencias de navegación.");

            if (!parametrosGenteMar.IsModuleEstupefacientes && datos.ContieneEstupefacienteVigente)
                throw new HttpStatusCodeException(HttpStatusCode.Conflict, "El usuario contiene verificación de carencia de informe por Trafico de Estupefacientes (VCITE), " +
                                                                           "por lo tanto no puede generar títulos y licencias de navegación.");

            bool isExistInGenteDeMar = datos != null;

            if (parametrosGenteMar.IsModuleEstupefacientes)
                await new EstupefacienteBO().GetDatosGenteMarEstupefacienteValidations(parametrosGenteMar.Identificacion, isExistInGenteDeMar);

            return Responses.SetOkResponse(datos);
        }

        /// <summary>
        /// cambia el estado del usuario por id 
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        public async Task<Respuesta> CambioEstadoIdUsuario(long idUsuario, int idEstado)
        {
            using (var repo = new DatosBasicosRepository())
            {
                var data = await repo.GetWithConditionAsync(x => x.id_gentemar == idUsuario);
                if (data == null)
                    throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encuentra registrada la persona");
                data.id_estado = idEstado;
                await repo.ActualizarDatosBasicosSinFoto(data);
                return Responses.SetOkResponse();
            }
        }

    }
}
