using DIMARCore.Api.Core.Filters;
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
    [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
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
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<ListadoDetalleCargoReglaDTO>))]
        [HttpPost]
        [Route("listado")]
        public async Task<IHttpActionResult> GetListarCargosReglaAsync([FromBody] DetalleReglaFilter filtro)
        {
            var query = await _service.GetListadoAsync(filtro);
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
        ///  Se obtiene el listado de cargos por sección.
        /// </summary>
        /// <param name="SeccionId"> parametro que contiene el id de la sección</param>
        /// <response code="200">OK. Devuelve la lista de cargos por sección solicitado.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No ha encontrado información de cargos por la sección.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>23/05/2022</Fecha>
        /// </remarks>
        [ResponseType(typeof(List<CargoTituloInfoDTO>))]
        [HttpGet]
        [Route("lista-by-seccion/{SeccionId}")]
        public async Task<IHttpActionResult> GetCargoTitulosBySeccionId(int SeccionId)
        {
            var query = await _service.GetCargosTituloBySeccionId(SeccionId);
            var listado = Mapear<IEnumerable<GENTEMAR_CARGO_TITULO>, IEnumerable<CargoTituloInfoDTO>>(query);
            return Ok(listado);
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
            return Created(string.Empty, respuesta);
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
            return Ok(respuesta);
        }
    }
}
