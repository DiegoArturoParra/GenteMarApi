using DIMARCore.Repositories.Repository;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using System.Threading.Tasks;

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
            var hayRolesEnAplicacion = await new AplicacionRolesRepository().
                AnyWithCondition(x => x.ID_APLICACION == (int)TipoAplicacionEnum.GenteDeMar);
            if (!hayRolesEnAplicacion)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No hay roles asignados a la aplicación de gente de mar."));
            return Responses.SetOkResponse();
        }
    }
}
