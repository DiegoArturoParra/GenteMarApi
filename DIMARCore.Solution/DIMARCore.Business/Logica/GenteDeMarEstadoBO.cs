using DIMARCore.Repositories.Repository;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class GenteDeMarEstadoBO
    {
        /// <summary>
        /// Lista estados de gente de mar 
        /// </summary>
        /// <returns>Lista de los estados </returns>
        /// <entidad>GENTEMAR_ESTADO</entidad>
        /// <tabla>GENTEMAR_ESTADO</tabla>
        public IList<GENTEMAR_ESTADO> GetEstado()
        {
            // Obtiene la lista
            return new GenteDeMarEstadoRepository().GetEstado();
        }

        /// <summary>
        /// Lista estados de gente de mar 
        /// </summary>
        /// <returns>Lista de los estados </returns>
        /// <entidad>GENTEMAR_ESTADO</entidad>
        /// <tabla>GENTEMAR_ESTADO</tabla>
        public IList<GENTEMAR_ESTADO> GetEstadoActivo()
        {
            // Obtiene la lista
            return new GenteDeMarEstadoRepository().GetAllAsQueryable().Where(x => x.activo == true).ToList();
        }
        /// <summary>
        /// crea un nuevo estado 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Respuesta> CrearEstado(GENTEMAR_ESTADO data)
        {
            using (var repo = new GenteDeMarEstadoRepository())
            {
                var validate = await repo.AnyWithCondition(x => x.sigla.Equals(data.sigla) || x.descripcion.Equals(data.descripcion));
                if (validate)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"El estado {data.descripcion} ya esta registrado."));
                await repo.Create(data);
                return Responses.SetCreatedResponse(data);
            }

        }
        /// <summary>
        /// actualiza un estado existente 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Respuesta> actualizarEstado(GENTEMAR_ESTADO data)
        {
            using (var repo = new GenteDeMarEstadoRepository())
            {
                var validate = await repo.GetWithCondition(x => x.id_estado == data.id_estado);
                if (validate == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El estado no existe."));
                validate.descripcion = data.descripcion;
                validate.sigla = data.sigla;
                await repo.Update(validate);
                return Responses.SetUpdatedResponse(validate);
            }
        }
        /// <summary>
        /// cambia el estado de un estado registrado 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Respuesta> cambiarEstado(int id)
        {
            using (var repo = new GenteDeMarEstadoRepository())
            {
                var validate = await repo.GetWithCondition(x => x.id_estado == id);
                if (validate == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El estado no existe."));
                validate.activo = !validate.activo;
                await repo.Update(validate);
                return Responses.SetUpdatedResponse(validate);
            }
        }
    }
}
