using DIMARCore.Business;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DIMARCore.Api.Controllers
{
    /// <summary>
    /// Api Usuarios
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/genero")]

    public class GeneroController : ApiController
    {
        /// <summary>
        /// Listado de GEnero con información basica
        /// </summary>
        /// <returns>Listado de Genero</returns>
        /// <Autor>Camilo Vargas </Autor>
        /// <Fecha>2022/02/22</Fecha>
        /// <UltimaActualizacion>2021/12/21 - Camilo Vargas - Creación del servicio</UltimaActualizacion>
        [HttpGet]
        [Route("lista")]
        [AllowAnonymous]
        public IHttpActionResult GetGenero()
        {
            var tipoDocuemento = new GeneroBO().GetGenero();
            return Ok(tipoDocuemento);
        }
    }
}
