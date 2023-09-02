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
    /// Servicios para los expedientes de estupefacientes
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/expedientes")]
    public class ExpedienteController : BaseApiController
    {
        private readonly ExpedienteBO _serviceExpediente;
        /// <summary>
        /// ctor
        /// </summary>
        public ExpedienteController()
        {
            _serviceExpediente = new ExpedienteBO();
        }

        // GET: Datos de los expedientes de estupefacientes
        /// <summary>
        ///  Datos de los expedientes de estupefacientes
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>01/08/2023</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el listado de expedientes.</response>        
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(List<ExpedienteDTO>))]
        [HttpGet]
        [AuthorizeRoles(RolesEnum.AdministradorEstupefacientes, RolesEnum.JuridicaEstupefacientes,
            RolesEnum.ConsultasEstupefacientes, RolesEnum.GestorEstupefacientes)]
        [Route("listar")]
        public async Task<IHttpActionResult> GetExpedientes()
        {
            var query = await _serviceExpediente.GetExpedientes();
            var listado = Mapear<IEnumerable<GENTEMAR_EXPEDIENTE>, IEnumerable<ExpedienteDTO>>(query);
            return Ok(listado);
        }
        // GET: Datos de los expedientes de estupefacientes por consolidado
        /// <summary>
        ///  Datos de los expedientes de estupefacientes
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>01/08/2023</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el listado de expedientes.</response>        
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(List<ExpedienteDTO>))]
        [HttpGet]
        [AuthorizeRoles(RolesEnum.AdministradorEstupefacientes, RolesEnum.JuridicaEstupefacientes,
            RolesEnum.ConsultasEstupefacientes, RolesEnum.GestorEstupefacientes)]
        [Route("listar-por-consolidado")]
        public async Task<IHttpActionResult> GetExpedientesPorConsolidado([FromUri] int consolidadoId)
        {
            var listado = await _serviceExpediente.GetExpedientesPorConsolidado(consolidadoId);
            return Ok(listado);
        }
    }
}
