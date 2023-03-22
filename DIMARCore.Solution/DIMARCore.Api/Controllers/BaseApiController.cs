using AutoMapper;
using DIMARCore.Api.Core;
using DIMARCore.UIEntities.Models;
using DIMARCore.Utilities.Config;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Seguridad;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Claims;
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

            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
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

        /// <summary>
        /// Valida el dto del titulo
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        protected Respuesta ValidarObjeto(object objeto)
        {
            Respuesta res = new Respuesta();
            this.Validate(objeto);
            if (!ModelState.IsValid)
            {
                var errores = GetErrorListFromModelState(ModelState);
                res.Estado = false;
                res.StatusCode = HttpStatusCode.BadRequest;
                res.Mensaje = string.Join(";", errores.Select(x => x.Key + "=" + x.Value).ToArray());
            }
            else
            {
                res.Estado = true;
            }

            return res;
        }

        #region Claims
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        protected bool GetUsuarioEstaAutenticado()
        {
            return User.Identity.IsAuthenticated;
        }


        /// <summary>
        /// Retorna el id de la aplicacion
        /// </summary>
        /// <returns>Retorna el id de la aplicacion</returns>
        [Authorize]
        protected int GetIdAplicacion()
        {
            string sIdAplicacion = string.Empty;

            if (GetUsuarioEstaAutenticado())
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    List<Claim> claims = identity.Claims.ToList();
                    sIdAplicacion = claims.Where(p => p.Type == ClaimsConfig.ID_APLICACION).FirstOrDefault()?.Value;
                }
            }
            return !string.IsNullOrWhiteSpace(sIdAplicacion) ? Convert.ToInt32(sIdAplicacion) : 0;
        }
        /// <summary>
        /// Retorna el id de la capitania
        /// </summary>
        /// <returns>Retorna el id de la capitania</returns>
        [Authorize]
        protected int GetIdCapitania()
        {
            string SidCapitania = string.Empty;

            if (GetUsuarioEstaAutenticado())
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    List<Claim> claims = identity.Claims.ToList();
                    SidCapitania = claims.Where(p => p.Type == ClaimsConfig.ID_CAPITANIA).FirstOrDefault()?.Value;
                }
            }
            return !string.IsNullOrWhiteSpace(SidCapitania) ? Convert.ToInt32(SidCapitania) : 0;
        }
        /// <summary>
        /// Retorna el login name del usuario autenticado
        /// </summary>
        /// <returns>Retorna el login name del usuario autenticado</returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>2020/05/06</Fecha>
        /// <UltimaActualizacion>2020/05/06 - Diego Parra - Creación del servicio</UltimaActualizacion>
        [Authorize]
        protected string GetLoginName()
        {
            string sLoginName = "";

            if (GetUsuarioEstaAutenticado())
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    List<Claim> claims = identity.Claims.ToList();
                    sLoginName = claims.Where(p => p.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                    sLoginName = string.IsNullOrEmpty(sLoginName) ? "" : sLoginName;
                }
            }
            return sLoginName;
        }

        /// <summary>
        /// Retorna el email
        /// </summary>
        /// <returns>Retorna el email</returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>2020/05/06</Fecha>
        /// <UltimaActualizacion>2020/05/06 - Diego Parra - Creación del servicio</UltimaActualizacion>
        [Authorize]
        protected string GetEmail()
        {
            string Email = "";

            if (GetUsuarioEstaAutenticado())
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    List<Claim> claims = identity.Claims.ToList();
                    Email = claims.Where(p => p.Type == ClaimTypes.Email).FirstOrDefault()?.Value;
                    Email = string.IsNullOrEmpty(Email) ? "" : SecurityEncrypt.Decrypt(ConfigurationManager.AppSettings["KeyPassword"].ToString(), Email);
                }
            }
            return Email;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        protected UserSesion GetUserSession()
        {
            var userSesion = new UserSesion();
            if (GetUsuarioEstaAutenticado())
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    // obtiene el id de la aplicación
                    string sAplicacionId = identity.Claims.Where(c => c.Type == ClaimsConfig.ID_APLICACION).Select(c => c.Value).SingleOrDefault();
                    sAplicacionId = string.IsNullOrEmpty(sAplicacionId) ? string.Empty : sAplicacionId;
                    if (int.TryParse(sAplicacionId, out int idAplicacion))
                    {
                        userSesion.Aplicacion = new AplicacionSession()
                        {
                            Id = idAplicacion
                        };
                    }
                    // obtiene el nombre del usuario
                    string sNombresUsuario = identity.Claims.Where(c => c.Type == ClaimsConfig.NOMBRES_USUARIO).Select(c => c.Value).SingleOrDefault();
                    sNombresUsuario = string.IsNullOrEmpty(sNombresUsuario) ? string.Empty : sNombresUsuario;
                    userSesion.NombresUsuario = sNombresUsuario;
                    // obtiene el apellido del usuario
                    string sApellidosUsuario = identity.Claims.Where(c => c.Type == ClaimsConfig.APELLIDOS_USUARIO).Select(c => c.Value).SingleOrDefault();
                    sApellidosUsuario = string.IsNullOrEmpty(sApellidosUsuario) ? string.Empty : sApellidosUsuario;
                    userSesion.ApellidosUsuario = sApellidosUsuario;

                    // obtiene el email del usuario
                    string sEmail = identity.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault();
                    sEmail = string.IsNullOrEmpty(sEmail) ? string.Empty : sEmail;
                    userSesion.Email = sEmail;

                    // obtiene el id de la categoria
                    string sIdCategoria = identity.Claims.Where(c => c.Type == ClaimsConfig.ID_CATEGORIA).Select(c => c.Value).SingleOrDefault();
                    sIdCategoria = string.IsNullOrEmpty(sIdCategoria) ? string.Empty : sIdCategoria;


                    // obtiene la capitanía del usuario
                    string sCapitania = identity.Claims.Where(c => c.Type == ClaimsConfig.CAPITANIA).Select(c => c.Value).SingleOrDefault();
                    sCapitania = string.IsNullOrEmpty(sCapitania) ? string.Empty : sCapitania;
                    userSesion.Capitania = new CapitaniaSession()
                    {
                        Descripcion = sCapitania,
                        Categoria = Convert.ToInt32(sIdCategoria)
                    };

                    // obtiene el estado del usuario
                    string sIdEstado = identity.Claims.Where(c => c.Type == ClaimsConfig.ID_ESTADO).Select(c => c.Value).SingleOrDefault();
                    sIdEstado = string.IsNullOrEmpty(sIdEstado) ? string.Empty : sIdEstado;
                    if (int.TryParse(sIdEstado, out int idEstado))
                    {
                        userSesion.EstadoId = idEstado;
                    }
                }
            }
            return userSesion;
        }




        /// <summary>
        /// Retorna el nombre del usuario si esta autenticado
        /// </summary>
        /// <returns>Nombre del usuario</returns>
        /// <Autor>Diego Parra</Autor>
        /// <Fecha>2020/05/06</Fecha>
        /// <UltimaActualizacion>2020/05/06 - Diego Parra - Creación del servicio</UltimaActualizacion>
        [Authorize]
        protected string GetNombreCompletoUsuario()
        {
            string sNombreCompletoUsuario = "";

            if (GetUsuarioEstaAutenticado())
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    List<Claim> claims = identity.Claims.ToList();
                    sNombreCompletoUsuario = claims.Where(p => p.Type == ClaimsConfig.NOMBRE_COMPLETO_USUARIO).FirstOrDefault()?.Value;
                    sNombreCompletoUsuario = string.IsNullOrEmpty(sNombreCompletoUsuario) ? "" : sNombreCompletoUsuario;
                }
            }
            return sNombreCompletoUsuario;
        }

        #endregion
    }
}