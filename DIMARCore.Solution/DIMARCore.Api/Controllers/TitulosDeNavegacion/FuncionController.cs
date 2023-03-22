using DIMARCore.Api.Core.Atributos;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Enums;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DIMARCore.Api.Controllers.TitulosDeNavegacion
{
    /// <summary>
    /// servicios para las funciones titulos de navegación
    /// <Autor>Diego Parra</Autor>
    /// 
    /// <Fecha>2022/02/26</Fecha>
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/funciones")]
    [AuthorizeRoles(RolesEnum.Administrador)]
    public class FuncionController : BaseApiController
    {

        private readonly FuncionBO _serviceFuncion;

        /// <summary>
        /// Constructor
        /// </summary>
        public FuncionController()
        {
            _serviceFuncion = new FuncionBO();
        }
        /// <summary>
        /// Retorna la lista de reglas por cargo del titulo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("lista-by-regla/{ReglaId}")]
        public async Task<IHttpActionResult> FuncionesByRegla(int ReglaId)
        {
            var existeRegla = await _serviceFuncion.Validaciones(ReglaId);
            if (existeRegla.Estado)
            {
                var query = await _serviceFuncion.GetFuncionesByRegla(ReglaId);
                var listado = Mapear<IEnumerable<GENTEMAR_REGLA_FUNCION>, IEnumerable<FuncionDTO>>(query);
                return Ok(listado);
            }
            return ResultadoStatus(existeRegla);
        }


        /// <summary>
        /// Listado de funciones
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("lista")]
        public IHttpActionResult Listado([FromUri] ActivoDTO dto)
        {
            var query = _serviceFuncion.GetAll(dto != null ? dto.Activo : null);
            var listado = Mapear<IEnumerable<GENTEMAR_FUNCIONES>, IEnumerable<FuncionDTO>>(query);
            return Ok(listado);
        }


        /// <summary>
        /// servicio get funcion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> GetFuncion(int id)
        {
            var entidad = await _serviceFuncion.GetByIdAsync(id);
            if (entidad.Estado)
            {
                var obj = Mapear<GENTEMAR_FUNCIONES, FuncionDTO>((GENTEMAR_FUNCIONES)entidad.Data);
                entidad.Data = obj;
            }
            return ResultadoStatus(entidad);
        }



        /// <summary>
        /// Servicio para crear una funcion
        /// </summary>
        /// <param name="funcion"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("crear")]
        public async Task<IHttpActionResult> Crear([FromBody] FuncionDTO funcion)
        {
            var data = Mapear<FuncionDTO, GENTEMAR_FUNCIONES>(funcion);
            var response = await _serviceFuncion.CrearAsync(data);
            return ResultadoStatus(response);
        }


        /// <summary>
        /// Servicio para editar una funcion
        /// </summary>
        /// <param name="funcion"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("editar")]
        public async Task<IHttpActionResult> Editar([FromBody] FuncionDTO funcion)
        {
            var data = Mapear<FuncionDTO, GENTEMAR_FUNCIONES>(funcion);
            var response = await _serviceFuncion.ActualizarAsync(data);
            return ResultadoStatus(response);
        }

        /// <summary>
        /// Servicio para Inactivar una funcion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        [HttpPut]
        [Route("anula-or-activa/{id}")]
        public async Task<IHttpActionResult> AnularOrActivar(int id)
        {
            var response = await _serviceFuncion.AnulaOrActivaAsync(id);
            return ResultadoStatus(response);
        }
    }
}
