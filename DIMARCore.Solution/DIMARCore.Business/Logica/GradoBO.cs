using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using System.Collections.Generic;
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
        public async Task<IList<GradoInfoDTO>> GetGradosPorFormacionId(int id, bool status)
        {
            var data = await new GradoRepository().GetGradosPorFormacionId(id, status);
            return data;
        }
        /// <summary>
        /// Lista de Grados con formación
        /// </summary>
        /// <returns>Lista de Grados con formación</returns>
        /// <entidad>APLICACIONES_GRADO</entidad>
        /// <tabla>APLICACIONES_GRADO</tabla>
        public async Task<IList<GradoInfoDTO>> GetGradosConFormacion()
        {
            // Obtiene la lista
            return await new GradoRepository().GetGradosConFormacion();
        }

        /// <summary>
        /// Lista de Grado
        /// </summary>
        /// <returns>Lista de Grado</returns>
        /// <entidad>APLICACIONES_GRADO</entidad>
        /// <tabla>APLICACIONES_GRADO</tabla>
        public async Task<IEnumerable<GradoDTO>> GetGradosActivos()
        {
            // Obtiene la lista
            return await new GradoRepository().GetGradosActivos();
        }

        /// <summary>
        /// >Lista de Grados activos con formación
        /// </summary>
        /// <returns>>Lista de Grados activos con formación</returns>
        /// <entidad>APLICACIONES_GRADO</entidad>
        /// <tabla>APLICACIONES_GRADO</tabla>
        public async Task<IList<GradoInfoDTO>> GetGradosActivosConFormacion()
        {
            // Obtiene la lista
            return await new GradoRepository().GetGradosConFormacion(true);
        }


        public async Task<Respuesta> CrearGrado(GradoInfoDTO data)
        {
            var validate = await new GradoRepository().AnyWithCondition(x => x.grado.Equals(data.grado) || x.sigla.Equals(data.sigla));
            if (validate)
                throw new HttpStatusCodeException(Responses.SetConflictResponse($"El grado {data.grado} ya está registrado."));
            await new GradoRepository().CrearGrados(data);
            return Responses.SetCreatedResponse(data);
        }


        public async Task<Respuesta> ActualizarGrado(GradoInfoDTO data)
        {
            using (var repo = new GradoRepository())
            {
                var validate = await repo.GetWithCondition(x => x.id_grado == data.id_grado);
                if (validate == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El grado no está registrado."));
                data.id_grado = validate.id_grado;
                data.activo = validate.activo;
                await new GradoRepository().ActualizarGrados(data);
                return Responses.SetUpdatedResponse(data);
            }

        }

        public async Task<Respuesta> CambiarGrado(int id)
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
