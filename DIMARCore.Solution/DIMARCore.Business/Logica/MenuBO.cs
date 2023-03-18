using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class MenuBO
    {
        public Task<IEnumerable<MenuDTO>> GetUsuarioWithMenu(int AplicacionId, string LoginName)
        {
            return new MenuRepository().GetUsuarioWithMenu(AplicacionId, LoginName);
        }
    }
}
