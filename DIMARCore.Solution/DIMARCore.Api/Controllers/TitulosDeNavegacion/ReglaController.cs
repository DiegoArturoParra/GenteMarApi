using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DIMARCore.Api.Controllers.TitulosDeNavegacion
{
    /// <summary>
    /// servicios  reglas
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/reglas")]
    [Authorize]
    public class ReglaController : BaseApiController
    {
        private readonly ReglaBO _serviceReglas;

        /// <summary>
        /// constructor
        /// </summary>
        public ReglaController()
        {
            _serviceReglas = new ReglaBO();
        }
        /// <summary>
        /// Retorna la lista de reglas por cargo del titulo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("lista-by-cargo-titulo/{CargoId}")]
        public async Task<IHttpActionResult> ReglasByCargoTitulo(int CargoId)
        {
            var existeCargotitulo = await new ReglaBO().Validaciones(CargoId);
            if (existeCargotitulo.Estado)
            {
                var listado = await _serviceReglas.GetReglasByCargoTitulo(CargoId);
        
                return Ok(listado);
            }
            return ResultadoStatus(existeCargotitulo);
        }

        /// <summary>
        /// Listado de reglas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("lista")]
        public IHttpActionResult ListReglas([FromUri] ActivoDTO dto)
        {
                var query = _serviceReglas.GetAll(dto != null ? dto.Activo : null);
            var listado = Mapear<IEnumerable<GENTEMAR_REGLAS>, IEnumerable<ReglaDTO>>(query);
            return Ok(listado);
        }


        /// <summary>
        /// servicio get regla
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> GetRegla(int id)
        {
            var regla = await _serviceReglas.GetByIdAsync(id);
            if (regla.Estado)
            {
                var obj = Mapear<GENTEMAR_REGLAS, ReglaDTO>((GENTEMAR_REGLAS)regla.Data);
                regla.Data = obj;
            }
            return ResultadoStatus(regla);
        }



        /// <summary>
        /// crear una regla
        /// </summary>
        /// <param name="regla"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("crear")]
        public async Task<IHttpActionResult> Crear([FromBody] ReglaDTO regla)
        {
            var data = Mapear<ReglaDTO, GENTEMAR_REGLAS>(regla);
            var response = await _serviceReglas.CrearAsync(data);
            return ResultadoStatus(response);
        }


        /// <summary>
        /// editar una regla
        /// </summary>
        /// <param name="regla"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task<IHttpActionResult> Editar([FromBody] ReglaDTO regla)
        {
            var data = Mapear<ReglaDTO, GENTEMAR_REGLAS>(regla);
            var response = await _serviceReglas.ActualizarAsync(data);
            return ResultadoStatus(response);
        }

        /// <summary>
        /// Servicio para Inactivar una regla
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        [HttpPut]
        [Route("anula-or-activa/{id}")]
        public async Task<IHttpActionResult> AnularOrActivar(int id)
        {
            var response = await _serviceReglas.AnulaOrActivaAsync(id);
            return ResultadoStatus(response);
        }
    }
}
