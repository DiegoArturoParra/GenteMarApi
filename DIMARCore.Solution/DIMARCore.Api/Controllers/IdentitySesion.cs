/***
Autor: Carlos Rodríguez
Fecha: 2021/09/07
Descripción: Esta clase permite obtener los datos del usuario autenticado del token de autenticación.
***/
using DIMARCore.UIEntities.Models;
using DIMARCore.Utilities.Config;
using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace DIMARCore.Api.Controllers
{
    /// <summary>
    /// IdentitySesion
    /// </summary>
    public class IdentitySesion
    {
        /// <summary>
        /// Retorna el usuario actual identificado
        /// </summary>
        /// <param name="controller"></param>
        /// <returns>Usuario autenticado</returns>
        /// <Autor>Carlos Rodríguez</Autor>
        /// <Fecha>2020/07/29</Fecha>
        /// <UltimaActualizacion>2020/05/20 - Carlos Rodríguez - Creación del método</UltimaActualizacion>
        public static UserSesion GetUsuarioActual(ApiController controller)
        {
            UserSesion userSesion = null;

            if (controller == null)
            {
                return null;
            }

            // obtiene los claims identity
            ClaimsIdentity identity = (ClaimsIdentity)controller.User.Identity;
            if (identity == null)
            {
                return null;
            }

            userSesion = new UserSesion();
            // obtiene el id de la aplicación
            string sAplicacionId = identity.Claims.Where(c => c.Type == ClaimsConfig.ID_APLICACION).Select(c => c.Value).SingleOrDefault();
            sAplicacionId = string.IsNullOrEmpty(sAplicacionId) ? string.Empty : sAplicacionId;
            if (int.TryParse(sAplicacionId, out int idAplicacion))
            {
                userSesion.IdAplicacion = idAplicacion;
            }
            // obtiene el id del usuario
            string sIdUsuario = identity.Claims.Where(c => c.Type == ClaimsConfig.ID_USUARIO).Select(c => c.Value).SingleOrDefault();
            sIdUsuario = string.IsNullOrEmpty(sIdUsuario) ? string.Empty : sIdUsuario;
            if (Decimal.TryParse(sIdUsuario, out decimal idUsuario))
            {
                userSesion.IdUsuario = idUsuario;
            }
            // obtiene el nombre del usuario
            string sNombresUsuario = identity.Claims.Where(c => c.Type == ClaimsConfig.NOMBRES_USUARIO).Select(c => c.Value).SingleOrDefault();
            sNombresUsuario = string.IsNullOrEmpty(sNombresUsuario) ? string.Empty : sNombresUsuario;
            userSesion.NombresUsuario = sNombresUsuario;
            // obtiene el apellido del usuario
            string sApellidosUsuario = identity.Claims.Where(c => c.Type == ClaimsConfig.APELLIDOS_USUARIO).Select(c => c.Value).SingleOrDefault();
            sApellidosUsuario = string.IsNullOrEmpty(sApellidosUsuario) ? string.Empty : sApellidosUsuario;
            userSesion.ApellidosUsuario = sApellidosUsuario;
            // obtiene el nombre completo del usuario
            string sNombreCompletoUsuario = identity.Claims.Where(c => c.Type == ClaimsConfig.NOMBRE_COMPLETO_USUARIO).Select(c => c.Value).SingleOrDefault();
            sNombreCompletoUsuario = string.IsNullOrEmpty(sNombreCompletoUsuario) ? string.Empty : sNombreCompletoUsuario;
            userSesion.NombreCompletoUsuario = sNombreCompletoUsuario;
            // obtiene el email del usuario
            string sEmail = identity.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault();
            sEmail = string.IsNullOrEmpty(sEmail) ? string.Empty : sEmail;
            userSesion.Email = sEmail;
            // obtiene la capitanía del usuario
            string sCapitania = identity.Claims.Where(c => c.Type == ClaimsConfig.CAPITANIA).Select(c => c.Value).SingleOrDefault();
            sCapitania = string.IsNullOrEmpty(sCapitania) ? string.Empty : sCapitania;
            userSesion.Capitania = sCapitania;
            // obtiene la agencia del usuario
            string sAgencia = identity.Claims.Where(c => c.Type == ClaimsConfig.AGENCIA).Select(c => c.Value).SingleOrDefault();
            sAgencia = string.IsNullOrEmpty(sAgencia) ? string.Empty : sAgencia;
            userSesion.Agencia = sAgencia;
            // obtiene el estado del usuario
            string sIdEstado = identity.Claims.Where(c => c.Type == ClaimsConfig.ID_ESTADO).Select(c => c.Value).SingleOrDefault();
            sIdEstado = string.IsNullOrEmpty(sIdEstado) ? string.Empty : sIdEstado;
            if (int.TryParse(sIdEstado, out int idEstado))
            {
                userSesion.IdEstado = idEstado;
            }


            return userSesion;
        }

    }
}