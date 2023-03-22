using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq;
namespace DIMARCore.Repositories.Repository
{

    public class MenuRepository : GenericRepository<APLICACIONES_ROL_MENU>
    {
        public async Task<IEnumerable<MenuDTO>> GetUsuarioWithMenu(int aplicacionId, string loginName)
        {
            var query = (from Menu in _context.APLICACIONES_MENU
                         join MenuRol in _context.APLICACIONES_ROL_MENU on Menu.ID_MENU equals MenuRol.ID_MENU
                         join rol in _context.APLICACIONES_ROLES on MenuRol.ID_ROL equals rol.ID_ROL
                         join loginRol in _context.APLICACIONES_LOGIN_ROL on rol.ID_ROL equals loginRol.ID_ROL
                         join login in _context.APLICACIONES_LOGINS on loginRol.ID_LOGIN equals login.ID_LOGIN
                         where Menu.ID_APLICACION == aplicacionId && login.LOGIN_NAME == loginName
                         select new
                         {
                             Menu,
                             MenuRol,
                             rol,
                             loginRol,
                             login
                         });
            return await query.Select(m => new MenuDTO
            {
                Controlador = m.Menu.CONTROLADOR,
                AplicacionId = m.Menu.ID_APLICACION,
                MenuId = m.Menu.ID_MENU,
                Nombre = m.Menu.NOMBRE,
                PadreId = m.Menu.ID_PADRE,
                Vista = m.Menu.VISTA
            }).ToListAsync();
        }
    }
}
