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
    /// Api Cargo
    /// </summary>
    [AllowAnonymous]
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
        [ResponseType(typeof(List<CargoLicenciaDTO>))]
        [HttpPost]
        [Route("lista")]
        public IHttpActionResult CargoLicenciaList([FromBody] CargoLicenciaDTO filtro)
        {
            var query = _service.GetCargoLicencia(filtro);
            var listado = Mapear<IEnumerable<GENTEMAR_CARGO_LICENCIA>, IEnumerable<CargoLicenciaDTO>>(query);
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
        [ResponseType(typeof(List<CargoLicenciaDTO>))]
        [HttpGet]
        [Route("lista-activos")]
        public IHttpActionResult CargoLicenciaActivosList()
        {
            var query = _service.CargoLicenciaActivo();
            //var listado = Mapear<IEnumerable<GENTEMAR_CARGO_LICENCIA>, IEnumerable<CargoLicenciaDTO>>(query);
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
        public async Task<IHttpActionResult> Crear([FromBody] CargoLicenciaDTO cargo)
        {
            var data = Mapear<CargoLicenciaDTO, GENTEMAR_CARGO_LICENCIA>(cargo);
            var response = await _service.CrearAsync(data);
            return ResultadoStatus(response);
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
        [ResponseType(typeof(Respuesta))]
        [HttpPut]
        [Route("actualizar")]
        public async Task<IHttpActionResult> ACtializar([FromBody] CargoLicenciaDTO cargo)
        {
            var data = Mapear<CargoLicenciaDTO, GENTEMAR_CARGO_LICENCIA>(cargo);
            var response = await _service.ActualizarAsync(data);
            return ResultadoStatus(response);
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
        [ResponseType(typeof(List<CargoLicenciaDTO>))]
        [HttpGet]
        [Route("lista-id-detalle/{id}")]
        public IHttpActionResult CargoLicenciaIdDetalle(long id)
        {
            var query = _service.GetCargoLicenciaIdDetalle(id);
            //var listado = Mapear<GENTEMAR_CARGO_LICENCIA, CargoLicenciaDTO>(query);
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
        [ResponseType(typeof(List<CargoLicenciaDTO>))]
        [HttpGet]
        [Route("lista-id/{id}")]
        public IHttpActionResult CargoLicenciaId(long id)
        {
            var query = _service.GetCargoLicenciaId(id);
            //var listado = Mapear<GENTEMAR_CARGO_LICENCIA, CargoLicenciaDTO>(query);
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
        [ResponseType(typeof(Respuesta))]
        [HttpPut]
        [Route("inhabilitar/{id}")]
        public async Task<IHttpActionResult> CambiarCargoLicenciaAsync(int id)
        {
            var respuesta = await _service.cambiarCargoLicencia(id);
            return ResultadoStatus(respuesta);
        }

    }
}