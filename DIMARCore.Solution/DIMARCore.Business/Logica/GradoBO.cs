using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class GradoBO
    {
        /// <summary>
        /// Lista de grados
        /// </summary>
        /// <returns>Lista de Frados</returns>
        /// <entidad>GradoDTO</entidad>
        /// <tabla>GENTEMAR_GRADO</tabla>
        public IList<GradoDTO> GetGradoIdGrado(int id, bool status)
        {
            var data = new GradoRepository().GetGradoIdGrado(id);
            // Obtiene la lista
            if (status)
            {
                data = data.Where(x => x.activo == status).ToList();
            }

            return data;
        }
        /// <summary>
        /// Lista de Grado
        /// </summary>
        /// <returns>Lista de Grado</returns>
        /// <entidad>APLICACIONES_GRADO</entidad>
        /// <tabla>APLICACIONES_GRADO</tabla>
        public IList<GradoDTO> GetGrado()
        {
            // Obtiene la lista
            return new GradoRepository().GetGrado();
        }

        /// <summary>
        /// Lista de Grado
        /// </summary>
        /// <returns>Lista de Grado</returns>
        /// <entidad>APLICACIONES_GRADO</entidad>
        /// <tabla>APLICACIONES_GRADO</tabla>
        public IList<GradoDTO> GetGradoActivo()
        {
            // Obtiene la lista
            return new GradoRepository().GetGrado().Where(x => x.activo == true).ToList();
        }


        public async Task<Respuesta> CrearGrado(GradoDTO data)
        {
            var validate = await new GradoRepository().AnyWithCondition(x => x.grado.Equals(data.grado) || x.sigla.Equals(data.sigla));
            if (validate)
                throw new HttpStatusCodeException(Responses.SetConflictResponse($"El grado {data.grado} ya está registrado."));
            await new GradoRepository().CrearGrados(data);
            return Responses.SetCreatedResponse(data);
        }


        public async Task<Respuesta> actualizarGrado(GradoDTO data)
        {
            using (var repo = new GradoRepository())
            {
                var validate = await repo.GetWithCondition(x => x.id_grado == data.id_grado);
                if (validate == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El grado no está registrado."));
                data.id_grado = validate.id_grado;
                data.activo = validate.activo;
                await new GradoRepository().actualizarGrados(data);
                return Responses.SetUpdatedResponse(data);
            }

        }

        public async Task<Respuesta> cambiarGrado(int id)
        {
            using (var repo = new GradoRepository())
            {
                var validate = await repo.GetWithCondition(x => x.id_grado == id);
                if (validate == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El grado no está registrado."));

                validate.activo = !validate.activo;
                await repo.Update(validate);
                return Responses.SetUpdatedResponse(validate);
            }
        }
    }


}
