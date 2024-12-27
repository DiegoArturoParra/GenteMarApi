using DIMARCore.Repositories.Repository;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DIMARCore.Utilities.Middleware;
namespace DIMARCore.Business.Logica
{
    public class LimitanteBO
    {
        /// <summary>
        /// Lista de Limitantes
        /// </summary>
        /// <returns>Lista de Limitantes</returns>
        /// <entidad>GENTEMAR_LIMITANTE</entidad>
        /// <tabla>GENTEMAR_LIMITANTE</tabla>
        public async Task<IList<GENTEMAR_LIMITANTE>> GetLimitantesAsync()
        {
            // Obtiene la lista
            return await new LimitanteRepository().GetLimitantesAsync();
        }

        /// <summary>
        /// Lista de Limitantes
        /// </summary>
        /// <returns>Lista de Limitantes</returns>
        /// <entidad>GENTEMAR_LIMITANTE</entidad>
        /// <tabla>GENTEMAR_LIMITANTE</tabla>
        public async Task<IList<GENTEMAR_LIMITANTE>> GetLimitantesActivoAsync()
        {
            // Obtiene la lista
            var data = await new LimitanteRepository().GetAllWithConditionAsync(x => x.activo == Constantes.ACTIVO);
            return data.ToList();
        }

        /// <summary>
        /// Obtener Limitante dado su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objeto Limitante dado su id</returns>
        /// <entidad>GENTEMAR_LIMITANTE</entidad>
        /// <tabla>GENTEMAR_LIMITANTE</tabla>
        public async Task<GENTEMAR_LIMITANTE> GetLimitanteAsync(int id)
        {
            return await new LimitanteRepository().GetByIdAsync(id);
        }


        /// <summary>
        /// crear limitante
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusCodeException"></exception>
        public async Task<Respuesta> CrearLimitante(GENTEMAR_LIMITANTE datos)
        {
            using (var repo = new LimitanteRepository())
            {
                datos.descripcion = datos.descripcion.Trim();
                var validate = await repo.AnyWithConditionAsync(x => x.descripcion.ToUpper().Equals(datos.descripcion.ToUpper()));
                if (validate)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"Ya se encuentra registrada la limitante {datos.descripcion}"));
                datos.activo = Constantes.ACTIVO;
                await repo.Create(datos);
                return Responses.SetCreatedResponse(datos);
            }
        }


        /// <summary>
        /// edita la limitante
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusCodeException"></exception>
        public async Task<Respuesta> EditarLimitanteAsync(GENTEMAR_LIMITANTE datos)
        {
            using (var repo = new LimitanteRepository())
            {
                datos.descripcion = datos.descripcion.Trim();
                var entidad = await repo.GetWithConditionAsync(x => x.id_limitante == datos.id_limitante);
                if (entidad == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encuentra registrada la limitante."));

                var validate = await repo.AnyWithConditionAsync(x => x.descripcion.ToUpper().Equals(datos.descripcion.ToUpper()) && x.id_limitante != datos.id_limitante);
                if (validate)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"Ya se encuentra registrada la limitante {datos.descripcion}"));

                entidad.descripcion = datos.descripcion;
                await new LimitanteRepository().Update(entidad);
                return Responses.SetUpdatedResponse(entidad);
            }
        }

        /// <summary>
        /// cambia estado de la limitante
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusCodeException"></exception>
        public async Task<Respuesta> CambiarLimitante(int id)
        {
            using (var repo = new LimitanteRepository())
            {
                var validate = await repo.GetWithConditionAsync(x => x.id_limitante == id);
                if (validate == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encuentra registrada la limitante."));
                validate.activo = !validate.activo;
                await repo.Update(validate);
                return Responses.SetUpdatedResponse(validate);
            }
        }
    }
}
