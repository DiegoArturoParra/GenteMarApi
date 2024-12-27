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
    /// Api Paises
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/municipios")]
    public class MunicipiosController : BaseApiController
    {
        /// <summary>
        /// Listado de Municipios
        /// </summary>
        /// <returns>Listado json de municipios</returns>
        /// <entidad>APLICACIONES_MUNICIPIO</entidad>
        /// <tabla>APLICACIONES_MUNICIPIO</tabla>
        /// <Autor>Camilo Vargas </Autor>
        /// <Fecha>2022/02/22</Fecha>
        /// <UltimaActualizacion>2022/02/22 - Camilo Vargas - Crear metodo</UltimaActualizacion>
        /// <response code="200">OK. Se obtiene el listado de los municipios.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(List<MunicipioDTO>))]
        [HttpGet]
        [Route("lista")]
        [Authorize]
        public async Task<IHttpActionResult> GetMunicipiosAsync()
        {
            var municipios = await new MunicipiosBO().GetMunicipios();
            var resultado = Mapear<IList<APLICACIONES_MUNICIPIO>, IList<MunicipioDTO>>(municipios);
            return Ok(resultado);
        }
    }
}
