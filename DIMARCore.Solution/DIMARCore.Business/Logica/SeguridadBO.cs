using DIMARCore.Business.Logica;
using DIMARCore.Repositories.Repository;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Seguridad;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Business
{
    public class SeguridadBO
    {
        public async Task<Respuesta> ValidarUsuario(string Username, string Password)
        {
            await new AplicacionRolesBO().ExisteRolesByAplicacion();

            var respuesta = new ActiveDirectory().GetUserActiveDirectory(Username, Password);
            if (respuesta.Estado)
            {
                UserDirectory userDirectory = (UserDirectory)respuesta.Data;
                var usuario = await new UsuarioRepository().GetUsuarioByLoginName(userDirectory.LoginName);
                if (usuario == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("La combinación usuario/contraseña es incorrecta."));
                else if (usuario.Roles.Count() == 0)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No cuenta con roles asignados al aplicativo de Gente De Mar, comuniquese con el administrador."));
                usuario.Identificacion = userDirectory.Identificacion;
                usuario.Email = userDirectory.Email;
                return Responses.SetOkResponse(usuario);
            }
            return respuesta;
        }

        public async Task<Respuesta> ValidarUsuarioTest(string username, string password)
        {
            await new AplicacionRolesBO().ExisteRolesByAplicacion();

            var claveEncrypt = SecurityEncrypt.Encrypt(ConfigurationManager.AppSettings["KeyPassword"].ToString(), password);
            var usuario = await new UsuarioRepository().GetUsuarioByLoginNameTesting(username, claveEncrypt);
            if (usuario == null)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("La combinación usuario/contraseña es incorrecta."));
            else if (usuario.Roles.Count() == 0)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No cuenta con roles asignados al aplicativo de Gente De Mar, comuniquese con el administrador."));

            return Responses.SetOkResponse(usuario);
        }
    }
}
