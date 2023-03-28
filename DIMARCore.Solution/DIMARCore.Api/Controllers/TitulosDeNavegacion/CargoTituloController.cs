using DIMARCore.Api.Core.Atributos;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DIMARCore.Api.Controllers.TitulosDeNavegacion
{
    /// <summary>
    /// servicios para los cargos de titulos
    /// <Autor>Diego Parra</Autor>  
    /// 
    /// <Fecha>2022/02/26</Fecha>
    /// </summary>
    [EnableCors("*", "*", "*")]
    [AllowAnonymous]
    [RoutePrefix("api/cargo-titulos")]
    [AuthorizeRoles(RolesEnum.Administrador)]
    public class CargoTituloController : BaseApiController
    {

        private readonly CargoTituloBO _service;

        /// <summary>
        /// Constructor
        /// </summary>
        public CargoTituloController()
        {
            _service = new CargoTituloBO();
        }
        /// <summary>
        /// Retorna los cargos de los titulos por cada sección.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("lista-by-seccion/{SeccionId}")]
        public async Task<IHttpActionResult> GetCargoTitulosBySeccionId(int SeccionId)
        {
            var query = await _service.GetCargoTitulosBySeccionId(SeccionId);
            if (query.Count() > 0)
            {
                var listado = Mapear<IEnumerable<GENTEMAR_CARGO_TITULO>, IEnumerable<CargoTituloDTO>>(query);
                return Ok(listado);
            }
            else
            {
                return Content(HttpStatusCode.NotFound, new Respuesta() { Mensaje = "No hay cargos con la sección indicada.", StatusCode = HttpStatusCode.NotFound });
            }
        }


        /// <summary>
        /// Listado de cargos de titulos de navegación
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("lista")]
        public IHttpActionResult Listado([FromUri] CargoTituloFilter dto)
        {
            var query = _service.GetAll(dto);
            return Ok(query);
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
        public async Task<IHttpActionResult> GetCargoTitulo(int id)
        {
            var entidad = await _service.GetByIdAsync(id);
            if (entidad.Estado)
            {
                var obj = Mapear<GENTEMAR_CARGO_TITULO, CargoTituloDTO>((GENTEMAR_CARGO_TITULO)entidad.Data);
                entidad.Data = obj;
            }
            return ResultadoStatus(entidad);
        }



        /// <summary>
        /// Servicio para crear un cargo de un titulo de navegación
        /// </summary>
        /// <param name="cargo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("crear")]
        public async Task<IHttpActionResult> Crear([FromBody] CargoTituloDTO cargo)
        {
            var data = Mapear<CargoTituloDTO, GENTEMAR_CARGO_TITULO>(cargo);
            var response = await _service.CrearAsync(data);
            return ResultadoStatus(response);
        }


        /// <summary>
        /// Servicio para editar un cargo de un titulo de navegación
        /// </summary>
        /// <param name="cargo"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("editar")]
        public async Task<IHttpActionResult> Editar([FromBody] CargoTituloDTO cargo)
        {
            var data = Mapear<CargoTituloDTO, GENTEMAR_CARGO_TITULO>(cargo);
            var response = await _service.ActualizarAsync(data);
            return ResultadoStatus(response);
        }

        /// <summary>
        /// Servicio para Inactivar un cargo de un titulo de navegación
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/03/2022</Fecha>
        [HttpPut]
        [Route("anula-or-activa/{id}")]
        public async Task<IHttpActionResult> AnularOrActivar(int id)
        {
            var response = await _service.AnulaOrActivaAsync(id);
            return ResultadoStatus(response);
        }
    }
}
