using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DIMARCore.Utilities.Middleware;
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
        public IList<FormacionDTO> GetFormacion(bool estado)
        {
            // Obtiene la lista
            return new FormacionRepository().GetFormacion(estado);
        }

        /// <summary>
        /// Lista de Formaciones
        /// </summary>
        /// <returns>Lista de Formacion</returns>
        /// <entidad>GENTEMAR_FORMACION</entidad>
        /// <tabla>GENTEMAR_FORMACION</tabla>
        public IList<FormacionDTO> GetTableFormacion()
        {
            // Obtiene la lista
            return new FormacionRepository().GetTableFormacion();
        }

        /// <summary>
        /// Lista de Formaciones activo
        /// </summary>
        /// <returns>Lista de Formacion</returns>
        /// <entidad>GENTEMAR_FORMACION</entidad>
        /// <tabla>GENTEMAR_FORMACION</tabla>
        public IList<FormacionDTO> GetTableFormacionActivo()
        {
            // Obtiene la lista
            return new FormacionRepository().GetTableFormacion().Where(x => x.activo == true).ToList();
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
                var validate = await repo.AnyWithCondition(x => x.formacion.ToUpper().Equals(data.formacion.ToUpper()));
                if (validate)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"La formación {data.formacion} ya esta registrada."));
                await repo.Create(data);
                return Responses.SetCreatedResponse(data);
            }
        }

        /// <summary>
        /// actualiza la informacion de una formacion 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Respuesta> actualizarFormacion(GENTEMAR_FORMACION data)
        {
            Respuesta respuesta = new Respuesta();
            using (var repo = new FormacionRepository())
            {
                var validate = await repo.GetWithCondition(x => x.id_formacion == data.id_formacion);
                if (validate == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("La formación no existe."));

                data.id_formacion = validate.id_formacion;
                data.activo = validate.activo;
                await new FormacionRepository().Update(data);
                return Responses.SetUpdatedResponse(validate);
            }
        }
        /// <summary>
        /// cambia el estado de la formación 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Respuesta> cambiarFormacion(int id)
        {
            using (var repo = new FormacionRepository())
            {
                var validate = await repo.GetWithCondition(x => x.id_formacion == id);
                if (validate == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("La formación no existe."));
                validate.activo = !validate.activo;
                await repo.Update(validate);
                return Responses.SetUpdatedResponse(validate);
            }
        }
    }
}
