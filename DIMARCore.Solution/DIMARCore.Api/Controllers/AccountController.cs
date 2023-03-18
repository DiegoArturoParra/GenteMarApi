using DIMARCore.Api.Core;
using DIMARCore.Business;
using DIMARCore.UIEntities.Models;
using DIMARCore.UIEntities.Requests;
using DIMARCore.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DIMARCore.Api.Controllers
{
    /// <summary>
    /// Api Autenticación
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/account")]
    public class AutenticacionController : BaseApiController
    {

        /// <summary>
        /// Servicio  Autenticación por medio del active directory
        /// </summary>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>01/07/2022</Fecha>
        /// <param name="login"></param>
        /// <returns></returns>
        /// <response code="401">Unauthorized. La combinación usuario/contraseña es incorrecta..</response>   
        /// <response code="404">Not found. Aplicación de gente mar no encontrada.</response>   
        /// <response code="200">OK. Devuelve la información del usuario con el token.</response>           
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Login([FromBody] LoginRequest login)
        {
            Respuesta respuesta;
            _logger.Info("Inicio metodo login...");
            // Se valida que el id de la aplicación sea valido
            respuesta = await new AplicacionBO().GetAplicacion(login.Aplicacion);
            if (respuesta.Estado)
            {
                // limpia los datos
                login.Username = login.Username.Trim();
                login.Password = login.Password.Trim();
                // Se busca y valida el usuario
                respuesta = await new SeguridadBO().ValidarUsuario(login.Username, login.Password);
                if (respuesta.Estado)
                {
                    respuesta = ResultadoAutenticacion((UserSesion)respuesta.Data);
                }
            }
            _logger.Info("Genera token del usuario...");
            return ResultadoStatus(respuesta);
        }

        /// <summary>
        /// Servicio  Autenticación Testing (capitanias)
        /// </summary>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>01/07/2022</Fecha>
        /// <param name="login"></param>
        /// <returns></returns>
        /// <response code="401">Unauthorized. La combinación usuario/contraseña es incorrecta..</response>   
        /// <response code="404">Not found. Aplicación de gente mar no encontrada.</response>   
        /// <response code="200">OK. Devuelve la información del usuario con el token.</response>           
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [HttpPost]
        [Route("login-test")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> LoginTest([FromBody] LoginRequest login)
        {
            //String[] correos = { "dparramol@dimar.mil.co", "gbuitrago@dimar.mil.co" };
            //await new EMailService().SendMail(correos, "envio", "diego", "GDM", Constantes.FOOTER_EMAIL);
            Respuesta respuesta;
            _logger.Info("Inicio metodo login...");
            // Se valida que el id de la aplicación sea valido
            respuesta = await new AplicacionBO().GetAplicacion(login.Aplicacion);
            if (respuesta.Estado)
            {
                // limpia los datos
                login.Username = login.Username.Trim();
                login.Password = login.Password.Trim();
                // Se busca y valida el usuario
                respuesta = await new SeguridadBO().ValidarUsuarioTest(login.Username, login.Password);
                if (respuesta.Estado)
                {
                    respuesta = ResultadoAutenticacion((UserSesion)respuesta.Data);
                }
            }
            _logger.Info("Genera token del usuario...");
            return ResultadoStatus(respuesta);
        }

        /// <summary>
        /// Servicio para obtener roles por autenticacion
        /// </summary>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>01/07/2022</Fecha>
        /// <returns></returns>
        /// <response code="401">Unauthorized. no hay autenticación.</response>   
        /// <response code="200">OK. Devuelve los roles.</response>           
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [HttpGet]
        [Route("roles")]
        [Authorize]
        public async Task<IHttpActionResult> GetRolesByAutenticacion()
        {
            string loginName = GetLoginName();
            return Ok(await new UsuarioBO().GetRoles(loginName));
        }

        /// <returns></returns>
        private static Respuesta ResultadoAutenticacion(UserSesion userSession)
        {
            // se obtiene el token
            String token = TokenGenerator.GenerarTokenJwt(userSession);
            List<string> roles = new List<string>();

            foreach (var item in userSession.Roles)
            {
                roles.Add(item.NombreRol);
            }

            Respuesta respuesta = new Respuesta
            {
                Estado = true,
                StatusCode = HttpStatusCode.OK,
                Mensaje = ConstantesBO.OK,
                Data = new
                {
                    Token = token,
                    LoginName = userSession.LoginName,
                    NombreCompleto = userSession.NombreCompletoUsuario,
                    Capitania = $"{userSession.Capitania.Sigla} {userSession.Capitania.Descripcion}",
                    Roles = roles,
                    Aplicacion = userSession.Aplicacion.Nombre
                }
            };
            return respuesta;
        }
    }
}