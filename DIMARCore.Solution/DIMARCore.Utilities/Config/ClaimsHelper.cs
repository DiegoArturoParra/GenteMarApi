using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading;

namespace DIMARCore.Utilities.Config
{
    public class ClaimsHelper
    {
        public static string ObtenerUsuarioActual()
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
                Debug.WriteLine(ex.Message);
                return "Anonimo";
            }
            return usuario;
        }
        public static int ObtenerCategoriaUsuario()
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
                Debug.WriteLine(ex.Message);
                return 0;
            }
            return categoria;
        }

        public static int ObtenerCapitaniaUsuario()
        {
            int categoria = 0;
            try
            {
                var claimsIdentity = (ClaimsIdentity)Thread.CurrentPrincipal.Identity;
                if (claimsIdentity != null)
                {
                    List<Claim> claims = claimsIdentity.Claims.ToList();
                    var claim = claims.Where(p => p.Type == ClaimsConfig.ID_CAPITANIA).FirstOrDefault();
                    categoria = claim == null ? 0 : Convert.ToInt32(claim.Value);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return 0;
            }
            return categoria;
        }
    }
}
