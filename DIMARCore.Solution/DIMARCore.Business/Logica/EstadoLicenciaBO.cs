using DIMARCore.Repositories.Repository;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public IList<GENTEMAR_ESTADO_LICENCIA> GetEstado()
        {
            // Obtiene la lista
            return new EstadoLicenciaRepository().GetEstado();
        }

        /// <summary>
        /// Lista estados de gente de mar 
        /// </summary>
        /// <returns>Lista de los estados </returns>
        /// <entidad>GENTEMAR_ESTADO_LICENCIAS</entidad>
        /// <tabla>GENTEMAR_ESTADO_LICENCIAS</tabla>
        public IList<GENTEMAR_ESTADO_LICENCIA> GetEstadoActivo()
        {
            // Obtiene la lista
            return new EstadoLicenciaRepository().GetEstado().Where(x => x.activo == true).ToList();
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
                var validate = await repo.AnyWithCondition(x => x.descripcion_estado == data.descripcion_estado);
                if (validate)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse("El estado ya se encuentra creado."));
                await repo.Create(data);
                return Responses.SetCreatedResponse(data);
            }

        }
        /// <summary>
        /// actualiza un estado existente 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Respuesta> actualizarEstado(GENTEMAR_ESTADO_LICENCIA data)
        {
            using (var repo = new EstadoLicenciaRepository())
            {
                var validate = await repo.GetWithCondition(x => x.id_estado_licencias == data.id_estado_licencias);
                if (validate == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El estado no existe."));
                data.id_estado_licencias = validate.id_estado_licencias;
                data.activo = validate.activo;
                await new EstadoLicenciaRepository().Update(data);
                return Responses.SetUpdatedResponse(data);
            }
        }
        /// <summary>
        /// cambia el estado de un estado registrado 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Respuesta> cambiarEstado(int id)
        {
            using (var repo = new EstadoLicenciaRepository())
            {
                var validate = await repo.GetWithCondition(x => x.id_estado_licencias == id);
                if (validate == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El estado no existe."));
                validate.activo = !validate.activo;
                await new EstadoLicenciaRepository().Update(validate);
                return Responses.SetUpdatedResponse(validate);
            }
        }
    }
}
