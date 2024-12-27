using DIMARCore.Business.Helpers;
using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Config;
using DIMARCore.Utilities.CorreoSMTP;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using GenteMarCore.Entities.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class ImpresionDocumentosBO
    {
        private CargoLicenciaBO cargolicenciaBo;
        private SgdeaBO sgdeaBo;
        private SGDEARepository sgdeaRepository;
        private TituloBO tituloBo;
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string _isEnvironment;
        public ImpresionDocumentosBO()
        {
            cargolicenciaBo = new CargoLicenciaBO();
            tituloBo = new TituloBO();
            sgdeaBo = new SgdeaBO();
            sgdeaRepository = new SGDEARepository();
            _isEnvironment = ConfigurationManager.AppSettings[Constantes.KEY_ENVIRONMENT];
        }
        /// <summary>
        /// Valida si la prevista ya ha sido generada, si ya fue generada devuelve el pdf que existe en el repositorio de archivos,
        /// de lo contrario crea el docuemento. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ImpresionDocumentoDTO> GetPrevistaLicencia(long id, string PathActual)
        {
            ImpresionDocumentoDTO impresionDocumentoDTO = new ImpresionDocumentoDTO();

            var data = await cargolicenciaBo.GetPlantillaLicencia(id);
            if (data is null)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se ha encontrado la licencia."));
            if (data.Radicado == 0)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se ha encontrado el radicado del tramite para generar la prevista."));

            var sgda = await sgdeaBo.GetPrevistaEstado(data.Radicado, Constantes.PREVISTAGENERADA, Constantes.TRAMITE_LICENCIA);
            var radicado = await sgdeaBo.GetPrevistaEstado(data.Radicado, Constantes.PREVISTATRAMITE, null);

            impresionDocumentoDTO.NombrePDF = data.NombreLicencia;
            impresionDocumentoDTO.Radicado = data.Radicado;
            impresionDocumentoDTO.Tramite = radicado.tipo_tramite;
            if (sgda != null)
            {
                string rutaServidor = ConfigurationManager.AppSettings["UrlSGDA"];
                impresionDocumentoDTO.Impreso = true;
                impresionDocumentoDTO.PdfBase64 = Reutilizables.DescargarArchivoServidor(rutaServidor + sgda.ruta_prevista);
                return impresionDocumentoDTO;
            }

            // plantilla del fromato html 
            string templateDirectory = "";
            switch (data.TipoLicencia.id_tipo_licencia)
            {
                case (int)TipoLicenciaEnum.NAVEGACIÓN:
                    templateDirectory = AppDomain.CurrentDomain.BaseDirectory + Constantes.PATH_PLANTILLA_LICENCIA_NAVEGACION;
                    break;
                case (int)TipoLicenciaEnum.PERITOS:
                    templateDirectory = AppDomain.CurrentDomain.BaseDirectory + Constantes.PLANTILLALICENCIAPERITO;
                    break;
                case (int)TipoLicenciaEnum.PILOTOS:
                    if (data.ActividadLicencia.id_actividad == (int)TipoActividadEnum.PERMISOPRACTICAJE)
                    {
                        templateDirectory = AppDomain.CurrentDomain.BaseDirectory + Constantes.PLANTILLALICENCIAPEP;
                        data.ParametroDinamico = Reutilizables.TablaHtmlVariables(data.Naves);

                        var licencias = (await new LicenciaRepository().GetlicenciasPorUsuarioId(data.IdGentemar)).Where(x => x.IdLicencia == data.IdLicenciaTituloPep && x.IdEstadoLicencia == (int)EstadosTituloLicenciaEnum.VIGENTE).Select(x => new { Id = x.IdLicencia, Nombre = x.CargoLicencia.CargoLicencia }).FirstOrDefault();

                        var titulo = (await new TituloRepository().GetTitulosFiltro("", data.IdGentemar)).Where(x => x.Id == data.IdLicenciaTituloPep && x.IdEstadoTramite == (int)EstadosTituloLicenciaEnum.VIGENTE)
                            .Select(x => new { Id = x.Id, Nombre = string.Join(",", x.Cargos.Select(y => y.CargoTitulo)) }).FirstOrDefault();

                        data.LicenciaTituloPep = licencias != null ? licencias.Nombre : titulo.Nombre;
                    }
                    else
                    {
                        var edad = Reutilizables.CalcularEdad(data.FechaNacimiento);
                        data.ParametroDinamico = edad >= 60 ? $"NOTA:<br />Debe certificar su aptitud física cada día de cumpleaños (Fecha de nacimiento {data.FechaNacimiento:dd/MM/yyyy})" : "";
                        templateDirectory = AppDomain.CurrentDomain.BaseDirectory + Constantes.PLANTILLALICENCIAPILOTO;
                    }
                    break;
                default:
                    throw new HttpStatusCodeException(Responses.SetConflictResponse("La licencia solicitada no tiene permitido generar prevista"));
            }
            try
            {
                // repmplaza las variables en el archivo html
                var HTML = Reutilizables.ReplaceVariables(Reutilizables.ReadFile(templateDirectory), data, "<br>");
                string pathLogoLicencia = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constantes.LOGO_LICENCIA);
                string logoLicenciaBase64 = null;
                var respuestaBuscarLogo = Reutilizables.DescargarArchivo($"{pathLogoLicencia}", out logoLicenciaBase64);
                if (respuestaBuscarLogo != null && respuestaBuscarLogo.Estado && !string.IsNullOrEmpty(logoLicenciaBase64))
                {
                    HTML = HTML.Replace("{ImageLogoBase64}", logoLicenciaBase64);
                }

                string FotoUserBase64 = null;
                var respuestaBuscarFoto = Reutilizables.DescargarArchivo($@"{PathActual}\{data.Foto}", out FotoUserBase64);
                if (respuestaBuscarFoto != null && respuestaBuscarFoto.Estado && !string.IsNullOrEmpty(FotoUserBase64))
                {
                    HTML = HTML.Replace("{FotoUserBase64}", FotoUserBase64);
                }

                var base64 = Reutilizables.GeneratePdftoBase64(HTML);
                // objeto con la informacion 
                impresionDocumentoDTO.Impreso = false;
                impresionDocumentoDTO.PdfBase64 = base64;

                GENTEMAR_OBSERVACIONES_LICENCIAS observacion = new GENTEMAR_OBSERVACIONES_LICENCIAS
                {
                    id_licencia = data.NumeroLicencia,
                    observacion = $"Se generó la prevista de la licencia con No de radicado: {data.Radicado}, pendiente de guardado."
                };
                await new ObservacionesBO().CrearObservacionesLicencias(observacion, string.Empty);
                return impresionDocumentoDTO;
            }
            catch (Exception ex)
            {
                throw new HttpStatusCodeException(Responses.SetInternalServerErrorResponse(ex, "Error al generar la prevista PDF."));
            }
        }


        /// <summary>
        /// Valida si la prevista ya ha sido generada, si ya fue generada devuelve el pdf que existe en el repositorio de archivos,
        /// de lo contrario crea el docuemento. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ImpresionDocumentoDTO> GetPrevistaTitulo(long id, string PathActual)
        {

            ImpresionDocumentoDTO impresionDocumentoDTO = new ImpresionDocumentoDTO();

            var data = await tituloBo.GetPlantillaTitulos(id);
            if (data is null)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se ha encontrado el título de navegación."));
            long RadicadoNumerico = 000000000000;

            if (Int64.TryParse(data.Radicado, out long Radicado))
                RadicadoNumerico = Radicado;

            else
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se ha encontrado el radicado del tramite para generar la prevista."));

            try
            {
                var sgda = await sgdeaBo.GetPrevistaEstado(RadicadoNumerico, Constantes.PREVISTAGENERADA, Constantes.TRAMITE_TITULOS);
                impresionDocumentoDTO.NombrePDF = data.Radicado;
                impresionDocumentoDTO.Radicado = RadicadoNumerico;
                impresionDocumentoDTO.Tramite = Constantes.TRAMITE_TITULOS;
                if (sgda != null)
                {
                    string rutaServidor = ConfigurationManager.AppSettings["UrlSGDA"];
                    impresionDocumentoDTO.Impreso = true;
                    impresionDocumentoDTO.PdfBase64 = Reutilizables.DescargarArchivoServidor(rutaServidor + sgda.ruta_prevista);
                    return impresionDocumentoDTO;
                }

                // plantilla del fromato html 
                string templateDirectoryTitulo = AppDomain.CurrentDomain.BaseDirectory + Constantes.PLANTILLATITULO;
                string templateDirectoryRefrendo = AppDomain.CurrentDomain.BaseDirectory + Constantes.PLANTILLAREFRENDO;

                data.TableFunciones = Reutilizables.TablaHtmlVariables(data.Funcion);
                data.TableCargos = Reutilizables.TablaHtmlVariables(data.Cargo);

                // repmplaza las variables en el archivo html
                var titulo = Reutilizables.ReplaceVariables(Reutilizables.ReadFile(templateDirectoryTitulo), data, " - ");
                var refrendo = Reutilizables.ReplaceVariables(Reutilizables.ReadFile(templateDirectoryRefrendo), data, " - ");

                string FotoUserBase64 = null;
                var respuestaBuscarArchivo = Reutilizables.DescargarArchivo($@"{PathActual}\{data.Foto}", out FotoUserBase64);
                if (respuestaBuscarArchivo != null && respuestaBuscarArchivo.Estado && !string.IsNullOrEmpty(FotoUserBase64))
                {
                    titulo = titulo.Replace("{FotoUserBase64}", FotoUserBase64);
                    refrendo = refrendo.Replace("{FotoUserBase64}", FotoUserBase64);
                }
                string pathLogoTitulo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constantes.LOGO_TITULO);
                string logoTituloBase64 = null;
                var respuestaBuscarLogo = Reutilizables.DescargarArchivo($"{pathLogoTitulo}", out logoTituloBase64);
                if (respuestaBuscarLogo != null && respuestaBuscarLogo.Estado && !string.IsNullOrEmpty(logoTituloBase64))
                {
                    titulo = titulo.Replace("{ImageLogoBase64}", logoTituloBase64);
                    refrendo = refrendo.Replace("{ImageLogoBase64}", logoTituloBase64);
                }

                var ArrayPdf = new string[]
                {
                    Reutilizables.GeneratePdftoBase64(titulo),
                    Reutilizables.GeneratePdftoBase64(refrendo),
                };
                // objeto con la informacion 
                impresionDocumentoDTO.Impreso = false;
                impresionDocumentoDTO.PdfBase64 = Reutilizables.CombinePdfListInBase64(ArrayPdf);
                GENTEMAR_OBSERVACIONES_TITULOS observacion = new GENTEMAR_OBSERVACIONES_TITULOS
                {
                    id_titulo = data.NumeroTitulo,
                    observacion = $"Se generó la prevista de el título de navegación con No de radicado: {data.Radicado}, pendiente de guardado."
                };
                await new ObservacionesBO().CrearObservacionesTitulos(observacion, string.Empty);
                return impresionDocumentoDTO;
            }
            catch (Exception ex)
            {
                throw new HttpStatusCodeException(Responses.SetInternalServerErrorResponse(ex, "Error al generar la prevista PDF"));
            }
        }
        /// <summary>
        /// Guarda el archivo generado y aprobado por el usuario en el repositorio del sgda
        /// </summary>
        /// <param name="impresionDocumento"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusCodeException"></exception>
        public async Task<Respuesta> SavePrevista(ImpresionDocumentoDTO impresionDocumento)
        {

            var lisPrevista = new List<SGDEA_PREVISTAS>();
            // obtiene el array de bits del archivo en base64
            var file = Reutilizables.GenerateBase64toBytes(impresionDocumento.PdfBase64);
            //Construye la ur del repo de archivos
            var previstaTramite = await sgdeaBo.GetPrevistaEstado(Convert.ToDecimal(impresionDocumento.Radicado),
                Constantes.PREVISTATRAMITE, impresionDocumento.Tramite);

            if (previstaTramite is null)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se ha encontrado el radicado del trámite para generar la prevista."));

            try
            {
                var urlFile = $"/{previstaTramite.tipo_tramite}/{impresionDocumento.Radicado}.pdf";
                // guarda el archivo en el repo de sgda 
                File.WriteAllBytes($"{ConfigurationManager.AppSettings["UrlSGDA"]}{urlFile}", file);
                var previstaGenerada = (SGDEA_PREVISTAS)previstaTramite.Clone();
                previstaGenerada.estado = Constantes.PREVISTAGENERADA;
                previstaGenerada.ruta_prevista = urlFile;
                lisPrevista.Add(previstaGenerada);
                var previstaImpreso = (SGDEA_PREVISTAS)previstaTramite.Clone();
                previstaImpreso.estado = Constantes.PREVISTAIMPRESA;
                previstaImpreso.ruta_prevista = urlFile;
                lisPrevista.Add(previstaImpreso);
                // gurda los registros en la tabla previstas.
                await sgdeaRepository.CreateSgdaPrevista(lisPrevista);
                await EnviarNotificacion(previstaTramite);
                await GuardarObservacion(previstaTramite.tipo_tramite, previstaTramite.radicado);
                return Responses.SetOkResponse(null, "Se ha guardado la prevista correctamente.");
            }
            catch (Exception ex)
            {
                throw new HttpStatusCodeException(Responses.SetInternalServerErrorResponse(ex, "Error al guardar el archivo de la prevista."));
            }
        }

        private async Task GuardarObservacion(string tramite, decimal radicado)
        {
            if (tramite.Contains(Constantes.TRAMITE_TITULOS))
            {
                var idTitulo = await new TituloRepository().GetIdTitulo(radicado.ToString());
                if (idTitulo > 0)
                {
                    GENTEMAR_OBSERVACIONES_TITULOS observacion = new GENTEMAR_OBSERVACIONES_TITULOS
                    {
                        id_titulo = idTitulo,
                        observacion = $"Se ha guardado la prevista de el título de navegación con No de radicado: {radicado} en el SGDEA."
                    };
                    await new ObservacionesBO().CrearObservacionesTitulos(observacion, string.Empty);

                }
            }
            else if (tramite.Contains(Constantes.TRAMITE_LICENCIA)
                || tramite.Contains(Constantes.TRAMITE_PERMISO_ESPECIAL_PRACTICAJE))
            {
                var idLicencia = await new LicenciaRepository().GetIdLicencia(radicado);
                if (idLicencia > 0)
                {
                    GENTEMAR_OBSERVACIONES_LICENCIAS observacion = new GENTEMAR_OBSERVACIONES_LICENCIAS
                    {
                        id_licencia = idLicencia,
                        observacion = $"Se ha guardado la prevista de la licencia con No de radicado: {radicado} en el SGDEA."
                    };
                    await new ObservacionesBO().CrearObservacionesLicencias(observacion, string.Empty);
                }
            }
        }

        private async Task EnviarNotificacion(SGDEA_PREVISTAS previstaTramite)
        {
            int enviroment = Convert.ToInt32(_isEnvironment);

            if ((int)EnvironmentEnum.Production != enviroment)
                return;

            String[] emails = await new UsuarioRepository().GetEmailsAdministradores();
            string emailLogeado = ClaimsHelper.GetEmail();
            var nombres = await new DatosBasicosRepository().GetNombrePorIdentificacion(previstaTramite.numero_identificacion_usuario);
            SendEmailRequest request = new SendEmailRequest
            {
                Titulo = $"Prevista generada {previstaTramite.tipo_tramite}",
                CorreosDestino = emails,
                Asunto = $"Prevista generada {previstaTramite.tipo_tramite} radicado: {previstaTramite.radicado}",
                CC = !string.IsNullOrEmpty(emailLogeado) && !emails.Contains(emailLogeado) ? emailLogeado : string.Empty,
                CuerpoDelMensaje = $"Se generó la prevista con trámite {previstaTramite.tipo_tramite} y radicado: " +
                                    $"{previstaTramite.radicado} para el usuario {nombres}" +
                                    $" con {previstaTramite.tipo_documento_usuario} {previstaTramite.numero_identificacion_usuario}",
                Footer = Constantes.FOOTER_EMAIL
            };
            await new EnvioNotificacionesHelper().SendNotificationByEmail(request);
        }
    }
}
