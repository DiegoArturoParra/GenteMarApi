using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using DIMARCore.Utilities.Middleware;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using DIMARCore.UIEntities;

namespace DIMARCore.Business
{
    public class ActividadBO
    {

        /// <summary>
        /// Lista de Actividades
        /// </summary>
        /// <returns>Lista de las Actividades</returns>
        /// <entidad>GENTEMAR_ACTIVIDAD</entidad>
        /// <tabla>GENTEMAR_ACTIVIDAD</tabla>
        public async Task<IEnumerable<ActividadTipoLicenciaDTO>> GetActividadesAsync()
        {
            // Obtiene la lista
            return await new ActividadRepository().GetActividades();
        }

        /// <summary>
        /// Lista de Actividades activas
        /// </summary>
        /// <returns>Lista de las Actividades</returns>
        /// <entidad>GENTEMAR_ACTIVIDAD</entidad>
        /// <tabla>GENTEMAR_ACTIVIDAD</tabla>
        public async Task<IEnumerable<GENTEMAR_ACTIVIDAD>> GetActividadesActivo()
        {
            // Obtiene la lista
            return await new ActividadRepository().GetAllWithConditionAsync(x => x.activo == true);
        }

        /// <summary>
        /// Lista de Actividades activas por id de tipo de licencia 
        /// </summary>
        /// <returns>Lista de las Actividades</returns>
        /// <entidad>GENTEMAR_ACTIVIDAD</entidad>
        /// <tabla>GENTEMAR_ACTIVIDAD</tabla>
        public async Task<IEnumerable<GENTEMAR_ACTIVIDAD>> GetActividadesActivoTipoLicencia(int id)
        {
            // Obtiene la lista
            return await new ActividadRepository().GetAllWithConditionAsync(x => x.activo == true && x.id_tipo_licencia == id);
        }

        public async Task<IEnumerable<ActividadLicenciaDTO>> GetActividadesActivasPorTiposDeLicencia(List<int> idsTipoLicencia)
        {
            return await new ActividadRepository().GetActividadesActivasPorTiposDeLicencia(idsTipoLicencia);
        }

        /// <summary>
        /// Obtener Actividad dado su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objeto Adtividad dado su id</returns>
        /// <entidad>GENTEMAR_ACTIVIDAD</entidad>
        /// <tabla>GENTEMAR_ACTIVIDAD</tabla>
        public async Task<GENTEMAR_ACTIVIDAD> GetActividad(int id)
        {
            return await new ActividadRepository().GetById(id);
        }

        /// <summary>
        /// crear una nueva Actividad
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusCodeException"></exception>
        public async Task<Respuesta> CrearActividadAsync(GENTEMAR_ACTIVIDAD datos)
        {
            using (var repo = new ActividadRepository())
            {
                var validate = await repo.AnyWithCondition(x => x.actividad.Equals(datos.actividad) &&
                x.id_tipo_licencia == datos.id_tipo_licencia);
                if (validate)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"Ya se encuentra registrada la actividad {datos.actividad}"));
                datos.activo = true;
                await repo.Create(datos);
            }
            return Responses.SetCreatedResponse(datos);
        }


        /// <summary>
        /// /
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusCodeException"></exception>
        public async Task<Respuesta> EditarActividadAsync(GENTEMAR_ACTIVIDAD datos)
        {

            using (var repo = new ActividadRepository())
            {
                var validate = await repo.AnyWithCondition(x => x.actividad.Equals(datos.actividad) && x.id_actividad != datos.id_actividad);
                if (validate)

                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"Ya se encuentra registrada la actividad {datos.actividad}"));

                var objeto = await repo.GetById(datos.id_actividad);
                if (objeto == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"No se encuentra registrada la actividad indicada."));

                objeto.actividad = datos.actividad;
                await new ActividadRepository().Update(objeto);

            }
            return Responses.SetUpdatedResponse(datos);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusCodeException"></exception>
        public async Task<Respuesta> cambiarActividad(int id)
        {
            using (var repo = new ActividadRepository())
            {
                var objeto = await repo.GetById(id);
                if (objeto == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"No se encuentra registrada la actividad indicada."));
                objeto.activo = !objeto.activo;
                await repo.Update(objeto);
            }
            return Responses.SetUpdatedResponse();
        }

        
    }
}







