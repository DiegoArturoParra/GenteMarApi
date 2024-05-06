
using DIMARCore.Api.Core.Filters;
using DIMARCore.Api.Core.Models;
using DIMARCore.Business;
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
    /// servicios de Secciones, para titulos y licencias
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/secciones")]
    public class SeccionesController : BaseApiController
    {
        #region secciones de titulos
        /// <summary>
        /// Listado de secciones por titulos con información basica
        /// </summary>
        /// <remarks>
        /// <param name="dto"></param>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="200">OK. Devuelve la lista de secciones de los titulos.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>             
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(List<SeccionDTO>))]
        [AuthorizeRolesFilter(RolesEnum.Consultas, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        [HttpGet]
        [Route("lista-por-titulos")]
        public async Task<IHttpActionResult> GetSeccionesTitulos([FromUri] ActivoDTO dto)
        {
            var secciones = await new SeccionBO().GetSeccionesTitulos(dto != null ? dto.Activo : null);
            var listado = Mapear<IEnumerable<GENTEMAR_SECCION_TITULOS>, IEnumerable<SeccionDTO>>(secciones);
            return Ok(listado);
        }
        /// <summary>
        ///  seccion de titulo por id
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/04/2022</Fecha>
        /// </remarks>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>

        [ResponseType(typeof(ResponseTypeSwagger<SeccionDTO>))]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        [HttpGet]
        [Route("por-titulo/{id}")]
        public async Task<IHttpActionResult> GetSeccionTitulo(int id)
        {
            var seccion = await new SeccionBO().GetSeccionTitulo(id);
            var obj = Mapear<GENTEMAR_SECCION_TITULOS, SeccionDTO>((GENTEMAR_SECCION_TITULOS)seccion.Data);
            seccion.Data = obj;
            return Ok(seccion);
        }

        /// <summary>
        /// Servicio para crear una seccion de un titulo
        /// </summary>
        /// <param name="seccion"></param>
        /// <returns></returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. Ya existe el nombre de la sección del titulo de navegación.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(ResponseCreatedTypeSwagger))]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        [HttpPost]
        [Route("crear-por-titulo")]
        public async Task<IHttpActionResult> CrearSeccionTitulo([FromBody] SeccionDTO seccion)
        {
            var data = Mapear<SeccionDTO, GENTEMAR_SECCION_TITULOS>(seccion);
            var response = await new SeccionBO().CrearSeccionTitulo(data);
            return Created(string.Empty, response);
        }


        /// <summary>
        /// Servicio para editar una seccion de un titulo
        /// </summary>
        /// <param name="seccion"></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/04/2022</Fecha>
        /// </remarks>
        /// <response code="200">OK. Devuelve el mensaje de actualización.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. Ya existe el nombre de la sección de un titulo de navegación.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        [HttpPut]
        [Route("editar-por-titulo")]
        public async Task<IHttpActionResult> EditarSeccionTitulo([FromBody] SeccionDTO seccion)
        {
            var data = Mapear<SeccionDTO, GENTEMAR_SECCION_TITULOS>(seccion);
            var response = await new SeccionBO().EditarSeccionTitulo(data);
            return Ok(response);
        }

        /// <summary>
        /// Servicio para Inactivar una sección de un titulo
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
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        [HttpPut]
        [Route("anula-or-activa-por-titulo/{id}")]
        public async Task<IHttpActionResult> InactivarSeccionTitulo(int id)
        {
            var obj = await new SeccionBO().AnulaOrActivaSeccionTitulo(id);
            return Ok(obj);
        }
        #endregion

        #region secciones de licencias
        /// <summary>
        /// Listado de secciones por licencias con información basica
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="200">OK. Devuelve la lista de secciones de las licencias.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>             
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(List<SeccionDTO>))]
        [AuthorizeRolesFilter(RolesEnum.Consultas, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        [HttpGet]
        [Route("lista-por-licencias")]
        public async Task<IHttpActionResult> GetSeccionesLicenciasAsync()
        {
            var secciones = await new SeccionBO().GetSeccionesLicencias();
            return Ok(secciones);
        }

        /// <summary>
        /// Listado de secciones activas por licencias con información basica
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="200">OK. Devuelve la lista de secciones de las licencias.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>             
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(List<SeccionDTO>))]
        [AuthorizeRolesFilter(RolesEnum.Consultas, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        [HttpGet]
        [Route("lista-por-licencias-activas")]
        public async Task<IHttpActionResult> GetSeccionesLicenciasActivas()
        {
            var secciones = await new SeccionBO().GetSeccionesLicenciasActivas();
            var listado = Mapear<IEnumerable<GENTEMAR_SECCION_LICENCIAS>, IEnumerable<SeccionDTO>>(secciones);
            return Ok(listado);
        }


        /// <summary>
        /// Listado de secciones activas por id de actividad
        /// </summary>
        /// <returns>Listado de secciones por id de actividad</returns>
        /// <param name="id"></param>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>2022/02/26</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(List<SeccionDTO>))]
        [AuthorizeRolesFilter(RolesEnum.Consultas, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        [HttpGet]
        [Route("lista-actividad-licencia/{id}")]
        public async Task<IHttpActionResult> GetSeccionesActividadId(int id)
        {
            var secciones = await new SeccionBO().GetSeccionesActividad(id);
            return Ok(secciones);
        }


        /// <summary>
        /// Listado de secciones activas por varios ids de actividad
        /// </summary>
        /// <returns>Listado de secciones por varios ids de actividad</returns>
        /// <param name="ids">parametro que trae los ids de activdades</param>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>28/11/2023</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(List<SeccionDTO>))]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        [HttpPost]
        [Route("lista-por-actividades-licencia")]
        public async Task<IHttpActionResult> GetSeccionesPorActividades(List<int> ids)
        {
            var secciones = await new SeccionBO().GetSeccionesPorActividadesIds(ids);
            return Ok(secciones);
        }


        /// <summary>
        ///  seccion de licencia por id
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/04/2022</Fecha>
        /// </remarks>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(ResponseTypeSwagger<SeccionDTO>))]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        [HttpGet]
        [Route("por-licencia/{id}")]
        public async Task<IHttpActionResult> GetSeccionLicencia(int id)
        {
            var seccion = await new SeccionBO().GetSeccionLicencia(id);
            var obj = Mapear<GENTEMAR_SECCION_LICENCIAS, SeccionDTO>((GENTEMAR_SECCION_LICENCIAS)seccion.Data);
            seccion.Data = obj;
            return Ok(seccion);
        }



        /// <summary>
        /// Servicio para crear una seccion de una licencia
        /// </summary>
        /// <param name="seccion"></param>
        /// <returns></returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. Ya existe el nombre de la seccion de la licencia de navegación.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(ResponseCreatedTypeSwagger))]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        [HttpPost]
        [Route("crear-por-licencia")]
        public async Task<IHttpActionResult> CrearSeccionLicencia([FromBody] SeccionDTO seccion)
        {
            var sec = Mapear<SeccionDTO, GENTEMAR_SECCION_LICENCIAS>(seccion);
            var act = Mapear<IList<ActividadTipoLicenciaDTO>, IList<GENTEMAR_ACTIVIDAD>>(seccion.Actividad);
            var response = await new SeccionBO().CrearSeccionLicencia(sec, act);
            return Created(string.Empty, response);
        }


        /// <summary>
        /// Servicio para editar una seccion de una licencia
        /// </summary>
        /// <param name="seccion"></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/04/2022</Fecha>
        /// </remarks>
        /// <response code="200">OK. Devuelve el mensaje de actualización.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. Ya existe el nombre de la sección de la licencia de navegación.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        [HttpPut]
        [Route("editar-por-licencia")]
        public async Task<IHttpActionResult> EditarSeccionLicencia([FromBody] SeccionDTO seccion)
        {
            var sec = Mapear<SeccionDTO, GENTEMAR_SECCION_LICENCIAS>(seccion);
            var act = Mapear<IList<ActividadTipoLicenciaDTO>, IList<GENTEMAR_ACTIVIDAD>>(seccion.Actividad);
            var response = await new SeccionBO().EditarSeccionLicencia(sec, act);
            return Ok(response);
        }


        /// <summary>
        /// Servicio para Inactivar una sección de una licencia
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
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        [HttpPut]
        [Route("inactivar-por-licencia/{id}")]
        public async Task<IHttpActionResult> InactivarSeccionLicencia(int id)
        {
            var obj = await new SeccionBO().InactivarSeccionLicencia(id);
            return Ok(obj);
        }
        #endregion

    }
}
