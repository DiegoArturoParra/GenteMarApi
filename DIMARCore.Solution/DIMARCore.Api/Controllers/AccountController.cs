using DIMARCore.Api.Core;
using DIMARCore.Api.Core.Models;
using DIMARCore.Business;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.Requests;
using DIMARCore.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers
{
    /// <summary>
    /// Api Autenticación
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/account")]
    public class AccountController : BaseApiController
    {
        private readonly SeguridadBO _seguridadService;

        /// <summary>
        /// Constructor
        /// </summary>
        public AccountController()
        {
            _seguridadService = new SeguridadBO();
        }

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
        [ResponseType(typeof(ResponseTypeSwagger<UserTokenDTO>))]
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Login([FromBody] LoginRequest login)
        {
            Respuesta respuesta;
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            _logger.Info("Inicio metodo login...");
            // Se valida que el id de la aplicación sea valido
            await new AplicacionBO().GetAplicacion(login.Aplicacion);
            // Se busca y valida el usuario
            respuesta = await _seguridadService.ValidarUsuario(login.UserName, login.Password);
            if (respuesta.Estado)
            {
                var user = (UserSesionDTO)respuesta.Data;
                // se obtiene el token
                _logger.Info("Genera token del usuario...");
                String token = TokenGenerator.GenerarTokenJwt(user);
                respuesta = _seguridadService.ResultadoAutenticacion(user, token);

            }
            _logger.Info($"{nameof(AccountController)} Login Tiempo de duración: {stopwatch.Elapsed.TotalSeconds}");
            stopwatch.Stop();
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
        [ResponseType(typeof(ResponseTypeSwagger<UserTokenDTO>))]
        [HttpPost]
        [Route("login-test")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> LoginTest([FromBody] LoginRequest login)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            Respuesta respuesta;
            _logger.Info("Inicio metodo login...");
            // Se valida que el id de la aplicación sea valido
            await new AplicacionBO().GetAplicacion(login.Aplicacion);
            // Se busca y valida el usuario
            respuesta = await _seguridadService.ValidarUsuarioTest(login.UserName, login.Password);
            if (respuesta.Estado)
            {
                var user = (UserSesionDTO)respuesta.Data;
                // se obtiene el token
                _logger.Info("Genera token del usuario...");
                String token = TokenGenerator.GenerarTokenJwt(user);
                respuesta = _seguridadService.ResultadoAutenticacion(user, token);
            }
            _logger.Info($"{nameof(AccountController)} login-test Tiempo de duración: {stopwatch.Elapsed.TotalSeconds}");
            stopwatch.Stop();
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
        [ResponseType(typeof(List<int>))]
        [HttpGet]
        [Route("roles")]
        [Authorize]
        public async Task<IHttpActionResult> GetRolesByAutenticacion()
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            _logger.Info("Inicio metodo login...");
            string loginName = GetLoginName();
            List<int> roles = await new UsuarioBO().GetRolesByLoginName(loginName);
            _logger.Info($"{nameof(AccountController)} GetRolesByAutenticacion Tiempo de duración: {stopwatch.Elapsed.TotalSeconds}");
            stopwatch.Stop();
            return Ok(roles);
        }
    }
}