﻿using DIMARCore.Api.Core.Filters;
using DIMARCore.Api.Core.Models;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers.Licencias
{
    /// <summary>
    /// API licencias
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/licencia")]
    public class LicenciaController : BaseApiController
    {
        private readonly LicenciaBO _service;

        /// <summary>
        /// Constructor
        /// </summary>
        public LicenciaController()
        {
            _service = new LicenciaBO();
        }

        /// <summary>
        /// Listado de licencias dependiendo el usuario
        /// </summary>    
        /// <param name="id"></param>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>2022/07/19</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información de las licencias.</response>
        /// <response code="204">No Content. No hay estado.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(List<LicenciaDTO>))]
        [HttpGet]
        [Route("lista-id/{id}")]
        [AuthorizeRolesFilter(RolesEnum.Consultas, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetlicenciaIdUsuario(long id)
        {
            var licencias = await _service.GetlicenciasPorUsuarioId(id);
            return Ok(licencias);
        }



        /// <summary>
        /// Lista licencia dependiendo el id de la licencia
        /// </summary>    
        /// <param name="id"></param>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>2022/07/19</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información de las licencias.</response>
        /// <response code="204">No Content. No hay estado.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(LicenciaDTO))]
        [HttpGet]
        [Route("lista-id-view/{id}")]
        [AuthorizeRolesFilter(RolesEnum.Consultas, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetlicenciaIdView(int id)
        {
            var licencias = await _service.GetlicenciaIdView(id);
            return Ok(licencias);
        }


        /// <summary>
        /// Listado de licencias por id 
        /// </summary>    
        /// <param name="id"></param>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>2022/07/19</Fecha>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información de las licencias.</response>
        /// <response code="204">No Content. No hay estado.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(LicenciaDTO))]
        [HttpGet]
        [Route("lista-licencia-id/{id}")]
        [AuthorizeRolesFilter(RolesEnum.Consultas, RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.ASEPAC, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> GetlicenciaId(int id)
        {
            var licencias = await _service.GetlicenciaPorIdAsync(id);
            return Ok(licencias);
        }

        /// <summary>
        /// Servicio para crear una licencia
        /// </summary>        
        /// <remarks>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>28/04/2022</Fecha>
        /// </remarks>
        /// <param>objeto para crear una licencia.</param>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud con el estado.</response>
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseCreatedTypeSwagger))]
        [HttpPost]
        [Route("crear")]
        [AuthorizeRolesFilter(RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.AdministradorGDM)]

        public async Task<IHttpActionResult> CrearLicencia()
        {
            Respuesta respuesta = new Respuesta();
            var req = HttpContext.Current.Request;
            var archivo = req.Files["File"];
            var format = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
            var datos = req["Data"];
            if (datos == null)
                return ResultadoStatus(Responses.SetBadRequestResponse($"Objeto invalido de {nameof(LicenciaDTO)}, debe enviar los datos correctos."));

            LicenciaDTO Licencia = JsonConvert.DeserializeObject<LicenciaDTO>(datos, dateTimeConverter);
            if (archivo != null)
                Licencia.Observacion.Archivo = archivo;
            ValidateModelAndThrowIfInvalid(datos);
            var data = Mapear<LicenciaDTO, GENTEMAR_LICENCIAS>(Licencia);
            respuesta = await new LicenciaBO().CrearLicencia(data, PathActual);
            return ResultadoStatus(respuesta);
        }

        /// <summary>
        /// Servicio para crear una licencia
        /// </summary>        
        /// <remarks>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>09/08/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud con el estado.</response>
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [HttpPut]
        [Route("modificar")]
        [AuthorizeRolesFilter(RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> Editar()
        {
            Respuesta respuesta = new Respuesta();
            var req = HttpContext.Current.Request;
            var archivo = req.Files["File"];
            var format = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
            var datos = req["Data"];
            if (datos == null)
                return ResultadoStatus(Responses.SetBadRequestResponse($"Objeto invalido de {nameof(LicenciaDTO)}, debe enviar los datos correctos."));

            LicenciaDTO Licencia = JsonConvert.DeserializeObject<LicenciaDTO>(datos, dateTimeConverter);
            if (archivo != null)
                Licencia.Observacion.Archivo = archivo;
            ValidateModelAndThrowIfInvalid(datos);
            var data = Mapear<LicenciaDTO, GENTEMAR_LICENCIAS>(Licencia);
            respuesta = await _service.ModificarLicencia(data, PathActual);
            return ResultadoStatus(respuesta);
        }
        /// <summary>
        /// Servicio para crear una licencia
        /// </summary>        
        /// <remarks>
        /// <Autor>Camilo Vargas</Autor>
        /// <Fecha>09/08/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. conflicto de solicitud con el estado.</response>
        /// <response code="500">Internal Server. Error En el servidor. </response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [HttpPut]
        [Route("cambiar-estado")]
        [AuthorizeRolesFilter(RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.AdministradorGDM)]
        public async Task<IHttpActionResult> ChangeStatus()
        {
            Respuesta respuesta = new Respuesta();
            var req = HttpContext.Current.Request;
            var archivo = req.Files["File"];
            var format = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
            var datos = req["Data"];
            if (datos == null)
                return ResultadoStatus(Responses.SetBadRequestResponse($"Objeto invalido de {nameof(CambioEstadoLicenciaDTO)}, debe enviar los datos correctos."));

            CambioEstadoLicenciaDTO Licencia = JsonConvert.DeserializeObject<CambioEstadoLicenciaDTO>(datos, dateTimeConverter);
            if (archivo != null)
                Licencia.Observacion.Archivo = archivo;

            ValidateModelAndThrowIfInvalid(datos);
            var data = Mapear<CambioEstadoLicenciaDTO, GENTEMAR_LICENCIAS>(Licencia);
            respuesta = await _service.CambiarEstadoLicencia(data, PathActual);
            return ResultadoStatus(respuesta);
        }
    }
}