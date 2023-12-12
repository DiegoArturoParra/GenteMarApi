using DIMARCore.Api.Core.Atributos;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers
{
    /// <summary>
    /// API Datos Basicos
    /// </summary>
    [Authorize]
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/datosBasicos")]
    public class DatosBasicosController : BaseApiController
    {

        private readonly DatosBasicosBO _service;

        /// <summary>
        /// Constructor
        /// </summary>
        public DatosBasicosController()
        {
            _service = new DatosBasicosBO();
        }
        /// <summary>
        /// Metodo para paginar datos basicos
        /// </summary>
        /// <param name="filtro"> filtro de la busqueda (ParametrosPaginacion) objeto que contiene los datos para paginar.</param>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>2022/02/26</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información de datos basicos.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(Paginador<ListadoDatosBasicosDTO>))]
        [HttpPost]
        [Route("paginar")]
        [AuthorizeRoles(RolesEnum.Consultas, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> PaginarDatosBasicosAsync([FromBody] DatosBasicosQueryFilter filtro)
        {
            if (filtro == null)
            {
                filtro = new DatosBasicosQueryFilter
                {
                    Paginacion = new ParametrosPaginacion()
                };
            }
            var queryable = _service.GetDatosBasicosQueryable(filtro);
            var listado = await GetPaginacion(filtro.Paginacion, queryable);
            var paginador = Paginador<ListadoDatosBasicosDTO>.CrearPaginador(queryable.Count(), listado, filtro.Paginacion);
            return Ok(paginador);
        }

        /// <summary>
        /// Servicio para crear los datos basicos de una persona 
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /crear content-type form-data 
        ///     {    
        ///     "IdTipoDocumento":0,
        ///     "DocumentoIdentificacion":"0",
        ///     "CodPais":"0",
        ///     "IdMunicipioExpedicion":0,
        ///     "FechaExpedicion":"2014-01-15",
        ///     "FechaVencimiento":"2014-01-15",
        ///     "Nombres":"string",
        ///     "Apellidos":"string",
        ///     "IdGenero":0,
        ///     "FechaNacimiento":"1996-12-23",
        ///     "IdMunicipioNacimiento":"0",
        ///     "Direccion":"2014-01-15",
        ///     "IdMunicipioResidencia":0,
        ///     "CorreoElectronico":"Prueba@gmail.com",
        ///     "Telefono":"0",
        ///     "NumeroMovil":"0",
        ///     "Formacion":0,
        ///     "IdFormacionGrado":0
        ///     }
        ///    data = objeto
        ///    file = archivo
        /// </remarks>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>2022/02/26</Fecha>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(Respuesta))]
        [HttpPost]
        [Route("crear")]
        [AuthorizeRoles(RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> Crear()
        {
            DatosBasicosDTO dataDatosBasicos = new DatosBasicosDTO();
            var req = HttpContext.Current.Request;
            var archivo = req.Files["File"];
            var format = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
            dataDatosBasicos = JsonConvert.DeserializeObject<DatosBasicosDTO>(req["Data"], dateTimeConverter);
            dataDatosBasicos.Archivo = archivo;
            if (dataDatosBasicos == null)
                return ResultadoStatus(Responses.SetBadRequestResponse($"Objeto invalido de {nameof(DatosBasicosDTO)}, debe enviar los datos correctos."));
            ValidateModelAndThrowIfInvalid(dataDatosBasicos);
            var data = Mapear<DatosBasicosDTO, GENTEMAR_DATOSBASICOS>(dataDatosBasicos);
            var response = await _service.CrearAsync(data, PathActual);
            return ResultadoStatus(response);
        }

        /// <summary>
        /// Servicio para actualizar los datos basicos de una persona 
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /crear content-type form-data 
        ///     {    
        ///     "IdTipoDocumento":0,
        ///     "DocumentoIdentificacion":"0",
        ///     "CodPais":"0",
        ///     "IdMunicipioExpedicion":0,
        ///     "FechaExpedicion":"2014-01-15",
        ///     "FechaVencimiento":"2014-01-15",
        ///     "Nombres":"string",
        ///     "Apellidos":"string",
        ///     "IdGenero":0,
        ///     "FechaNacimiento":"1996-12-23",
        ///     "IdMunicipioNacimiento":"0",
        ///     "Direccion":"2014-01-15",
        ///     "IdMunicipioResidencia":0,
        ///     "CorreoElectronico":"Prueba@gmail.com",
        ///     "Telefono":"0",
        ///     "NumeroMovil":"0",
        ///     "Formacion":0,
        ///     "IdFormacionGrado":0
        ///     }
        ///    data = objeto
        ///    file = archivo
        /// </remarks>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>2022/02/26</Fecha>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(Respuesta))]
        [HttpPut]
        [Route("actualizar")]
        [AuthorizeRoles(RolesEnum.GestorSedeCentral, RolesEnum.AdministradorGDM, RolesEnum.Capitania)]
        public async Task<IHttpActionResult> Actualizar()
        {
            DatosBasicosDTO dataDatosBasicos = new DatosBasicosDTO();
            var req = HttpContext.Current.Request;
            var archivo = req.Files["File"];

            //acepta fechas en el formato especificado
            var format = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
            dataDatosBasicos = JsonConvert.DeserializeObject<DatosBasicosDTO>(req["Data"], dateTimeConverter);
            dataDatosBasicos.Archivo = archivo;
            if (dataDatosBasicos == null)
                return ResultadoStatus(Responses.SetBadRequestResponse($"Objeto invalido de {nameof(DatosBasicosDTO)}, debe enviar los datos correctos."));
            ValidateModelAndThrowIfInvalid(dataDatosBasicos);
            var data = Mapear<DatosBasicosDTO, GENTEMAR_DATOSBASICOS>(dataDatosBasicos);
            var response = await _service.ActualizarAsync(data, PathActual);
            return ResultadoStatus(response);
        }

        /// <summary>
        /// Servicio para actualizar los datos basicos de una persona 
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /crear content-type form-data 
        ///     {
        ///     "IdGentemar":0,
        ///     "IdEstado":0,
        ///     "Observacion":{
        ///        "Observacion":"string",
        ///        "IdTablaRelacion":0,
        ///        "ArchivoObser":file
        ///     }
        ///     }
        ///    data = objeto
        ///    file = archivo
        /// </remarks>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>2022/02/26</Fecha>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(Respuesta))]
        [HttpPut]
        [Route("cambiar-estado")]
        [AuthorizeRoles(RolesEnum.GestorSedeCentral, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> ChangeStatus()
        {
            DatosBasicosDTO dataDatosBasicos = new DatosBasicosDTO();
            var req = HttpContext.Current.Request;
            var archivo = req.Files["File"];

            var format = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
            var data1 = req["Data"];
            dataDatosBasicos = JsonConvert.DeserializeObject<DatosBasicosDTO>(req["Data"], dateTimeConverter);

            if (dataDatosBasicos == null)
                return ResultadoStatus(Responses.SetBadRequestResponse($"Objeto invalido de {nameof(DatosBasicosDTO)}, debe enviar los datos correctos."));

            if (archivo != null)
                dataDatosBasicos.Observacion.Archivo = archivo;

            ValidateModelAndThrowIfInvalid(dataDatosBasicos);

            var data = Mapear<DatosBasicosDTO, GENTEMAR_DATOSBASICOS>(dataDatosBasicos);
            var response = await _service.ChangeStatus(data, PathActual);
            return ResultadoStatus(response);
        }

        /// <summary>
        /// Servicio para busqueda de gente de mar por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="404">Not Found No se encontro la persona de gente de mar.</response>
        /// <response code="200">OK. Devuelve la información de la persona de gente de mar.</response>      
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [HttpGet]
        [Route("listar/{id}")]
        [AuthorizeRoles(RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.Consultas, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public IHttpActionResult GetById(long id)
        {
            var DatosBasicos = _service.GetDatosBasicosId(id, PathActual);
            return Ok(DatosBasicos);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parametrosGenteMar"></param>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>2022/02/26</Fecha>
        /// <returns>retorna nombre persona</returns>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [HttpPost]
        [Route("por-filtro")]
        [AuthorizeRoles(RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.Consultas, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM,
            RolesEnum.AdministradorVCITE, RolesEnum.JuridicaVCITE, RolesEnum.GestorVCITE)]
        public async Task<IHttpActionResult> GetPersonaByIdentificacionOrId(ParametrosGenteMarDTO parametrosGenteMar)
        {
            var data = await _service.ValidationsStatusPersona(parametrosGenteMar);
            return Ok(data);
        }


        /// <summary>
        /// Listado de licencias y titulos dependiendo el docuemnto del usuario
        /// </summary>    
        /// <param name="documento"></param>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>2022/07/19</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información de las licencias.</response>
        /// <response code="204">No Content. No hay estado.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>

        [ResponseType(typeof(LicenciasTitulosDTO))]
        [HttpGet]
        [Route("lista-titulo-licencia-documento/{documento}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetlicenciaDocumentoUsuario(string documento)
        {
            var data = await _service.GetlicenciaTituloVigentesPorDocumentoUsuario(documento);
            return Ok(data);
        }

    }
}