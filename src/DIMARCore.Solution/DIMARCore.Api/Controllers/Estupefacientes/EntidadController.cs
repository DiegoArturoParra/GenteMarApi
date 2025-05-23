﻿using DIMARCore.Api.Core.Filters;
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
    /// servicios para las entidades de estupefacientes
    /// <Autor>Diego Parra</Autor>
    /// <Fecha>08/07/2022</Fecha>
    /// </summary>
    [EnableCors("*", "*", "*")]

    [RoutePrefix("api/entidades")]
    public class EntidadController : BaseApiController
    {
        private readonly EntidadBO _serviceEntidad;

        /// <summary>
        /// Constructor
        /// </summary>
        public EntidadController()
        {
            _serviceEntidad = new EntidadBO();
        }



        /// <summary>
        /// servicio get Entidades por filtro de estado
        /// </summary>
        /// <param name="dto"> objeto para filtro por estado</param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>08/07/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(List<EntidadDTO>))]
        [HttpGet]
        [Route("lista")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorVCITE, RolesEnum.GestorVCITE,
            RolesEnum.JuridicaVCITE, RolesEnum.ConsultasVCITE)]
        public async Task<IHttpActionResult> Listado([FromUri] ActivoDTO dto)
        {
            var query = await _serviceEntidad.GetAllAsync(dto != null ? dto.Activo : null);
            var listado = Mapear<IEnumerable<GENTEMAR_ENTIDAD_ANTECEDENTE>, IEnumerable<EntidadDTO>>(query);
            return Ok(listado);
        }


        /// <summary>
        /// servicio get Entidad por id
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>08/07/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        [ResponseType(typeof(ResponseTypeSwagger<EntidadDTO>))]
        [HttpGet]
        [Route("{id}")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorVCITE)]
        public async Task<IHttpActionResult> GetEntidad(int id)
        {
            var entidad = await _serviceEntidad.GetByIdAsync(id);

            var obj = Mapear<GENTEMAR_ENTIDAD_ANTECEDENTE, EntidadDTO>((GENTEMAR_ENTIDAD_ANTECEDENTE)entidad.Data);
            entidad.Data = obj;

            return ResultadoStatus(entidad);
        }



        /// <summary>
        /// Servicio para crear una Entidad
        /// </summary>
        /// <param name="Entidad"></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>08/07/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="201">Created. la solicitud ha tenido éxito y ha llevado a la creación de la entidad de estupefaciente.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseCreatedTypeSwagger))]
        [HttpPost]
        [Route("crear")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorVCITE)]
        public async Task<IHttpActionResult> Crear([FromBody] EntidadDTO Entidad)
        {
            var data = Mapear<EntidadDTO, GENTEMAR_ENTIDAD_ANTECEDENTE>(Entidad);
            var response = await _serviceEntidad.CrearAsync(data);
            return ResultadoStatus(response);
        }


        /// <summary>
        /// Servicio para editar una Entidad
        /// </summary>
        /// <param name="Entidad"></param>
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
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [HttpPut]
        [Route("editar")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorVCITE)]
        public async Task<IHttpActionResult> Editar([FromBody] EntidadDTO Entidad)
        {
            var data = Mapear<EntidadDTO, GENTEMAR_ENTIDAD_ANTECEDENTE>(Entidad);
            var response = await _serviceEntidad.ActualizarAsync(data);
            return ResultadoStatus(response);
        }

        /// <summary>
        /// Servicio para Inactivar una Entidad
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>08/07/2022</Fecha>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. anular o activa la entidad indicada.</response>   
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">Internal Server Error. ha ocurrido un error.</response>
        /// <returns></returns>
        [ResponseType(typeof(ResponseEditTypeSwagger))]
        [HttpPut]
        [Route("anula-or-activa/{id}")]
        [AuthorizeRolesFilter(RolesEnum.AdministradorVCITE)]
        public async Task<IHttpActionResult> AnularOrActivar(int id)
        {
            var response = await _serviceEntidad.AnulaOrActivaAsync(id);
            return ResultadoStatus(response);
        }
    }
}
