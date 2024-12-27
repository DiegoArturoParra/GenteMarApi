using DIMARCore.Api.Core;
using DIMARCore.Api.Core.Models;
using DIMARCore.Business;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.Requests;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Seguridad;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers
{
    /// <summary>
    /// Servicios de Autenticación
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/account")]
    public class AccountController : BaseApiController
    {
        private readonly AutenticacionBO _autenticacionService;

        /// <summary>
        /// Constructor
        /// </summary>
        public AccountController()
        {
            _autenticacionService = new AutenticacionBO();
        }

        /// <summary>
        /// Servicio  Autenticación por medio del active directory
        /// </summary>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>01/07/2022</Fecha>
        /// <param name="request"> parametro body encriptado</param>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información del usuario con el token.</response>       
        /// <response code="401">Unauthorized. La combinación usuario/contraseña es incorrecta.</response>   
        /// <response code="404">Not found. Aplicación de gente mar no encontrada.</response>   
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(ResponseTypeSwagger<UserTokenDTO>))]
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Login([FromBody] BodyRequest request)
        {
            var bodyDecrpt = new EncryptDecryptRequestService().DecryptString(request.Body);
            LoginRequest login = JsonConvert.DeserializeObject<LoginRequest>(bodyDecrpt);
            var user = await _autenticacionService.Login(login);
            // se obtiene el token
            string token = TokenGenerator.GenerarTokenJwt(user);
            // Se crea el registro de autenticación
            var response = await _autenticacionService.RegistroDeAutenticacion(user, token, login.IpAddress);
            return Ok(response);
        }

        /// <summary>
        /// Servicio Autenticación Testing
        /// </summary>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>01/07/2022</Fecha>
        /// <param name="login"></param>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información del usuario con el token.</response>           
        /// <response code="401">Unauthorized. La combinación usuario/contraseña es incorrecta.</response>   
        /// <response code="404">Not found. Aplicación de gente mar no encontrada.</response>   
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(ResponseTypeSwagger<UserTokenDTO>))]
        [HttpPost]
        [Route("login-test")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> LoginTest([FromBody] LoginRequest login)
        {
            var user = await _autenticacionService.LoginTest(login);
            // se obtiene el token
            string token = TokenGenerator.GenerarTokenJwt(user);
            // Se crea el registro de autenticación
            var response = await _autenticacionService.RegistroDeAutenticacion(user, token, login.IpAddress);
            return Ok(response);

        }

        /// <summary>
        /// Servicio logout
        /// </summary>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>08/07/2024</Fecha>
        /// <param name="request">request token</param>
        /// <returns></returns>
        /// <response code="200">OK. inactiva el token y cierra la sesión.</response>           
        /// <response code="401">Unauthorized. el token ya no existe.</response>   
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(ResponseTypeSwagger<Respuesta>))]
        [Authorize]
        [HttpPost]
        [Route("logout")]
        public async Task<IHttpActionResult> LogOut([FromBody] BodyRequest request)
        {
            var response = await _autenticacionService.LogOut(request.Body);
            return Ok(response);
        }

        /// <summary>
        /// Servicio para obtener roles por autenticacion
        /// </summary>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>01/07/2022</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve los roles.</response>           
        /// <response code="401">Unauthorized. no hay autenticación.</response>   
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(List<int>))]
        [Authorize]
        [HttpGet]
        [Route("roles")]
        public async Task<IHttpActionResult> GetRolesByAutenticacion()
        {
            string loginName = GetLoginName();
            List<int> roles = await new UsuarioBO().GetRolesByLoginName(loginName);
            return Ok(roles);
        }
    }
}