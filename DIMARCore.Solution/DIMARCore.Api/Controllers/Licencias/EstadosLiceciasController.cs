using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers.Licencias
{
    /// <summary>
    /// Api estados licencia
    /// </summary>
    [Authorize]
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/estado-licencia")]
    public class EstadosLiceciasController : BaseApiController
    {
        private readonly EstadoLicenciaBO _service;

        /// <summary>
        /// Constructor
        /// </summary>
        public EstadosLiceciasController()
        {
            _service = new EstadoLicenciaBO();
        }

        /// <summary>
        /// Metodo para listar estados activos
        /// </summary>    
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>2022/02/26</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información del estado.</response>
        /// <response code="204">No Content. No hay estado.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(List<GENTEMAR_ESTADO_LICENCIA>))]
        [HttpGet]
        [Route("listaActivo")]
        public IHttpActionResult GetEstadoActivo()
        {
            var ListaEstado = _service.GetEstadoActivo();
            var data = Mapear<IList<GENTEMAR_ESTADO_LICENCIA>, IList<EstadoLicenciaDTO>>(ListaEstado);
            return Ok(data);
        }


        /// <summary>
        /// Metodo para listar estados 
        /// </summary>    
        /// <summary>
        /// Metodo para listar estados 
        /// </summary>    
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>2022/02/26</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información del estado.</response>
        /// <response code="204">No Content. No hay estado.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(List<GENTEMAR_ESTADO_LICENCIA>))]
        [HttpGet]
        [Route("lista")]
        public IHttpActionResult GetEstado()
        {
            var ListaEstado = _service.GetEstado();
            var data = Mapear<IList<GENTEMAR_ESTADO_LICENCIA>, IList<EstadoLicenciaDTO>>(ListaEstado);
            return Ok(data);
        }


        /// <summary>
        /// Servicio para crear un estado
        /// </summary>        
        /// <remarks>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>28/04/2022</Fecha>
        /// </remarks>
        /// <param name="estado">objeto para crear un estado.</param>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud con el estado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(Respuesta))]
        [HttpPost]
        [Route("crear")]
        public IHttpActionResult CrearEstado(EstadoLicenciaDTO estado)
        {
            var data = Mapear<EstadoLicenciaDTO, GENTEMAR_ESTADO_LICENCIA>(estado);
            var respuesta = _service.CrearEstado(data);
            return Ok(respuesta);
        }
        /// <summary>
        /// Servicio para editar un estado
        /// </summary>
        /// <param name="estado">objeto para editar un estado.</param>
        /// <remarks>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud con el estado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(Respuesta))]
        [HttpPut]
        [Route("actualizar")]
        public IHttpActionResult actualizarEstado(EstadoLicenciaDTO estado)
        {
            var data = Mapear<EstadoLicenciaDTO, GENTEMAR_ESTADO_LICENCIA>(estado);
            var respuesta = _service.actualizarEstado(data);
            return Ok(respuesta);
        }

        /// <summary>
        /// Servicio para Inactivar un estado
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
        public IHttpActionResult CambiarEstado(int id)
        {
            var respuesta = _service.cambiarEstado(id);
            return Ok(respuesta);
        }
    }
}