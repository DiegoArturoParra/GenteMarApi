﻿using DIMARCore.Api.Core.Filters;
using DIMARCore.Api.Core.Models;
using DIMARCore.Business;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers
{
    /// <summary>
    /// servicios Usuarios
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/usuarios")]
    public class UsuariosController : BaseApiController
    {
        /// <summary>
        /// Servicio que retorna el listado de usuarios del directorio activo.
        /// </summary>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>01/03/2023</Fecha>
        /// <returns></returns>
        /// <response code="401">Unauthorized. El usuario no esta autorizado..</response>   
        /// <response code="200">OK. Devuelve la información del listado.</response>           
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <param name="filtro">filtro para mostrar usuarios dependiendo el (nombre,identificacion y login-name)</param>
        /// <returns></returns>
        [ResponseType(typeof(ResponseTypeSwagger<List<UserDirectory>>))]
        [HttpGet]
        [Route("listar-directorio-activo")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        public IHttpActionResult GetUsuariosPorDirectorioActivo([FromUri] ActiveDirectoryFilter filtro)
        {
            var listado = new UsuarioBO().GetUsuariosPorDirectorioActivo(filtro);
            return Ok(listado);
        }


        /// <summary>
        /// Servicio que retorna los usuarios registrados en Gente de mar.
        /// </summary>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>25/02/2023</Fecha>
        /// <returns></returns>
        /// <response code="401">Unauthorized. El usuario no esta autorizado.</response>   
        /// <response code="200">OK. Devuelve la información del listado de menu.</response>           
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(ResponseTypeSwagger<List<RolSessionDTO>>))]
        [HttpGet]
        [Route("listar-roles-GDM")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetRoles()
        {
            var listado = await new AplicacionRolesBO().GetRoles();
            return Ok(listado);
        }

        /// <summary>
        /// Servicio que retorna los roles de gente de mar.
        /// </summary>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>25/02/2023</Fecha>
        /// <returns></returns>
        /// <response code="401">Unauthorized. El usuario no esta autorizado.</response>   
        /// <response code="200">OK. Devuelve la información del listado de menu.</response>           
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(ResponseTypeSwagger<List<InfoUsuarioDTO>>))]
        [HttpGet]
        [Route("listar")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetUsuarios()
        {
            var listado = await new UsuarioBO().GetUsuarios();
            return Ok(listado);
        }

        /// <summary>
        /// Servicio que retorna el menu correspondiente al usuario logueado.
        /// </summary>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>01/07/2022</Fecha>
        /// <returns></returns>
        /// <response code="401">Unauthorized. El usuario no esta autorizado.</response>   
        /// <response code="200">OK. Devuelve la información del listado de menu.</response>           
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(ResponseTypeSwagger<List<MenuDTO>>))]
        [HttpGet]
        [Authorize]
        [Route("menu")]
        public async Task<IHttpActionResult> GetMenuPorUsuarioLoginName()
        {
            var listado = await new MenuBO().GetMenuPorUsuarioLoginName(GetIdAplicacion(), GetLoginName());
            return Ok(listado);
        }

        /// <summary>
        /// Servicio para crear un usuario en el sistema de gente de mar.
        /// </summary>        
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>23/02/2023</Fecha>
        /// </remarks>
        /// <param name="usuario">objeto para crear un usuario.</param>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="201">Created. la solicitud ha tenido éxito y ha llevado a la creación del usuario en el sistema de gente de mar.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud con el estado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseCreatedTypeSwagger))]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        [HttpPost]
        [Route("crear-usuario-GDM")]
        public async Task<IHttpActionResult> CrearUsuarioGDM([FromBody] UsuariogdmDTO usuario)
        {
            var response = await new UsuarioBO().CreateUserGDM(usuario);
            return Created(string.Empty, response);
        }

        /// <summary>
        /// Servicio para crear un usuario en la aplicación gente de mar.
        /// </summary>        
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>24/02/2023</Fecha>
        /// </remarks>
        /// <param name="usuario">objeto para crear un usuario.</param>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="201">Created. la solicitud ha tenido éxito y ha llevado a la creación de la entidad de estupefaciente.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud con el estado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        [HttpPut]
        [Route("editar-usuario-GDM")]
        public async Task<IHttpActionResult> EditarUsuarioGDM([FromBody] UsuariogdmDTO usuario)
        {
            var response = await new UsuarioBO().UpdateUserGDM(usuario);
            return Ok(response);
        }

        /// <summary>
        /// Servicio para activar o retirar un usuario del sistema de gente de mar.
        /// </summary>        
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>28/04/2022</Fecha>
        /// </remarks>
        /// <param name="id">id del usuario de aplicaciones_login.</param>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. la solicitud ha tenido éxito y ha llevado a la creación de la entidad de estupefaciente.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud con el estado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        [HttpPut]
        [Route("retirar-o-activar/{id}")]
        public async Task<IHttpActionResult> InactivarOActivarUsuario(int id)
        {
            var response = await new UsuarioBO().InactivarOActivarUsuarioGDM(id);
            return Ok(response);
        }
    }
}
