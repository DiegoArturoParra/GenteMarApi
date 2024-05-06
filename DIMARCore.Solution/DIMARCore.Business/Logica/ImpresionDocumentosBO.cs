using DIMARCore.Business.Helpers;
using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Config;
using DIMARCore.Utilities.CorreoSMTP;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class ImpresionDocumentosBO
    {
        private CargoLicenciaBO cargolicenciaBo;
        private SgdeaBO sgdeaBo;
        private SGDEARepository sgdeaRepository;
        private TituloBO tituloBo;

        public ImpresionDocumentosBO()
        {
            cargolicenciaBo = new CargoLicenciaBO();
            tituloBo = new TituloBO();
            sgdeaBo = new SgdeaBO();
            sgdeaRepository = new SGDEARepository();
        }
        /// <summary>
        /// Valida si la prevista ya ha sido generada, si ya fue generada devuelve el pdf que existe en el repositorio de archivos,
        /// de lo contrario crea el docuemento. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ImpresionDocumentoDTO> GetPrevistaLicencias(long id)
        {
            ImpresionDocumentoDTO impresionDocumentoDTO = new ImpresionDocumentoDTO();

            var data = await cargolicenciaBo.GetPlantillaLicencias(id);
            if (data is null)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se ha encontrado la licencia."));
            if (data.Radicado == 0)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se ha encontrado el radicado del tramite para generar la prevista."));

            var sgda = await sgdeaBo.GetPrevistaEstado(data.Radicado, Constantes.PREVISTAGENERADA, Constantes.TRAMITE_LICENCIA);

            impresionDocumentoDTO.NombrePDF = data.NombreLicencia;
            impresionDocumentoDTO.Radicado = data.Radicado;
            impresionDocumentoDTO.Tramite = Constantes.TRAMITE_LICENCIA;
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
                    }
                    else
                    {
                        var edad = Reutilizables.CalcularEdad(data.FechaNacimiento);
                        data.ParametroDinamico = edad >= 60 ? $"NOTA:<br />Debe certificar su aptitud física cada día de cumpleaños (Fecha de nacimiento {data.FechaNacimiento.ToString("dd/MM/yyyy")})" : "";
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
                HTML = HTML.Replace("{url}", $"{ConfigurationManager.AppSettings["WebSite_API"]}");
                var base64 = Reutilizables.GeneratePdftoBase64(HTML);
                // objeto con la informacion 
                impresionDocumentoDTO.Impreso = false;
                impresionDocumentoDTO.PdfBase64 = base64;
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
        public async Task<ImpresionDocumentoDTO> GetPrevistaTitulo(long id)
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
                titulo = titulo.Replace("{url}", $"{ConfigurationManager.AppSettings["WebSite_API"]}");

                var refrendo = Reutilizables.ReplaceVariables(Reutilizables.ReadFile(templateDirectoryRefrendo), data, " - ");
                refrendo = refrendo.Replace("{url}", $"{ConfigurationManager.AppSettings["WebSite_API"]}");

                var ArrayPdf = new string[]
                {
                    Reutilizables.GeneratePdftoBase64(titulo),
                    Reutilizables.GeneratePdftoBase64(refrendo),
                };
                var mergepdf = Reutilizables.CombinePdfListInBase64(ArrayPdf);
                // objeto con la informacion 
                impresionDocumentoDTO.Impreso = false;
                impresionDocumentoDTO.PdfBase64 = mergepdf;
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
            // obtieneel array de bits del archivo en base64
            var file = Reutilizables.GenerateBase64toBytes(impresionDocumento.PdfBase64);
            //Construye la ur del repo de archivos
            var previstaTramite = await sgdeaBo.GetPrevistaEstado(Convert.ToDecimal(impresionDocumento.Radicado), Constantes.PREVISTATRAMITE, impresionDocumento.Tramite);
            if (previstaTramite is null)
            {
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se ha encontrado el radicado del trámite para generar la prevista."));
            }
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

                return Responses.SetOkResponse(null, "Se ha guardado la prevista correctamente.");
            }
            catch (Exception ex)
            {
                throw new HttpStatusCodeException(Responses.SetInternalServerErrorResponse(ex, "Error al guardar el archivo de la prevista."));
            }
        }
    }
}
