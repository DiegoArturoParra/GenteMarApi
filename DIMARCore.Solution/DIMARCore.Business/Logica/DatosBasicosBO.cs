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
                var validate = await repo.AnyWithCondition(x => x.documento_identificacion.Equals(entidad.documento_identificacion));
                if (validate)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse($@"El usuario con # de identificación: 
                                                                                      {entidad.documento_identificacion} ya se encuentra registrado."));
                try
                {
                    entidad.id_estado = (int)EstadoGenteMarEnum.ENPROCESO;
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
                    _ = new DbLogger().InsertLogToDatabase(respuesta);
                }
            }
            return respuesta;
        }
        #endregion

        #region metodo que actualiza los datos basicos de una persona
        /// <summary>
        /// metodo que actualiza los datos basicos de una persona 
        /// </summary>
        /// <param name="entidad"></param>
        /// <param name="rutaInicial"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusCodeException"></exception>
        public async Task<Respuesta> ActualizarAsync(GENTEMAR_DATOSBASICOS entidad, string rutaInicial)
        {
            Respuesta respuesta = new Respuesta();
            using (var repo = new DatosBasicosRepository())
            {
                var existeDocumento = await repo.AnyWithCondition(x => x.documento_identificacion.Equals(entidad.documento_identificacion)
                && x.id_gentemar != entidad.id_gentemar);
                if (existeDocumento)
                    throw new HttpStatusCodeException(HttpStatusCode.Conflict, "Ya se encuentra registrada una persona con el documento digitado.");

                var userGenteDeMar = await repo.GetWithCondition(x => x.id_gentemar == entidad.id_gentemar);
                if (userGenteDeMar == null)
                    throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encuentra la persona registrada.");
                try
                {
                    if (entidad.Archivo == null)
                    {
                        var reemplazo = GENTEMAR_DATOSBASICOS.UpdateId(userGenteDeMar.id_gentemar, userGenteDeMar.id_estado, userGenteDeMar.fecha_hora_creacion,
                                    userGenteDeMar.usuario_creador_registro, entidad);
                        await repo.ActualizarDatosBasicos(userGenteDeMar, reemplazo, null);
                        return Responses.SetUpdatedResponse(reemplazo);
                    }
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
                                RutaArchivo = archivo.PathArchivo,
                            };
                            var reemplazo = GENTEMAR_DATOSBASICOS.UpdateId(userGenteDeMar.id_gentemar, userGenteDeMar.id_estado, userGenteDeMar.fecha_hora_creacion,
                                userGenteDeMar.usuario_creador_registro, entidad);
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
                    _ = new DbLogger().InsertLogToDatabase(respuesta);
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
                var data = await repo.GetWithCondition(x => x.id_gentemar == datos.id_gentemar);
                if (data == null)
                    throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encuentra registrado el usuario.");
                data.observacion = datos.observacion;
                var obser = await new ObservacionesBO().CrearObservacionesDatosBasicos(datos.observacion, rutaInicial);
                if (!obser.Estado)
                    return obser;

                var (estado, isSend, mensaje) = await CambiarEstadoTitulosLicenciasByEstadoPersona(repo, data.id_estado, datos.id_estado, data.id_gentemar);
                if (isSend)
                    _ = Task.Run(async () =>
                    {
                        String[] emails = await new UsuarioRepository().GetEmailsAdministradores();
                        SendEmailRequest request = new SendEmailRequest
                        {
                            CorreosAEnviar = emails,
                            Asunto = $"Actualización estado {mensaje} a la hora {DateTime.Now:dd/MM/yyyy hh: mm:ss tt}",
                            CuerpoDelMensaje = $"Se actualizaron {mensaje} a {estado} de la persona {data.nombres} {data.apellidos}."
                        };
                        await new EnvioNotificacionesBO().SendNotificationByEmail(_logger, request);
                    });

                data.id_estado = datos.id_estado;
                await repo.ActualizarDatosBasicosSinFoto(data);
                return Responses.SetUpdatedResponse();
            }
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
        private async Task<(string estado, bool isSendEmail, string mensaje)> CambiarEstadoTitulosLicenciasByEstadoPersona(DatosBasicosRepository repo,
            int estadoGenteDeMarAcualId, int estadoGenteDeMarId, long gentemarId)
        {
            IEnumerable<GENTEMAR_LICENCIAS> licencias = new List<GENTEMAR_LICENCIAS>();
            IEnumerable<GENTEMAR_TITULOS> titulos = new List<GENTEMAR_TITULOS>();
            string estadoTituloLicencia = string.Empty;
            if (estadoGenteDeMarId == (int)EstadoGenteMarEnum.FALLECIDO || estadoGenteDeMarId == (int)EstadoGenteMarEnum.INACTIVO)
            {
                licencias = await new LicenciaRepository().GetAllWithConditionAsync(x => x.id_gentemar == gentemarId
                                                                    && x.id_estado_licencia != (int)EstadosTituloLicenciaEnum.CANCELADO);

                titulos = await new TituloRepository().GetAllWithConditionAsync(x => x.id_gentemar == gentemarId
                                                                    && x.id_estado_tramite != (int)EstadosTituloLicenciaEnum.CANCELADO);

                GetLicenciasTitulosPorEstado(ref licencias, ref titulos, EstadosTituloLicenciaEnum.CANCELADO);

                estadoTituloLicencia = $"estado {EstadosTituloLicenciaEnum.CANCELADO}";
            }
            else if (estadoGenteDeMarId == (int)EstadoGenteMarEnum.ACTIVO
                && (estadoGenteDeMarAcualId == (int)EstadoGenteMarEnum.FALLECIDO || estadoGenteDeMarAcualId == (int)EstadoGenteMarEnum.INACTIVO))
            {
                licencias = await new LicenciaRepository().GetAllWithConditionAsync(x => x.id_gentemar == gentemarId
                                                                  && x.id_estado_licencia == (int)EstadosTituloLicenciaEnum.CANCELADO);

                titulos = await new TituloRepository().GetAllWithConditionAsync(x => x.id_gentemar == gentemarId
                                                                   && x.id_estado_tramite == (int)EstadosTituloLicenciaEnum.CANCELADO);

                GetLicenciasTitulosPorEstado(ref licencias, ref titulos, EstadosTituloLicenciaEnum.VIGENTE);
                estadoTituloLicencia = $"estado {EstadosTituloLicenciaEnum.VIGENTE}";
            }
            else
            {
                return (estadoTituloLicencia, false, string.Empty);
            }
            if (licencias.Any() && titulos.Any())
            {
                await repo.CambiarEstadoTitulosLicenciasByEstadoPersona(licencias, titulos);
                return (estadoTituloLicencia, true, "licencias y titulos");

            }
            else if (licencias.Any())
            {
                await repo.CambiarEstadoTitulosLicenciasByEstadoPersona(licencias, titulos);
                return (estadoTituloLicencia, true, "licencias");
            }
            else if (titulos.Any())
            {
                await repo.CambiarEstadoTitulosLicenciasByEstadoPersona(licencias, titulos);
                return (estadoTituloLicencia, true, "titulos");
            }

            return (estadoTituloLicencia, false, string.Empty);
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
        public DatosBasicosDTO GetDatosBasicosId(long id, string rutaInicial)
        {
            var data = new DatosBasicosRepository().GetDatosBasicosId(id);
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
                    new DbLogger().InsertLogToDatabase(respuestaBuscarArchivo);
                }
            }
            return data;
        }

        public IQueryable<ListadoDatosBasicosDTO> GetDatosBasicosQueryable(DatosBasicosQueryFilter filtro)
        {
            var datos = new DatosBasicosRepository().GetDatosBasicosQueryable(filtro).OrderByDescending(x => x.FechaRegistro);
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
                throw new HttpStatusCodeException(HttpStatusCode.Conflict, $"El usuario está en estado {datos.NombreEstado}" +
                                                                            "no puede generar títulos y licencias de navegación.");

            if (!parametrosGenteMar.IsModuleEstupefacientes && datos.ContieneEstupefacienteVigente)
                throw new HttpStatusCodeException(HttpStatusCode.Conflict, "El usuario contiene verificación de carencia de informe por Trafico de Estupefacientes (VCITE)," +
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
                var data = await repo.GetWithCondition(x => x.id_gentemar == idUsuario);
                if (data == null)
                    throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encuentra registrada la persona");
                data.id_estado = idEstado;
                await repo.ActualizarDatosBasicosSinFoto(data);
                return Responses.SetOkResponse();
            }
        }

    }
}
