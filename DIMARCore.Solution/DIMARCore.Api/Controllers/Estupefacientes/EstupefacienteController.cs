using DIMARCore.Api.Core.Filters;
using DIMARCore.Api.Core.Models;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
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
        /// <response code="204">No Content. No hay estupefacientes con los filtros correspondientes.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response> 
        [ResponseType(typeof(Paginador<ListadoEstupefacientesDTO>))]
        [HttpPost]
        [Route("filtro")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorVCITE, RolesEnum.GestorVCITE, RolesEnum.JuridicaVCITE,
            RolesEnum.ConsultasVCITE)]
        public async Task<IHttpActionResult> PaginarAsync([FromBody] EstupefacientesFilter filtro)
        {
            if (filtro == null)
            {
                filtro = new EstupefacientesFilter
                {
                    Paginacion = new ParametrosPaginacion()
                };
            }
            else if (filtro.Paginacion == null)
            {
                filtro.Paginacion = new ParametrosPaginacion();
            }

            var queryable = _serviceEstupefacientes.GetEstupefacientesByFiltro(filtro);
            if (!queryable.Any())
            {

                if (filtro.FechaInicial.HasValue && filtro.FechaFinal.HasValue)
                    return Content(HttpStatusCode.OK, Responses.SetOkResponse(null, $"No se encontraron resultados entre la fecha {filtro.FechaInicial:dd-MM-yyyy} - {filtro.FechaFinal:dd-MM-yyyy}"));
                else
                {
                    var fechaActual = DateTime.Now;
                    if (filtro.EstadosId.Any() && string.IsNullOrWhiteSpace(filtro.Identificacion)
                        && string.IsNullOrWhiteSpace(filtro.Radicado) && filtro.ConsolidadoId == 0)
                    {
                        return ResultadoStatus(Responses.SetOkResponse(null, $"No se encontraron resultados de la fecha: {fechaActual:dd-MM-yyyy}"));
                    }
                    else if (!string.IsNullOrWhiteSpace(filtro.Identificacion) || filtro.EstadosId.Any())
                    {
                        return ResultadoStatus(Responses.SetOkResponse(null, $"No se encontraron resultados."));
                    }
                    return ResultadoStatus(Responses.SetOkResponse(null,
                                                $"No se encontraron resultados en estado {EnumConfig.GetDescription(EstadoEstupefacienteEnum.ParaEnviar)}" +
                                                $" y {EnumConfig.GetDescription(EstadoEstupefacienteEnum.Consulta)} de la fecha: {fechaActual:dd-MM-yyyy}"));
                }

            }
            var listado = await GetPaginacion(filtro.Paginacion, queryable);
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
        [ResponseType(typeof(ResponseTypeSwagger<UsuarioGenteMarDTO>))]
        [HttpGet]
        [AuthorizeRolesFilter(RolesEnum.AdministradorVCITE, RolesEnum.GestorVCITE, RolesEnum.JuridicaVCITE, RolesEnum.ConsultasVCITE)]
        [Route("datos-gentemar-por-cedula")]
        public async Task<IHttpActionResult> GetGenteMarEstupefaciente([FromUri] CedulaDTO obj)
        {
            var respuesta = await _serviceEstupefacientes.GetDatosGenteMarEstupefacienteValidations(obj.Identificacion);
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
        [AuthorizeRolesFilter(RolesEnum.AdministradorVCITE, RolesEnum.JuridicaVCITE, RolesEnum.GestorVCITE, RolesEnum.ConsultasVCITE)]
        [Route("info/{id}")]
        public async Task<IHttpActionResult> GetDetallePersonaEstupefaciente(long id)
        {
            var respuesta = await _serviceEstupefacientes.GetDetallePersonaEstupefaciente(id);
            return Ok(respuesta);
        }


        // POST: 
        /// <summary>
        ///  Get estupefacientes sin observaciones para edición masiva
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>05/06/2023</Fecha>
        /// </remarks>
        /// <response code="200">OK. Devuelve y extrae la lista de estupefacientes que no tienen observaciones.</response>        
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(List<EstupefacientesBulkDTO>))]
        [HttpPost]
        [AuthorizeRolesFilter(RolesEnum.AdministradorVCITE, RolesEnum.JuridicaVCITE)]
        [Route("listar/sin-observaciones-para-edicion-masiva")]
        public async Task<IHttpActionResult> GetEstupefacientesSinObservaciones([FromBody] List<long> ids)
        {
            var listado = await _serviceEstupefacientes.GetEstupefacientesSinObservaciones(ids);
            return Ok(listado);
        }


        /// <summary>
        /// Servicio para agregar estupefaciente
        /// </summary>
        /// <param name="estupefaciente"></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>08/07/2022</Fecha>
        /// </remarks>
        /// <response code="201">Created. la solicitud ha tenido éxito y ha llevado a la creación de la tramite de estupefaciente.</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseCreatedTypeSwagger))]
        [HttpPost]
        [Route("crear")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorVCITE, RolesEnum.GestorVCITE, RolesEnum.JuridicaVCITE)]
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
        /// <response code="200">OK. se ha actualizado el recurso (estupefaciente).</response>   
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud con el estado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [HttpPut]
        [Route("editar")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorVCITE, RolesEnum.JuridicaVCITE)]
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
            var dataEstupefaciente = Mapear<EditInfoEstupefacienteDTO, GENTEMAR_ANTECEDENTES>(estupefaciente);
            var datosBasicos = Mapear<EstupefacienteDatosBasicosDTO, GENTEMAR_ANTECEDENTES_DATOSBASICOS>(estupefaciente.DatosBasicos);
            respuesta = await _serviceEstupefacientes.EditarAsync(dataEstupefaciente, datosBasicos, PathActual);
            return ResultadoStatus(respuesta);
        }

        // GET: radicados-sgdea-titulos
        /// <summary>
        ///  Listado de radicados de los titulos de navegación
        /// </summary>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>28/04/2022</Fecha>
        /// </remarks>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(List<string>))]
        [HttpGet]
        [AuthorizeRolesFilter(RolesEnum.AdministradorVCITE, RolesEnum.GestorVCITE, RolesEnum.JuridicaVCITE, RolesEnum.ConsultasVCITE)]
        [Route("radicados-sgdea-titulos")]
        public async Task<IHttpActionResult> GetRadicadosTitulos([FromUri] CedulaDTO obj)
        {
            var respuesta = await _serviceEstupefacientes.GetRadicadosTitulosByDocumento(obj.Identificacion);
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
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(List<string>))]
        [HttpGet]
        [AuthorizeRolesFilter(RolesEnum.AdministradorVCITE, RolesEnum.GestorVCITE, RolesEnum.JuridicaVCITE, RolesEnum.ConsultasVCITE)]
        [Route("radicados-sgdea-licencias")]
        public async Task<IHttpActionResult> GetRadicadosLicencias([FromUri] CedulaDTO obj)
        {
            var respuesta = await _serviceEstupefacientes.GetRadicadosLicenciasByDocumento(obj.Identificacion);
            return Ok(respuesta);
        }
    }
}
