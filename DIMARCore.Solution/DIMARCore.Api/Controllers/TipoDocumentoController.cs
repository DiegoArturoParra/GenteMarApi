using DIMARCore.Business;
using DIMARCore.UIEntities.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers
{
    /// <summary>
    /// servicios de tipo documento
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
        /// <response code="200">OK. Devuelve la lista de documentos.</response>        
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado data.</response>
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<CapitaniaDTO>))]
        [HttpGet]
        [Route("lista")]
        [Authorize]
        public async Task<IHttpActionResult> GetTipoDocumento()
        {
            var documentos = await new TipoDocumentoBO().GetTiposDocumento();
            return Ok(documentos);
        }
    }
}