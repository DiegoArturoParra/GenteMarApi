using DIMARCore.UIEntities.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using DIMARCore.Business.Logica;
using GenteMarCore.Entities.Models;

namespace DIMARCore.Api.Controllers
{
    /// <summary>
    /// Api Paises
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/naves")]
    public class NavesController : BaseApiController
    {

        /// <summary>
        /// Listado de las naves Colombianas
        /// </summary>
        /// <returns>Lista de naves</returns>
        [Authorize]
        [HttpGet]
        [Route("lista-naves")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetNaves()
        {
            var naves = await new NaveBO().GetAll();
            var data = Mapear<ICollection<NAVES_BASE>, ICollection<NavesDTO>>(naves);
            return Ok(data);
        }
    }
}