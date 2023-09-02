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
        public IList<GENTEMAR_LIMITANTE> GetLimitantes()
        {
            // Obtiene la lista
            return new LimitanteRepository().GetLimitantes();
        }

        /// <summary>
        /// Lista de Limitantes
        /// </summary>
        /// <returns>Lista de Limitantes</returns>
        /// <entidad>GENTEMAR_LIMITANTE</entidad>
        /// <tabla>GENTEMAR_LIMITANTE</tabla>
        public IList<GENTEMAR_LIMITANTE> GetLimitantesActivo()
        {
            // Obtiene la lista
            return new LimitanteRepository().GetAllWithCondition(x => x.activo == true).ToList();
        }



        /// <summary>
        /// Obtener Limitante dado su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objeto Limitante dado su id</returns>
        /// <entidad>GENTEMAR_LIMITANTE</entidad>
        /// <tabla>GENTEMAR_LIMITANTE</tabla>
        public GENTEMAR_LIMITANTE GetLimitante(int id)
        {
            return new LimitanteRepository().GetLimitante(id);
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
                var validate = await repo.AnyWithCondition(x => x.descripcion.Equals(datos.descripcion));
                if (validate)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"Ya se encuentra registrada la limitante {datos.descripcion}"));
                datos.activo = true;
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
                var validate = await repo.GetWithCondition(x => x.id_limitante == datos.id_limitante);
                if (validate == null)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse("No se encuentra registrada la limitante."));
                datos.id_limitante = validate.id_limitante;
                datos.activo = validate.activo;
                await new LimitanteRepository().Update(datos);
                return Responses.SetUpdatedResponse(datos);
            }
        }

        /// <summary>
        /// cambia estado de la limitante
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusCodeException"></exception>
        public async Task<Respuesta> cambiarLimitante(int id)
        {
            using (var repo = new LimitanteRepository())
            {
                var validate = await repo.GetWithCondition(x => x.id_limitante == id);
                if (validate == null)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse("No se encuentra registrada la limitante."));
                validate.activo = !validate.activo;
                await repo.Update(validate);
                return Responses.SetUpdatedResponse(validate);
            }
        }
    }
}
