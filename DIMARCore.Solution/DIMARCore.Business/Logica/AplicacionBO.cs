using DIMARCore.Repositories.Repository;
using DIMARCore.Utilities.Helpers;
using System.Threading.Tasks;

namespace DIMARCore.Business
{
    public class AplicacionBO
    {
        /// <summary>
        /// metodo para obtener la aplicacion de gente de mar
        /// </summary>
        /// <param name="idAplicacion"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusCodeException"></exception>
        public async Task<Respuesta> GetAplicacion(int idAplicacion)
        {
            var existe = await new AplicacionRepository().AnyWithCondition(x => x.ID_APLICACION == idAplicacion);
            if (!existe)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encontro la aplicación de gente de mar."));
            return Responses.SetOkResponse();
        }

    }
}
