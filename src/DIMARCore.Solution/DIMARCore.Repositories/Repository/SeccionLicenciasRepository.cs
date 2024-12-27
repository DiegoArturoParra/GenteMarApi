using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class SeccionLicenciasRepository : GenericRepository<GENTEMAR_SECCION_LICENCIAS>
    {

        /// <summary>
        /// Lista de las secciones por id de la actividad
        /// </summary>
        /// <returns>Lista de las secciones</returns>
        public async Task<IList<SeccionDTO>> GetSeccionActividad(int id)
        {
            return await (from actividadSeccion in _context.GENTEMAR_ACTIVIDAD_SECCION_LICENCIA
                          join seccion in _context.GENTEMAR_SECCION_LICENCIAS on actividadSeccion.id_seccion equals
                          seccion.id_seccion
                          where actividadSeccion.id_actividad == id && seccion.activo == Constantes.ACTIVO
                          select new SeccionDTO
                          {
                              Id = seccion.id_seccion,
                              Descripcion = seccion.actividad_a_bordo,
                              IsActive = seccion.activo,
                              IdActividaSeccion = actividadSeccion.id_actividad_seccion_licencia
                          }).ToListAsync();
        }

        /// <summary>
        /// Lista de las secciones por ids de la actividad
        /// </summary>
        /// <returns>Lista de las secciones</returns>
        public async Task<IList<SeccionDTO>> GetSeccionesPorActividadesIds(List<int> ids)
        {
            return await (from actividadSeccion in _context.GENTEMAR_ACTIVIDAD_SECCION_LICENCIA
                          join seccion in _context.GENTEMAR_SECCION_LICENCIAS on actividadSeccion.id_seccion equals
                          seccion.id_seccion
                          where ids.Contains(actividadSeccion.id_actividad) && seccion.activo == Constantes.ACTIVO
                          group new { actividadSeccion, seccion } by new { actividadSeccion.id_seccion, seccion.actividad_a_bordo, seccion.activo } into g
                          select new SeccionDTO
                          {
                              Id = g.Key.id_seccion,
                              Descripcion = g.Key.actividad_a_bordo,
                              IsActive = g.Key.activo
                          }).ToListAsync();
        }


        /// <summary>
        /// Lista de formacion con los grados asignados 
        /// </summary>
        /// <returns>Lista de los tipos de formacion</returns>
        public async Task<IEnumerable<SeccionDTO>> GetTableSecciones()
        {

            var resultado = await (from seccion in _context.GENTEMAR_SECCION_LICENCIAS
                                   select new SeccionDTO
                                   {
                                       Id = seccion.id_seccion,
                                       Descripcion = seccion.actividad_a_bordo,
                                       IsActive = seccion.activo,
                                       Actividad = (from actividad in _context.GENTEMAR_ACTIVIDAD
                                                    join seccionActividad in _context.GENTEMAR_ACTIVIDAD_SECCION_LICENCIA on
                                                    actividad.id_actividad equals seccionActividad.id_actividad
                                                    where seccionActividad.id_seccion == seccion.id_seccion
                                                    select new ActividadTipoLicenciaDTO
                                                    {
                                                        Actividad = actividad.actividad,
                                                        IdActividad = actividad.id_actividad,
                                                        Activo = actividad.activo
                                                    }
                                                    ).ToList()

                                   }).ToListAsync();
            return resultado;
        }

        /// <summary>
        /// metodo para crear una seccion con sus actividades
        /// </summary>
        /// <param name="entidad"></param>
        /// <param name="actividad"></param>
        /// <returns></returns>
        public async Task CrearActividadSeccion(GENTEMAR_SECCION_LICENCIAS entidad, IList<GENTEMAR_ACTIVIDAD> actividad)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.GENTEMAR_SECCION_LICENCIAS.Add(entidad);
                    await SaveAllAsync();
                    await CrateInCascadeActividadSeccion(actividad, entidad.id_seccion);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ObtenerException(ex, entidad);
                }
            }
        }


        /// <summary>
        /// metodo para actualizar una seccion con sus actividades
        /// </summary>
        /// <param name="entidad"></param>
        /// <param name="actividad"></param>
        /// <returns></returns>
        public async Task ActualizarActividadSeccion(GENTEMAR_SECCION_LICENCIAS entidad, IList<GENTEMAR_ACTIVIDAD> actividad)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await Update(entidad);
                    //elimina los registros ya creados 
                    _context.GENTEMAR_ACTIVIDAD_SECCION_LICENCIA.RemoveRange(
                        _context.GENTEMAR_ACTIVIDAD_SECCION_LICENCIA.Where(x => x.id_seccion == entidad.id_seccion)
                    );
                    //agrega nuevamente los registros  
                    await CrateInCascadeActividadSeccion(actividad, entidad.id_seccion);
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ObtenerException(ex, entidad);
                }
            }
        }


        private async Task CrateInCascadeActividadSeccion(IList<GENTEMAR_ACTIVIDAD> actividad, int id)
        {
            foreach (GENTEMAR_ACTIVIDAD item in actividad)
            {
                var ActividadSeccion = new GENTEMAR_ACTIVIDAD_SECCION_LICENCIA
                {
                    id_seccion = id,
                    id_actividad = item.id_actividad
                };
                _context.GENTEMAR_ACTIVIDAD_SECCION_LICENCIA.Add(ActividadSeccion);
            }
            await SaveAllAsync();
        }
    }
}
