using DIMARCore.Api.Core.Filters;
using DIMARCore.Api.Core.Models;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.UIEntities.QueryFilters.Reports;
using DIMARCore.Utilities.Enums;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers.Licencias
{
    /// <summary>
    /// Api Cargo
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/cargo-licencia")]
    public class CargoLicenciaController : BaseApiController
    {
        private readonly CargoLicenciaBO _service;

        /// <summary>
        /// Constructor
        /// </summary>
        public CargoLicenciaController()
        {
            _service = new CargoLicenciaBO();
        }
        /// <summary>
        ///  Se obtiene el listado de los cargos.
        /// </summary>
        /// <remarks>
        /// <Autor>Camilo Varagas</Autor>
        /// <Fecha>13/06/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(List<CargoInfoLicenciaDTO>))]
        [HttpPost]
        [Route("lista")]
        [AuthorizeRolesFilter(RolesEnum.Consultas, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> CargoLicenciaListAsync([FromBody] CargoLicenciaFilter filtro)
        {
            var data = await _service.GetCargosLicenciaAsync(filtro);
            return Ok(data);

        }

        /// <summary>
        ///  Se obtiene el listado de los cargos activos.
        /// </summary>
        /// <remarks>
        /// <Autor>Camilo Varagas</Autor>
        /// <Fecha>13/06/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(List<CargoInfoLicenciaDTO>))]
        [HttpGet]
        [Route("lista-activos-por-categoria-capitania")]
        [AuthorizeRolesFilter(RolesEnum.Consultas, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> CargosLicenciaActivosPorCapitaniaCategoria()
        {
            var query = await _service.GetCargosLicenciaActivosPorCapitaniaCategoria();
            return Ok(query);
        }

        /// <summary>
        ///  Se obtiene el listado de los cargos para el reporte por un filtro especifico.
        /// </summary>
        /// <param name="cargoLicenciaFilter"></param>
        /// <remarks>
        /// <Autor>Diego Para </Autor>
        /// <Fecha>13/06/2023</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(List<CargoLicenciaDTO>))]
        [HttpPost]
        [Route("lista-activos-filtro-reporte")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> CargosLicenciaActivosPorFiltroParaReporte([FromBody] CargoLicenciaReportFilter cargoLicenciaFilter)
        {
            var query = await _service.GetCargosLicenciaActivosPorFiltroParaReporte(cargoLicenciaFilter);
            return Ok(query);
        }

        /// <summary>
        /// Servicio para crear un cargo 
        /// </summary>        
        /// <remarks>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>13/06/2022</Fecha>
        /// </remarks>
        /// <param name="cargo">objeto para crear una capacidad.</param>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud con el estado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseCreatedTypeSwagger))]
        [HttpPost]
        [Route("crear")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]

        public async Task<IHttpActionResult> Crear([FromBody] CargoInfoLicenciaDTO cargo)
        {
            var data = Mapear<CargoInfoLicenciaDTO, GENTEMAR_CARGO_LICENCIA>(cargo);
            var response = await _service.CrearAsync(data);
            return Created(string.Empty, response);
        }

        /// <summary>
        /// Servicio para actualizar un cargo 
        /// </summary>        
        /// <remarks>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>15/07/2022</Fecha>
        /// </remarks>
        /// <param name="cargo">objeto para editar un cargo.</param>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud con el estado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [HttpPut]
        [Route("actualizar")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> Actualizar([FromBody] CargoInfoLicenciaDTO cargo)
        {
            var data = Mapear<CargoInfoLicenciaDTO, GENTEMAR_CARGO_LICENCIA>(cargo);
            var response = await _service.ActualizarAsync(data);
            return Ok(response);
        }

        /// <summary>
        ///  Se obtiene el cargo por id detalle 
        /// </summary>
        /// <remarks>
        /// <Autor>Camilo Varagas</Autor>
        /// <Fecha>15/06/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(List<CargoInfoLicenciaDTO>))]
        [HttpGet]
        [Route("lista-id-detalle/{id}")]
        [AuthorizeRolesFilter(RolesEnum.Consultas, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> CargoLicenciaIdDetalleAsync(long id)
        {
            var query = await _service.GetCargoLicenciaIdDetalleAsync(id);
            return Ok(query);
        }

        /// <summary>
        ///  Se obtiene el cargo por id.
        /// </summary>
        /// <remarks>
        /// <Autor>Camilo Varagas</Autor>
        /// <Fecha>15/06/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(List<CargoInfoLicenciaDTO>))]
        [HttpGet]
        [Route("lista-id/{id}")]
        [AuthorizeRolesFilter(RolesEnum.Consultas, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> CargoLicenciaIdAsync(long id)
        {
            var query = await _service.GetCargoLicenciaIdAsync(id);
            return Ok(query);
        }
        /// <summary>
        /// cambia el estado de un cargo licencia 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [HttpPut]
        [Route("inhabilitar/{id}")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> CambiarCargoLicenciaAsync(int id)
        {
            var respuesta = await _service.CambiarCargoLicencia(id);
            return Ok(respuesta);
        }

    }
}