using DIMARCore.Repositories.Repository;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DIMARCore.Utilities.Middleware;
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
        public async Task<IList<GENTEMAR_ESTADO>> GetEstadosAsync()
        {
            // Obtiene la lista
            return await new GenteDeMarEstadoRepository().GetEstadosAsync();
        }

        /// <summary>
        /// Lista estados de gente de mar 
        /// </summary>
        /// <returns>Lista de los estados </returns>
        /// <entidad>GENTEMAR_ESTADO</entidad>
        /// <tabla>GENTEMAR_ESTADO</tabla>
        public async Task<IList<GENTEMAR_ESTADO>> GetEstadosActivoAsync()
        {
            // Obtiene la lista
            var data = await new GenteDeMarEstadoRepository().GetAllWithConditionAsync(x => x.activo == Constantes.ACTIVO);
            return data.ToList();
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
                data.descripcion = data.descripcion.Trim();
                data.sigla = data.sigla.Trim();
                var validate = await repo.AnyWithConditionAsync(x => x.descripcion.Equals(data.descripcion.Trim()));
                if (validate)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"El estado {data.descripcion} ya se encuentra registrado."));

                data.activo = Constantes.ACTIVO;
                await repo.Create(data);
                return Responses.SetCreatedResponse(data);
            }

        }
        /// <summary>
        ///  actualiza un estado existente 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusCodeException"></exception>
        public async Task<Respuesta> ActualizarEstado(GENTEMAR_ESTADO data)
        {
            using (var repo = new GenteDeMarEstadoRepository())
            {
                data.descripcion = data.descripcion.Trim();
                data.sigla = data.sigla.Trim();
                var entidad = await repo.GetWithConditionAsync(x => x.id_estado == data.id_estado);
                if (entidad == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El estado no existe."));

                var validate = await repo.AnyWithConditionAsync(x => x.descripcion.Equals(data.descripcion.Trim()) && x.id_estado != data.id_estado);
                if (validate)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"El estado {data.descripcion} ya se encuentra registrado."));
                entidad.descripcion = data.descripcion;
                entidad.sigla = data.sigla;
                await repo.Update(entidad);
                return Responses.SetUpdatedResponse(entidad);
            }
        }
        /// <summary>
        /// cambia el estado activo/inactivo de un estado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusCodeException"></exception>
        public async Task<Respuesta> CambiarEstado(int id)
        {
            using (var repo = new GenteDeMarEstadoRepository())
            {
                var validate = await repo.GetWithConditionAsync(x => x.id_estado == id);
                if (validate == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El estado no existe."));
                validate.activo = !validate.activo;
                await repo.Update(validate);
                return Responses.SetUpdatedResponse(validate);
            }
        }
    }
}
