using DIMARCore.Api.Core.Atributos;
using DIMARCore.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DIMARCore.Api.Controllers.Reportes
{
    /// <summary>
    /// Servicios que genera los reportes de estupefacientes
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/reportes-vcite")]
    [AuthorizeRoles(RolesEnum.AdministradorVCITE)]
    public class ReportesVCITEController : BaseApiController
    {

    }
}
