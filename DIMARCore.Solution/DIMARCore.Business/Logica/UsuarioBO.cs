using DIMARCore.Repositories.Repository;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DIMARCore.Business
{
    public class UsuarioBO
    {

        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Obtiene un usuario dado su login
        /// </summary>
        /// <param name="login">Login del usuario</param>
        /// <returns>Usuario (Incluye información del perfil de usuario)</returns>
        public async Task<APLICACIONES_LOGINS> GetUsuarioByLogin(string login)
        {
            using (var repositorio = new UsuarioRepository())
            {
                APLICACIONES_LOGINS usuario = await repositorio.GetWithCondition(u => u.LOGIN_NAME.Equals(login));
                return usuario;
            }
        }
        public async Task<List<int>> GetRoles(string loginName)
        {
            using (var repositorio = new UsuarioRepository())
            {
                return await repositorio.GetRoles(loginName);
            }
        }
        /// <summary>
        /// Obtiene un usuario dado su id
        /// </summary>
        /// <param name="id">Id del usuario</param>
        /// <returns>Usuario</returns>
        public async Task<APLICACIONES_LOGINS> GetUsuario(decimal id)
        {
            // Se busca el usuario utilizando el id
            using (var repositorio = new UsuarioRepository())
            {
                APLICACIONES_LOGINS usuario = await repositorio.GetWithCondition(u => u.ID_LOGIN == id);
                return usuario;
            }
        }
    }
}
