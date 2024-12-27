using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using DIMARCore.Utilities.Middleware;
namespace DIMARCore.Business.Logica
{
    public class AplicacionRolesBO
    {
        /// <summary>
        /// Obtiene los roles que tiene cada usuario del aplicativo de gente de mar
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpStatusCodeException"></exception>
        public async Task<Respuesta> ExisteRolesByAplicacion()
        {
            var hayRolesEnAplicacion = await new AplicacionRolesRepository().AnyWithConditionAsync(x => x.ID_APLICACION == (int)TipoAplicacionEnum.GenteDeMar);
            if (!hayRolesEnAplicacion)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No hay roles asignados a la aplicación de gente de mar."));
            return Responses.SetOkResponse();
        }
        public async Task<IEnumerable<RolSessionDTO>> GetRoles()
        {
            using (var repositorio = new AplicacionRolesRepository())
            {
                return await repositorio.GetRoles();
            }
        }

    }
}
