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
        public async Task<UserSesionDTO> ValidarUsuario(string userName, string password)
        {
            // limpia los datos
            userName = userName.Trim();
            password = password.Trim();
           await new AplicacionRolesBO().ExisteRolesByAplicacion();
            var respuesta = new ActiveDirectory().GetUserByActiveDirectory(userName, password);
            UserDirectory userDirectory = (UserDirectory)respuesta.Data;
            await IsExistLoginNameOfUser(userDirectory.LoginName);
            var usuario = await new UsuarioRepository().GetUsuarioByLoginName(userDirectory.LoginName);
            VerificarEstadoUsuarioSesion(usuario);
            usuario.Identificacion = userDirectory.Identificacion;
            usuario.Email = userDirectory.Email;
            return usuario;
        }

      
        public Respuesta ResultadoAutenticacion(UserSesionDTO userSession, string token)
        {
            List<string> roles = new List<string>();
            foreach (var item in userSession.Roles)
            {
                roles.Add(item.NombreRol);
            }

            if (userSession.Capitania is null)
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

        public async Task<UserSesionDTO> ValidarUsuarioTest(string userName, string password)
        {
            await new AplicacionRolesBO().ExisteRolesByAplicacion();
            userName = userName.Trim();
            password = password.Trim();
            var claveEncrypt = SecurityEncrypt.EncryptWithSaltHash(ConfigurationManager.AppSettings[Constantes.NAME_KEY_ENCRYPTION].ToString(), password);
            await IsExistLoginNameOfUser(userName);
            var usuario = await new UsuarioRepository().GetUsuarioByLoginNameTesting(userName, claveEncrypt);
            VerificarEstadoUsuarioSesion(usuario);
            return usuario;
        }

        private async Task IsExistLoginNameOfUser(string loginName)
        {
            bool exist = await new UsuarioRepository().AnyWithCondition(x => x.LOGIN_NAME.ToUpper().Equals(loginName.ToUpper()));
            if (!exist)
                throw new HttpStatusCodeException(Responses.SetUnathorizedResponse("El usuario no tiene permisos para acceder a este aplicativo."));
        }

        private static void VerificarEstadoUsuarioSesion(UserSesionDTO usuario)
        {
            if (usuario is null)
                throw new HttpStatusCodeException(Responses.SetUnathorizedResponse("La combinación usuario/contraseña es incorrecta."));

            else if (!usuario.Roles.Any())
                throw new HttpStatusCodeException(Responses.SetUnathorizedResponse("No cuenta con roles asignados en el aplicativo de Gente De Mar, comuniquese con el administrador."));

            else if (usuario.EstadoId == (int)EstadoUsuarioLoginEnum.RETIRO)
                throw new HttpStatusCodeException(Responses.SetConflictResponse("Su cuenta esta desactivada, comuniquese con el administrador."));
        }

    }
}
