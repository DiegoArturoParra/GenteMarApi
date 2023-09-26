using DIMARCore.Business.Logica;
using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using DIMARCore.Utilities.Seguridad;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace DIMARCore.Business
{
    public class SeguridadBO
    {
        public async Task<Respuesta> ValidarUsuario(string userName, string password)
        {
            // limpia los datos
            userName = userName.Trim();
            password = password.Trim();
            await new AplicacionRolesBO().ExisteRolesByAplicacion();

            var respuesta = new ActiveDirectory().GetUserByActiveDirectory(userName, password);
            if (respuesta.Estado)
            {
                UserDirectory userDirectory = (UserDirectory)respuesta.Data;
                var usuario = await new UsuarioRepository().GetUsuarioByLoginName(userDirectory.LoginName);
                ValidacionesDeNegocio(usuario);

                usuario.Identificacion = userDirectory.Identificacion;
                usuario.Email = userDirectory.Email;
                return Responses.SetOkResponse(usuario);
            }
            return respuesta;
        }

        private static void ValidacionesDeNegocio(UserSesionDTO usuario)
        {
            if (usuario == null)
                throw new HttpStatusCodeException(Responses.SetUnathorizedResponse("La combinación usuario/contraseña es incorrecta."));

            else if (!usuario.Roles.Any())
                throw new HttpStatusCodeException(Responses.SetUnathorizedResponse("No cuenta con roles asignados en el aplicativo de Gente De Mar, comuniquese con el administrador."));

            else if (usuario.EstadoId == (int)EstadoUsuarioLoginEnum.RETIRO)
                throw new HttpStatusCodeException(Responses.SetConflictResponse("Su cuenta esta desactivada, comuniquese con el administrador."));
        }

        public Respuesta ResultadoAutenticacion(UserSesionDTO userSession, string token)
        {
            List<string> roles = new List<string>();
            foreach (var item in userSession.Roles)
            {
                roles.Add(item.NombreRol);
            }

            if (userSession.Capitania == null)
                throw new HttpStatusCodeException(Responses.SetConflictResponse("La persona no cuenta con una capitania asociada."));


            string cacheKey = $"Roles_{userSession.LoginName}";

            MemoryCache memoryCache = MemoryCache.Default;
            if (memoryCache.Contains(cacheKey))
            {
                memoryCache.Remove(cacheKey);
            }

            return Responses.SetOkResponse(new UserTokenDTO
            {
                Token = token,
                LoginName = userSession.LoginName,
                NombreCompleto = userSession.NombreCompletoUsuario,
                Capitania = $"{userSession.Capitania.Sigla} {userSession.Capitania.Descripcion}",
                Roles = roles,
                Aplicacion = userSession.Aplicacion.Nombre
            });
        }

        public async Task<Respuesta> ValidarUsuarioTest(string userName, string password)
        {
            await new AplicacionRolesBO().ExisteRolesByAplicacion();
            userName = userName.Trim();
            password = password.Trim();
            var claveEncrypt = SecurityEncrypt.EncryptWithSaltHash(ConfigurationManager.AppSettings[Constantes.NAME_KEY_ENCRYPTION].ToString(), password);
            var usuario = await new UsuarioRepository().GetUsuarioByLoginNameTesting(userName, claveEncrypt);

            ValidacionesDeNegocio(usuario);

            return Responses.SetOkResponse(usuario);
        }
    }
}
