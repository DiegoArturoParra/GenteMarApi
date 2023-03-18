using DIMARCore.Business;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
namespace DIMARCore.Api.Controllers.Licencias
{
    /// <summary>
    /// Api categorias
    /// </summary>
    [Authorize]
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/categoria")]
    public class CategoriaController : BaseApiController
    {
        private readonly CategoriaBO _service;
        /// <summary>
        /// Constructor
        /// </summary>
        public CategoriaController()
        {
            _service = new CategoriaBO();
        }

        /// <summary>
        /// Listado de categorias con información basica
        /// </summary>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>2022/07/12</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información de la categoria.</response>
        /// <response code="204">No Content. No hay estado.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(List<CategoriaDTO>))]
        [HttpGet]
        [Route("lista")]
        public IHttpActionResult GetTipoLicencias()
        {
            var categoria = _service.GetAll();
            var data = Mapear<IEnumerable<APLICACIONES_CATEGORIA>, IEnumerable<CategoriaDTO>>(categoria);
            return Ok(data);
        }

    }
}