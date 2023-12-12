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
using System.Runtime.Caching;
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
                APLICACIONES_LOGINS usuario = await repositorio.GetWithCondition(u => u.LOGIN_NAME.Equals(login));
                return usuario;
            }
        }
        public async Task<List<int>> GetRolesByLoginName(string loginName)
        {
            // Verificar si el resultado ya está en caché
            MemoryCache memoryCache = MemoryCache.Default;
            string cacheKey = $"Roles_{loginName}";
            if (memoryCache.Contains(cacheKey))
            {
                var cachedRoles = (List<int>)memoryCache[cacheKey];
                return cachedRoles;
            }

            using (var repositorio = new UsuarioRepository())
            {
                var roles = await repositorio.GetRolesByLoginName(loginName);
                // Si no está en caché, obtener los roles y almacenarlos en caché por un tiempo determinado
                // 7 dias de expiración
                var cachePolicy = new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTime.Now.AddDays(_expirationCache)
                };
                memoryCache.Set(cacheKey, roles, cachePolicy);
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
                APLICACIONES_LOGINS usuario = await repositorio.GetWithCondition(u => u.ID_LOGIN == id);
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

        public async Task<Respuesta> CreateUserTriton(UsuarioTritonDTO usuarioTriton)
        {
            using (var repositorio = new UsuarioRepository())
            {
                await ValidacionCrearUsuarioTriton(usuarioTriton);
                var claveEncrypt = SecurityEncrypt.EncryptWithSaltHash(ConfigurationManager.AppSettings[Constantes.NAME_KEY_ENCRYPTION].ToString(), Constantes.PASS_DEFAULT);
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
                await repositorio.CreateUserTriton(user, usuarioTriton.RolesId);
                return Responses.SetCreatedResponse(usuarioTriton);
            }
        }

        private async Task ValidacionCrearUsuarioTriton(UsuarioTritonDTO usuarioTriton, bool isUpdate = false)
        {
            if (!isUpdate)
            {
                var existeUser = await new ServiciosAplicacionesRepository<APLICACIONES_LOGINS>().AnyWithCondition(y => y.LOGIN_NAME == usuarioTriton.LoginName);
                if (existeUser)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"El usuario ya se encuentra registrado en Gente De Mar."));
            }
            else
            {
                var existeUser = await new ServiciosAplicacionesRepository<APLICACIONES_LOGINS>().AnyWithCondition(y => y.LOGIN_NAME == usuarioTriton.LoginName && y.ID_LOGIN != usuarioTriton.LoginId);
                if (existeUser)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"El usuario ya se encuentra registrado en Gente De Mar."));
            }

            var existeCapitania = await new ServiciosAplicacionesRepository<APLICACIONES_CAPITANIAS>().AnyWithCondition(y => y.ID_CAPITANIA == usuarioTriton.CapitaniaId);
            if (!existeCapitania)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"No existe la capitania."));

            var tasks = usuarioTriton.RolesId.Select(item => new AplicacionRolesRepository().AnyWithCondition(y => y.ID_ROL == item));
            var firstCompletedTask = await Task.WhenAny(tasks);
            var existeRol = await firstCompletedTask;
            if (!existeRol)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"No existe el rol."));
        }

        public async Task<Respuesta> UpdateUserTriton(UsuarioTritonDTO usuarioTriton)
        {
            using (var repositorio = new UsuarioRepository())
            {
                await ValidacionCrearUsuarioTriton(usuarioTriton, true);
                var data = await repositorio.GetById(usuarioTriton.LoginId)
                    ?? throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"No existe el usuario."));
                data.CORREO = usuarioTriton.Correo.Trim();
                data.LOGIN_NAME = usuarioTriton.LoginName.Trim();
                data.FECHA_MODIFICACION = DateTime.Now;
                data.ID_CAPITANIA = usuarioTriton.CapitaniaId;
                data.NOMBRES = usuarioTriton.Nombres;
                data.ID_TIPO_ESTADO = (int)EstadoUsuarioLoginEnum.ACTIVO;
                data.APELLIDOS = usuarioTriton.Apellidos;
                await repositorio.UpdateUserTriton(data, usuarioTriton.RolesId);
                return Responses.SetUpdatedResponse(usuarioTriton);
            }
        }

        public async Task<Respuesta> InactivarOActivarUsuario(int id)
        {

            using (var repo = new UsuarioRepository())
            {
                var entidad = await repo.GetById(id) ?? throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"No existe el usuario"));
                bool activoOretiro = await repo.InactivarOActivarUsuario(entidad.ID_LOGIN);

                if (activoOretiro)
                    return Responses.SetOkResponse(entidad, $"Se inactivo el usuario {entidad.LOGIN_NAME}");

                return Responses.SetOkResponse(entidad, $"Se activo el usuario {entidad.LOGIN_NAME}");

            }
        }
    }
}
