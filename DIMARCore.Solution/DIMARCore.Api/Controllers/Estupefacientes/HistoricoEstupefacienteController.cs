using DIMARCore.Api.Core.Atributos;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers.Estupefacientes
{
    /// <summary>
    /// servicios para historico de estupefacientes (de aclaraciones y de una persona)
    /// <Autor>Diego Parra</Autor>
    /// <Fecha>23/03/2023</Fecha>
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/historico")]
    public class HistoricoEstupefacienteController : BaseApiController
    {
        private readonly HistoricoEstupefacienteBO _serviceHistorico;
        /// <summary>
        /// ctor
        /// </summary>
        public HistoricoEstupefacienteController()
        {
            _serviceHistorico = new HistoricoEstupefacienteBO();
        }

        // GET: Historico de estupefacientes de una persona seleccionada
        /// <summary>
        /// Historico de estupefacientes de una persona seleccionada
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>29/03/2023</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(Respuesta))]
        [HttpGet]
        [AuthorizeRoles(RolesEnum.AdministradorVCITE, RolesEnum.GestorVCITE,
            RolesEnum.JuridicaVCITE, RolesEnum.ConsultasVCITE)]
        [Route("estupefacientes-por-persona")]
        public async Task<IHttpActionResult> GetHistoricoEstupefacientesPorPersona([FromUri] CedulaDTO obj)
        {
            var respuesta = await _serviceHistorico.GetHistoricoEstupefacientesPorPersona(obj);
            return Ok(respuesta);
        }
    }
}
