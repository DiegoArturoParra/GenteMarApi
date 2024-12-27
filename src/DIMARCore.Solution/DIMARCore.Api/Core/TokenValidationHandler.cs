using DIMARCore.Business.Helpers;
using DIMARCore.Business.Logica;
using DIMARCore.Utilities.Config;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using log4net;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace DIMARCore.Api.Core
{
    /// <summary>
    /// Validador de token para solicitud de autorización utilizando un DelegatingHandler
    /// </summary>
    internal class TokenValidationHandler : DelegatingHandler
    {
        private static ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Validación del token en el encabezado de la solicitud
        /// </summary>
        /// <param name="request">Solicitud</param>
        /// <param name="token">Token</param>
        /// <returns>Retorna si se recibio el token</returns>
        private static bool TryRetrieveToken(HttpRequestMessage request, out string token)
        {
            token = null;
            IEnumerable<string> authzHeaders;
            if (!request.Headers.TryGetValues("Authorization", out authzHeaders) || authzHeaders.Count() > 1)
            {
                return false;
            }
            var bearerToken = authzHeaders.ElementAt(0);
            token = bearerToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) ? bearerToken.Substring(7) : bearerToken;
            return true;
        }

        /// <summary>
        /// Realiza la validación del token al recibir una solicitud
        /// </summary>
        /// <param name="request">Solicitud</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpStatusCode statusCode;
            string token;
            bool existeToken = false;
            bool tokenActivo = true;

            // TODO: Debe ser del tipo de entidad de la tabla donde se guardaran los tokens
            var registroTokenAutenticacion = new GENTEMAR_AUTENTICACION();

            // determinar si existe un jwt o no
            if (!TryRetrieveToken(request, out token))
            {
                //statusCode = HttpStatusCode.Unauthorized;
                return base.SendAsync(request, cancellationToken);
            }

            try
            {
                var secretKey = ConfigurationManager.AppSettings[AutenticacionConfig.TOKEN_JWT_SECRET_KEY];
                var audienceToken = ConfigurationManager.AppSettings[AutenticacionConfig.TOKEN_SITE_URL];
                var issuerToken = ConfigurationManager.AppSettings[AutenticacionConfig.TOKEN_SITE_URL];
                var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));

                SecurityToken securityToken;
                var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = audienceToken,
                    ValidIssuer = issuerToken,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    LifetimeValidator = this.LifetimeValidator,
                    IssuerSigningKey = securityKey
                };

                // se busca el token en el sistema - base de datos
                // TODO: Adicionar validación TOKEN contra base de datos
                existeToken = true;
                tokenActivo = true;
                // EJEMPLO
                registroTokenAutenticacion = new AutenticacionBO().GetToken(token);
                existeToken = registroTokenAutenticacion != null ? true : false;
                if (existeToken)
                {
                    tokenActivo = registroTokenAutenticacion.Estado == (int)EstadoTokenEnum.Activo;
                    if (tokenActivo)
                    {
                        // se valida si el token ha expirado
                        if (registroTokenAutenticacion.IsExpired)
                        {
                            tokenActivo = false;
                        }
                    }
                }
                else
                {
                    tokenActivo = false;
                }

                // Extrae y asigna el CurrentPrincipal y el User - Extract and assign Current Principal and user
                Thread.CurrentPrincipal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);
                HttpContext.Current.User = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                // Se valida el token de la base de datos
                if (!existeToken)
                {
                    // el token es valido pero no existe en la base de datos
                    throw new SecurityTokenValidationException("El token es valido pero no existe en la base de datos.");
                }
                else if (!tokenActivo)
                {
                    // el token es valido pero no se encuentra activo
                    throw new SecurityTokenValidationException("El token es valido pero no se encuentra activo en la base de datos.");
                }

                // Si llego hasta aqui el token es valido - la autenticación es valida
                return base.SendAsync(request, cancellationToken);
            }
            catch (SecurityTokenValidationException ex)
            {
                // excepción al validar el CurrentPrincipal o el user
                // token no valido
                _logger.Warn(ex);
                statusCode = HttpStatusCode.Unauthorized;
                var response = new Respuesta
                {
                    StatusCode = statusCode,
                    Mensaje = "El usuario no esta autorizado",
                    MensajeIngles = "The user is not authorized",
                };

                string json = JsonConvert.SerializeObject(response);
                _ = new DbLoggerHelper().InsertLogToDatabase(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                // excepción al validar el CurrentPrincipal o el user
                // token no valiado
                statusCode = HttpStatusCode.InternalServerError;
                var response = new Respuesta
                {
                    StatusCode = statusCode,
                    MensajeExcepcion = ex.Message,
                    MensajeIngles = ex.Message,
                    Data = ex.StackTrace
                };

                string json = JsonConvert.SerializeObject(response);
                _ = new DbLoggerHelper().InsertLogToDatabase(response);

            }

            if (statusCode == HttpStatusCode.Unauthorized && existeToken && tokenActivo)
            {
                // si el token no esta autorizado, 
                // existe el token en la base de datos 
                // y el token esta activo
                try
                {
                    // se desactiva el token en la base de datos
                    // TODO: Desactivar el token en la base de datos, cambiar estado
                    // EJEMPLO
                    _ = new AutenticacionBO().DesactivarToken(registroTokenAutenticacion.Token, AutenticacionConfig.COMENTARIO_FINALIZACION_VALIDACION_SISTEMA);

                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    var response = new Respuesta
                    {
                        StatusCode = statusCode,
                        MensajeExcepcion = ex.Message,
                        MensajeIngles = ex.Message,
                        Data = ex.StackTrace
                    };

                    string json = JsonConvert.SerializeObject(response);
                    _ = new DbLoggerHelper().InsertLogToDatabase(response);
                    // throw;
                    // no se hace nada - se continua
                }
            }

            #region solución cors al retornar 401 0 500
            if (statusCode == HttpStatusCode.Unauthorized || statusCode == HttpStatusCode.InternalServerError)
            {
                var respuesta = new Respuesta
                {
                    StatusCode = statusCode,
                    Mensaje = "El usuario no esta autorizado",
                    MensajeIngles = "The user is not authorized",
                };
                string json = JsonConvert.SerializeObject(respuesta);
                var response = new HttpResponseMessage(statusCode);
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var tsc = new TaskCompletionSource<HttpResponseMessage>();
                tsc.SetResult(response);
                return tsc.Task;
                //}
            }
            #endregion

            // se retorna el resultado
            return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode) { });

        }

        /// <summary>
        /// Valida el tiempo del token
        /// </summary>
        /// <param name="notBefore"></param>
        /// <param name="expires"></param>
        /// <param name="securityToken"></param>
        /// <param name="validationParameters"></param>
        /// <returns></returns>
        public bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                if (DateTime.UtcNow < expires) return true;
            }
            return false;
        }
    }
}