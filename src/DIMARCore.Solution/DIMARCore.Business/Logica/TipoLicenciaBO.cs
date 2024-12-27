using DIMARCore.Repositories.Repository;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace DIMARCore.Business
{
    public class TipoLicenciaBO
    {

        /// <summary>
        /// Lista de Tipo Licencias
        /// </summary>
        /// <returns>Lista de Tipo Licenciass</returns>
        /// <entidad>GENTEMAR_TIPO_LICENCIA</entidad>
        /// <tabla>GENTEMAR_TIPO_LICENCIA</tabla>
        public async Task<IEnumerable<GENTEMAR_TIPO_LICENCIA>> GetTipoLicencias()
        {
            // Obtiene la lista
            return await new TipoLicenciaRepository().GetTipoLicencias();
        }

        /// <summary>
        /// Lista de Tipo Licencias
        /// </summary>
        /// <returns>Lista de Tipo Licenciass</returns>
        /// <entidad>GENTEMAR_TIPO_LICENCIA</entidad>
        /// <tabla>GENTEMAR_TIPO_LICENCIA</tabla>
        public async Task<IEnumerable<GENTEMAR_TIPO_LICENCIA>> GetTipoLicenciasActivo()
        {
            // Obtiene la lista
            return await new TipoLicenciaRepository().GetAllWithConditionAsync(x => x.activo == Constantes.ACTIVO);
        }


        /// <summary>
        /// Obtener Tipo Licencia dado su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objeto Tipo Licencias dado su id</returns>
        /// <entidad>GENTEMAR_TIPO_LICENCIA</entidad>
        /// <tabla>GENTEMAR_TIPO_LICENCIA</tabla>
        public GENTEMAR_TIPO_LICENCIA GetTipoLicencia(int id)
        {
            return new TipoLicenciaRepository().GetTipoLicencia(id);
        }

        /// <summary>
        /// Crea un tipo de licancia  
        /// </summary>
        /// <param name="datos">Información del tipo de licencia </param>
        /// <returns>Respuesta resultado</returns>
        public async Task<Respuesta> CrearTipoLicenciaAsync(GENTEMAR_TIPO_LICENCIA datos)
        {
            using (var repo = new TipoLicenciaRepository())
            {
                var validate = await repo.GetWithConditionAsync(x => x.tipo_licencia.Equals(datos.tipo_licencia));
                if (validate != null)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"El tipo de licencia {datos.tipo_licencia} ya está creada."));
                datos.activo = Constantes.ACTIVO;
                await repo.Create(datos);
                return Responses.SetCreatedResponse(datos);
            }

        }


        /// <summary>
        /// Edita la información de un tipo de licencia  
        /// </summary>
        /// <param name="datos">Datos de un tipo de licencia </param>
        /// <returns>Respuesta resultado</returns>
        public async Task<Respuesta> EditarTipoLicenciaAsync(GENTEMAR_TIPO_LICENCIA datos)
        {
            using (var repo = new TipoLicenciaRepository())
            {
                // busca el tipo de licencia
                var entidad = await repo.GetWithConditionAsync(x => x.id_tipo_licencia == datos.id_tipo_licencia);

                if (entidad == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El tipo de licencia no existe."));


                var validate = await repo.GetWithConditionAsync(x => x.tipo_licencia.Equals(datos.tipo_licencia));
                if (validate != null)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"El tipo de licencia {datos.tipo_licencia} ya está creada."));

                entidad.tipo_licencia = datos.tipo_licencia;
                await repo.Update(entidad);
                return Responses.SetUpdatedResponse(entidad);
            }
        }

        /// <summary>
        /// cambia el estado del rango 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Respuesta> CambiarTipoLicencia(int id)
        {
            using (var repo = new TipoLicenciaRepository())
            {
                var entidad = await repo.GetWithConditionAsync(x => x.id_tipo_licencia == id);
                if (entidad == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El tipo de licencia no existe"));
                entidad.activo = !entidad.activo;
                await repo.Update(entidad);
                return Responses.SetUpdatedResponse(entidad);
            }
        }

    }
}






