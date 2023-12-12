using DIMARCore.Api.Core.Atributos;
using DIMARCore.Api.Core.Models;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.Utilities.Enums;
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
        [AuthorizeRoles(RolesEnum.Consultas, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public IHttpActionResult CargoLicenciaList([FromBody] CargoInfoLicenciaDTO filtro)
        {
            var query = _service.GetCargosLicencia(filtro);
            var listado = Mapear<IEnumerable<GENTEMAR_CARGO_LICENCIA>, IEnumerable<CargoInfoLicenciaDTO>>(query);
            return Ok(listado);

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
        [AuthorizeRoles(RolesEnum.Consultas, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> CargosLicenciaActivosPorCapitaniaCategoria()
        {
            var query = await _service.GetCargosLicenciaActivosPorCapitaniaCategoria();
            return Ok(query);
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
        [ResponseType(typeof(List<CargoLicenciaDTO>))]
        [HttpPost]
        [Route("lista-activos-filtro-reporte")]
        [AuthorizeRoles(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> CargosLicenciaActivos([FromBody] CargoLicenciaFilter cargoLicenciaFilter)
        {
            var query = await _service.GetCargosLicenciaActivosPorFiltro(cargoLicenciaFilter);
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
        [ResponseType(typeof(Respuesta))]
        [HttpPost]
        [Route("crear")]
        [AuthorizeRoles(RolesEnum.AdministradorGDM)]

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
        [AuthorizeRoles(RolesEnum.AdministradorGDM)]
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
        [AuthorizeRoles(RolesEnum.Consultas, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public IHttpActionResult CargoLicenciaIdDetalle(long id)
        {
            var query = _service.GetCargoLicenciaIdDetalle(id);
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
        [AuthorizeRoles(RolesEnum.Consultas, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public IHttpActionResult CargoLicenciaId(long id)
        {
            var query = _service.GetCargoLicenciaId(id);
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
        [AuthorizeRoles(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> CambiarCargoLicenciaAsync(int id)
        {
            var respuesta = await _service.CambiarCargoLicencia(id);
            return Ok(respuesta);
        }

    }
}