using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class MenuBO
    {
        public async Task<IEnumerable<MenuDTO>> GetMenuPorUsuarioLoginName(int AplicacionId, string LoginName)
        {
            var menu = await new MenuRepository().GetMenuPorUsuarioLoginName(AplicacionId, LoginName);
            return menu;
        }
    }
}
