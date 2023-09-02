using DIMARCore.Api.Core.Atributos;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
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
        private readonly EstupefacienteBO _serviceEstupefacientes;
        /// <summary>
        /// ctor
        /// </summary>
        public EstupefacienteController()
        {
            _serviceEstupefacientes = new EstupefacienteBO();
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
        [AuthorizeRoles(RolesEnum.AdministradorEstupefacientes,
            RolesEnum.GestorEstupefacientes,
            RolesEnum.JuridicaEstupefacientes,
            RolesEnum.ConsultasEstupefacientes)]

        public IHttpActionResult Paginar([FromBody] EstupefacientesFilter filtro)
        {

            if (filtro == null)
            {
                filtro = new EstupefacientesFilter();
            }

            var queryable = _serviceEstupefacientes.GetEstupefacientesByFiltro(filtro);
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
            var respuesta = await _serviceEstupefacientes.GetDatosGenteMarEstupefaciente(obj.IdentificacionConPuntos);
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
        [AuthorizeRoles(RolesEnum.AdministradorEstupefacientes, RolesEnum.JuridicaEstupefacientes, RolesEnum.GestorEstupefacientes,
            RolesEnum.ConsultasEstupefacientes)]
        [Route("info/{id}")]
        public async Task<IHttpActionResult> GetDetallePersonaEstupefaciente(long id)
        {
            var respuesta = await _serviceEstupefacientes.GetDetallePersonaEstupefaciente(id);
            return Ok(respuesta);
        }




        // GET: 
        /// <summary>
        ///  Get estupefacientes sin observaciones para edición masiva
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/06/2023</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve y extrae la lista de estupefacientes que no tienen observaciones.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(Respuesta))]
        [HttpPost]
        [AuthorizeRoles(RolesEnum.AdministradorEstupefacientes, RolesEnum.JuridicaEstupefacientes)]
        [Route("sin-observaciones-para-bulk")]
        public async Task<IHttpActionResult> GetEstupefacientesSinObservaciones(List<long> ids)
        {
            var respuesta = await _serviceEstupefacientes.GetEstupefacientesSinObservaciones(ids);
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
            var response = await _serviceEstupefacientes.CrearAsync(dataEstupefaciente, datosBasicos);
            return ResultadoStatus(response);
        }


        /// <summary>
        /// Servicio para editar un estupefaciente
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>08/07/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. se ha actualizado el recurso (estupefaciente).</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud con el estado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [HttpPut]
        [Route("editar")]
        [AuthorizeRoles(RolesEnum.AdministradorEstupefacientes, RolesEnum.GestorEstupefacientes,
            RolesEnum.JuridicaEstupefacientes, RolesEnum.ConsultasEstupefacientes)]
        public async Task<IHttpActionResult> Editar()
        {
            Respuesta respuesta = new Respuesta();
            var req = HttpContext.Current.Request;
            var archivo = req.Files["File"];
            var format = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
            var datos = req["Data"];
            if (datos == null)
                return ResultadoStatus(Responses.SetBadRequestResponse($"Objeto invalido de {nameof(EditInfoEstupefacienteDTO)}, debe enviar los datos correctos."));

            EditInfoEstupefacienteDTO estupefaciente = JsonConvert.DeserializeObject<EditInfoEstupefacienteDTO>(datos, dateTimeConverter);
            if (archivo != null)
                estupefaciente.Observacion.Archivo = archivo;

            ValidateModelAndThrowIfInvalid(estupefaciente);

            estupefaciente.CapitaniaId = GetIdCapitania();

            var dataEstupefaciente = Mapear<EditInfoEstupefacienteDTO, GENTEMAR_ANTECEDENTES>(estupefaciente);
            var datosBasicos = Mapear<EstupefacienteDatosBasicosDTO, GENTEMAR_ANTECEDENTES_DATOSBASICOS>(estupefaciente.DatosBasicos);
          

            respuesta = await _serviceEstupefacientes.EditarAsync(dataEstupefaciente, datosBasicos, PathActual);
            return ResultadoStatus(respuesta);
        }
        /// <summary>
        /// Servicio para editar masivamente estados de estupefacientes
        /// </summary>
        /// <param name="estupefacientes"></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>28/04/2023</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. se ha actualizado los recursos (estupefacientes).</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud con el estado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [HttpPut]
        [Route("edicion-masiva")]
        [AuthorizeRoles(RolesEnum.AdministradorEstupefacientes, RolesEnum.JuridicaEstupefacientes)]
        public async Task<IHttpActionResult> EditarBulk([FromBody] EditBulkEstupefacientesDTO estupefacientes)
        {
            var response = await _serviceEstupefacientes.EditBulk(estupefacientes);
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
            var respuesta = await _serviceEstupefacientes.GetRadicadosTitulosByDocumento(obj.IdentificacionConPuntos);
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
            var respuesta = await _serviceEstupefacientes.GetRadicadosLicenciasByDocumento(obj.IdentificacionConPuntos);
            return Ok(respuesta);
        }
    }
}
