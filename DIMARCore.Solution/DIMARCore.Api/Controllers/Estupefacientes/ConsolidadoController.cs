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

namespace DIMARCore.Api.Controllers.Estupefacientes
{
    /// <summary>
    /// Servicios para los consolidados de estupefacientes
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/consolidados")]
    public class ConsolidadoController : BaseApiController
    {
        private readonly ConsolidadoBO _serviceConsolidado;
        /// <summary>
        /// ctor
        /// </summary>
        public ConsolidadoController()
        {
            _serviceConsolidado = new ConsolidadoBO();
        }

        // GET: Datos de los consolidados de estupefacientes
        /// <summary>
        ///  Datos de los consolidados de estupefacientes
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>01/08/2023</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el listado de consolidados.</response>        
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(List<ConsolidadoDTO>))]
        [HttpGet]
        [AuthorizeRoles(RolesEnum.AdministradorEstupefacientes, RolesEnum.JuridicaEstupefacientes,
            RolesEnum.ConsultasEstupefacientes, RolesEnum.GestorEstupefacientes)]
        [Route("listar")]
        public async Task<IHttpActionResult> GetConsolidados()
        {
            var query = await _serviceConsolidado.GetConsolidados();
            var listado = Mapear<IEnumerable<GENTEMAR_CONSOLIDADO>, IEnumerable<ConsolidadoDTO>>(query);
            return Ok(listado);
        }

        /// <summary>
        /// Servicio para exportar archivo excel de estupefacientes en estado por iniciar
        /// </summary>
        /// <param></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>01/06/2023</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">Ok. La solicitud ha exportado el archivo excel.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [HttpPost]
        [Route("generar-excel-para-envio-a-entidades")]
        [AuthorizeRoles(RolesEnum.AdministradorEstupefacientes, RolesEnum.JuridicaEstupefacientes)]
        public async Task<IHttpActionResult> FolioExcelParaEnvioAConsultas([FromBody] CrearConsolidadoExcelDTO consolidadoDTO)
        {
            string email = GetEmail();
            var response = await _serviceConsolidado.GenerarExcelConConsolidadoDeEstupefacientes(email, consolidadoDTO);
            return ResultadoStatus(response);
        }
    }
}
