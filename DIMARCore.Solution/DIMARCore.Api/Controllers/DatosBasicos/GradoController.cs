using DIMARCore.Api.Core.Atributos;
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
    /// Api Grado
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/grado")]
    public class GradoController : BaseApiController
    {
        private readonly GradoBO _service;
        /// <summary>
        /// Constructor
        /// </summary>
        public GradoController()
        {
            _service = new GradoBO();
        }
        /// <summary>
        /// Listado de Grados
        /// </summary>
        /// <returns>Listado json de grados</returns>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <response code="200">OK. Devuelve la información del grado.</response>
        /// <response code="204">No Content. No hay estado.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(List<GradoDTO>))]
        [HttpGet]
        [Route("listaidformacion/{id}/{status}")]
        [AuthorizeRoles(RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.Consultas, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public IHttpActionResult GetGradoIdFormacion(int id, bool status)
        {
            var grados = _service.GetGradoIdGrado(id, status);

            //var data = Mapear<IList<APLICACIONES_GRADO>, IList<GradoDTO>>(grados);
            return Ok(grados);

        }

        /// <summary>
        /// Metodo para listar los grados 
        /// </summary>    
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>2022/02/26</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información del grado.</response>
        /// <response code="204">No Content. No hay estado.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(List<GradoDTO>))]
        [HttpGet]
        [Route("lista")]
        [AuthorizeRoles(RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.Consultas, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public IHttpActionResult GetFormacion()
        {
            var grado = _service.GetGrado();
            //var data = Mapear<IList<GENTEMAR_FORMACION>, IList<GradoDTO>>(formacion);
            return Ok(grado);
        }

        /// <summary>
        /// Metodo para listar grados activos
        /// </summary>    
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>2022/02/26</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información del grado.</response>
        /// <response code="204">No Content. No hay estado.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(List<GradoDTO>))]
        [HttpGet]
        [Route("listaActivo")]
        [AuthorizeRoles(RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.Consultas, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public IHttpActionResult GetFormacionActivos()
        {
            var grado = _service.GetGradoActivo();
            //var data = Mapear<IList<GENTEMAR_FORMACION>, IList<GradoDTO>>(formacion);
            return Ok(grado);
        }
        /// <summary>
        /// Servicio para crear un estado
        /// </summary>        
        /// <remarks>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>28/04/2022</Fecha>
        /// </remarks>
        /// <param name="grado">objeto para crear un grado.</param>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud con el grado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(Respuesta))]
        [HttpPost]
        [Route("crear")]
        [AuthorizeRoles(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> CrearFormacion(GradoDTO grado)
        {
            //var data = Mapear<GradoDTO, APLICACIONES_GRADO>(grado);
            var respuesta = await _service.CrearGrado(grado);
            return ResultadoStatus(respuesta);
        }
        /// <summary>
        /// Servicio para editar un grado
        /// </summary>
        /// <param name="grado">objeto para editar un grado.</param>
        /// <remarks>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud con el grado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(Respuesta))]
        [HttpPut]
        [Route("actualizar")]
        [AuthorizeRoles(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> actualizarGrado(GradoDTO grado)
        {
            //var data = Mapear<GradoDTO, APLICACIONES_GRADO>(grado);
            var respuesta = await _service.actualizarGrado(grado);
            return ResultadoStatus(respuesta);
        }
        /// <summary>
        /// Servicio para Inactivar un grado
        /// </summary>
        /// <param name="id">id del grado</param>
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
        [AuthorizeRoles(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> cambiarGrado(int id)
        {
            var respuesta = await _service.cambiarGrado(id);
            return ResultadoStatus(respuesta);
        }

    }
}
