using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Config;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Seguridad;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DIMARCore.Api.Core
{
    /// <summary>
    /// Clase encargada en la generación de unToken 
    /// </summary>
    internal static class TokenGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public static string GenerarTokenJwt(UserSesionDTO usuario)
        {
            if (usuario == null)
            {
                return null;
            }

            // Configuración del Token JWT
            var secretKey = ConfigurationManager.AppSettings[AutenticacionConfig.TOKEN_JWT_SECRET_KEY];
            var audienceToken = ConfigurationManager.AppSettings[AutenticacionConfig.TOKEN_SITE_URL];
            var issuerToken = ConfigurationManager.AppSettings[AutenticacionConfig.TOKEN_SITE_URL];
            var expireTime = ConfigurationManager.AppSettings[AutenticacionConfig.TOKEN_JWT_EXPIRE_MINUTES];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // crear el claimsIdentity
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, SecurityEncrypt.EncryptWithSaltHash(usuario.LoginId.ToString(),ConfigurationManager.AppSettings[Constantes.NAME_KEY_ENCRYPTION].ToString())),
                new Claim(ClaimsConfig.ID_APLICACION, usuario.Aplicacion != null ? usuario.Aplicacion.Id.ToString() : ((int)TipoAplicacionEnum.ServicioLocal).ToString()), // NOTA: se convierte en string
                new Claim(ClaimsConfig.NOMBRE_APLICACION, usuario.Aplicacion != null ? usuario.Aplicacion.Nombre : TipoAplicacionEnum.ServicioLocal.ToString()), // NOTA: se convierte en string
                new Claim(ClaimsConfig.NOMBRES_USUARIO, !String.IsNullOrEmpty(usuario.NombresUsuario) ? usuario.NombresUsuario : String.Empty),
                new Claim(ClaimsConfig.APELLIDOS_USUARIO, !String.IsNullOrEmpty(usuario.ApellidosUsuario) ? usuario.ApellidosUsuario : String.Empty),
                new Claim(ClaimsConfig.NOMBRE_COMPLETO_USUARIO, !String.IsNullOrEmpty(usuario.NombreCompletoUsuario) ? usuario.NombreCompletoUsuario : String.Empty),
                new Claim(ClaimTypes.NameIdentifier, !String.IsNullOrEmpty(usuario.LoginName) ?SecurityEncrypt.EncryptWithSaltHash(usuario.LoginName,ConfigurationManager.AppSettings[Constantes.NAME_KEY_ENCRYPTION].ToString()) : string.Empty),
                new Claim(ClaimTypes.Email, !String.IsNullOrEmpty(usuario.Email) ? SecurityEncrypt.EncryptWithSaltHash(usuario.Email,ConfigurationManager.AppSettings[Constantes.NAME_KEY_ENCRYPTION].ToString()) : string.Empty),
                new Claim(ClaimsConfig.ID_ESTADO, usuario.EstadoId.ToString()),
                new Claim(ClaimsConfig.CAPITANIA, usuario.Capitania != null ? !String.IsNullOrEmpty(usuario.Capitania.Descripcion)
                ? usuario.Capitania.Descripcion : string.Empty : string.Empty),
                new Claim(ClaimsConfig.ID_CATEGORIA, usuario.CategoriaId.ToString()),
                new Claim(ClaimsConfig.ID_CAPITANIA, usuario.Capitania != null ? usuario.Capitania.Id.ToString() : ((int)TipoAplicacionEnum.ServicioLocal).ToString()) // NOTA: se convierte en string
            };


            foreach (var item in usuario.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item.NombreRol));
            }
            // crear el token para el usuario
            var token = new JwtSecurityToken(issuer: issuerToken,
                            audience: audienceToken,
                            claims: claims,
                            expires: DateTime.Now.AddMinutes(Convert.ToInt32(expireTime)),
                            signingCredentials: signingCredentials);

            var jwtTokenString = new JwtSecurityTokenHandler().WriteToken(token);
            // retornar el token
            return jwtTokenString;
        }


    }
}