using DIMARCore.Api.Core.Filters;
using DIMARCore.Api.Core.Models;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Enums;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers
{
    /// <summary>
    /// servicios clases de titulos y licencias
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/clases")]
    public class ClasesController : BaseApiController
    {
        private readonly ClaseTituloBO _serviceClaseTitulos;
        private readonly ClaseLicenciasBO _serviceClaseLicencias;
        /// <summary>
        /// constructor
        /// </summary>
        public ClasesController()
        {
            _serviceClaseTitulos = new ClaseTituloBO();
            _serviceClaseLicencias = new ClaseLicenciasBO();
        }

        #region clases de titulos

        /// <summary>
        ///  Se obtiene el listado de las clases de los titulos de navegación.
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/05/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(List<ClaseDTO>))]
        [HttpGet]
        [Route("lista-por-titulos")]
        [AuthorizeRolesFilter(RolesEnum.Consultas, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetClasesTitulos([FromUri] ActivoDTO dto)
        {
            var clases = await _serviceClaseTitulos.GetAllAsync(dto != null ? dto.Activo : null);
            var listado = Mapear<IEnumerable<GENTEMAR_CLASE_TITULOS>, IEnumerable<ClaseDTO>>(clases);
            return Ok(listado);

        }

        /// <summary>
        ///  clase de titulo por id
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/05/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(ResponseTypeSwagger<ClaseDTO>))]
        [HttpGet]
        [Route("por-titulo/{id}")]
        [AuthorizeRolesFilter(RolesEnum.Consultas, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetClaseitulo(int id)
        {
            var clase = await _serviceClaseTitulos.GetByIdAsync(id);
            var obj = Mapear<GENTEMAR_CLASE_TITULOS, ClaseDTO>((GENTEMAR_CLASE_TITULOS)clase.Data);
            clase.Data = obj;
            return Ok(clase);
        }

        /// <summary>
        /// Servicio para crear una clase de un titulo
        /// </summary>
        /// <param name="clase"></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="201">Created. la solicitud ha tenido éxito y ha llevado a la creación de la clase del titulo.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. Ya existe el nombre de la clase del titulo de navegación.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(ResponseCreatedTypeSwagger))]
        [HttpPost]
        [Route("crear-por-titulo")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> CrearSeccionTitulo([FromBody] ClaseDTO clase)
        {
            var data = Mapear<ClaseDTO, GENTEMAR_CLASE_TITULOS>(clase);
            var response = await _serviceClaseTitulos.CrearAsync(data);
            return Created(string.Empty, response);
        }


        /// <summary>
        /// Servicio para editar una clase de un titulo
        /// </summary>
        /// <param name="clase"></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el mensaje de edición.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. Ya existe el nombre de la clase del titulo de navegación.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [HttpPut]
        [Route("editar-por-titulo")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> EditarClaseTitulo([FromBody] ClaseDTO clase)
        {
            var data = Mapear<ClaseDTO, GENTEMAR_CLASE_TITULOS>(clase);
            var response = await _serviceClaseTitulos.ActualizarAsync(data);
            return Ok(response);
        }

        /// <summary>
        /// Servicio para Inactivar una clase de un titulo
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="200">OK. Devuelve el mensaje de tipo respuesta.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [HttpPut]
        [Route("anula-or-activa-por-titulo/{id}")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> InactivarClaseTitulo(int id)
        {
            var obj = await _serviceClaseTitulos.AnulaOrActivaAsync(id);
            return Ok(obj);
        }
        #endregion

        #region clases de licencias

        /// <summary>
        ///  Se obtiene el listado de las clases de las licencias de navegación.
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(List<ClaseDTO>))]
        [HttpGet]
        [Route("lista-por-licencias")]
        [AuthorizeRolesFilter(RolesEnum.Consultas, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetClasesLicenciasAsync()
        {
            var clases = await _serviceClaseLicencias.GetAllClaseLicenciasAsync();
            return Ok(clases);
        }

        /// <summary>
        /// Listado de clases activas por id de seccion
        /// </summary>
        /// <returns>Listado de clases por id de seccion</returns>
        /// <param name="id"></param>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>2022/02/26</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve Listado de clases por id de sección.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(List<ClaseDTO>))]
        [HttpGet]
        [Route("lista-seccion-licencia/{id}")]
        [AuthorizeRolesFilter(RolesEnum.Consultas, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetClasesPorSeccionId(int id)
        {
            var clases = await _serviceClaseLicencias.GetClasesPorSeccionId(id);
            return Ok(clases);
        }

        /// <summary>
        /// Listado de clases activas por ids de seccion
        /// </summary>
        /// <returns>Listado de clases por ids de seccion</returns>
        /// <param name="ids"></param>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>2022/02/26</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve Listado de clases por ids de sección.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(List<ClaseDTO>))]
        [HttpPost]
        [Route("lista-por-secciones-licencia")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetClasesPorSecciones(List<int> ids)
        {
            var clases = await _serviceClaseLicencias.GetClasesPorSeccionesIds(ids);
            return Ok(clases);
        }

        /// <summary>
        ///  Se obtiene el listado de las clases activas de las licencias de navegación.
        /// </summary>
        /// <remarks>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="200">OK. Devuelve el listado de las clases activas.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(List<ClaseDTO>))]
        [HttpGet]
        [Route("lista-por-licencias-activas")]
        [AuthorizeRolesFilter(RolesEnum.Consultas, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetClasesLicenciasActivas()
        {
            var clases = await _serviceClaseLicencias.GetAllClaseLicenciasActivas();
            var listado = Mapear<IEnumerable<GENTEMAR_CLASE_LICENCIAS>, IEnumerable<ClaseDTO>>(clases);
            return Ok(listado);

        }
        /// <summary>
        ///  clase de licencia por id
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/04/2022</Fecha>
        /// </remarks>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(ResponseTypeSwagger<ClaseDTO>))]
        [HttpGet]
        [Route("por-licencia/{id}")]
        [AuthorizeRolesFilter(RolesEnum.Consultas, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetClaseLicencia(int id)
        {
            var clase = await _serviceClaseLicencias.GetByIdAsync(id);
            var obj = Mapear<GENTEMAR_CLASE_LICENCIAS, ClaseDTO>((GENTEMAR_CLASE_LICENCIAS)clase.Data);
            clase.Data = obj;
            return Ok(clase);
        }

        /// <summary>
        /// Servicio para crear una clase de una licencia de navegación
        /// </summary>
        /// <param name="clase"></param>
        /// <remarks>
        /// <Autor>Juan Camilo</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="201">Created. la solicitud ha tenido éxito y ha llevado a la creación de la clase de una licencia.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. Ya existe el nombre de la clase de la licencia de navegación.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(ResponseCreatedTypeSwagger))]
        [HttpPost]
        [Route("crear-por-licencia")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> CrearClaseLicencia([FromBody] ClaseDTO clase)
        {
            var data = Mapear<ClaseDTO, GENTEMAR_CLASE_LICENCIAS>(clase);
            var sec = Mapear<IList<SeccionDTO>, IList<GENTEMAR_SECCION_LICENCIAS>>(clase.Seccion);
            var response = await _serviceClaseLicencias.CrearAsync(data, sec);
            return Created(string.Empty, response);
        }


        /// <summary>
        /// Servicio para editar una clase de una licencia
        /// </summary>
        /// <param name="clase"></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/04/2022</Fecha>
        /// </remarks>
        /// <response code="200">OK. Devuelve el mensaje de actualización.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. Ya existe el nombre de la clase de la licencia de navegación.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [HttpPut]
        [Route("editar-por-licencia")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> EditarClaseLicencia([FromBody] ClaseDTO clase)
        {
            var data = Mapear<ClaseDTO, GENTEMAR_CLASE_LICENCIAS>(clase);
            var sec = Mapear<IList<SeccionDTO>, IList<GENTEMAR_SECCION_LICENCIAS>>(clase.Seccion);
            var response = await _serviceClaseLicencias.ActualizarAsync(data, sec);
            return Ok(response);
        }
        /// <summary>
        /// Servicio para Inactivar una clase de una licencia
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/04/2022</Fecha>
        /// </remarks>
        /// <response code="200">OK. Devuelve el mensaje de actualización.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [HttpPut]
        [Route("anula-or-activa-por-licencia/{id}")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> InactivarLicencia(int id)
        {
            var obj = await _serviceClaseLicencias.AnulaOrActivaAsync(id);
            return Ok(obj);
        }
        #endregion
    }
}
