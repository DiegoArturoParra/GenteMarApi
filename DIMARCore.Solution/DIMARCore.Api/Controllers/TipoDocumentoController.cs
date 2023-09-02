using DIMARCore.Business;
using System.Web.Http;
using System.Web.Http.Cors;
namespace DIMARCore.Api.Controllers
{
    /// <summary>
    /// Api Usuarios
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/tipodocumento")]
    public class TipoDocumentoController : BaseApiController
    {
        /// <summary>
        /// Listado de Tipo Documentos con información basica
        /// </summary>
        /// <returns>Listado de Tipo Documentos</returns>
        /// <Autor>Camilo Vargas </Autor>
        /// <Fecha>2022/02/22</Fecha>
        /// <UltimaActualizacion>2021/12/21 - Camilo Vargas - Creación del servicio</UltimaActualizacion>
        [HttpGet]
        [Route("lista")]
        [AllowAnonymous]
        public IHttpActionResult GetTipoDocumento()
        {
            var tipoDocuemento = new TipoDocumentoBO().GetTipoDocumento();
            return Ok(tipoDocuemento);
        }
    }
}