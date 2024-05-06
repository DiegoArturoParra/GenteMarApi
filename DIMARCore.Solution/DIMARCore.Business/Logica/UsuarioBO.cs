using DIMARCore.Business.Helpers;
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
            var data = new ActiveDirectory().GetUsersByActiveDirectory(filtro);
            if (!data.Estado)
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, data.Mensaje);
            return (IList<UserDirectory>)data.Data;
        }

        public async Task<Respuesta> CreateUserGDM(UsuarioGDMDTO usuarioTriton)
        {
            using (var repositorio = new UsuarioRepository())
            {
                var (User, tieneRoles) = await ValidacionCrearUsuarioGDM(usuarioTriton);
                if (!tieneRoles && User is null)
                {
                    var claveEncrypt = SecurityEncrypt.EncryptWithSaltHash(ConfigurationManager.AppSettings[Constantes.NAME_KEY_ENCRYPTION].ToString(),
                                                                          Constantes.PASS_DEFAULT);
                    APLICACIONES_LOGINS user = new APLICACIONES_LOGINS()
                    {
                        NOMBRES = usuarioTriton.Nombres,
                        CORREO = usuarioTriton.Correo.Trim(),
                        LOGIN_NAME = usuarioTriton.LoginName.Trim(),
                        FECHA_CREACION = DateTime.Now,
                        ID_CAPITANIA = usuarioTriton.CapitaniaId,
                        ID_ESTADO = (int)EstadoUsuarioLoginEnum.ACTIVO,
                        ID_USUARIO_REGISTRO = ClaimsHelper.GetLoginId(),
                        ID_UNIDAD = usuarioTriton.CapitaniaId != (int)CapitaniasEnum.DIMAR ? (int)TipoUnidadEnum.SUBMERC : (int)TipoUnidadEnum.CP,
                        APELLIDOS = usuarioTriton.Apellidos,
                        ID_TIPO_ESTADO = (int)EstadoUsuarioLoginEnum.USUARIONUEVO,
                        PASSWORD = claveEncrypt
                    };
                    await repositorio.CreateUserGDM(user, usuarioTriton.RolesId);
                    return Responses.SetCreatedResponse(usuarioTriton);
                }
                else
                {
                    var response = await UpdateUser(User, usuarioTriton);
                    if (!response.Estado)
                        throw new HttpStatusCodeException(response);

                    return Responses.SetCreatedResponse(usuarioTriton);
                }

            }
        }

        private async Task<(APLICACIONES_LOGINS user, bool IsExist)> ValidacionCrearUsuarioGDM(UsuarioGDMDTO usuarioTriton, bool isUpdate = false)
        {
            if (!isUpdate)
            {
                var existeUser = await new UsuarioRepository().GetWithConditionAsync(y => y.LOGIN_NAME.ToUpper().Equals(usuarioTriton.LoginName.ToUpper()));
                if (existeUser != null)
                {
                    var roles = await new UsuarioRepository().GetRolesByLoginName(existeUser.LOGIN_NAME);
                    if (roles.Any())
                        throw new HttpStatusCodeException(Responses.SetConflictResponse($"El usuario {usuarioTriton.LoginName} ya contiene roles en el aplicativo de Gente De Mar."));
                    else
                        return (existeUser, false);
                }
            }
            else
            {
                var existeUser = await new AplicacionLoginRepository().AnyWithConditionAsync(y => y.LOGIN_NAME.Equals(usuarioTriton.LoginName)
                && y.ID_LOGIN != usuarioTriton.LoginId);
                if (existeUser)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"El usuario {usuarioTriton.LoginName} ya se encuentra registrado en Gente De Mar."));
            }

            var existeCapitania = await new AplicacionCapitaniaRepository().AnyWithConditionAsync(y => y.ID_CAPITANIA == usuarioTriton.CapitaniaId);
            if (!existeCapitania)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"No existe la capitania."));

            var tasks = usuarioTriton.RolesId.Select(item => new AplicacionRolesRepository().AnyWithConditionAsync(y => y.ID_ROL == item));
            var firstCompletedTask = await Task.WhenAny(tasks);
            var existeRol = await firstCompletedTask;
            if (!existeRol)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"No existe el rol."));
            return (null, false);
        }

        public async Task<Respuesta> UpdateUserGDM(UsuarioGDMDTO usuarioTriton)
        {
            using (var repositorio = new UsuarioRepository())
            {
                await ValidacionCrearUsuarioGDM(usuarioTriton, true);
                var data = await repositorio.GetByIdAsync(usuarioTriton.LoginId)
                    ?? throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"No existe el usuario."));
                return await UpdateUser(data, usuarioTriton);
            }
        }

        private async Task<Respuesta> UpdateUser(APLICACIONES_LOGINS data, UsuarioGDMDTO dto)
        {
            data.CORREO = dto.Correo.Trim();
            data.LOGIN_NAME = dto.LoginName.Trim();
            data.FECHA_MODIFICACION = DateTime.Now;
            data.ID_CAPITANIA = dto.CapitaniaId;
            data.NOMBRES = dto.Nombres;
            data.ID_TIPO_ESTADO = (int)EstadoUsuarioLoginEnum.ACTIVO;
            data.APELLIDOS = dto.Apellidos;
            if (string.IsNullOrEmpty(data.PASSWORD))
            {
                var claveEncrypt = SecurityEncrypt.EncryptWithSaltHash(ConfigurationManager.AppSettings[Constantes.NAME_KEY_ENCRYPTION].ToString(),
                                                                                             Constantes.PASS_DEFAULT);
                data.PASSWORD = claveEncrypt;
            }
            await new UsuarioRepository().UpdateUserTriton(data, dto.RolesId);
            RemoveRolesCacheados(data);
            return Responses.SetUpdatedResponse(dto);
        }

        private static void RemoveRolesCacheados(APLICACIONES_LOGINS data)
        {
            MemoryCacheHelper.Remove($"Roles_{data.LOGIN_NAME}");
            MemoryCacheHelper.Remove($"Roles_{data.ID_LOGIN}");
        }

        public async Task<Respuesta> InactivarOActivarUsuario(int id)
        {

            using (var repo = new UsuarioRepository())
            {
                var entidad = await repo.GetByIdAsync(id) ?? throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"No existe el usuario"));
                bool activoOretiro = await repo.InactivarOActivarUsuario(entidad.ID_LOGIN);
                RemoveRolesCacheados(entidad);
                if (activoOretiro)
                    return Responses.SetOkResponse(entidad, $"Se inactivo el usuario {entidad.LOGIN_NAME}");

                return Responses.SetOkResponse(entidad, $"Se activo el usuario {entidad.LOGIN_NAME}");

            }
        }
    }
}
