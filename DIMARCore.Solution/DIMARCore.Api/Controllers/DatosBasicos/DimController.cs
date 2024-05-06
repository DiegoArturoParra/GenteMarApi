using DIMARCore.Api.Core.Filters;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Enums;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers
{

    /// <summary>
    /// API Dim
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/dimImpresion")]
    public class DimController : BaseApiController
    {
        private readonly DimBO _service;

        /// <summary>
        /// Constructor
        /// </summary>
        public DimController()
        {
            _service = new DimBO();
        }
        /// <summary>
        /// Metodo para traer los dim impresos por una persona 
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información del dim.</response>
        /// <response code="204">No Content. No hay estado.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(List<DIM_IMPRESION>))]
        [HttpPost]
        [Route("listar")]
        [AuthorizeRolesFilter(RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.Consultas, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetDimIdAsync(DatosBasicosDTO usuario)
        {
            var DimPersona = await _service.GetDimImpresionIdAsync(usuario.DocumentoIdentificacion);
            return Ok(DimPersona);
        }
    }
}