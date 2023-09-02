using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using DIMARCore.Utilities.Middleware;

namespace DIMARCore.Business.Logica
{
    public class ClaseLicenciasBO
    {
        public async Task<Respuesta> ActualizarAsync(GENTEMAR_CLASE_LICENCIAS datos, IList<GENTEMAR_SECCION_LICENCIAS> secciones)
        {
            Respuesta respuesta = new Respuesta();
            using (var repo = new ClaseLicenciasRepository())
            {
                // busca la Limitación en el sistema

                var validate = await repo.GetWithCondition(x => x.id_clase == datos.id_clase);

                if (validate == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("La clase no existe."));

                datos.id_clase = validate.id_clase;
                datos.activo = validate.activo;
                await new ClaseLicenciasRepository().ActualizarClaseSeccion(datos, secciones);
                return Responses.SetUpdatedResponse(respuesta);
            }
        }


        public async Task<Respuesta> AnulaOrActivaAsync(int id)
        {
            using (var repo = new ClaseLicenciasRepository())
            {
                var validate = await repo.GetWithCondition(x => x.id_clase == id);
                if (validate == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("La clase no existe."));
                validate.activo = !validate.activo;
                await repo.Update(validate);
                return Responses.SetUpdatedResponse(validate);
            }
        }

        public async Task<Respuesta> CrearAsync(GENTEMAR_CLASE_LICENCIAS entidad, IList<GENTEMAR_SECCION_LICENCIAS> secciones)
        {
            using (var repo = new ClaseLicenciasRepository())
            {
                var validate = await repo.AnyWithCondition(x => x.descripcion_clase.Equals(entidad.descripcion_clase));
                if (validate)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse("La clase ya existe."));

                entidad.activo = true;
                await repo.CrearClaseSeccion(entidad, secciones);
                return Responses.SetCreatedResponse(entidad);
            }
        }


        public IEnumerable<ClaseDTO> GetAllClaseLicencias()
        {
            return new ClaseLicenciasRepository().GetTableClase();
        }

        public IEnumerable<ClaseDTO> GetClaseSecciones(int id)
        {
            return new ClaseLicenciasRepository().GetClaseSeccion(id);
        }

        public IEnumerable<GENTEMAR_CLASE_LICENCIAS> GetAllClaseLicenciasActivas()
        {
            return new ClaseLicenciasRepository().GetAllWithCondition(x => x.activo == true);
        }

    }
}
