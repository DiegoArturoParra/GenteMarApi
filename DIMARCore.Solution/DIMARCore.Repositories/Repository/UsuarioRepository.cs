using DIMARCore.UIEntities.Models;
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


        public async Task<List<int>> GetRoles(string Loginname)
        {
            return await (from login in _context.APLICACIONES_LOGINS
                          join login_rol in _context.APLICACIONES_LOGIN_ROL on login.ID_LOGIN equals login_rol.ID_LOGIN
                          join roles in _context.APLICACIONES_ROLES on login_rol.ID_ROL equals roles.ID_ROL
                          where login.LOGIN_NAME.Equals(Loginname) && roles.ID_APLICACION == (int)TipoAplicacionEnum.GenteDeMar && login_rol.ID_ESTADO == 1
                          select roles.ID_ROL
                               ).ToListAsync();
        }

        public async Task<UserSesion> GetUsuarioByLoginName(string loginName)
        {
            UserSesion userSesion = null;
            var user = await GetWithCondition(x => x.LOGIN_NAME.ToUpper().Equals(loginName.ToUpper()));
            if (user != null)
            {
                var query = (from login in _context.APLICACIONES_LOGINS
                             where login.ID_LOGIN == user.ID_LOGIN
                             //join
                             select new
                             {
                                 login,
                             });
                userSesion = await query.Select(m => new UserSesion
                {

                    NombresUsuario = m.login.NOMBRES,
                    ApellidosUsuario = m.login.APELLIDOS,
                    LoginName = m.login.LOGIN_NAME,
                    Aplicacion = (from aplicaciones in _context.APLICACIONES
                                  where aplicaciones.ID_APLICACION == (int)TipoAplicacionEnum.GenteDeMar
                                  select new AplicacionSession() { Id = aplicaciones.ID_APLICACION, Nombre = aplicaciones.NOMBRE }).FirstOrDefault(),

                    Capitania = (from logins in _context.APLICACIONES_LOGINS
                                 join capitania in _context.APLICACIONES_CAPITANIAS on logins.ID_CAPITANIA equals capitania.ID_CAPITANIA
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
                             where login_rol.ID_LOGIN == user.ID_LOGIN && detalleRoles.ID_APLICACION == (int)TipoAplicacionEnum.GenteDeMar && login_rol.ID_ESTADO == 1
                             select new Rol { Id = detalleRoles.ID_ROL, NombreRol = detalleRoles.ROL }).ToList()
                }).FirstOrDefaultAsync();

            }
            return userSesion;
        }

        public async Task<UserSesion> GetUsuarioByLoginNameTesting(string username, string password)
        {
            UserSesion userSesion = null;
            var user = await GetWithCondition(x => x.LOGIN_NAME.ToUpper().Equals(username.ToUpper()) && x.PASSWORD.Equals(password));
            if (user != null)
            {
                var query = (from login in _context.APLICACIONES_LOGINS
                             where login.ID_LOGIN == user.ID_LOGIN
                             //join
                             select new
                             {
                                 login,
                             });
                userSesion = await query.Select(m => new UserSesion
                {

                    NombresUsuario = m.login.NOMBRES,
                    ApellidosUsuario = m.login.APELLIDOS,
                    LoginName = m.login.LOGIN_NAME,
                    Email = m.login.CORREO,
                    Aplicacion = (from aplicaciones in _context.APLICACIONES
                                  where aplicaciones.ID_APLICACION == (int)TipoAplicacionEnum.GenteDeMar
                                  select new AplicacionSession() { Id = aplicaciones.ID_APLICACION, Nombre = aplicaciones.NOMBRE }).FirstOrDefault(),

                    Capitania = (from sucursal in _context.APLICACIONES_LOGIN_SUCURSAL
                                 join capitania in _context.APLICACIONES_CAPITANIAS on sucursal.ID_SUCURSAL equals capitania.ID_CAPITANIA into dcap
                                 from detalleCapitania in dcap.DefaultIfEmpty()
                                 join categoria in _context.APLICACIONES_CATEGORIA on detalleCapitania.ID_CATEGORIA equals categoria.ID_CATEGORIA into dcat
                                 from detalleCategoria in dcat.DefaultIfEmpty()
                                 where sucursal.ID_APLICACION == (int)TipoAplicacionEnum.GenteDeMar
                                 select new CapitaniaSession()
                                 {
                                     Id = detalleCapitania.ID_CAPITANIA,
                                     Descripcion = detalleCapitania.DESCRIPCION,
                                     Sigla = detalleCapitania.SIGLA_CAPITANIA,
                                     Categoria = detalleCategoria.ID_CATEGORIA
                                 }).FirstOrDefault(),
                    EstadoId = m.login.ID_TIPO_ESTADO,
                    Roles = (from login_rol in _context.APLICACIONES_LOGIN_ROL
                             join roles in _context.APLICACIONES_ROLES on login_rol.ID_ROL equals roles.ID_ROL
                             into rol
                             from detalleRoles in rol.DefaultIfEmpty()
                             where login_rol.ID_LOGIN == user.ID_LOGIN && detalleRoles.ID_APLICACION == (int)TipoAplicacionEnum.GenteDeMar && login_rol.ID_ESTADO == 1
                             select new Rol { Id = detalleRoles.ID_ROL, NombreRol = detalleRoles.ROL }).ToList()
                }).FirstOrDefaultAsync();

            }
            return userSesion;
        }
    }
}