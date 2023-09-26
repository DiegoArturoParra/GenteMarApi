using DIMARCore.Business;
using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers
{
    /// <summary>
    /// Servicios Paises
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/paises")]
    public class PaisesController : BaseApiController
    {
        /// <summary>
        /// Listado de paises/banderas
        /// </summary>
        /// <returns>Listado json de paises/banderas</returns>
        /// <entidad>Bandera</entidad>
        /// <tabla>t_nav_band</tabla>
        /// <Autor>Carlos Rodríguez</Autor>
        /// <Fecha>2020/05/26</Fecha>
        /// <UltimaActualizacion>2020/07/23 - Carlos Rodríguez - Cambio consulta para obtener los paises</UltimaActualizacion>
        [ResponseType(typeof(List<PaisDTO>))]
        [HttpGet]
        [Route("list-basic")]
        [Authorize]
        public async Task<IHttpActionResult> GetPaises()
        {
            var paises = await new PaisBO().GetPaises();
            var data = Mapear<IList<PAISES>, IList<PaisDTO>>(paises);
            return Ok(data);
        }


        /// <summary>
        /// PAIS COLOMBIA
        /// </summary>
        /// <returns>pais colombia</returns>
        /// <entidad>Bandera</entidad>
        /// <tabla>t_nav_band</tabla>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>7/03/2022</Fecha>
        /// <UltimaActualizacion>7/03/2022</UltimaActualizacion>
        [ResponseType(typeof(List<PaisDTO>))]
        [HttpGet]
        [Route("colombia")]
        [Authorize]
        public async Task<IHttpActionResult> GetPaisColombia()
        {
            var pais = await new PaisBO().GetPaisColombia();
            var data = Mapear<IList<PAISES>, IList<PaisDTO>>(pais);
            return Ok(data);
        }
    }
}
