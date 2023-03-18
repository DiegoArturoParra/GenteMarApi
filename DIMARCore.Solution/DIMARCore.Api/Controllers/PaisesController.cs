using DIMARCore.Business;
using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DIMARCore.Api.Controllers
{
    /// <summary>
    /// Api Paises
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/paises")]
    public class PaisesController : BaseApiController
    {
        /// <summary>
        /// Listado de paises/banderas
        /// </summary>
        /// <returns>Listado json de paises/banderas</returns>
        /// <entidad>Bandera</entidad>
        /// <tabla>t_nav_band</tabla>
        /// <Autor>Carlos Rodríguez</Autor>
        /// <Fecha>2020/05/26</Fecha>
        /// <UltimaActualizacion>2020/07/23 - Carlos Rodríguez - Cambio consulta para obtener los paises</UltimaActualizacion>
        [HttpGet]
        [Route("list-basic")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetPaises()
        {
            var paises =await  new PaisBO().GetPaises();
            var data = Mapear<IList<PAISES>, IList<PaisDTO>>(paises);
            return Ok(data);
        }


        /// <summary>
        /// PAIS COLOMBIA
        /// </summary>
        /// <returns>pais colombia</returns>
        /// <entidad>Bandera</entidad>
        /// <tabla>t_nav_band</tabla>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>7/03/2022</Fecha>
        /// <UltimaActualizacion>7/03/2022</UltimaActualizacion>
        [HttpGet]
        [Route("colombia")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetPaisColombia()
        {
            var pais = await new PaisBO().GetPaisColombia();
            var data = Mapear<IList<PAISES>, IList<PaisDTO>>(pais);
            return Ok(data);
        }

        /// <summary>
        /// Listado de paises/banderas extranjeros
        /// </summary>
        /// <returns>Listado json de paises/banderas</returns>
        /// <entidad>Bandera</entidad>
        /// <tabla>t_nav_band</tabla>
        /// <Autor>Carlos Rodríguez</Autor>
        /// <Fecha>2020/05/26</Fecha>
        /// <UltimaActualizacion>2020/07/23 - Carlos Rodríguez - Cambio consulta para obtener los paises</UltimaActualizacion>
        [HttpGet]
        [Route("extranjeros/list-basic")]
        [AllowAnonymous]
        public IHttpActionResult GetPaisesExtranjeros()
        {
            var paises = new PaisBO().GetPaisesExtranjeros();
            var result = (from p in paises
                          select new
                          {
                              Codigo = p.cod_pais,
                              Nombre = p.des_pais,
                              Sigla = p.sigla,
                              Sigla2 = p.sigla_2
                          }).ToList();
            return Ok(result);
        }


        [HttpGet]
        [Route("comunidad-andina/list-basic")]
        [AllowAnonymous]
        public IHttpActionResult GetPaisesComunidadAndina()
        {
            var result = new PaisBO().GetAll(x => x.esComunidadAndina == true) //&& !x.cod_pais.Equals(PaisConfig.COLOMBIA_CODIGO))
                                    .OrderBy(x => x.des_pais)
                                    .AsEnumerable()
                                    .Select(x => new { Codigo = x.cod_pais, Nombre = x.des_pais }).ToList();
            return Ok(result);
        }
    }
}
