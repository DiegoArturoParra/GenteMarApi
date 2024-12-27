using DIMARCore.UIEntities.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using DIMARCore.Business.Logica;
using GenteMarCore.Entities.Models;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers
{
    /// <summary>
    /// servicios naves
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/naves")]
    public class NavesController : BaseApiController
    {
        /// <summary>
        /// Listado de las naves Colombianas
        /// </summary>
        /// <returns>Lista de naves</returns>
        /// <summary>
        /// Listado de naves
        /// </summary>
        /// <returns>Listado json de naves</returns>
        /// <Autor>Camilo Vargas </Autor>
        /// <Fecha>24/04/2023</Fecha>
        /// <UltimaActualizacion>24/04/2023 - Camilo Vargas </UltimaActualizacion>
        /// <response code="200">OK. Se obtiene el listado de las naves colombianas.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(List<NavesDTO>))]
        [Authorize]
        [HttpGet]
        [Route("lista")]
        public async Task<IHttpActionResult> GetNaves()
        {
            var naves = await new NaveBO().GetAll();            
            return Ok(naves);
        }
    }
}