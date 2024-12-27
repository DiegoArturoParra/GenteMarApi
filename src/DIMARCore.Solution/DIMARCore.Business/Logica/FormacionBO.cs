using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace DIMARCore.Business.Logica
{
    public class FormacionBO
    {
        /// <summary>
        /// Lista de Formaciones
        /// </summary>
        /// <returns>Lista de Formacion</returns>
        /// <entidad>GENTEMAR_FORMACION</entidad>
        /// <tabla>GENTEMAR_FORMACION</tabla>
        public async Task<IList<FormacionDTO>> GetFormaciones(bool estado)
        {
            // Obtiene la lista
            return await new FormacionRepository().GetFormaciones(estado);
        }

        /// <summary>
        /// Lista de Formaciones
        /// </summary>
        /// <returns>Lista de Formacion</returns>
        /// <entidad>GENTEMAR_FORMACION</entidad>
        /// <tabla>GENTEMAR_FORMACION</tabla>
        public async Task<IList<FormacionDTO>> GetTableFormacion()
        {
            // Obtiene la lista
            return await new FormacionRepository().GetTableFormacion();
        }

        /// <summary>
        /// Lista de Formaciones activo
        /// </summary>
        /// <returns>Lista de Formacion</returns>
        /// <entidad>GENTEMAR_FORMACION</entidad>
        /// <tabla>GENTEMAR_FORMACION</tabla>
        public async Task<IList<FormacionDTO>> GetTableFormacionActivo()
        {
            // Obtiene la lista
            return await new FormacionRepository().GetTableFormacion(true);
        }
        /// <summary>
        /// crea una nueva formacion 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Respuesta> CrearFormacion(GENTEMAR_FORMACION data)
        {
            using (var repo = new FormacionRepository())
            {
                var validate = await repo.AnyWithConditionAsync(x => x.formacion.Equals(data.formacion));
                if (validate)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"La formación {data.formacion} ya se encuentra registrada."));
                await repo.Create(data);
                return Responses.SetCreatedResponse(data);
            }
        }

        /// <summary>
        /// actualiza la informacion de una formacion 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Respuesta> ActualizarFormacion(GENTEMAR_FORMACION data)
        {
            Respuesta respuesta = new Respuesta();
            using (var repo = new FormacionRepository())
            {
                var entidad = await repo.GetWithConditionAsync(x => x.id_formacion == data.id_formacion);
                if (entidad == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("La formación no existe."));

                var validate = await repo.AnyWithConditionAsync(x => x.formacion.Equals(data.formacion) && x.id_formacion != data.id_formacion);
                if (validate)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"La formación {data.formacion} ya se encuentra registrada."));
                entidad.formacion = data.formacion;
                await new FormacionRepository().Update(entidad);
                return Responses.SetUpdatedResponse(entidad);
            }
        }
        /// <summary>
        /// cambia el estado de la formación 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Respuesta> CambiarFormacion(int id)
        {
            using (var repo = new FormacionRepository())
            {
                var validate = await repo.GetWithConditionAsync(x => x.id_formacion == id);
                if (validate == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("La formación no existe."));
                validate.activo = !validate.activo;
                await repo.Update(validate);
                return Responses.SetUpdatedResponse(validate);
            }
        }
    }
}
