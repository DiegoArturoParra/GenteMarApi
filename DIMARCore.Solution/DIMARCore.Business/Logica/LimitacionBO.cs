using DIMARCore.Repositories.Repository;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public IList<GENTEMAR_LIMITACION> GetLimitaciones()
        {
            // Obtiene la lista
            return new LimitacionRepository().GetLimitaciones();
        }

        /// <summary>
        /// Lista de Limitaciones
        /// </summary>
        /// <returns>Lista de Limitaciones</returns>
        /// <entidad>GENTEMAR_LIMITACION</entidad>
        /// <tabla>GENTEMAR_LIMITACION</tabla>
        public IList<GENTEMAR_LIMITACION> GetLimitacionesActivo()
        {
            // Obtiene la lista
            return new LimitacionRepository().GetAllWithCondition(x => x.activo == true).ToList();
        }



        /// <summary>
        /// Obtener Limitacion dado su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objeto Limitacion dado su id</returns>
        /// <entidad>GENTEMAR_LIMITACION</entidad>
        /// <tabla>GENTEMAR_LIMITACION</tabla>
        public GENTEMAR_LIMITACION GetLimitacion(int id)
        {
            return new LimitacionRepository().GetLimitacion(id);
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
                var validate = await repo.AnyWithCondition(x => x.limitaciones.Equals(datos.limitaciones));
                if (validate)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"Ya se encuentra registrada la limitación {datos.limitaciones}"));
                datos.activo = true;
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
        public async Task<Respuesta> EditarLimitaciónAsync(GENTEMAR_LIMITACION datos)
        {
            using (var repo = new LimitacionRepository())
            {
                // busca la Limitación en el sistema

                var validate = await repo.GetWithCondition(x => x.id_limitacion == datos.id_limitacion);

                if (validate == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encuentra registrada la limitación."));
                datos.id_limitacion = validate.id_limitacion;
                datos.activo = validate.activo;
                await new LimitacionRepository().Update(datos);
                return Responses.SetUpdatedResponse(validate);
            }
        }

        /// <summary>
        /// cambia el estado del rango 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Respuesta> cambiarLimitacion(int id)
        {
            using (var repo = new LimitacionRepository())
            {
                var validate = await repo.GetWithCondition(x => x.id_limitacion == id);
                if (validate == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encuentra registrada la limitación."));
                validate.activo = !validate.activo;
                await repo.Update(validate);
                return Responses.SetUpdatedResponse(validate);
            }
        }
    }
}






