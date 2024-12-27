using DIMARCore.Repositories.Repository;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using DIMARCore.Utilities.Middleware;
namespace DIMARCore.Business.Logica
{
    public class RangoBO
    {
        /// <summary>
        /// Lista de rangos
        /// </summary>
        /// <returns>Lista de rangos</returns>
        /// <entidad>APLICACIONES_RANGO</entidad>
        /// <tabla>APLICACIONES_RANGO</tabla>
        public async Task<IList<APLICACIONES_RANGO>> GetRangosAsync(bool estado)
        {
            // Obtiene la lista
            return await new RangoRepository().GetRangosAsync(estado);
        }
        /// <summary>
        /// crea un nuevo rango  
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Respuesta> CrearRango(APLICACIONES_RANGO data)
        {
            var validate = await new RangoRepository().AnyWithConditionAsync(x => x.rango.Equals(data.rango));
            if (validate)
                throw new HttpStatusCodeException(Responses.SetConflictResponse($"El Rango {data.rango} ya se encuentra registrado."));
            await new RangoRepository().Create(data);
            return Responses.SetCreatedResponse(data);
        }

        /// <summary>
        /// actualiza la informacion de un rango 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Respuesta> ActualizarRango(APLICACIONES_RANGO data)
        {

            using (var repo = new RangoRepository())
            {
                var entidad = await repo.GetWithConditionAsync(x => x.id_rango == data.id_rango);
                if (entidad == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El Rango no existe."));

                var validate = await repo.AnyWithConditionAsync(x => x.rango.Equals(data.rango) && x.id_rango != data.id_rango);
                if (validate)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"El Rango {data.rango} ya se encuentra registrado."));
                entidad.rango = data.rango;
                await new RangoRepository().Update(entidad);
                return Responses.SetUpdatedResponse(entidad);
            }
        }
        /// <summary>
        /// cambia el estado del rango 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Respuesta> CambiarRango(int id)
        {
            using (var repo = new RangoRepository())
            {
                var validate = await repo.GetWithConditionAsync(x => x.id_rango == id);
                if (validate == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El Rango no existe."));

                validate.activo = !validate.activo;
                await repo.Update(validate);
                return Responses.SetUpdatedResponse(validate);
            }
        }
    }
}
