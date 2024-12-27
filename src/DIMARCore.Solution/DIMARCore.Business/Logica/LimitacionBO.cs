using DIMARCore.Repositories.Repository;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DIMARCore.Utilities.Middleware;
namespace DIMARCore.Business
{
    public class LimitacionBO
    {

        /// <summary>
        /// Lista de Limitaciones
        /// </summary>
        /// <returns>Lista de Limitaciones</returns>
        /// <entidad>GENTEMAR_LIMITACION</entidad>
        /// <tabla>GENTEMAR_LIMITACION</tabla>
        public async Task<IList<GENTEMAR_LIMITACION>> GetLimitacionesAsync()
        {
            // Obtiene la lista
            return await new LimitacionRepository().GetLimitaciones();
        }

        /// <summary>
        /// Lista de Limitaciones
        /// </summary>
        /// <returns>Lista de Limitaciones</returns>
        /// <entidad>GENTEMAR_LIMITACION</entidad>
        /// <tabla>GENTEMAR_LIMITACION</tabla>
        public async Task<IList<GENTEMAR_LIMITACION>> GetLimitacionesActivoAsync()
        {
            // Obtiene la lista
            var data = await new LimitacionRepository().GetAllWithConditionAsync(x => x.activo == Constantes.ACTIVO);
            return data.ToList();
        }



        /// <summary>
        /// Obtener Limitacion dado su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objeto Limitacion dado su id</returns>
        /// <entidad>GENTEMAR_LIMITACION</entidad>
        /// <tabla>GENTEMAR_LIMITACION</tabla>
        public async Task<GENTEMAR_LIMITACION> GetLimitacionAsync(int id)
        {
            return await new LimitacionRepository().GetByIdAsync(id);
        }


        /// <summary>
        /// Crea una Limitación
        /// </summary>
        /// <param name="datos">Información de la Limitación</param>
        /// <returns>Respuesta resultado</returns>
        public async Task<Respuesta> CrearLimitacion(GENTEMAR_LIMITACION datos)
        {
            using (var repo = new LimitacionRepository())
            {
                var validate = await repo.AnyWithConditionAsync(x => x.limitaciones.Equals(datos.limitaciones));
                if (validate)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"Ya se encuentra registrada la limitación {datos.limitaciones}"));
                datos.activo = Constantes.ACTIVO;
                await repo.Create(datos);
                return Responses.SetCreatedResponse(datos);
            }
        }


        /// <summary>
        /// Edita la información de una Limitación 
        /// </summary>
        /// <param name="datos">Datos de una Limitación</param>
        /// <param name="usuario">Usuario </param> //Todo: Se deja para implementar Autenticación
        /// <returns>Respuesta resultado</returns>
        public async Task<Respuesta> EditarLimitacionAsync(GENTEMAR_LIMITACION datos)
        {
            using (var repo = new LimitacionRepository())
            {
                // busca la Limitación en el sistema

                var entidad = await repo.GetWithConditionAsync(x => x.id_limitacion == datos.id_limitacion);

                if (entidad == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encuentra registrada la limitación."));

                var validate = await repo.AnyWithConditionAsync(x => x.limitaciones.Equals(datos.limitaciones));
                if (validate)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"Ya se encuentra registrada la limitación {datos.limitaciones}"));

                entidad.limitaciones = datos.limitaciones;
                await new LimitacionRepository().Update(entidad);
                return Responses.SetUpdatedResponse(entidad);
            }
        }

        /// <summary>
        /// cambia el estado del rango 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Respuesta> CambiarLimitacion(int id)
        {
            using (var repo = new LimitacionRepository())
            {
                var validate = await repo.GetWithConditionAsync(x => x.id_limitacion == id);
                if (validate == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encuentra registrada la limitación."));
                validate.activo = !validate.activo;
                await repo.Update(validate);
                return Responses.SetUpdatedResponse(validate);
            }
        }
    }
}






