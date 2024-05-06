using DIMARCore.Repositories.Repository;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DIMARCore.Utilities.Middleware;

namespace DIMARCore.Business.Logica
{
    public class EstadoLicenciaBO
    {
        /// <summary>
        /// Lista estados de licencias
        /// </summary>
        /// <returns>Lista de los estados </returns>
        /// <entidad>GENTEMAR_ESTADO_LICENCIAS</entidad>
        /// <tabla>GENTEMAR_ESTADO_LICENCIAS</tabla>
        public async Task<IEnumerable<GENTEMAR_ESTADO_LICENCIA>> GetEstadosAsync()
        {
            // Obtiene la lista
            return await new EstadoLicenciaRepository().GetEstados();
        }

        /// <summary>
        /// Lista estados de gente de mar 
        /// </summary>
        /// <returns>Lista de los estados </returns>
        /// <entidad>GENTEMAR_ESTADO_LICENCIAS</entidad>
        /// <tabla>GENTEMAR_ESTADO_LICENCIAS</tabla>
        public async Task<IEnumerable<GENTEMAR_ESTADO_LICENCIA>> GetEstadosActivoAsync()
        {
            // Obtiene la lista
            return await new EstadoLicenciaRepository().GetAllWithConditionAsync(x => x.activo == Constantes.ACTIVO);
        }
        /// <summary>
        /// crea un nuevo estado 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Respuesta> CrearEstado(GENTEMAR_ESTADO_LICENCIA data)
        {
            using (var repo = new EstadoLicenciaRepository())
            {
                data.descripcion_estado = data.descripcion_estado.Trim().ToUpper();
                data.activo = Constantes.ACTIVO;
                var validate = await repo.AnyWithConditionAsync(x => x.descripcion_estado == data.descripcion_estado);
                if (validate)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"El estado {data.descripcion_estado} ya se encuentra creado."));
                await repo.Create(data);
                return Responses.SetCreatedResponse(data);
            }

        }
        /// <summary>
        /// actualiza un estado existente 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Respuesta> ActualizarEstado(GENTEMAR_ESTADO_LICENCIA data)
        {
            using (var repo = new EstadoLicenciaRepository())
            {
                data.descripcion_estado = data.descripcion_estado.Trim().ToUpper();
                var entidad = await repo.GetWithConditionAsync(x => x.id_estado_licencias == data.id_estado_licencias);
                if (entidad == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El estado no existe."));
                var validate = await repo.AnyWithConditionAsync(x => x.descripcion_estado == data.descripcion_estado && x.id_estado_licencias != entidad.id_estado_licencias);
                if (validate)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"El estado {data.descripcion_estado} ya se encuentra creado."));
                entidad.descripcion_estado = data.descripcion_estado;
                await new EstadoLicenciaRepository().Update(entidad);
                return Responses.SetUpdatedResponse(entidad);
            }
        }
        /// <summary>
        /// cambia el estado de un estado registrado 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Respuesta> CambiarEstado(int id)
        {
            using (var repo = new EstadoLicenciaRepository())
            {
                var validate = await repo.GetWithConditionAsync(x => x.id_estado_licencias == id);
                if (validate == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El estado no existe."));
                validate.activo = !validate.activo;
                await new EstadoLicenciaRepository().Update(validate);
                return Responses.SetUpdatedResponse(validate);
            }
        }
    }
}
