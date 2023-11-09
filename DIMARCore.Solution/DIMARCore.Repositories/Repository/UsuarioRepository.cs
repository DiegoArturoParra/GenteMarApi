using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Enums;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class UsuarioRepository : GenericRepository<APLICACIONES_LOGINS>
    {
        public async Task CreateUserTriton(APLICACIONES_LOGINS user, List<int> rolesId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.APLICACIONES_LOGINS.Add(user);
                    await SaveAllAsync();

                    if (rolesId.Count > 0)
                    {
                        foreach (var item in rolesId)
                        {
                            APLICACIONES_LOGIN_ROL rol = new APLICACIONES_LOGIN_ROL
                            {
                                FECHA_ASIGNACION = DateTime.Now,
                                ID_ESTADO = 1,
                                ID_LOGIN = user.ID_LOGIN,
                                ID_ROL = item,
                            };
                            _context.APLICACIONES_LOGIN_ROL.Add(rol);
                        }
                    }
                    await SaveAllAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ObtenerException(ex, user);
                }
            }
        }
        public async Task UpdateUserTriton(APLICACIONES_LOGINS user, List<int> rolesId)
        {
            _context.APLICACIONES_LOGIN_ROL.RemoveRange(_context.APLICACIONES_LOGIN_ROL.Where(x => x.ID_LOGIN == user.ID_LOGIN));
            if (rolesId.Count > 0)
            {
                foreach (var item in rolesId)
                {
                    APLICACIONES_LOGIN_ROL rol = new APLICACIONES_LOGIN_ROL
                    {
                        FECHA_ASIGNACION = DateTime.Now,
                        ID_ESTADO = (byte)EstadoUsuarioLoginEnum.ACTIVO,
                        ID_LOGIN = user.ID_LOGIN,
                        ID_ROL = item,
                    };
                    _context.APLICACIONES_LOGIN_ROL.Add(rol);
                }
            }
            await Update(user);
        }

        public async Task<List<int>> GetRolesByLoginName(string Loginname)
        {
            return await (from login in _context.APLICACIONES_LOGINS
                          join login_rol in _context.APLICACIONES_LOGIN_ROL on login.ID_LOGIN equals login_rol.ID_LOGIN
                          join roles in _context.APLICACIONES_ROLES on login_rol.ID_ROL equals roles.ID_ROL
                          where login.LOGIN_NAME.Equals(Loginname) && roles.ID_APLICACION == (int)TipoAplicacionEnum.GenteDeMar
                          && login_rol.ID_ESTADO == (byte)EstadoUsuarioLoginEnum.ACTIVO
                          select roles.ID_ROL).ToListAsync();
        }

        public async Task<UserSesionDTO> GetUsuarioByLoginName(string loginName)
        {
            UserSesionDTO userSesion = null;
            var user = await GetWithCondition(x => x.LOGIN_NAME.ToUpper().Equals(loginName.ToUpper()));
            if (user != null)
            {
                var query = (from login in _context.APLICACIONES_LOGINS
                             where login.ID_LOGIN == user.ID_LOGIN
                             select new
                             {
                                 login,
                             });
                userSesion = await query.Select(m => new UserSesionDTO
                {
                    LoginId = m.login.ID_LOGIN,
                    NombresUsuario = m.login.NOMBRES,
                    ApellidosUsuario = m.login.APELLIDOS,
                    LoginName = m.login.LOGIN_NAME,
                    Aplicacion = (from aplicaciones in _context.APLICACIONES
                                  where aplicaciones.ID_APLICACION == (int)TipoAplicacionEnum.GenteDeMar
                                  select new AplicacionSession() { Id = aplicaciones.ID_APLICACION, Nombre = aplicaciones.NOMBRE }).FirstOrDefault(),
                    Capitania = (from logins in _context.APLICACIONES_LOGINS
                                 join capitania in _context.APLICACIONES_CAPITANIAS on logins.ID_CAPITANIA equals capitania.ID_CAPITANIA
                                 where logins.ID_CAPITANIA == m.login.ID_CAPITANIA
                                 select new CapitaniaSession()
                                 {
                                     Id = capitania.ID_CAPITANIA,
                                     Descripcion = capitania.DESCRIPCION,
                                     Sigla = capitania.SIGLA_CAPITANIA,
                                     Categoria = capitania.ID_CATEGORIA
                                 }).FirstOrDefault(),
                    EstadoId = m.login.ID_TIPO_ESTADO,
                    Roles = (from login_rol in _context.APLICACIONES_LOGIN_ROL
                             join roles in _context.APLICACIONES_ROLES on login_rol.ID_ROL equals roles.ID_ROL
                             into rol
                             from detalleRoles in rol.DefaultIfEmpty()
                             where login_rol.ID_LOGIN == user.ID_LOGIN && detalleRoles.ID_APLICACION == (int)TipoAplicacionEnum.GenteDeMar
                             && login_rol.ID_ESTADO == (byte)EstadoUsuarioLoginEnum.ACTIVO
                             select new RolSession { Id = detalleRoles.ID_ROL, NombreRol = detalleRoles.ROL }).ToList()
                }).FirstOrDefaultAsync();

            }
            return userSesion;
        }

        public async Task<UserSesionDTO> GetUsuarioByLoginNameTesting(string username, string password)
        {
            UserSesionDTO userSesion = null;
            var user = await GetWithCondition(x => x.LOGIN_NAME.ToUpper().Equals(username.ToUpper()) && x.PASSWORD.Equals(password));
            if (user != null)
            {
                var query = (from login in _context.APLICACIONES_LOGINS
                             where login.ID_LOGIN == user.ID_LOGIN
                             select new
                             {
                                 login,
                             });
                userSesion = await query.Select(m => new UserSesionDTO
                {
                    LoginId = m.login.ID_LOGIN,
                    NombresUsuario = m.login.NOMBRES,
                    ApellidosUsuario = m.login.APELLIDOS,
                    LoginName = m.login.LOGIN_NAME,
                    Email = m.login.CORREO,
                    Aplicacion = (from aplicaciones in _context.APLICACIONES
                                  where aplicaciones.ID_APLICACION == (int)TipoAplicacionEnum.GenteDeMar
                                  select new AplicacionSession() { Id = aplicaciones.ID_APLICACION, Nombre = aplicaciones.NOMBRE }).FirstOrDefault(),

                    Capitania = (from logins in _context.APLICACIONES_LOGINS
                                 join capitania in _context.APLICACIONES_CAPITANIAS on logins.ID_CAPITANIA equals capitania.ID_CAPITANIA
                                 where logins.ID_CAPITANIA == m.login.ID_CAPITANIA
                                 select new CapitaniaSession()
                                 {
                                     Id = capitania.ID_CAPITANIA,
                                     Descripcion = capitania.DESCRIPCION,
                                     Sigla = capitania.SIGLA_CAPITANIA,
                                     Categoria = capitania.ID_CATEGORIA
                                 }).FirstOrDefault(),
                    EstadoId = m.login.ID_TIPO_ESTADO,
                    Roles = (from login_rol in _context.APLICACIONES_LOGIN_ROL
                             join roles in _context.APLICACIONES_ROLES on login_rol.ID_ROL equals roles.ID_ROL
                             into rol
                             from detalleRoles in rol.DefaultIfEmpty()
                             where login_rol.ID_LOGIN == user.ID_LOGIN && detalleRoles.ID_APLICACION == (int)TipoAplicacionEnum.GenteDeMar
                             && login_rol.ID_ESTADO == (byte)EstadoUsuarioLoginEnum.ACTIVO
                             select new RolSession { Id = detalleRoles.ID_ROL, NombreRol = detalleRoles.ROL }).ToList()
                }).FirstOrDefaultAsync();

            }
            return userSesion;
        }

        public async Task<IEnumerable<InfoUsuarioDTO>> GetUsuariosConRoles()
        {
            var query = (from loginRol in _context.APLICACIONES_LOGIN_ROL
                         join login in _context.APLICACIONES_LOGINS on loginRol.ID_LOGIN equals login.ID_LOGIN
                         join roles in _context.APLICACIONES_ROLES on loginRol.ID_ROL equals roles.ID_ROL
                         where roles.ID_APLICACION == (int)TipoAplicacionEnum.GenteDeMar && roles.ID_ESTADO == (byte)EstadoUsuarioLoginEnum.ACTIVO
                         select new
                         {
                             login,
                             roles,
                             loginRol
                         });
            return await query.GroupBy(m => m.login.LOGIN_NAME)
                .Select(grupo => new InfoUsuarioDTO
                {
                    LoginId = grupo.FirstOrDefault().login.ID_LOGIN,
                    Nombres = grupo.FirstOrDefault().login.NOMBRES,
                    Apellidos = grupo.FirstOrDefault().login.APELLIDOS,
                    LoginName = grupo.Key,
                    FechaCreacion = grupo.FirstOrDefault().login.FECHA_CREACION,
                    IsActive = grupo.FirstOrDefault().login.ID_TIPO_ESTADO == (int)EstadoUsuarioLoginEnum.ACTIVO
                    || grupo.FirstOrDefault().login.ID_TIPO_ESTADO == (int)EstadoUsuarioLoginEnum.USUARIONUEVO,
                    Correo = grupo.FirstOrDefault().login.CORREO,
                    Capitania = (from logins in _context.APLICACIONES_LOGINS
                                 join capitania in _context.APLICACIONES_CAPITANIAS on logins.ID_CAPITANIA equals capitania.ID_CAPITANIA
                                 where logins.ID_CAPITANIA == grupo.FirstOrDefault().login.ID_CAPITANIA
                                 select new CapitaniaSession()
                                 {
                                     Id = capitania.ID_CAPITANIA,
                                     Descripcion = capitania.DESCRIPCION,
                                     Sigla = capitania.SIGLA_CAPITANIA,
                                     Categoria = capitania.ID_CATEGORIA
                                 }).FirstOrDefault(),
                    Roles = (from login_rol in _context.APLICACIONES_LOGIN_ROL
                             join roles in _context.APLICACIONES_ROLES on login_rol.ID_ROL equals roles.ID_ROL
                             where login_rol.ID_LOGIN == grupo.FirstOrDefault().login.ID_LOGIN
                             && roles.ID_APLICACION == (int)TipoAplicacionEnum.GenteDeMar
                             && login_rol.ID_ESTADO == (byte)EstadoUsuarioLoginEnum.ACTIVO
                             group roles by new { roles.ID_ROL, roles.ROL, roles.DESCRIPCION } into grupoRol
                             select new RolSession { Id = grupoRol.Key.ID_ROL, NombreRol = grupoRol.Key.DESCRIPCION }).ToList()
                }).ToListAsync();
        }

        public async Task<string[]> GetEmailsAdministradores()
        {
            var data = from loginRol in _context.APLICACIONES_LOGIN_ROL
                       join login in _context.APLICACIONES_LOGINS on loginRol.ID_LOGIN equals login.ID_LOGIN
                       join roles in _context.APLICACIONES_ROLES on loginRol.ID_ROL equals roles.ID_ROL
                       where roles.ID_APLICACION == (int)TipoAplicacionEnum.GenteDeMar && roles.ID_ESTADO == (int)EstadoUsuarioLoginEnum.ACTIVO
                       && roles.ID_ROL == (int)RolesEnum.AdministradorGDM && loginRol.ID_ESTADO == (int)EstadoUsuarioLoginEnum.ACTIVO
                       && (login.ID_TIPO_ESTADO == (int)EstadoUsuarioLoginEnum.ACTIVO || login.ID_TIPO_ESTADO == (int)EstadoUsuarioLoginEnum.USUARIONUEVO)
                       select new
                       {
                           Email = login.CORREO,
                           RoleCount = _context.APLICACIONES_LOGIN_ROL.Count(lr => lr.ID_LOGIN == login.ID_LOGIN) // Count of roles for each login
                       };

            var emailsWithMultipleRoles = await data
                .GroupBy(item => item.Email)
                .Where(group => group.Sum(item => item.RoleCount) >= 1) // Filter groups by sum of role counts
                .Select(group => group.Key)
                .ToListAsync();


            return emailsWithMultipleRoles.ToArray();
        }

        public async Task<bool> InactivarOActivarUsuario(int loginId)
        {
            bool activar = true;
            var dataRoles = await _context.APLICACIONES_LOGIN_ROL.Where(x => x.ID_LOGIN == loginId).ToListAsync();
            if (dataRoles.Any())
            {
                activar = dataRoles.FirstOrDefault().ID_ESTADO == (byte)EstadoUsuarioLoginEnum.ACTIVO;
                foreach (var item in dataRoles)
                {
                    if (item.ID_ESTADO.HasValue)
                    {
                        item.ID_ESTADO = item.ID_ESTADO == (byte)EstadoUsuarioLoginEnum.ACTIVO
                            ? (byte)EstadoUsuarioLoginEnum.INACTIVO : (byte)EstadoUsuarioLoginEnum.ACTIVO;
                    }
                    _context.Entry(item).State = EntityState.Modified;
                }
                await SaveAllAsync();
            }
            return activar;
        }
    }
}