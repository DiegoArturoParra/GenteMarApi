using DIMARCore.Business.Helpers;
using DIMARCore.Business.Logica;
using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Config;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using DIMARCore.Utilities.Seguridad;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DIMARCore.Business
{
    public class UsuarioBO
    {

        private readonly int _expirationCache;
        public UsuarioBO()
        {
            _expirationCache = int.Parse(ConfigurationManager.AppSettings[Constantes.KEY_EXPIRATION_CACHE]);
        }

        /// <summary>
        /// Obtiene un usuario dado su login
        /// </summary>
        /// <param name="login">Login del usuario</param>
        /// <returns>Usuario (Incluye información del perfil de usuario)</returns>
        public async Task<APLICACIONES_LOGINS> GetUsuarioByLogin(string login)
        {
            using (var repositorio = new UsuarioRepository())
            {
                APLICACIONES_LOGINS usuario = await repositorio.GetWithConditionAsync(u => u.LOGIN_NAME.Equals(login));
                return usuario;
            }
        }
        public async Task<List<int>> GetRolesByLoginName(string loginName)
        {
            // Verificar si el resultado ya está en caché

            string cacheKey = $"Roles_{loginName}";
            if (MemoryCacheHelper.Contains(cacheKey))
            {
                var cachedRoles = MemoryCacheHelper.Get<List<int>>(cacheKey);
                return cachedRoles;
            }

            using (var repositorio = new UsuarioRepository())
            {
                var roles = await repositorio.GetRolesByLoginName(loginName);
                MemoryCacheHelper.Add(cacheKey, roles, _expirationCache);
                return roles;
            }
        }

        public async Task<List<int>> GetRolesByLoginId(int loginId)
        {
            string cacheKey = $"Roles_{loginId}";
            if (MemoryCacheHelper.Contains(cacheKey))
            {
                var cachedRoles = MemoryCacheHelper.Get<List<int>>(cacheKey);
                return cachedRoles;
            }

            using (var repositorio = new UsuarioRepository())
            {
                var roles = await repositorio.GetRolesByLoginId(loginId);
                MemoryCacheHelper.Add(cacheKey, roles, _expirationCache);
                return roles;
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
                APLICACIONES_LOGINS usuario = await repositorio.GetWithConditionAsync(u => u.ID_LOGIN == id);
                return usuario;
            }
        }

        /// <summary>
        ///  Obtiene los usuarios registrados en Gente de mar
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<InfoUsuarioDTO>> GetUsuarios()
        {
            using (var repositorio = new UsuarioRepository())
            {
                IEnumerable<InfoUsuarioDTO> usuarios = await repositorio.GetUsuariosConRoles();
                return usuarios;
            }
        }

        public IEnumerable<UserDirectory> GetUsuariosPorDirectorioActivo(ActiveDirectoryFilter filtro)
        {
            if (filtro == null)
            {
                return new List<UserDirectory>();
            }
            var data = new ConnectionActiveDirectory().GetUsersByActiveDirectory(filtro);
            if (!data.Estado)
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, data.Mensaje);
            return (IList<UserDirectory>)data.Data;
        }

        public async Task<Respuesta> CreateUserGDM(UsuariogdmDTO usuarioGDM)
        {
            using (var repositorio = new UsuarioRepository())
            {
                var (User, tieneRoles) = await ValidacionCrearUsuarioGDM(usuarioGDM);
                await ExisteCapitania(usuarioGDM.CapitaniaId);
                await ExisteRoles(usuarioGDM.RolesId);
                if (!tieneRoles && User is null)
                {
                    var claveEncrypt = EncryptDecryptService.EncryptWithSaltHash(ConfigurationManager.AppSettings[Constantes.KEY_ENCRYPTION_WITHOUT_HASH].ToString(),
                                                                          Constantes.PASS_DEFAULT);
                    APLICACIONES_LOGINS user = new APLICACIONES_LOGINS()
                    {
                        NOMBRES = usuarioGDM.Nombres.ToUpper().Trim(),
                        APELLIDOS = usuarioGDM.Apellidos.ToUpper().Trim(),
                        CORREO = usuarioGDM.Correo.Trim(),
                        LOGIN_NAME = usuarioGDM.LoginName.Trim(),
                        FECHA_CREACION = DateTime.Now,
                        ID_CAPITANIA = usuarioGDM.CapitaniaId,
                        ID_ESTADO = (int)EstadoUsuarioLoginEnum.ACTIVO,
                        ID_USUARIO_REGISTRO = ClaimsHelper.GetLoginId(),
                        ID_UNIDAD = usuarioGDM.CapitaniaId != (int)CapitaniasEnum.DIMAR ? (int)TipoUnidadEnum.SUBMERC : (int)TipoUnidadEnum.CP,
                        ID_TIPO_ESTADO = (int)EstadoUsuarioLoginEnum.USUARIONUEVO,
                        PASSWORD = claveEncrypt
                    };
                    await repositorio.CreateUserGDM(user, usuarioGDM.RolesId);
                    return Responses.SetCreatedResponse(usuarioGDM);
                }
                else
                {
                    var response = await UpdateUser(User, usuarioGDM);
                    if (!response.Estado)
                        throw new HttpStatusCodeException(response);

                    return Responses.SetCreatedResponse(usuarioGDM);
                }
            }
        }

        public async Task<Respuesta> UpdateUserGDM(UsuariogdmDTO usuarioGDM)
        {
            using (var repositorio = new UsuarioRepository())
            {
                await ValidacionActualizarUsuarioGDM(usuarioGDM);
                await ExisteCapitania(usuarioGDM.CapitaniaId);
                await ExisteRoles(usuarioGDM.RolesId);
                var data = await repositorio.GetByIdAsync(usuarioGDM.LoginId)
                    ?? throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"No existe el usuario en la aplicación de gente de mar."));
                return await UpdateUser(data, usuarioGDM);
            }
        }

        public async Task<Respuesta> InactivarOActivarUsuarioGDM(int id)
        {

            using (var repo = new UsuarioRepository())
            {
                var entidad = await repo.GetByIdAsync(id) ?? throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"No existe el usuario"));
                bool activoOretiro = await repo.InactivarOActivarUsuarioGDM(entidad.ID_LOGIN);
                RemoveRolesCacheados(entidad);
                if (activoOretiro)
                {
                    await new AutenticacionBO().RemoverTokensPorUsuarioId(entidad.ID_LOGIN);
                    return Responses.SetOkResponse(entidad, $"Se inactivo el usuario {entidad.LOGIN_NAME}");
                }
                return Responses.SetOkResponse(entidad, $"Se activo el usuario {entidad.LOGIN_NAME}");

            }
        }
        private async Task<(APLICACIONES_LOGINS user, bool IsExist)> ValidacionCrearUsuarioGDM(UsuariogdmDTO usuarioTriton)
        {
            APLICACIONES_LOGINS existeUser = null;
            existeUser = await new UsuarioRepository().GetWithConditionAsync(y => y.LOGIN_NAME.ToUpper().Equals(usuarioTriton.LoginName.ToUpper()));
            if (existeUser != null)
            {
                var roles = await new UsuarioRepository().GetRolesByLoginName(existeUser.LOGIN_NAME);
                if (roles.Any())
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"El usuario {usuarioTriton.LoginName} " +
                                                    $"ya contiene roles en el aplicativo de Gente De Mar."));
                return (existeUser, true);
            }
            return (existeUser, false);
        }

        private async Task ValidacionActualizarUsuarioGDM(UsuariogdmDTO usuarioTriton)
        {
            var existeUser = await new AplicacionLoginRepository().GetWithConditionAsync(
                        y => y.LOGIN_NAME.ToUpper().Equals(usuarioTriton.LoginName.ToUpper()) && y.ID_LOGIN != usuarioTriton.LoginId);
            if (existeUser != null)
            {
                var roles = await new UsuarioRepository().GetRolesByLoginName(existeUser.LOGIN_NAME);
                if (roles.Any())
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"El usuario {usuarioTriton.LoginName} " +
                                                    $"ya contiene roles en el aplicativo de Gente De Mar."));
            }
        }

        private async Task ExisteCapitania(int capitaniaId)
        {
            var existeCapitania = await new AplicacionCapitaniaRepository().AnyWithConditionAsync(y => y.ID_CAPITANIA == capitaniaId);
            if (!existeCapitania)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"No existe la capitania."));
        }

        private async Task ExisteRoles(List<int> rolesId)
        {
            var tasks = rolesId.Select(item => new AplicacionRolesRepository().AnyWithConditionAsync(y => y.ID_ROL == item));
            var firstCompletedTask = await Task.WhenAny(tasks);
            var existeRol = await firstCompletedTask;
            if (!existeRol)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"No existe el rol."));
        }
        private async Task<Respuesta> UpdateUser(APLICACIONES_LOGINS data, UsuariogdmDTO dto)
        {
            if (data == null)
                return Responses.SetNotFoundResponse($"No existe el usuario en la aplicación de gente de mar.");

            data.NOMBRES = dto.Nombres.ToUpper().Trim();
            data.APELLIDOS = dto.Apellidos.ToUpper().Trim();
            data.CORREO = dto.Correo.Trim();
            data.LOGIN_NAME = dto.LoginName.Trim();
            data.FECHA_MODIFICACION = DateTime.Now;
            data.ID_CAPITANIA = dto.CapitaniaId;
            data.ID_TIPO_ESTADO = (int)EstadoUsuarioLoginEnum.ACTIVO;
            if (string.IsNullOrWhiteSpace(data.PASSWORD))
            {
                var claveEncrypt = EncryptDecryptService.EncryptWithSaltHash(ConfigurationManager.AppSettings[Constantes.KEY_ENCRYPTION_WITHOUT_HASH].ToString(),
                                                                                             Constantes.PASS_DEFAULT);
                data.PASSWORD = claveEncrypt;
            }
            await new UsuarioRepository().UpdateUserRolesGDM(data, dto.RolesId);
            await new AutenticacionBO().RemoverTokensPorUsuarioId(data.ID_LOGIN);
            RemoveRolesCacheados(data);
            return Responses.SetUpdatedResponse(dto);
        }

        private static void RemoveRolesCacheados(APLICACIONES_LOGINS data)
        {
            if (data == null)
                return;
            if (data.ID_LOGIN == 0)
                return;
            if (string.IsNullOrWhiteSpace(data.LOGIN_NAME))
                return;
            MemoryCacheHelper.Remove($"Roles_{data.LOGIN_NAME}");
            MemoryCacheHelper.Remove($"Roles_{data.ID_LOGIN}");
        }


    }
}
