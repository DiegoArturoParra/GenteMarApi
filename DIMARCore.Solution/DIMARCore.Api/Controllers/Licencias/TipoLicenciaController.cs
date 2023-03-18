using DIMARCore.Business;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers
{
    /// <summary>
    /// Api tipolicencias
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/tipolicencias")]
    public class TipoLicenciaController : BaseApiController
    {

        private readonly TipoLicenciaBO _service;
        /// <summary>
        /// Constructor
        /// </summary>
        public TipoLicenciaController()
        {
            _service = new TipoLicenciaBO();
        }
        /// <summary>
        /// Listado de Tipo Licencias con información basica
        /// </summary>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>2022/02/26</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información del tipo de licencia.</response>
        /// <response code="204">No Content. No hay estado.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(List<TipoLicenciaDTO>))]
        [HttpGet]
        [Route("lista")]
        public IHttpActionResult GetTipoLicencias()
        {
            var tipoLicencias = _service.GetTipoLicencias();
            var data = Mapear<IList<GENTEMAR_TIPO_LICENCIA>, IList<TipoLicenciaDTO>>(tipoLicencias);
            return Ok(data);
        }

        /// <summary>
        /// Listado de Tipo Licencias activas con información basica 
        /// </summary>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>2022/02/26</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información del tipo de licencia.</response>
        /// <response code="204">No Content. No hay estado.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(List<TipoLicenciaDTO>))]
        [HttpGet]
        [Route("lista-activo")]
        [AllowAnonymous]
        public IHttpActionResult GetTipoLicenciasActivo()
        {
            var tipoLicencias = _service.GetTipoLicenciasActivo();
            var data = Mapear<IList<GENTEMAR_TIPO_LICENCIA>, IList<TipoLicenciaDTO>>(tipoLicencias);
            return Ok(data);
        }

        /// <summary>
        /// Retorna un Tipo Licencia dado un Id
        /// </summary>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>2022/02/26</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información de la actividad .</response>
        /// <response code="204">No Content. No hay estado.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(GENTEMAR_TIPO_LICENCIA))]
        [HttpGet]
        [Route("id")]
        public IHttpActionResult GetTipoLicencia(int id)
        {
            var tipoLicencia = _service.GetTipoLicencia(id);
            return Ok(tipoLicencia);
        }


        /// <summary>
        /// Crea un Tipo Licencia
        /// </summary>
        /// <remarks>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>28/04/2022</Fecha>
        /// </remarks>
        /// <param name="datos">objeto para crear un estado.</param>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud con el estado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(Respuesta))]
        [HttpPost]
        [Route("crear")]
        public async Task<IHttpActionResult> CrearTipoLicenciaAsync(TipoLicenciaDTO datos)
        {
            var data = Mapear<TipoLicenciaDTO, GENTEMAR_TIPO_LICENCIA>(datos);
            var tipo = await _service.CrearTipoLicenciaAsync(data);
            return ResultadoStatus(tipo);
        }


        /// <summary>
        /// Edita / Modifica un TipoLicencia
        /// </summary>
        /// <param name="datos">objeto para editar una actividad.</param>
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
        [Route("editar")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> EditarTipoLicenciaAsync(TipoLicenciaDTO datos)
        {
            var data = Mapear<TipoLicenciaDTO, GENTEMAR_TIPO_LICENCIA>(datos);
            var tipo = await _service.EditarTipoLicenciaAsync(data);
            return ResultadoStatus(tipo);
        }

        /// <summary>
        /// cambia ek estado de un rango 
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
        [AllowAnonymous]
        public async Task<IHttpActionResult> CambiarTipoLicenciaAsync(int id)
        {
            var respuesta = await _service.cambiarTipoLicencia(id);
            return ResultadoStatus(respuesta);
        }


    }
}
