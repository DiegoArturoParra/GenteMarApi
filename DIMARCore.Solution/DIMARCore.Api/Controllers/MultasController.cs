using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers
{    /// <summary>
     /// Servicios multas
     /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/multas")]
    public class MultasController : BaseApiController
    {
        /// <summary>
        ///  Se obtiene el listado de multas de una persona en gente de mar.
        /// </summary>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve la lista de multas.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>04/07/2023</Fecha>
        /// </remarks>
        [ResponseType(typeof(List<MultaDTO>))]
        [HttpGet]
        [Route("lista-por-usuario")]
        [Authorize]
        public async Task<IHttpActionResult> GetMultasPorUsuario([FromUri] CedulaDTO cedula)
        {
            var data = await new MultasBO().GetMultasPorUsuario(cedula.IdentificacionConPuntos);
            return Ok(data);
        }
    }
}
