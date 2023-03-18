
using DIMARCore.Business;
using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
namespace DIMARCore.Api.Controllers
{
    /// <summary>
    /// Api Territorios
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/territorios")]
    public class TerritorioController : BaseApiController
    {
        // <summary>
        /// Listado de territorios con información basica
        /// </summary>
        /// <returns>Listado de Limitaciones</returns>
        /// <Autor>Victor Fuentes</Autor>
        /// <Fecha>2021/12/21</Fecha>
        /// <UltimaActualizacion>2021/12/21 - Victor Fuentes - Creación del servicio</UltimaActualizacion>
        [HttpGet]
        [Route("lista")]
        [AllowAnonymous]
        public IHttpActionResult GetTerritorio()
        {
            var limitaciones = new TerritorioBO().GetTerritorios();
            var data = Mapear<IList<GENTEMAR_TERRITORIO>, IList<TerritorioDTO>>(limitaciones);
            return Ok(data);
        }

        // <summary>
        /// Listado de territorios activos con información basica
        /// </summary>
        /// <returns>Listado de Limitaciones</returns>
        /// <Autor>Victor Fuentes</Autor>
        /// <Fecha>2021/12/21</Fecha>
        /// <UltimaActualizacion>2021/12/21 - Victor Fuentes - Creación del servicio</UltimaActualizacion>
        [HttpGet]
        [Route("lista-activo")]
        [AllowAnonymous]
        public IHttpActionResult GetTerritorioActivo()
        {
            var limitaciones = new TerritorioBO().GetTerritoriosActivo();
            var data = Mapear<IList<GENTEMAR_TERRITORIO>, IList<TerritorioDTO>>(limitaciones);
            return Ok(data);
        }

        /// <summary>
        /// Retorna una territorio dado un Id
        /// </summary>
        /// <returns>Retorna una Limitacón dado un Id</returns>
        /// <Autor>Victor Fuentes</Autor>
        /// <Fecha>2021/12/21</Fecha>
        /// <UltimaActualizacion>2021/12/21 - Victor Fuentes - Creación del servicio</UltimaActualizacion>
        [HttpGet]
        [Route("id")]
        [AllowAnonymous]
        public IHttpActionResult GetTerritorioId(int id)
        {
            var limitacion = new TerritorioBO().GetTerritorio(id);
            return Ok(limitacion);
        }


        /// <summary>
        /// Crea una territorio
        /// </summary>
        /// <returns>Crea una Limitación</returns>
        /// <Autor>Victor Fuentes</Autor>
        /// <Fecha>2021/12/21</Fecha>
        /// <UltimaActualizacion>2021/12/21 - Victor Fuentes - Creación del servicio</UltimaActualizacion>
        [HttpPost]
        [Route("crear")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> CrearTerritorio(TerritorioDTO datos)
        {

            var data = Mapear<TerritorioDTO, GENTEMAR_TERRITORIO>(datos);
            var limitacion = await new TerritorioBO().CrearTerritorioAsync(data);
            return ResultadoStatus(limitacion);
        }


        /// <summary>
        /// Edita / Modifica un territorio
        /// </summary>
        /// <returns>Edita / Modifica una Limitación</returns>
        /// <Autor>Victor Fuentes</Autor>
        /// <Fecha>2021/12/21</Fecha>
        /// <UltimaActualizacion>2021/12/21 - Victor Fuentes - Creación del servicio</UltimaActualizacion>
        [HttpPut]
        [Route("editar")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> EditarTerritorio(TerritorioDTO datos)
        {
            var data = Mapear<TerritorioDTO, GENTEMAR_TERRITORIO>(datos);
            var limitacion = await new TerritorioBO().EditarTerritorioAsync(data);
            return ResultadoStatus(limitacion);
        }

        /// <summary>
        /// cambia ek estado de un territorio 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// /// <Fecha>2022/02/22</Fecha>
        /// <UltimaActualizacion>2021/12/21 - Camilo Vargas - Creación del servicio</UltimaActualizacion>
        [HttpPut]
        [Route("inhabilitar/{id}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> CambiarTerritorioAsync(int id)
        {
            var respuesta = await new TerritorioBO().cambiarTerritorio(id);
            return ResultadoStatus(respuesta);
        }


    }
}
