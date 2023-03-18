/***
Autor: Carlos Rodríguez
Fecha: 2021/09/06
Descripción: Esta clase tiene los servicios web api de autenticación.
***/
using DIMARCore.Api.Core;
using DIMARCore.Business;
using DIMARCore.UIEntities.Requests;
using DIMARCore.Utilities.Config;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DIMARCore.Api.Controllers
{
    /// <summary>
    /// Api Autenticación
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/autenticacion")]
    public class AutenticacionController : BaseApiController
    {
        log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Salir - logout - Cierra/Desactiva token
        /// </summary>
        /// <returns></returns>
        /// <Autor>Carlos Rodríguez</Autor>
        /// <Fecha>2021/09/06</Fecha>
        /// <UltimaActualizacion>2021/09/06 - Carlos Rodríguez - Creación del servicio</UltimaActualizacion>        
        [HttpPost]
        [Route("salir")]
        public IHttpActionResult Logout()
        {
            var headers = HttpContext.Current.Request.Headers;
            if (headers != null && headers.AllKeys.Contains(AutenticacionConfig.HEADERS_AUTHORIZATION))
            {
                string token = "";
                string bearerToken = headers.GetValues(AutenticacionConfig.HEADERS_AUTHORIZATION).FirstOrDefault();
                // bearer .....
                if (!String.IsNullOrEmpty(bearerToken))
                {
                    token = bearerToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) ? bearerToken.Substring(7) : bearerToken;

                    // Busca en el encabezado el id de la aplicación
                    if (headers.AllKeys.Contains(AutenticacionConfig.HEADERS_ID_APLICACION))
                    {
                        string sIdAplicacionHeader = headers.GetValues(AutenticacionConfig.HEADERS_ID_APLICACION).FirstOrDefault();
                    }

                    int idAplicacion;
                    var sIdAplicacion = GetIdAplicacion();

                    var convertir = int.TryParse(sIdAplicacion, out idAplicacion);
                    // inactivar token
                    // TODO: cambiar estado del token en la base de datos
                    // EJEMPLO var resultado = (new StmAutenticacionBO().Salir(token, idAplicacion));

                }
            }

            return Ok();
        }


        /// <summary>
        /// Autenticación usuario - retorna token y datos del usuario
        /// </summary>
        /// <param name="login">Datos para realizar la autenticación</param>
        /// <returns>Token y datos del usuario</returns>
        /// <returns>Información de autenticación</returns>
        /// <Autor>Carlos Rodríguez</Autor>
        /// <Fecha>2021/09/06</Fecha>
        /// <UltimaActualizacion>2021/09/06 - Carlos Rodríguez - Cración servicio</UltimaActualizacion>
        [HttpPost]
        [Route("autenticar")]
        [AllowAnonymous]
        public IHttpActionResult Login([FromBody] LoginRequest login)
        {
            //log.Info("Inicio metodo login...");
            Respuesta respuesta = new Respuesta();

            try
            {
                var sTiempoExpiracion = System.Configuration.ConfigurationManager.AppSettings["JWT_EXPIRE_MINUTES"];
                int tiempoExpiracion = Convert.ToInt32(sTiempoExpiracion); // en minutos

                if (!ModelState.IsValid)
                {
                    string error = "Usuario o clave incorrecta. Intente de nuevo.";
                    return BadRequest(error);
                }

                // Se valida que el id de la aplicación sea valido
                if (!(new AplicacionBO()).ValidarLoginIdTipoAplicacion(login.Aplicacion))
                {
                    string error = "Origen invalido.";
                    return BadRequest(error);
                }

                // limpia los datos
                login.Username = string.IsNullOrEmpty(login.Username) ? "" : login.Username.Trim();
                login.Password = string.IsNullOrEmpty(login.Password) ? "" : login.Password.Trim();

                // Se busca y valida el usuario
                var (resultadoValidacion, usuario, mensajeValidacion) = (new SeguridadBO()).ValidarUsuario(login.Username, login.Password);
                if (!resultadoValidacion)
                {
                    if (string.IsNullOrEmpty(mensajeValidacion))
                    {
                        mensajeValidacion = "Error al validar el usuario. Por favor contáctese con el Administrador del Sistema.";
                    }
                    respuesta = new Respuesta
                    {
                        Mensaje = !String.IsNullOrEmpty(mensajeValidacion) ? mensajeValidacion :
                            "Usuario o datos incorrectos. Por favor intente de nuevo.",
                        Data = null
                    };
                    var mensajeNoAutorizado = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = respuesta.Mensaje };
                    // throw new HttpResponseException(mensajeNoAutorizado);
                    return ResponseMessage(mensajeNoAutorizado);
                }

                if (usuario == null)
                {
                    string error = "Usuario o clave incorrecta. Intente de nuevo.";
                    return BadRequest(error);
                }
                else
                {
                    //log.Info("Fin método login...");
                    respuesta = ResultadoAutenticacion(login.Aplicacion, usuario, null);
                    return Ok(respuesta);

                }

            }
            catch (Exception ex)
            {
                log.Fatal("Error al iniciar sesión", ex);
                var error = $"Se presentó un problema manejando su solicitud. Error: {ex.Message}";
                var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(error, System.Text.Encoding.UTF8, "text/plain")
                };
                return ResponseMessage(httpResponseMessage);
            }
        }

        /// <summary>
        /// Arma el resultado de la autenticación del usuario
        /// </summary>
        /// <param name="tipoAplicacion">Id tipo aplicación</param>
        /// <param name="idAplicacion">Id aplicación en el sistema, si la autenticación se realiza en otra aplicación</param>
        /// <param name="usuario">Datos del usuario</param>
        /// <returns></returns>
        private static Respuesta ResultadoAutenticacion(int tipoAplicacion, APLICACIONES_LOGINS usuario, int? idAplicacion)
        {
            Respuesta respuesta;
            var sTiempoExpiracion = System.Configuration.ConfigurationManager.AppSettings["JWT_EXPIRE_MINUTES"];
            int tiempoExpiracion = Convert.ToInt32(sTiempoExpiracion); // en minutos
            // se obtiene la ip del request
            var hostname = HttpContext.Current.Request.UserHostAddress;
            // se obtiene el token
            String token = TokenGenerator.GenerarTokenJwt(usuario, tipoAplicacion);

            DateTime fechaActual = DateTime.Now;
            // TODO: Armar objeto autenticación a guardar en la base de datos para validar el TOKEN
            // EJEMPLO:
            ////StmAutenticacion oStmAutenticacion = new StmAutenticacion();
            ////oStmAutenticacion.Aplicacion = tipoAplicacion;
            ////oStmAutenticacion.IdUsuario = usuario.idUsuario;
            ////oStmAutenticacion.Token = token;
            ////oStmAutenticacion.Ip = string.IsNullOrEmpty(hostname) ? "::1" : hostname;
            ////oStmAutenticacion.Mac = ""; // En web se deja vacio, por seguridad del usuario no se deberia tomar, TODO: crodriguez, quitar
            ////oStmAutenticacion.FechaHoraInicioSesion = fechaActual;
            ////oStmAutenticacion.FechaHoraFinSesion = null;
            ////oStmAutenticacion.Estado = ConstantesBO.STM_AUTENTICACION_ESTADO_ACTIVO;
            ////oStmAutenticacion.FechaExpiracion = fechaActual.AddMinutes(tiempoExpiracion);
            ////oStmAutenticacion.FechaUltimaActualizacion = fechaActual;
            ////oStmAutenticacion.Comentario = "";
            ////oStmAutenticacion.IdAplicacion = idAplicacion;
            //// guardar token en el sistema, se recibe la tupla
            ////var autenticacion = (new StmAutenticacionBO()).Crear(oStmAutenticacion);
            ////oStmAutenticacion.Id = autenticacion.Id;

            respuesta = new Respuesta
            {
                Estado = true,
                StatusCode = HttpStatusCode.OK,
                Mensaje = DIMARCore.Business.ConstantesBO.OK,
                Data = new
                {
                    Token = token,
                    Aplicacion = tipoAplicacion,
                    Usuario = new
                    {
                        Id = usuario.ID_LOGIN,
                        Login = usuario.LOGIN_NAME,
                        Nombres = usuario.NOMBRES,
                        Apellidos = usuario.APELLIDOS,
                        NombreCompleto = usuario.NombreCompleto,
                    }
                }
            };
            return respuesta;
        }


        /// <summary>
        /// Autenticación usuario por aplicación - retorna token y datos del usuario
        /// </summary>
        /// <param name="loginApp">Datos para realizar la autenticación</param>
        /// <returns>Token y datos del usuario</returns>
        /// <returns>Información de autenticación</returns>
        /// <Autor>Carlos Rodríguez</Autor>
        /// <Fecha>2021/09/06</Fecha>
        /// <UltimaActualizacion>2021/09/06 - Carlos Rodríguez - Creación Servicio</UltimaActualizacion>
        [HttpPost]
        [Route("autenticar-app")]
        [AllowAnonymous]
        public IHttpActionResult LoginApp([FromBody] LoginAppRequest loginApp)
        {
            //log.Info("Inicio metodo login...");
            Respuesta respuesta = new Respuesta();

            try
            {
                if (!ModelState.IsValid)
                {
                    string error = "Datos invalidos. Intente de nuevo.";
                    foreach (var state in ModelState)
                    {
                        foreach (var item in state.Value.Errors)
                        {
                            error += $" {item.ErrorMessage}";
                        }

                    }
                    return BadRequest(error);
                }

                // Se valida que el id de la aplicación sea valido
                if (!(new AplicacionBO()).ValidarLoginIdTipoAplicacion(loginApp.TipoAplicacion))
                {
                    string error = "Origen invalido.";
                    return BadRequest(error);
                }

                // Se busca y valida el usuario
                var (resultadoValidacion, usuario, mensajeValidacion, idAplicacion) = (new SeguridadBO()).ValidarUsuarioApp(loginApp);
                if (!resultadoValidacion)
                {
                    if (string.IsNullOrEmpty(mensajeValidacion))
                    {
                        mensajeValidacion = "Error al validar el usuario. Por favor contáctese con el Administrador del Sistema.";
                    }
                    respuesta = new Respuesta
                    {
                        Mensaje = !String.IsNullOrEmpty(mensajeValidacion) ? mensajeValidacion :
                            "Usuario o datos incorrectos. Por favor intente de nuevo.",
                        Data = null
                    };
                    var mensajeNoAutorizado = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = respuesta.Mensaje };
                    return ResponseMessage(mensajeNoAutorizado);
                }

                if (usuario == null)
                {
                    string error = "Usuario o datos invalidos. Intente de nuevo.";
                    return BadRequest(error);
                }
                else
                {
                    //log.Info("Fin método login...");
                    respuesta = ResultadoAutenticacion(loginApp.TipoAplicacion, usuario, idAplicacion);
                    return Ok(respuesta);

                    //respuesta = new Respuesta
                    //{
                    //    Resultado = DIMARCore.Business.ConstantesBO.RESULTADO_ERROR,
                    //    Mensaje = "Usuario o datos incorrectos. Por favor intente de nuevo.",
                    //    Data = null
                    //};

                    //var mensajeNoAutorizado = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = respuesta.Mensaje };
                    //// throw new HttpResponseException(mensajeNoAutorizado);
                    //return ResponseMessage(mensajeNoAutorizado);

                }

            }
            catch (Exception ex)
            {
                log.Fatal("Error al iniciar sesión", ex);
                var error = $"Se presentó un problema manejando su solicitud. Error: {ex.Message}";
                var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(error, System.Text.Encoding.UTF8, "text/plain")
                };
                return ResponseMessage(httpResponseMessage);
            }
        }



    }
}