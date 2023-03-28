
using DIMARCore.Business.Logica;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DIMARCore.Api.Controllers
{
    /// <summary>
    /// Api Usuarios
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/usuarios")]
    public class UsuariosController : BaseApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("info/{id}")]
        public IHttpActionResult GetUsuario(int id)
        {
            return Ok();
        }

        /// <summary>
        /// Servicio que retorna el menu correspondiente al usuario logueado.
        /// </summary>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>01/07/2022</Fecha>
        /// <returns></returns>
        /// <response code="401">Unauthorized. El usuario no esta autorizado..</response>   
        /// <response code="200">OK. Devuelve la información del listado de menu.</response>           
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [HttpGet]
        [Authorize]
        [Route("menu")]
        public async Task<IHttpActionResult> GetUsuarioWithMenu()
        {
            var listado = await new MenuBO().GetUsuarioWithMenu(GetIdAplicacion(), GetLoginName());
            return Ok(listado);
        }
    }
}
