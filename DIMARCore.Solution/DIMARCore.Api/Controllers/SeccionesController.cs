
using DIMARCore.Business;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DIMARCore.Api.Controllers
{
    /// <summary>
    /// servicios de Secciones, para titulos y licencias
    /// </summary>
    [Authorize]
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/secciones")]
    public class SeccionesController : BaseApiController
    {
        #region secciones de titulos

        /// <summary>
        /// Listado de secciones por titulos con información basica
        /// </summary>
        /// <returns>Listado de secciones</returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        [HttpGet]
        [Route("lista-por-titulos")]
        public IHttpActionResult GetSeccionesTitulos([FromUri] ActivoDTO dto)
        {
            try
            {
                var secciones = new SeccionBO().GetSeccionesTitulos(dto != null ? dto.Activo : null);
                var listado = Mapear<IEnumerable<GENTEMAR_SECCION_TITULOS>, IEnumerable<SeccionDTO>>(secciones);
                return Ok(listado);
            }
            catch (Exception ex)

            {
                return Content(System.Net.HttpStatusCode.InternalServerError, new Respuesta
                {
                    MensajeExcepcion = ex.Message,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                });
            }
        }

        /// <summary>
        ///  seccion por titulo con información basica
        /// </summary>
        /// <returns>Listado de secciones</returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        [HttpGet]
        [Route("por-titulo/{id}")]
        public async Task<IHttpActionResult> GetSeccionTitulo(int id)
        {
            var seccion = await new SeccionBO().GetSeccionTitulo(id);
            if (seccion.Estado)
            {
                var obj = Mapear<GENTEMAR_SECCION_TITULOS, SeccionDTO>((GENTEMAR_SECCION_TITULOS)seccion.Data);
                seccion.Data = obj;
            }
            return ResultadoStatus(seccion);
        }



        /// <summary>
        /// Servicio para crear una sección de un titulo
        /// </summary>
        /// <param name="seccion"></param>
        /// <returns></returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        [HttpPost]
        [Route("crear-por-titulo")]
        public async Task<IHttpActionResult> CrearSeccionTitulo([FromBody] SeccionDTO seccion)
        {
            var data = Mapear<SeccionDTO, GENTEMAR_SECCION_TITULOS>(seccion);
            var response = await new SeccionBO().CrearSeccionTitulo(data);
            return ResultadoStatus(response);
        }


        /// <summary>
        /// Servicio para editar una sección de un titulo
        /// </summary>
        /// <param name="seccion"></param>
        /// <returns></returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        [HttpPut]
        [Route("editar-por-titulo")]
        public async Task<IHttpActionResult> EditarSeccionTitulo([FromBody] SeccionDTO seccion)
        {
            var data = Mapear<SeccionDTO, GENTEMAR_SECCION_TITULOS>(seccion);
            var response = await new SeccionBO().EditarSeccionTitulo(data);
            return ResultadoStatus(response);
        }

        /// <summary>
        /// Servicio para Inactivar una sección de un titulo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        [HttpPut]
        [Route("anula-or-activa-por-titulo/{id}")]
        public async Task<IHttpActionResult> InactivarSeccionTitulo(int id)
        {
            var obj = await new SeccionBO().AnulaOrActivaSeccionTitulo(id);
            return ResultadoStatus(obj);
        }
        #endregion

        #region secciones de licencias
        /// <summary>
        /// Listado de secciones por licencias con información basica
        /// </summary>
        /// <returns>Listado de secciones por licencias</returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        [HttpGet]
        [Route("lista-por-licencias")]
        public IHttpActionResult GetSeccionesLicencias()
        {
            var secciones = new SeccionBO().GetSeccionesLicencias();
            //var listado = Mapear<IEnumerable<GENTEMAR_SECCION_LICENCIAS>, IEnumerable<SeccionDTO>>(secciones);
            return Ok(secciones);
        }

        /// <summary>
        /// Listado de secciones activas por licencias con información basica
        /// </summary>
        /// <returns>Listado de secciones por licencias</returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        [HttpGet]
        [Route("lista-por-licencias-activas")]
        public IHttpActionResult GetSeccionesLicenciasActivas()
        {
            var secciones = new SeccionBO().GetSeccionesLicenciasActivas();
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
        /// <response code="200">OK. Devuelve la información del estado.</response>
        /// <response code="204">No Content. No hay estado.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [HttpGet]
        [Route("lista-secciones-actividad-licencias/{id}")]
        public IHttpActionResult GetSeccionesActividadId(int id)
        {
            var secciones = new SeccionBO().GetSeccionesActividad(id);
            //var listado = Mapear<IEnumerable<GENTEMAR_SECCION_LICENCIAS>, IEnumerable<SeccionDTO>>(id);
            return Ok(secciones);
        }

        /// <summary>
        /// seccion de licencia
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        [HttpGet]
        [Route("por-licencia/{id}")]
        public async Task<IHttpActionResult> GetSeccionLicencia(int id)
        {
            var seccion = await new SeccionBO().GetSeccionLicencia(id);
            if (seccion.Estado)
            {
                var obj = Mapear<GENTEMAR_SECCION_LICENCIAS, SeccionDTO>((GENTEMAR_SECCION_LICENCIAS)seccion.Data);
                seccion.Data = obj;
            }
            return ResultadoStatus(seccion);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="seccion"></param>
        /// <returns></returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. Ya existe el nombre de la clase de la licencia de navegación.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [HttpPost]
        [Route("crear-por-licencia")]
        public async Task<IHttpActionResult> CrearSeccionLicencia([FromBody] SeccionDTO seccion)
        {
            var sec = Mapear<SeccionDTO, GENTEMAR_SECCION_LICENCIAS>(seccion);
            var act = Mapear<IList<ActividadDTO>, IList<GENTEMAR_ACTIVIDAD>>(seccion.Actividad);
            var response = await new SeccionBO().CrearSeccionLicencia(sec, act);
            return ResultadoStatus(response);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="seccion"></param>
        /// <returns></returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        [HttpPut]
        [Route("editar-por-licencia")]
        public async Task<IHttpActionResult> EditarSeccionLicencia([FromBody] SeccionDTO seccion)
        {
            var sec = Mapear<SeccionDTO, GENTEMAR_SECCION_LICENCIAS>(seccion);
            var act = Mapear<IList<ActividadDTO>, IList<GENTEMAR_ACTIVIDAD>>(seccion.Actividad);
            var response = await new SeccionBO().EditarSeccionLicencia(sec, act);
            return ResultadoStatus(response);
        }


        /// <summary>
        /// Servicio para Inactivar una sección de una licencia
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        [HttpPut]
        [Route("inactivar-por-licencia/{id}")]
        public async Task<IHttpActionResult> InactivarSeccionLicencia(int id)
        {
            var obj = await new SeccionBO().InactivarSeccionLicencia(id);
            return ResultadoStatus(obj);
        }

        #endregion

    }
}
