using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading;

namespace DIMARCore.Utilities.Config
{
    public class AuditoriaHelper
    {
        public string ObtenerUsuarioActual()
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
    }
}
