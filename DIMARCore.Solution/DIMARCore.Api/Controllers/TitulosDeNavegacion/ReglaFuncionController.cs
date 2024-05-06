using DIMARCore.Api.Core.Filters;
using DIMARCore.Api.Core.Models;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers.TitulosDeNavegacion
{
    /// <summary>
    /// servicios para la relación entre regla y sus funciones
    /// <Autor>Diego Parra</Autor>
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/regla-funciones")]
    [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
    public class ReglaFuncionController : BaseApiController
    {
        private readonly ReglaFuncionBO _serviceReglaFuncion;

        /// <summary>
        /// Constructor
        /// </summary>
        public ReglaFuncionController()
        {
            _serviceReglaFuncion = new ReglaFuncionBO();
        }


        /// <summary>
        ///  Se obtiene el listado de las reglas con las funciones relacionadas.
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
        [ResponseType(typeof(List<InfoReglaFuncionDTO>))]
        [HttpGet]
        [Route("lista")]
        public async Task<IHttpActionResult> Listado()
        {
            var query = await _serviceReglaFuncion.GetAll();
            return Ok(query);
        }

        /// <summary>
        ///  Se obtiene el listado de las reglas sin asignar funciones.
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>28/04/2022</Fecha>
        /// </remarks>
        /// <response code="200">OK. Se obtiene el listado de las reglas sin asignar funciones.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(List<ReglaDTO>))]
        [HttpGet]
        [Route("lista-exenta")]
        public async Task<IHttpActionResult> ReglasSinFunciones()
        {
            var query = await _serviceReglaFuncion.ReglasSinFunciones();
            return Ok(query);
        }


        /// <summary>
        /// Servicio para agregar funciones a una regla.
        /// </summary>        
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>28/04/2022</Fecha>
        /// </remarks>
        /// <param name="info">objeto para asignarle a una regla funciones.</param>
        /// <response code="201">Created. la solicitud ha tenido éxito y ha llevado a la creación de las funciones a la regla.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud Ya existe la relación indicada.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseCreatedTypeSwagger))]
        [HttpPost]
        [Route("crear")]
        public async Task<IHttpActionResult> Crear([FromBody] ReglaFuncionDTO info)
        {
            var response = await _serviceReglaFuncion.CrearAsync(info);
            return Created(string.Empty, response);
        }


        /// <summary>
        /// Servicio para editar las funciones de una regla.
        /// <param name="info">objeto para editar una regla con funciones.</param>
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="200">OK. Devuelve el mensaje de edición.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud con el estado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [HttpPut]
        [Route("editar")]
        public async Task<IHttpActionResult> Editar([FromBody] ReglaFuncionDTO info)
        {
            var response = await _serviceReglaFuncion.ActualizarAsync(info);
            return Ok(response);
        }
    }
}