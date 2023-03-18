using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers
{
    /// <summary>
    /// Servicios para los radicados de licencias y titulos
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/sgdea")]
    [AllowAnonymous]
    public class SGDEAController : BaseApiController
    {


        // GET: radicados-titulos
        /// <summary>
        ///  Listado de radicados de los titulos de navegación
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>28/04/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(List<RadicadoDTO>))]
        [HttpPost]
        [Route("radicados-titulos")]
        public IHttpActionResult GetRadicadosTitulos(CedulaDTO obj)     
        {

            var respuesta = new SgdeaBO().GetRadicadosTitulosByCedula(obj.Identificacion);
            return Ok(respuesta);

        }
        // GET: radicados-licencias
        /// <summary>
        ///  Listado de radicados de las licencias 
        /// </summary>
        /// <remarks>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>28/04/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(List<RadicadoDTO>))]
        [HttpPost]
        [Route("radicados-licencias")]
        public IHttpActionResult GetRadicadosLicencias(CedulaDTO obj)
        {

            var respuesta = new SgdeaBO().GetRadicadosLicenciasByCedula(obj.Identificacion);
            return Ok(respuesta);

        }
    }
}
