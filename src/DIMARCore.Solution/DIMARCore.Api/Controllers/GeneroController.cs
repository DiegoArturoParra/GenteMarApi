using DIMARCore.Business;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers
{
    /// <summary>
    /// servicios genero
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/genero")]
    public class GeneroController : ApiController
    {
        /// <summary>
        /// Listado de Generos con información basica
        /// </summary>
        /// <returns>Listado de Genero</returns>
        /// <Autor>Camilo Vargas </Autor>
        /// <Fecha>2022/02/22</Fecha>
        /// <UltimaActualizacion>2021/12/21 - Camilo Vargas - Creación del servicio</UltimaActualizacion>
        /// <response code="200">OK. Se obtiene el listado de los generos.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(List<APLICACIONES_GENERO>))]
        [HttpGet]
        [Route("lista")]
        [Authorize]
        public async Task<IHttpActionResult> GetGeneros()
        {
            var generos = await new GeneroBO().GetGeneros();
            return Ok(generos);
        }
    }
}
