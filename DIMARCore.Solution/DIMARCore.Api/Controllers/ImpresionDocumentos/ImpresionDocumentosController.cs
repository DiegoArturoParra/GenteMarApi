
using DIMARCore.Business;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers
{
    /// <summary>
    /// servicios de Secciones, para titulos y licencias
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/impresionDocumentos")]
    public class ImpresionDocumentosController : BaseApiController
    {

        private readonly ImpresionDocumentosBO _service;


        public ImpresionDocumentosController()
        {
            _service = new ImpresionDocumentosBO();
        }
        #region secciones Previstas

        /// <summary>
        ///  Generacion de la prevista del documento 
        /// </summary>
        /// <param name="idLicencia"></param>
        /// <Fecha>2023/07/31</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información de la prevista de una licencia.</response>
        /// <response code="204">No Content. No hay prevista.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(ImpresionDocumentoDTO))]
        [HttpGet]
        [Route("previstaLicencia/{idLicencia}")]
        public async Task<IHttpActionResult> GetPrevistaLicencias(int idLicencia)
        {
            var data = await _service.GetPrevistaLicencias(idLicencia);
            return Ok(data);
        }
        /// <summary>
        ///  Generacion de la prevista del Titulo
        /// </summary>
        /// <param name="idTitulo"></param>
        /// <Fecha>2023/07/31</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información de la prevista de una licencia.</response>
        /// <response code="204">No Content. No hay prevista.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(ImpresionDocumentoDTO))]
        [HttpGet]
        [Route("previstaTitulo/{idTitulo}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetPrevistaTitulo(int idTitulo)
        {
            var data = await _service.GetPrevistaTitulo(idTitulo);
            return Ok(data);
        }

        /// <summary>
        ///  Gurdado de la prevista del documento 
        /// </summary>
        /// <param name="impresionDocumento"></param>
        /// <Fecha>2023/07/31</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve ok  cuando el documento sw guardo exitosamente en 
        /// el servidor de archivos del sgda </response>
        /// <response code="204">No Content. No hay prevista.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(ImpresionDocumentoDTO))]
        [HttpPost]
        [Route("GuardarPrevista")]
        public async Task<IHttpActionResult> SavePrevistas(ImpresionDocumentoDTO impresionDocumento)
        {
            await _service.SavePrevista(impresionDocumento);
            return Ok();
        }

        #endregion

    }
}
