using DIMARCore.Repositories.Repository;
using DIMARCore.Utilities.Helpers;
using System.Threading.Tasks;
using DIMARCore.Utilities.Middleware;
namespace DIMARCore.Business
{
    public class AplicacionBO
    {
        /// <summary>
        /// metodo para obtener la aplicacion de gente de mar
        /// </summary>
        /// <param name="nombreAplicacion"></param>
        /// <returns></returns>
        public async Task<Respuesta> GetAplicacion(string nombreAplicacion)
        {
            var existe = await new AplicacionRepository().AnyWithConditionAsync(x => x.NOMBRE.Equals(nombreAplicacion));
            if (!existe)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encontro la aplicación de gente de mar."));
            return Responses.SetOkResponse();
        }

    }
}
