using DIMARCore.Api.Core.Atributos;
using DIMARCore.Api.Core.Models;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.Utilities.Enums;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers.TitulosDeNavegacion
{
    /// <summary>
    /// servicios para el detalle de cargo reglas
    /// <Autor>Diego Parra</Autor>
    /// <Fecha>3/03/2022</Fecha>
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/regla-cargos")]
    [AuthorizeRoles(RolesEnum.AdministradorGDM)]
    public class ReglaCargoController : BaseApiController
    {

        private readonly ReglaCargoBO _service;

        /// <summary>
        /// Constructor
        /// </summary>
        public ReglaCargoController()
        {
            _service = new ReglaCargoBO();
        }

        /// <summary>
        ///  Se obtiene el listado del detalle de reglas cargo.
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>23/03/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<ListadoDetalleCargoReglaDTO>))]
        [HttpGet]
        [Route("listado")]
        public IHttpActionResult GetDetalles([FromUri] DetalleReglaFilter filtro)
        {
            if (filtro == null)
            {
                filtro = new DetalleReglaFilter();
            }
            var query = _service.GetListado(filtro);
            return Ok(query);
        }


        /// <summary>
        ///  Se obtiene el detalle de la tabla regla-cargo por Id
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>23/03/2022</Fecha>
        /// </remarks>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseTypeSwagger<CargoReglaDTO>))]
        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> GetDetallesById(int id)
        {
            var query = await _service.GetById(id);
            return Ok(query);
        }

        /// <summary>
        ///  Creación cargo regla
        /// </summary>
        /// <remarks>
        ///  Servicio para crear la relación del cargo con regla
        /// </remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>28/04/2022</Fecha>
        /// <param name="obj">Objeto dto para crear la relación cargo regla</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseCreatedTypeSwagger))]
        [HttpPost]
        [Route("crear")]
        public async Task<IHttpActionResult> CrearCargoRegla(CargoReglaDTO obj)
        {
            var data = Mapear<CargoReglaDTO, GENTEMAR_REGLAS_CARGO>(obj);
            var respuesta = await _service.CrearCargoRegla(data);
            return ResultadoStatus(respuesta);
        }

        /// <summary>
        ///  Edición cargo regla
        /// </summary>
        /// <remarks>
        ///  Servicio para editar la relación del cargo con regla
        /// </remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>28/04/2022</Fecha>
        /// <param name="obj">Objeto dto para editar la relación cargo regla</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [HttpPut]
        [Route("editar")]
        public async Task<IHttpActionResult> EditarCargoRegla(CargoReglaDTO obj)
        {
            var data = Mapear<CargoReglaDTO, GENTEMAR_REGLAS_CARGO>(obj);
            var respuesta = await _service.EditarCargoRegla(data);
            return ResultadoStatus(respuesta);
        }
    }
}
