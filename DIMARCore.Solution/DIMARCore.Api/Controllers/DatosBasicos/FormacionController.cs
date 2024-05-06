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

namespace DIMARCore.Api.Controllers
{
    /// <summary>
    /// Api Formacion
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/formacion")]
    public class FormacionController : BaseApiController
    {
        private readonly FormacionBO _service;
        /// <summary>
        /// Constructor
        /// </summary>
        public FormacionController()
        {
            _service = new FormacionBO();
        }
        /// <summary>
        /// Metodo para listar las formaciones  
        /// </summary>    
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>2022/02/26</Fecha>
        /// <returns></returns>
        /// <param name="estado">Booleano para consultar .</param>
        /// <response code="200">OK. Devuelve la información de la formación.</response>
        /// <response code="204">No Content. No hay estado.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(List<FormacionDTO>))]
        [HttpGet]
        [Route("{estado}")]
        [AuthorizeRolesFilter(RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.Consultas, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetFormaciones(bool estado)
        {
            var formacion = await _service.GetFormaciones(estado);
            return Ok(formacion);
        }

        /// <summary>
        /// Metodo para listar las formaciones  
        /// </summary>    
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>2022/02/26</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información de la formación.</response>
        /// <response code="204">No Content. No hay estado.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(List<FormacionDTO>))]
        [HttpGet]
        [Route("lista")]
        [AuthorizeRolesFilter(RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.Consultas, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetTableFormacion()
        {
            var formacion = await _service.GetTableFormacion();
            return Ok(formacion);
        }

        /// <summary>
        /// Metodo para listar formaciones activas
        /// </summary>    
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>2022/02/26</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información de la formación.</response>
        /// <response code="204">No Content. No hay estado.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(List<FormacionDTO>))]
        [HttpGet]
        [Route("listaActivo")]
        [AuthorizeRolesFilter(RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.Consultas, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetTableFormacionActivo()
        {
            var formacion = await _service.GetTableFormacionActivo();
            return Ok(formacion);
        }
        /// <summary>
        /// Servicio para crear una formación
        /// </summary>        
        /// <remarks>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>28/04/2022</Fecha>
        /// </remarks>
        /// <param name="fomacion">objeto para crear un estado.</param>
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
        public async Task<IHttpActionResult> CrearFormacion(FormacionDTO fomacion)
        {
            var data = Mapear<FormacionDTO, GENTEMAR_FORMACION>(fomacion);
            var respuesta = await _service.CrearFormacion(data);
            return Created(string.Empty, respuesta);
        }
        /// <summary>
        /// Servicio para editar una formación 
        /// </summary>
        /// <param name="formacion">objeto para editar un estado.</param>
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
        public async Task<IHttpActionResult> ActualizarFormacion(FormacionDTO formacion)
        {
            var data = Mapear<FormacionDTO, GENTEMAR_FORMACION>(formacion);
            var respuesta = await _service.ActualizarFormacion(data);
            return Ok(respuesta);
        }
        /// <summary>
        /// Servicio para Inactivar una formación 
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
        public async Task<IHttpActionResult> CambiarFormacion(int id)
        {
            var respuesta = await _service.CambiarFormacion(id);
            return Ok(respuesta);
        }
    }
}
