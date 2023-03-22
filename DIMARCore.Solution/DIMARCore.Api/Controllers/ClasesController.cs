using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DIMARCore.Api.Controllers
{
    /// <summary>
    /// Api Clases
    /// </summary>
    [Authorize]
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
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [HttpGet]
        [Route("lista-por-titulos")]
        public IHttpActionResult GetClasesTitulos([FromUri] ActivoDTO dto)
        {

            var clases = _serviceClaseTitulos.GetAll(dto != null ? dto.Activo : null);
            var listado = Mapear<IEnumerable<GENTEMAR_CLASE_TITULOS>, IEnumerable<ClaseDTO>>(clases);
            return Ok(listado);

        }

        /// <summary>
        ///  clase de titulo por id
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [HttpGet]
        [Route("por-titulo/{id}")]
        public async Task<IHttpActionResult> GetClaseitulo(int id)
        {
            var clase = await _serviceClaseTitulos.GetByIdAsync(id);
            if (clase.Estado)
            {
                var obj = Mapear<GENTEMAR_CLASE_TITULOS, ClaseDTO>((GENTEMAR_CLASE_TITULOS)clase.Data);
                clase.Data = obj;
            }
            return ResultadoStatus(clase);
        }



        /// <summary>
        /// Servicio para crear una clase de un titulo
        /// </summary>
        /// <param name="clase"></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. Ya existe el nombre de la clase del titulo de navegación.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [HttpPost]
        [Route("crear-por-titulo")]
        public async Task<IHttpActionResult> CrearSeccionTitulo([FromBody] ClaseDTO clase)
        {
            var data = Mapear<ClaseDTO, GENTEMAR_CLASE_TITULOS>(clase);
            var response = await _serviceClaseTitulos.CrearAsync(data);
            return ResultadoStatus(response);
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
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. Ya existe el nombre de la clase del titulo de navegación.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [HttpPut]
        [Route("editar-por-titulo")]
        public async Task<IHttpActionResult> EditarSeccionTitulo([FromBody] ClaseDTO clase)
        {
            var data = Mapear<ClaseDTO, GENTEMAR_CLASE_TITULOS>(clase);
            var response = await _serviceClaseTitulos.ActualizarAsync(data);
            return ResultadoStatus(response);
        }

        /// <summary>
        /// Servicio para Inactivar una clase de un titulo
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [HttpPut]
        [Route("anula-or-activa-por-titulo/{id}")]
        public async Task<IHttpActionResult> InactivarClaseTitulo(int id)
        {
            var obj = await _serviceClaseTitulos.AnulaOrActivaAsync(id);
            return ResultadoStatus(obj);
        }
        /// <summary>
        /// Servicio para Inactivar una clase de una licencia
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [HttpPut]
        [Route("anula-or-activa-por-titulo/{id}")]
        public async Task<IHttpActionResult> InactivarClaseLicencia(int id)
        {
            var obj = await _serviceClaseTitulos.AnulaOrActivaAsync(id);
            return ResultadoStatus(obj);
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
        [HttpGet]
        [Route("lista-por-licencias")]
        public IHttpActionResult GetClasesLicencias()
        {
            var clases = _serviceClaseLicencias.GetAllClaseLicencias();
            //var listado = Mapear<IEnumerable<GENTEMAR_CLASE_LICENCIAS>, IEnumerable<ClaseDTO>>(clases);
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
        /// <response code="200">OK. Devuelve la información del estado.</response>
        /// <response code="204">No Content. No hay estado.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [HttpGet]
        [Route("lista-clase-seccion-licencias/{id}")]
        public IHttpActionResult GetSeccionesActividadId(int id)
        {
            var secciones = _serviceClaseLicencias.GetClaseSecciones(id);
            //var listado = Mapear<IEnumerable<GENTEMAR_SECCION_LICENCIAS>, IEnumerable<SeccionDTO>>(id);
            return Ok(secciones);
        }

        /// <summary>
        ///  Se obtiene el listado de las clases activas de las licencias de navegación.
        /// </summary>
        /// <remarks>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [HttpGet]
        [Route("lista-por-licencias-activas")]
        public IHttpActionResult GetClasesLicenciasActivas()
        {
            var clases = _serviceClaseLicencias.GetAllClaseLicenciasActivas();
            var listado = Mapear<IEnumerable<GENTEMAR_CLASE_LICENCIAS>, IEnumerable<ClaseDTO>>(clases);
            return Ok(listado);

        }
        /// <summary>
        ///  clase de licencia por id
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [HttpGet]
        [Route("por-licencia/{id}")]
        public async Task<IHttpActionResult> GetClaseLicencia(int id)
        {
            var clase = await _serviceClaseTitulos.GetByIdAsync(id);
            if (clase.Estado)
            {
                var obj = Mapear<GENTEMAR_CLASE_LICENCIAS, ClaseDTO>((GENTEMAR_CLASE_LICENCIAS)clase.Data);
                clase.Data = obj;
            }
            return ResultadoStatus(clase);
        }



        /// <summary>
        /// Servicio para crear una clase de un titulo
        /// </summary>
        /// <param name="clase"></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. Ya existe el nombre de la clase de la licencia de navegación.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [HttpPost]
        [Route("crear-por-licencia")]
        public async Task<IHttpActionResult> CrearClaseLicencia([FromBody] ClaseDTO clase)
        {
            var data = Mapear<ClaseDTO, GENTEMAR_CLASE_LICENCIAS>(clase);
            var sec = Mapear<IList<SeccionDTO>, IList<GENTEMAR_SECCION_LICENCIAS>>(clase.Seccion);
            var response = await _serviceClaseLicencias.CrearAsync(data, sec);
            return ResultadoStatus(response);
        }


        /// <summary>
        /// Servicio para editar una clase de una licencia
        /// </summary>
        /// <param name="clase"></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. Ya existe el nombre de la clase de la licencia de navegación.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [HttpPut]
        [Route("editar-por-licencia")]
        public async Task<IHttpActionResult> EditarClaseLicencia([FromBody] ClaseDTO clase)
        {
            var data = Mapear<ClaseDTO, GENTEMAR_CLASE_LICENCIAS>(clase);
            var sec = Mapear<IList<SeccionDTO>, IList<GENTEMAR_SECCION_LICENCIAS>>(clase.Seccion);
            var response = await _serviceClaseLicencias.ActualizarAsync(data, sec);
            return ResultadoStatus(response);
        }
        /// <summary>
        /// Servicio para Inactivar una clase de una licencia
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [HttpPut]
        [Route("anula-or-activa-por-licencia/{id}")]
        public async Task<IHttpActionResult> InactivarLicencia(int id)
        {
            var obj = await _serviceClaseLicencias.AnulaOrActivaAsync(id);
            return ResultadoStatus(obj);
        }


        #endregion
    }
}
