using DIMARCore.Api.Core.Atributos;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Enums;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers.TitulosDeNavegacion
{
    /// <summary>
    /// servicios para las habilitaciones del titulo de navegación
    /// <Autor>Diego Parra</Autor>
    /// versión 1.0
    /// 2022/02/26
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/habilitaciones")]
    public class HabilitacionController : BaseApiController
    {

        private readonly HabilitacionBO _serviceHabilitacion;
        /// <summary>
        /// Constructor
        /// </summary>
        public HabilitacionController()
        {
            _serviceHabilitacion = new HabilitacionBO();
        }


        /// <summary>
        /// Servicio para el listado de habilitaciones dependiendo la regla y el cargo.
        /// </summary>
        /// <remarks>
        /// listado de habilitaciones con filtro.
        /// </remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>2022/02/26</Fecha>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado data.</response>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <param name="items">(IdsReglaCargoDTO) objeto que tiene los ids de regla y cargo (llave compuesta)</param>
        /// <returns></returns>

        [ResponseType(typeof(List<HabilitacionDTO>))]
        [HttpGet]
        [Route("lista-by-regla-cargo")]
        [AuthorizeRoles(RolesEnum.Administrador, RolesEnum.GestorSedeCentral)]
        public async Task<IHttpActionResult> GetHabilitacionesByReglaId([FromUri] IdsTablasForaneasDTO items)
        {
            var existeRelacion = await new ReglaCargoBO().GetIdByTablasForaneas(items);
            if (existeRelacion.Estado)
            {
                int Id = (int)existeRelacion.Data;
                var query = _serviceHabilitacion.GetHabilitacionesByReglaCargoId(Id);
                var listado = Mapear<IEnumerable<GENTEMAR_CARGO_HABILITACION>, IEnumerable<HabilitacionDTO>>(query);
                return Ok(listado);
            }
            return ResultadoStatus(existeRelacion);
        }


        /// <summary>
        /// Listado de habilitaciones
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("lista")]
        public IHttpActionResult Listado([FromUri] ActivoDTO dto)
        {
            var query = _serviceHabilitacion.GetAll(dto != null ? dto.Activo : null);
            var listado = Mapear<IEnumerable<GENTEMAR_HABILITACION>, IEnumerable<HabilitacionDTO>>(query);
            return Ok(listado);
        }


        /// <summary>
        /// servicio get habilitacion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> GetHabilitacion(int id)
        {
            var entidad = await _serviceHabilitacion.GetByIdAsync(id);
            if (entidad.Estado)
            {
                var obj = Mapear<GENTEMAR_HABILITACION, HabilitacionDTO>((GENTEMAR_HABILITACION)entidad.Data);
                entidad.Data = obj;
            }
            return ResultadoStatus(entidad);
        }



        /// <summary>
        /// Servicio para crear una habilitacion
        /// </summary>
        /// <param name="habilitacion"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("crear")]
        public async Task<IHttpActionResult> Crear([FromBody] HabilitacionDTO habilitacion)
        {
            var data = Mapear<HabilitacionDTO, GENTEMAR_HABILITACION>(habilitacion);
            var response = await _serviceHabilitacion.CrearAsync(data);
            return ResultadoStatus(response);
        }


        /// <summary>
        ///  Servicio para editar una habilitacion
        /// </summary>
        /// <param name="habilitacion"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("editar")]
        public async Task<IHttpActionResult> Editar([FromBody] HabilitacionDTO habilitacion)
        {
            var data = Mapear<HabilitacionDTO, GENTEMAR_HABILITACION>(habilitacion);
            var response = await _serviceHabilitacion.ActualizarAsync(data);
            return ResultadoStatus(response);
        }

        /// <summary>
        /// Servicio para Inactivar una habilitacion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        [HttpPut]
        [Route("anula-or-activa/{id}")]
        public async Task<IHttpActionResult> AnularOrActivar(int id)
        {
            var response = await _serviceHabilitacion.AnulaOrActivaAsync(id);
            return ResultadoStatus(response);
        }
    }
}
