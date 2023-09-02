using AutoMapper;
using DIMARCore.Api.Core;
using DIMARCore.Utilities.Config;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using log4net;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ModelBinding;
using WebGrease.Css.Extensions;

namespace DIMARCore.Api.Controllers
{

    /// <summary>
    /// ApiController con opciones base
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/base-api")]
    public class BaseApiController : ApiController
    {
        /// <summary>
        /// atributo para el log del aplicativo
        /// </summary>
        protected static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// atributo para mapear
        /// </summary>
        public readonly IMapper _mapper = AutoMapperProfiles.CreateMapper();

        /// <summary>
        /// Returns a Key/Value pair with all the errors in the model
        /// according to the data annotation properties.
        /// </summary>
        /// <param name="errDictionary"></param>
        /// <returns>
        /// Key: Name of the property
        /// Value: The error message returned from data annotation
        /// </returns>
        protected Dictionary<string, string> GetErrorListFromModelState(ModelStateDictionary errDictionary)
        {
            var errors = new Dictionary<string, string>();
            errDictionary.Where(k => k.Value.Errors.Count > 0).ForEach(i =>
            {
                var er = string.Join(", ", i.Value.Errors.Select(e => e.ErrorMessage).ToArray());
                errors.Add(i.Key, er);
            });
            return errors;
        }

        /// <summary>
        /// obtiene el path actual
        /// </summary>
        /// <returns></returns>
        private string GetRutaInicial()
        {
            var path = System.Web.HttpContext.Current.Server.MapPath(Constantes.PATH_REPOSITORIO_ARCHIVOS);
            return path;
        }

        /// <summary>
        /// atributo path actual
        /// </summary>
        public string PathActual => GetRutaInicial();

        /// <summary>
        /// retorna el estado de la api                                     
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        protected IHttpActionResult ResultadoStatus(Respuesta response)
        {

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                _logger.Error(response.MensajeExcepcion);
            }
            return Content(response.StatusCode, response);
        }

        /// <summary>
        /// Metodo para Mapear cualquier entidad Generic
        /// </summary>
        /// <typeparam name="TDTO"></typeparam>
        /// <typeparam name="TEntidad"></typeparam>
        /// <param name="DTO"></param>
        /// <returns></returns>
        protected TEntidad Mapear<TDTO, TEntidad>(TDTO DTO) where TEntidad : class
        {

            return _mapper.Map<TEntidad>(DTO);

        }
        /// <summary>
        /// Metodo para paginar cualquier entidad
        /// </summary>
        /// <typeparam name="TEntidad"></typeparam>
        /// <param name="paginacionDTO"></param>
        /// <param name="queryable"></param>
        /// <returns></returns>
        protected List<TEntidad> GetPaginacion<TEntidad>(ParametrosPaginacion paginacionDTO,
            IQueryable<TEntidad> queryable)
            where TEntidad : class
        {
            return queryable.Paginar(paginacionDTO).ToList();
        }

        #region Claims

        /// <summary>
        /// retorna si esta autenticado el usuario
        /// </summary>
        /// <returns></returns>
        protected bool GetUsuarioEstaAutenticado()
        {
            return User.Identity.IsAuthenticated;
        }


        /// <summary>
        /// Retorna el id de la aplicacion
        /// </summary>
        /// <returns>Retorna el id de la aplicacion</returns>
        protected int GetIdAplicacion()
        {
            if (GetUsuarioEstaAutenticado())
            {
                return ClaimsHelper.GetAplicacionId();
            }
            return 0;
        }
        /// <summary>
        /// Retorna el id de la capitania
        /// </summary>
        /// <returns>Retorna el id de la capitania</returns>
        protected int GetIdCapitania()
        {
            if (GetUsuarioEstaAutenticado())
            {
                return ClaimsHelper.GetCapitaniaUsuario();
            }
            return 0;
        }
        /// <summary>
        /// Retorna el login name del usuario autenticado
        /// </summary>
        /// <returns>Retorna el login name del usuario autenticado</returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>2020/05/06</Fecha>
        /// <UltimaActualizacion>2020/05/06 - Diego Parra - Creación del servicio</UltimaActualizacion>
        protected string GetLoginName()
        {
            if (GetUsuarioEstaAutenticado())
            {
                return ClaimsHelper.GetLoginName();
            }
            return "Anonimo";
        }

        /// <summary>
        /// Retorna el email
        /// </summary>
        /// <returns>Retorna el email</returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>2020/05/06</Fecha>
        /// <UltimaActualizacion>2020/05/06 - Diego Parra - Creación del servicio</UltimaActualizacion>

        protected string GetEmail()
        {
            if (GetUsuarioEstaAutenticado())
            {
                return ClaimsHelper.GetEmail();
            }
            return "Anonimo";
        }
        #endregion

        /// <summary>
        /// Valida el dto de un parametro de request del servicio
        /// </summary>
        /// <param name="request">parametro del body de la petición</param>
        /// <returns></returns>
        protected Respuesta ValidateModelAndThrowIfInvalid<T>(T request)
        {
            Validate(request);
            if (!ModelState.IsValid)
            {
                var errores = GetErrorListFromModelState(ModelState);
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, string.Join(";", errores.Select(x => x.Key + "=" + x.Value).ToArray()));
            }
            return Responses.SetOkResponse();
        }
    }
}