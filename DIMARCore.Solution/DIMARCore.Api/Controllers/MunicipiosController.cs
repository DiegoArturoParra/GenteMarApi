using DIMARCore.Business;
using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DIMARCore.Api.Controllers
{
    /// <summary>
    /// Api Paises
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/municipios")]
    public class MunicipiosController : BaseApiController
    {
        /// <summary>
        /// Listado de Municipios
        /// </summary>
        /// <returns>Listado json de municipios</returns>
        /// <entidad>APLICACIONES_MUNICIPIO</entidad>
        /// <tabla>APLICACIONES_MUNICIPIO</tabla>
        /// <Autor>Camilo Vargas </Autor>
        /// <Fecha>2022/02/22</Fecha>
        /// <UltimaActualizacion>2022/02/22 - Camilo Vargas - Crear metodo</UltimaActualizacion>
        [HttpGet]
        [Route("lista")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetMunicipiosAsync()
        {
            var municipios = await new MunicipiosBO().GetMunicipios();
            var resultado = Mapear<IList<APLICACIONES_MUNICIPIO>, IList<MunicipioDTO>>(municipios);
            return Ok(resultado);
        }

    }
}
