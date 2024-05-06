using DIMARCore.Api.Core.Filters;
using DIMARCore.Api.Core.Models;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers.DatosBasicos
{
    /// <summary>
    /// Api Rangos
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/rango")]
    public class RangoController : BaseApiController
    {

        private readonly RangoBO _service;
        /// <summary>
        /// Constructor
        /// </summary>
        public RangoController()
        {
            _service = new RangoBO();
        }
        /// <summary>
        /// Metodo para listar los rangos  
        /// </summary>    
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>2022/02/26</Fecha>
        /// <returns></returns>
        /// <param name="estado">Booleano para consultar .</param>
        /// <response code="200">OK. Devuelve la información del rango.</response>
        /// <response code="204">No Content. No hay estado.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(List<FormacionDTO>))]
        [HttpGet]
        [Route("lista/{estado}")]
        [AuthorizeRolesFilter(RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.Consultas, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetRangoAsync(bool estado)
        {
            var rango = await _service.GetRangosAsync(estado);
            var data = Mapear<IList<APLICACIONES_RANGO>, IList<RangoDTO>>(rango);
            return Ok(data);
        }
        /// <summary>
        /// Servicio para crear un rango
        /// </summary>        
        /// <remarks>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>28/04/2022</Fecha>
        /// </remarks>
        /// <param name="rango">objeto para crear un estado.</param>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud de la formación.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseCreatedTypeSwagger))]
        [HttpPost]
        [Route("crear")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> CrearRangoAsync(RangoDTO rango)
        {
            var data = Mapear<RangoDTO, APLICACIONES_RANGO>(rango);
            var response = await _service.CrearRango(data);
            return Created(string.Empty, response);
        }
        /// <summary>
        /// Servicio para editar un rango 
        /// </summary>
        /// <param name="rango">objeto para editar un estado.</param>
        /// <remarks>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud de la formación.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [HttpPut]
        [Route("actualizar")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> ActualizarRangoAsync(RangoDTO rango)
        {
            var data = Mapear<RangoDTO, APLICACIONES_RANGO>(rango);
            var respuesta = await _service.ActualizarRango(data);
            return Ok(respuesta);
        }
        /// <summary>
        /// Servicio para Inactivar un rango 
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
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> CambiarRangoAsync(int id)
        {
            var respuesta = await _service.CambiarRango(id);
            return ResultadoStatus(respuesta);
        }
    }
}
