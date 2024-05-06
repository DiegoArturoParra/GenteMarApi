using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class ClaseLicenciasBO
    {
        public async Task<Respuesta> GetByIdAsync(int Id)
        {
            var entidad = await new ClaseLicenciasRepository().GetByIdAsync(Id);
            return entidad == null
                ? throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"No se encuentra registrada la clase."))
                : Responses.SetOkResponse(entidad);
        }

        public async Task<Respuesta> AnulaOrActivaAsync(int id)
        {
            using (var repo = new ClaseLicenciasRepository())
            {
                var validate = await repo.GetWithConditionAsync(x => x.id_clase == id);
                if (validate == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encuentra registrada la clase."));
                validate.activo = !validate.activo;
                await repo.Update(validate);
                return Responses.SetUpdatedResponse(validate);
            }
        }

        public async Task<Respuesta> CrearAsync(GENTEMAR_CLASE_LICENCIAS entidad, IList<GENTEMAR_SECCION_LICENCIAS> secciones)
        {
            using (var repo = new ClaseLicenciasRepository())
            {
                entidad.descripcion_clase = entidad.descripcion_clase.ToUpper();
                var validate = await repo.AnyWithConditionAsync(x => x.descripcion_clase.Equals(entidad.descripcion_clase));
                if (validate)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"La clase {entidad.descripcion_clase} ya se encuentra registrada."));

                entidad.activo = Constantes.ACTIVO;
                await repo.CrearClaseSeccion(entidad, secciones);
                return Responses.SetCreatedResponse(entidad);
            }
        }
        public async Task<Respuesta> ActualizarAsync(GENTEMAR_CLASE_LICENCIAS datos, IList<GENTEMAR_SECCION_LICENCIAS> secciones)
        {
            Respuesta respuesta = new Respuesta();
            using (var repo = new ClaseLicenciasRepository())
            {
                // busca la Limitación en el sistema
                datos.descripcion_clase = datos.descripcion_clase.ToUpper();
                var entidad = await repo.GetWithConditionAsync(x => x.id_clase == datos.id_clase);

                if (entidad == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encuentra registrada la clase."));

                var validate = await repo.AnyWithConditionAsync(x => x.descripcion_clase.Equals(datos.descripcion_clase) && x.id_clase != datos.id_clase);
                if (validate)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"La clase {datos.descripcion_clase} ya se encuentra registrada."));

                entidad.descripcion_clase = datos.descripcion_clase;
                await new ClaseLicenciasRepository().ActualizarClaseSeccion(entidad, secciones);
                return Responses.SetUpdatedResponse(respuesta);
            }
        }

        public async Task<IEnumerable<ClaseDTO>> GetAllClaseLicenciasAsync()
        {
            return await new ClaseLicenciasRepository().GetTableClase();
        }

        public async Task<IEnumerable<ClaseDTO>> GetClasesPorSeccionId(int id)
        {
            return await new ClaseLicenciasRepository().GetClasesPorSeccionId(id);
        }

        public async Task<IEnumerable<GENTEMAR_CLASE_LICENCIAS>> GetAllClaseLicenciasActivas()
        {
            return await new ClaseLicenciasRepository().GetAllWithConditionAsync(x => x.activo == Constantes.ACTIVO);
        }

        public async Task<IEnumerable<ClaseDTO>> GetClasesPorSeccionesIds(List<int> ids)
        {
            return await new ClaseLicenciasRepository().GetClasesPorSeccionesIds(ids);
        }
    }
}
