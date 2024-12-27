using DIMARCore.Api.Core.Filters;
using DIMARCore.Api.Core.Models;
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
        /// <response code="200">OK. Devuelve el listado de consolidados.</response>        
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(List<ConsolidadoDTO>))]
        [HttpGet]
        [AuthorizeRolesFilter(RolesEnum.AdministradorVCITE, RolesEnum.JuridicaVCITE, RolesEnum.ConsultasVCITE, RolesEnum.GestorVCITE)]
        [Route("listar")]
        public async Task<IHttpActionResult> GetConsolidados()
        {
            var query = await _serviceConsolidado.GetConsolidados();
            var listado = Mapear<IEnumerable<GENTEMAR_CONSOLIDADO>, IEnumerable<ConsolidadoDTO>>(query);
            return Ok(listado);
        }

        // GET: Datos de los consolidados de estupefacientes que estan en uso
        /// <summary>
        ///  Datos de los consolidados de estupefacientes que estan en uso
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>01/08/2023</Fecha>
        /// </remarks>
        /// <response code="200">OK. Devuelve el listado de consolidados.</response>        
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(ResponseTypeSwagger<List<ConsolidadoDTO>>))]
        [HttpGet]
        [AuthorizeRolesFilter(RolesEnum.AdministradorVCITE, RolesEnum.JuridicaVCITE, RolesEnum.GestorVCITE)]
        [Route("listar-en-uso")]
        public async Task<IHttpActionResult> GetConsolidadosEnUso()
        {
            var listado = await _serviceConsolidado.GetConsolidadosEnUso();
            return Ok(listado);
        }

        // GET: numero del siguiente consolidado de estupefacientes que se va a usar
        /// <summary>
        ///  numero del siguiente consolidado de estupefacientes que se va a usar
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>03/10/2023</Fecha>
        /// </remarks>
        /// <response code="200">OK. Devuelve el numero del siguiente consolidado de estupefacientes que se va a usar.</response>        
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(ResponseTypeSwagger<ConsolidadoDTO>))]
        [HttpGet]
        [AuthorizeRolesFilter(RolesEnum.AdministradorVCITE, RolesEnum.JuridicaVCITE)]
        [Route("siguiente-a-asignar")]
        public async Task<IHttpActionResult> GetConsolidadoNext()
        {
            var consolidadoNext = await _serviceConsolidado.GetConsolidadoNext();
            return Ok(consolidadoNext);
        }

        // GET: ids de estupefacientes por el numero de consolidado.
        /// <summary>
        /// ids de estupefacientes por el numero de consolidado.
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>01/08/2023</Fecha>
        /// </remarks>
        /// <response code="200">OK. Devuelve el listado de ids de estupefacientes por un consolidado.</response>        
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(ResponseTypeSwagger<List<long>>))]
        [HttpGet]
        [AuthorizeRolesFilter(RolesEnum.AdministradorVCITE, RolesEnum.JuridicaVCITE)]
        [Route("listar-ids-estupefaciente/{consolidadoId}")]
        public async Task<IHttpActionResult> GetEstupefacientesIdPorConsolidado(int consolidadoId)
        {
            var listado = await _serviceConsolidado.GetAllIdsEstupefacienteByConsolidado(consolidadoId);
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
        /// <response code="200">Ok. La solicitud ha exportado el archivo excel.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseTypeSwagger<ArchivoExcelDTO>))]
        [HttpPost]
        [Route("generar-excel-para-envio-a-entidades")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorVCITE, RolesEnum.JuridicaVCITE)]
        public async Task<IHttpActionResult> FolioExcelParaEnvioAConsultas([FromBody] CrearConsolidadoExcelDTO consolidadoDTO)
        {
            string email = GetEmail();
            var response = await _serviceConsolidado.GenerarExcelConConsolidadoDeEstupefacientes(email, consolidadoDTO);
            return Ok(response);
        }
    }
}
