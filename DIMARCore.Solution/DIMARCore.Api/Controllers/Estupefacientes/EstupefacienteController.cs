using DIMARCore.Api.Core.Atributos;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.Utilities.CorreoSMTP;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers.Estupefacientes
{
    /// <summary>
    /// servicios para l de estupefacientes
    /// <Autor>Diego Parra</Autor>
    /// <Fecha>08/07/2022</Fecha>
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/estupefacientes")]
    public class EstupefacienteController : BaseApiController
    {
        private readonly EstupefacienteBO _service;
        /// <summary>
        /// 
        /// </summary>
        public EstupefacienteController()
        {
            _service = new EstupefacienteBO();
        }
        /// <summary>
        /// Listado de estupefacientes dependiendo el filtro.
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información de estupefacientes mediante filtros.</response>
        /// <response code="204">No Content. No hay titulos.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response> 
        [ResponseType(typeof(Paginador<ListadoEstupefacientesDTO>))]
        [HttpPost]
        [Route("filtro")]
        [AuthorizeRoles(RolesEnum.AdministradorEstupefacientes, RolesEnum.GestorEstupefacientes, RolesEnum.JuridicaEstupefacientes, RolesEnum.ConsultasEstupefacientes)]
        //[AuthorizeRoles(RolesEnum.Administrador, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.Consultas, RolesEnum.ASEPAC)]
        public IHttpActionResult Paginar([FromBody] EstupefacientesFilter filtro)
        {

            if (filtro == null)
            {
                filtro = new EstupefacientesFilter();
            }

            var queryable = _service.GetEstupefacientesByFiltro(filtro);
            if (queryable == null)
            {
                return Content(HttpStatusCode.NoContent,
                    new Respuesta() { Mensaje = "No hay datos de estupefacientes.", StatusCode = HttpStatusCode.NoContent });
            }

            var listado = GetPaginacion(filtro.Paginacion, queryable);
            var paginador = Paginador<ListadoEstupefacientesDTO>.CrearPaginador(queryable.Count(), listado, filtro.Paginacion);
            return Ok(paginador);

        }






        // GET: Datos de gente de mar en estupefacientes
        /// <summary>
        ///  Datos de gente de mar en estupefacientes
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>28/04/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(Respuesta))]
        [HttpGet]
        [AuthorizeRoles(RolesEnum.AdministradorEstupefacientes, RolesEnum.GestorEstupefacientes,
            RolesEnum.JuridicaEstupefacientes, RolesEnum.ConsultasEstupefacientes)]
        [Route("datos-gentemar-por-cedula")]
        public async Task<IHttpActionResult> GetGenteMarEstupefaciente([FromUri] CedulaDTO obj)
        {
            var respuesta = await new EstupefacienteBO().GetDatosGenteMarEstupefaciente(obj.IdentificacionConPuntos);
            return Ok(respuesta);
        }



        // GET: Información completa de un registro de estupefaciente  
        /// <summary>
        ///  Get estupefaciente por id
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>28/12/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <param name="id"> id estupefaciente</param>
        /// <returns></returns>
        [ResponseType(typeof(Respuesta))]
        [HttpGet]
        [AuthorizeRoles(RolesEnum.AdministradorEstupefacientes, RolesEnum.JuridicaEstupefacientes)]
        [Route("info/{id}")]
        public async Task<IHttpActionResult> GetDetallePersonaEstupefaciente(long id)
        {
            var respuesta = await new EstupefacienteBO().GetDetallePersonaEstupefaciente(id);
            return Ok(respuesta);
        }

        /// <summary>
        /// Servicio para agregar estupefaciente
        /// </summary>
        /// <param name="estupefaciente"></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>08/07/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="201">Created. la solicitud ha tenido éxito y ha llevado a la creación de la tramite de estupefaciente.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [HttpPost]
        [Route("crear")]
        [AuthorizeRoles(RolesEnum.AdministradorEstupefacientes, RolesEnum.GestorEstupefacientes,
            RolesEnum.JuridicaEstupefacientes, RolesEnum.ConsultasEstupefacientes)]
        public async Task<IHttpActionResult> Crear([FromBody] EstupefacienteCrearDTO estupefaciente)
        {
            var dataEstupefaciente = Mapear<EstupefacienteCrearDTO, GENTEMAR_ANTECEDENTES>(estupefaciente);
            var datosBasicos = Mapear<EstupefacienteDatosBasicosDTO, GENTEMAR_ANTECEDENTES_DATOSBASICOS>(estupefaciente.DatosBasicos);

            dataEstupefaciente.id_capitania = GetIdCapitania();
            var response = await _service.CrearAsync(dataEstupefaciente, datosBasicos);
            return ResultadoStatus(response);
        }


        /// <summary>
        /// Servicio para editar una tramite
        /// </summary>
        /// <param name="estupefaciente"></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>08/07/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. se ha actualizado el recurso (capacidad).</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud con el estado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [HttpPut]
        [Route("editar")]
        [AuthorizeRoles(RolesEnum.AdministradorEstupefacientes, RolesEnum.GestorEstupefacientes,
            RolesEnum.JuridicaEstupefacientes, RolesEnum.ConsultasEstupefacientes)]
        public async Task<IHttpActionResult> Editar([FromBody] EstupefacienteCrearDTO estupefaciente)
        {
            var data = Mapear<EstupefacienteCrearDTO, GENTEMAR_ANTECEDENTES>(estupefaciente);
            var response = await _service.EditarAsync(data);
            return Ok(response);
        }


        // GET: radicados-sgdea-titulos
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
        [ResponseType(typeof(List<string>))]
        [HttpGet]
        [AuthorizeRoles(RolesEnum.AdministradorEstupefacientes, RolesEnum.GestorEstupefacientes,
            RolesEnum.JuridicaEstupefacientes, RolesEnum.ConsultasEstupefacientes)]
        [Route("radicados-sgdea-titulos")]
        public async Task<IHttpActionResult> GetRadicadosTitulos([FromUri] CedulaDTO obj)
        {
            var respuesta = await new EstupefacienteBO().GetRadicadosTitulosByDocumento(obj.IdentificacionConPuntos);
            return Ok(respuesta);
        }


        // GET: radicados-sgdea-licencias
        /// <summary>
        ///  Listado de radicados de las licencias de navegación
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>28/04/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(List<string>))]
        [HttpGet]
        [AuthorizeRoles(RolesEnum.AdministradorEstupefacientes, RolesEnum.GestorEstupefacientes,
            RolesEnum.JuridicaEstupefacientes, RolesEnum.ConsultasEstupefacientes)]
        [Route("radicados-sgdea-licencias")]
        public async Task<IHttpActionResult> GetRadicadosLicencias([FromUri] CedulaDTO obj)
        {
            var respuesta = await new EstupefacienteBO().GetRadicadosLicenciasByDocumento(obj.IdentificacionConPuntos);
            return Ok(respuesta);
        }
    }
}
