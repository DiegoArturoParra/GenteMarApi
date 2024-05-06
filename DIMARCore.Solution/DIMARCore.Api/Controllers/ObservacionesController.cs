using DIMARCore.Api.Core.Filters;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers
{
    /// <summary>
    /// Servicios para tabla observaciones
    /// </summary>
    [AllowAnonymous]
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/observaciones")]
    public class ObservacionesController : BaseApiController
    {
        private readonly ObservacionesBO _serviceObservaciones;
        /// <summary>
        /// Constructor
        /// </summary>
        public ObservacionesController()
        {
            _serviceObservaciones = new ObservacionesBO();
        }
        #region Observaciones de datos basicos

        /// <summary>
        /// Metodo para listar las observaciones por persona de gente de mar.
        /// </summary>
        /// <param name="id"> id de gente de mar</param>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>2022/04/26</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve el listado de observaciones por persona de gente de mar.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="404">Not Found. Id no encontrado.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <returns></returns>
        [ResponseType(typeof(List<ObservacionDTO>))]
        [HttpGet]
        [Route("datos-basicos/{id}")]
        [AuthorizeRolesFilter(RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetObservacionesDatosBasicosbyIdGenteMar(int id)
        {
            var observacionesDatosBasicos = await new ObservacionesBO().GetObservacionesId(id, PathActual, ObservacionEnum.DatosBasicos);
            return Ok(observacionesDatosBasicos);
        }

        /// <summary>
        /// Metodo para crear las observaciones por persona de gente de mar.
        /// </summary>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>2022/04/26</Fecha>
        /// <returns></returns>
        /// <response code="201">Created. Devuelve mensaje, de creado correctamente.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="404">Not Found. Id no encontrado.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <returns></returns>
        [ResponseType(typeof(Respuesta))]
        [HttpPost]
        [Route("datos-basicos-crear")]
        [AuthorizeRolesFilter(RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> CrearObservacionesDatosBasicos()
        {

            Respuesta respuesta;
            respuesta = ValidarObservacion();
            var datos = Mapear<ObservacionDTO, GENTEMAR_OBSERVACIONES_DATOSBASICOS>((ObservacionDTO)respuesta.Data);
            respuesta = await _serviceObservaciones.CrearObservacionesDatosBasicos(datos, PathActual);
            return ResultadoStatus(respuesta);
        }

        #endregion

        #region Observaciones de titulos

        /// <summary>
        /// Metodo para listar las observaciones por titulos.
        /// </summary>
        /// <param name="id"> id titulor</param>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>2022/05/26</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve el listado de observaciones por titulo.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="404">Not Found. Id no encontrado.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor.</response>
        /// <returns></returns>
        [ResponseType(typeof(List<ObservacionDTO>))]
        [HttpGet]
        [Route("titulos/{id}")]
        [AuthorizeRolesFilter(RolesEnum.GestorSedeCentral, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetObservacionesTitulos(int id)
        {
            await new TituloBO().ExistById(id);
            var observacionesTitulos = await new ObservacionesBO().GetObservacionesId(id, PathActual, ObservacionEnum.Titulos);
            return Ok(observacionesTitulos);
        }

        /// <summary>
        /// Metodo para crear las observaciones por titulo.
        /// </summary>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>2022/05/26</Fecha>
        /// <returns></returns>
        /// <response code="201">Created. Devuelve mensaje, de creado correctamente.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="404">Not Found. Id no encontrado.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <returns></returns>
        [ResponseType(typeof(Respuesta))]
        [HttpPost]
        [Route("titulos-crear")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> CrearObservacionesTitulos()
        {
            Respuesta respuesta;
            respuesta = ValidarObservacion();
            var datos = Mapear<ObservacionDTO, GENTEMAR_OBSERVACIONES_TITULOS>((ObservacionDTO)respuesta.Data);
            respuesta = await _serviceObservaciones.CrearObservacionesTitulos(datos, PathActual);
            return ResultadoStatus(respuesta);
        }

        #endregion

        #region Observaciones de licencias

        /// <summary>
        /// Metodo para listar las observaciones por licencia
        /// </summary>
        /// <param name="id"> id licencia</param>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>2022/09/26</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve el listado de observaciones por licencia.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="404">Not Found. Id no encontrado.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor.</response>
        /// <returns></returns>
        [ResponseType(typeof(List<ObservacionDTO>))]
        [HttpGet]
        [Route("licencias/{id}")]
        [AuthorizeRolesFilter(RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetObservacionesLicenciasbyIdGenteMar(int id)
        {
            var ObservacionesLicencias = await new ObservacionesBO().GetObservacionesId(id, PathActual, ObservacionEnum.Licencias);
            return Ok(ObservacionesLicencias);

        }


        /// <summary>
        /// Metodo para crear las observaciones por licencia.
        /// </summary>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>2022/09/26</Fecha>
        /// <returns></returns>
        /// <response code="201">Created. Devuelve mensaje, de creado correctamente.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="404">Not Found. Id no encontrado.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <returns></returns>
        [ResponseType(typeof(Respuesta))]
        [HttpPost]
        [Route("licencias-crear")]
        [AuthorizeRolesFilter(RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> CrearObservacionesLicencias()
        {

            Respuesta respuesta;
            respuesta = ValidarObservacion();
            if (respuesta.Estado)
            {
                var datos = Mapear<ObservacionDTO, GENTEMAR_OBSERVACIONES_LICENCIAS>((ObservacionDTO)respuesta.Data);
                respuesta = await _serviceObservaciones.CrearObservacionesLicencias(datos, PathActual);
            }
            return ResultadoStatus(respuesta);
        }


        #endregion

        #region Observaciones de estupefacientes
        /// <summary>
        /// Metodo para listar las observaciones de un estupefaciente en especifico.
        /// </summary>
        /// <param name="id"> id estupefaciente</param>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>2022/12/16</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve el listado de observaciones por estupefaciente.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="404">Not Found. Id no encontrado.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <returns></returns>
        [ResponseType(typeof(List<ObservacionDTO>))]
        [HttpGet]
        [Route("estupefacientes/{id}")]
        [AuthorizeRolesFilter(RolesEnum.GestorVCITE, RolesEnum.JuridicaVCITE, 
            RolesEnum.AdministradorVCITE, RolesEnum.ConsultasVCITE)]
        public async Task<IHttpActionResult> GetObservacionesEstupefacientes(int id)
        {
            var observacionesEstupefacientes = await new ObservacionesBO().GetObservacionesId(id, PathActual, ObservacionEnum.Estupefacientes);
            return Ok(observacionesEstupefacientes);
        }


        /// <summary>
        /// Metodo para crear las observaciones por estupefaciente.
        /// </summary>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>2022/12/16</Fecha>
        /// <returns></returns>
        /// <response code="201">Created. Devuelve mensaje, de creado correctamente.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="404">Not Found. Id no encontrado.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <returns></returns>
        [ResponseType(typeof(Respuesta))]
        [HttpPost]
        [Route("estupefacientes-crear")]
        [AuthorizeRolesFilter(RolesEnum.JuridicaVCITE, RolesEnum.AdministradorVCITE)]

        public async Task<IHttpActionResult> CrearObservacionesEstupefacientes()
        {
            Respuesta respuesta;
            respuesta = ValidarObservacion();
            if (respuesta.Estado)
            {
                var datos = Mapear<ObservacionDTO, GENTEMAR_OBSERVACIONES_ANTECEDENTES>((ObservacionDTO)respuesta.Data);
                respuesta = await _serviceObservaciones.CrearObservacionesEstupefacientes(datos, PathActual);
            }
            return ResultadoStatus(respuesta);
        }

        #endregion

        private Respuesta ValidarObservacion()
        {
            var req = HttpContext.Current.Request;
            var archivo = req.Files["File"];
            ObservacionDTO observacion = JsonConvert.DeserializeObject<ObservacionDTO>(req["Data"]);
            if (archivo != null)
            {
                observacion.Archivo = archivo;
            }
            ValidateModelAndThrowIfInvalid(observacion);
            return Responses.SetOkResponse(observacion);
        }
    }
}
