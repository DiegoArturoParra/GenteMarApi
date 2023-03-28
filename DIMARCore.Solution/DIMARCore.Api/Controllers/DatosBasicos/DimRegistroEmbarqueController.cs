using DIMARCore.Api.Core.Atributos;
using DIMARCore.Business.Logica;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Enums;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DIMARCore.Api.Controllers.DatosBasicos
{
    /// <summary>
    /// API dim embarque
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/dimRegistroEmbarque")]
    public class DimRegistroEmbarqueController : BaseApiController
    {

        private readonly DimRegistroEmbarqueBO _service;

        /// <summary>
        /// Constructor
        /// </summary>
        public DimRegistroEmbarqueController()
        {
            _service = new DimRegistroEmbarqueBO();
        }
        /// <summary>
        /// Metodo para traer los dim de registros de embarque por una persona 
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        /// <response code="200">OK. Devuelve la información del dim de embarque.</response>
        /// <response code="204">No Content. No hay estado.</response>
        /// <response code="400">Bad request. Objeto invalido.</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="500">Internal Server. Error En el servidor. </response>
        [ResponseType(typeof(List<DimRegistroEmbarqueDTO>))]
        [HttpPost]
        [Route("listar")]
        [AuthorizeRoles(RolesEnum.GestorSedeCentral, RolesEnum.Capitania, RolesEnum.Consultas, RolesEnum.ASEPAC, RolesEnum.Administrador)]
        public IHttpActionResult GetDimRegistrosEmbarque(DatosBasicosDTO usuario)
        {
            var DimPersona = _service.GetDimRegistroEmbarque(usuario.DocumentoIdentificacion);
            //var data = Mapear<GENTEMAR_DATOSBASICOS, DatosBasicosDTO>(DatosBasicos);
            return Ok(DimPersona);
        }
    }
}