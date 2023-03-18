using DIMARCore.Repositories.Repository;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public IList<APLICACIONES_RANGO> GetFormacion(bool estado)
        {
            // Obtiene la lista
            return new RangoRepository().GetRango(estado);
        }
        /// <summary>
        /// crea un nuevo rango  
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Respuesta> CrearRango(APLICACIONES_RANGO data)
        {
            var validate = await new RangoRepository().AnyWithCondition(x => x.rango.Equals(data.rango));
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
        public async Task<Respuesta> actualizarRango(APLICACIONES_RANGO data)
        {

            using (var repo = new RangoRepository())
            {
                var validate = await repo.GetWithCondition(x => x.id_rango == data.id_rango);
                if (validate == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El Rango no existe."));

                data.id_rango = validate.id_rango;
                data.activo = validate.activo;
                await new RangoRepository().Update(data);
                return Responses.SetUpdatedResponse(data);
            }
        }
        /// <summary>
        /// cambia el estado del rango 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Respuesta> cambiarRango(int id)
        {
            using (var repo = new RangoRepository())
            {
                var validate = await repo.GetWithCondition(x => x.id_rango == id);
                if (validate == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El Rango no existe."));

                validate.activo = !validate.activo;
                await repo.Update(validate);
                return Responses.SetUpdatedResponse(validate);
            }
        }
    }
}
