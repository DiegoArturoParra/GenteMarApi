using DIMARCore.Api.Core.Filters;
using DIMARCore.Api.Core.Models;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using GenteMarCore.Entities.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers.TitulosDeNavegacion
{
    /// <summary>
    /// servicios para los titulos de navegación
    /// <Autor>Diego Parra</Autor>
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/titulos")]
    public class TituloController : BaseApiController
    {
        private readonly TituloBO _serviceTitulo;

        /// <summary>
        /// Constructor
        /// </summary>
        public TituloController()
        {
            _serviceTitulo = new TituloBO();
        }

        /// <summary>
        /// Metodo para paginar titulos
        /// </summary>
        /// <param name="paginacion"> paginacion (ParametrosPaginacion) objeto que contiene los datos para paginar.</param>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>/26/02/2022</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información del titulo.</response>
        /// <response code="204">No Content. No hay titulos.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(Paginador<ListadoTituloDTO>))]
        [HttpGet]
        [Route("pagination")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.Consultas, RolesEnum.ASEPAC)]
        public async Task<IHttpActionResult> PaginarAsync([FromUri] ParametrosPaginacion paginacion)
        {
            if (paginacion == null)
                paginacion = new ParametrosPaginacion();

            var queryable = _serviceTitulo.GetTitulosQueryable();
            if (!queryable.Any())
                return Ok(Responses.SetOkResponse(null, "No hay titulos"));

            var listado = await GetPaginacion(paginacion, queryable);
            var paginador = Paginador<ListadoTituloDTO>.CrearPaginador(queryable.Count(), listado, paginacion);
            return Ok(paginador);
        }


        /// <summary>
        /// Servicio que retorna toda la información de un titulo en especifico por Id
        /// </summary>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>/26/02/2022</Fecha>
        /// <param name="id">Id (long) del titulo de navegación de gente de mar.</param>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información del titulo.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>     
        /// <response code="404">NotFound. No existe el título solicitado.</response>
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(ResponseTypeSwagger<InfoTituloDTO>))]
        [HttpGet]
        [Route("{id}")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.Consultas, RolesEnum.ASEPAC)]
        public async Task<IHttpActionResult> GetTituloPorId(long id)
        {
            var response = await _serviceTitulo.GetTituloById(id);
            return Ok(response);
        }


        /// <summary>
        /// Servicio que hace la paginacion de titulos por identificacion de la persona
        /// </summary>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>/26/02/2022</Fecha>
        /// <param name="filtro"></param>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve el listado de titulos de la persona de gente de mar..</response>        
        /// <response code="204">No Content. no hay titulos de la persona buscada.</response>        
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>      
        /// <response code="404">NotFound. No se ha encontrado la persona de gente de mar solicitada.</response>
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(Paginador<ListadoTituloDTO>))]
        [HttpPost]
        [Route("filter-by-identification")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.Consultas, RolesEnum.ASEPAC)]
        public async Task<IHttpActionResult> GetTitulosByIdentificacion(DocumentFilter filtro)
        {
            await _serviceTitulo.ExistePersonaByIdentificacion(filtro.Identificacion);
            var query = await _serviceTitulo.GetTitulosFiltro(filtro.Identificacion);
            return Ok(query);
        }

        /// <summary>
        /// Servicio que hace la paginacion de titulos por identificacion de la persona
        /// </summary>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>/26/02/2022</Fecha>
        /// <param name="filtro">parametro para el fitro de paginación con Id de gente de mar.</param>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve el listado de titulos de la persona de gente de mar..</response>        
        /// <response code="204">No Content. no hay titulos de la persona buscada..</response>        
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">NotFound. No se ha encontrado la persona de gente de mar solicitada.</response>
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(Paginador<ListadoTituloDTO>))]
        [HttpPost]
        [Route("filter-by-id")]
        [AuthorizeRolesFilter(RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.Consultas, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetTitulosById(IdentificatorFilter filtro)
        {
            await new DatosBasicosBO().ExisteById(Convert.ToInt64(filtro.Id));
            var query = await _serviceTitulo.GetTitulosFiltro(string.Empty, Convert.ToInt64(filtro.Id));
            return Ok(query);
        }


        /// <summary>
        /// Servicio para crear un titulo
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /crear content-type form-data 
        ///     {
        ///         "IdsLlaveCompuesta":
        ///         { 
        ///         "ReglaId":0, 
        ///         "CargoTituloId":0 
        ///         },
        ///         "TituloId":0, 
        ///         "CargoReglaId":0,
        ///         "GenteMarId":0, 
        ///         "FechaVencimiento":"2022-06-03T21:06:26.589Z", 
        ///         "FechaExpedicion":"2022-06-03T21:06:26.589Z", 
        ///         "CodigoPais":"", "EstadoTramiteId":0, 
        ///         "CapitaniaId":0, "TipoSolicitudId":0, 
        ///         "Radicado":"string", "CapitaniaFirmanteId":0, 
        ///         "Observacion":
        ///         { 
        ///         "Observacion":"string", 
        ///         "IdTablaRelacion":0
        ///         } 
        ///     }
        ///    data = objeto
        ///    file = archivo
        /// </remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>/26/02/2022</Fecha>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(ResponseCreatedTypeSwagger))]
        [HttpPost]
        [Route("crear")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM, RolesEnum.GestorSedeCentral)]
        public async Task<IHttpActionResult> Crear()
        {
            Respuesta respuesta = new Respuesta();
            var req = HttpContext.Current.Request;
            var archivo = req.Files["File"];
            var format = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
            var datos = req["Data"] ??
                throw new HttpStatusCodeException(Responses.SetBadRequestResponse($"Objeto invalido de {nameof(TituloDTO)}, debe enviar los datos correctos."));

            TituloDTO titulo = JsonConvert.DeserializeObject<TituloDTO>(datos, dateTimeConverter);
            if (archivo != null)
                titulo.Observacion.Archivo = archivo;

            ValidateModelAndThrowIfInvalid(titulo);
            titulo.CapitaniaId = GetIdCapitania();
            var data = Mapear<TituloDTO, GENTEMAR_TITULOS>(titulo);
            respuesta = await _serviceTitulo.CrearAsync(data, PathActual);
            return ResultadoStatus(respuesta);
        }



        /// <summary>
        /// Servicio para editar un titulo
        /// </summary>
        /// <remarks>
        ///  Servicio para editar un titulo de navegación de personal en gente de mar.
        /// </remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>2022/02/26</Fecha>
        /// <param>FORM-DATA objeto para actualizar el titulo de la persona de gente de mar.</param>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [HttpPut]
        [Route("editar")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM, RolesEnum.GestorSedeCentral)]
        public async Task<IHttpActionResult> Editar()
        {
            Respuesta respuesta;
            var req = HttpContext.Current.Request;
            var archivo = req.Files["File"];
            var format = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
            TituloDTO titulo = JsonConvert.DeserializeObject<TituloDTO>(req["Data"], dateTimeConverter);
            if (archivo != null)
                titulo.Observacion.Archivo = archivo;
            ValidateModelAndThrowIfInvalid(titulo);
            titulo.CapitaniaId = GetIdCapitania();
            var data = Mapear<TituloDTO, GENTEMAR_TITULOS>(titulo);
            respuesta = await _serviceTitulo.ActualizarAsync(data, PathActual);
            return ResultadoStatus(respuesta);
        }

        /// <summary>
        /// Servicio para desactivar un cargo en especifico de un titulo
        /// </summary>
        /// <remarks>
        ///  Servicio para desactivar un cargo en especifico de un titulo de navegación de personal en gente de mar.
        /// </remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>2023/07/14</Fecha>
        /// <param name="desactivateCargo"></param>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [HttpPut]
        [Route("desactivar-cargo")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM, RolesEnum.GestorSedeCentral)]
        public async Task<IHttpActionResult> DesactivarCargoDelTitulo(DesactivateCargoDTO desactivateCargo)
        {
            Respuesta respuesta = await _serviceTitulo.DesactivarCargoDelTitulo(desactivateCargo);
            return Ok(respuesta);
        }

        /// <summary>
        /// fechas por defecto de un titulo de navegación si una persona de gente de mar tiene titulos con la sección puente
        /// </summary>
        /// <remarks>
        /// obtiene fechas por defecto de un titulo de navegación si una persona de gente de mar tiene titulos con la sección puente
        /// </remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>2022/02/26</Fecha>
        /// <param name="idGenteMar">Id (long) de gente de mar.</param>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(ResponseTypeSwagger<FechasRadioOperadoresDTO>))]
        [HttpGet]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM, RolesEnum.GestorSedeCentral)]
        [Route("fechas-radio-operadores/{idGenteMar}")]
        public async Task<IHttpActionResult> GetFechasRadioOperadores(long idGenteMar)
        {
            var response = await _serviceTitulo.GetFechasRadioOperadores(idGenteMar);
            return Ok(response);
        }
    }
}