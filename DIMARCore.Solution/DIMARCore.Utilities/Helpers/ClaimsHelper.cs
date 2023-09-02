using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Seguridad;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading;

namespace DIMARCore.Utilities.Config
{
    public class ClaimsHelper
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static int GetLoginId()
        {
            int usuarioId = 0;
            try
            {
                var claimsIdentity = (ClaimsIdentity)Thread.CurrentPrincipal.Identity;
                if (claimsIdentity != null)
                {
                    List<Claim> claims = claimsIdentity.Claims.ToList();
                    var claim = claims.Where(p => p.Type == ClaimTypes.Sid).FirstOrDefault();
                    usuarioId = claim == null ? 0 : int.Parse(SecurityEncrypt.DecryptWithSaltHash(claim.Value,
                                                            ConfigurationManager.AppSettings[Constantes.NAME_KEY_ENCRYPTION].ToString()));
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return usuarioId;
        }

        public static string GetNombreCompletoUsuario()
        {
            string usuario = string.Empty;
            try
            {
                var claimsIdentity = (ClaimsIdentity)Thread.CurrentPrincipal.Identity;
                if (claimsIdentity != null)
                {
                    List<Claim> claims = claimsIdentity.Claims.ToList();
                    var claim = claims.Where(p => p.Type == ClaimsConfig.NOMBRE_COMPLETO_USUARIO).FirstOrDefault();
                    usuario = claim == null ? "Anonimo" : string.IsNullOrEmpty(claim.Value) ? "Anonimo" : claim.Value;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return usuario;
        }
        public static int GetCategoriaUsuario()
        {
            int categoria = 0;
            try
            {
                var claimsIdentity = (ClaimsIdentity)Thread.CurrentPrincipal.Identity;
                if (claimsIdentity != null)
                {
                    List<Claim> claims = claimsIdentity.Claims.ToList();
                    var claim = claims.Where(p => p.Type == ClaimsConfig.ID_CATEGORIA).FirstOrDefault();
                    categoria = claim == null ? 0 : Convert.ToInt32(claim.Value);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return categoria;
        }

        public static int GetCapitaniaUsuario()
        {
            int capitania = 0;
            try
            {
                var claimsIdentity = (ClaimsIdentity)Thread.CurrentPrincipal.Identity;
                if (claimsIdentity != null)
                {
                    List<Claim> claims = claimsIdentity.Claims.ToList();
                    var claim = claims.Where(p => p.Type == ClaimsConfig.ID_CAPITANIA).FirstOrDefault();
                    capitania = claim == null ? 0 : Convert.ToInt32(claim.Value);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return capitania;
        }
        public static string GetLoginName()
        {
            string loginName = string.Empty;
            try
            {
                var claimsIdentity = (ClaimsIdentity)Thread.CurrentPrincipal.Identity;
                if (claimsIdentity != null)
                {
                    List<Claim> claims = claimsIdentity.Claims.ToList();
                    var claim = claims.Where(p => p.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                    loginName = claim == null ? "Anonimo" : SecurityEncrypt.DecryptWithSaltHash(claim.Value,
                                                            ConfigurationManager.AppSettings[Constantes.NAME_KEY_ENCRYPTION].ToString());
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return loginName;
        }
        public static string GetEmail()
        {
            string email = string.Empty;
            try
            {
                var claimsIdentity = (ClaimsIdentity)Thread.CurrentPrincipal.Identity;
                if (claimsIdentity != null)
                {
                    List<Claim> claims = claimsIdentity.Claims.ToList();
                    var claim = claims.Where(p => p.Type == ClaimTypes.Email).FirstOrDefault();
                    email = claim == null ? "Anonimo" : SecurityEncrypt.DecryptWithSaltHash(claim.Value,
                                                            ConfigurationManager.AppSettings[Constantes.NAME_KEY_ENCRYPTION].ToString());
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return email;
        }

        public static int GetAplicacionId()
        {
            int aplicacionId = 0;
            try
            {
                var claimsIdentity = (ClaimsIdentity)Thread.CurrentPrincipal.Identity;
                if (claimsIdentity != null)
                {
                    List<Claim> claims = claimsIdentity.Claims.ToList();
                    var claim = claims.Where(p => p.Type == ClaimsConfig.ID_APLICACION).FirstOrDefault();
                    aplicacionId = claim == null ? 0 : Convert.ToInt32(claim.Value);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return aplicacionId;
        }
    }
}
