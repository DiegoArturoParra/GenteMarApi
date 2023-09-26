using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class ClaseLicenciasRepository : GenericRepository<GENTEMAR_CLASE_LICENCIAS>
    {

        /// <summary>
        /// Lista de clase con las secciones  
        /// </summary>
        /// <returns>Lista de los tipos de formacion</returns>
        public IList<ClaseDTO> GetTableClase()
        {
            try
            {
                var resultado = (from clase in _context.GENTEMAR_CLASE_LICENCIAS
                                 select new ClaseDTO
                                 {
                                     Id = clase.id_clase,
                                     Descripcion = clase.descripcion_clase,
                                     IsActive = clase.activo,
                                     Sigla = clase.sigla,
                                     Seccion = (from seccion in _context.GENTEMAR_SECCION_LICENCIAS
                                                join claseSeccion in _context.GENTEMAR_SECCION_CLASE on
                                                seccion.id_seccion equals claseSeccion.id_seccion
                                                where claseSeccion.id_clase == clase.id_clase
                                                select new SeccionDTO
                                                {
                                                    Id = seccion.id_seccion,
                                                    Descripcion = seccion.actividad_a_bordo,
                                                    IsActive = seccion.activo,
                                                    IdActividaSeccion = claseSeccion.id_seccion_clase

                                                }).ToList(),

                                 }).ToList();

                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lista de clase dependiendo el id de la seccion 
        /// </summary>
        /// <returns>Lista de las clases</returns>
        public async Task<IEnumerable<ClaseDTO>> GetClaseSeccion(int id)
        {
            return await (from seccionClase in _context.GENTEMAR_SECCION_CLASE
                          join clase in _context.GENTEMAR_CLASE_LICENCIAS on
                          seccionClase.id_clase equals clase.id_clase
                          where seccionClase.id_seccion == id && clase.activo == true
                          select new ClaseDTO
                          {
                              Id = clase.id_clase,
                              Descripcion = clase.descripcion_clase,
                              IsActive = clase.activo,
                              Sigla = clase.sigla,
                          }).ToListAsync();
        }

        /// <summary>
        /// Metodo para crear los cargo licencia con limitaciones.
        /// </summary>
        /// <param name="entidad"></param>
        /// <param name="seciones"></param>
        /// <returns></returns>
        public async Task CrearClaseSeccion(GENTEMAR_CLASE_LICENCIAS entidad, IList<GENTEMAR_SECCION_LICENCIAS> seciones)
        {
            using (_context)
            {
                using (var trassaction = _context.Database.BeginTransaction())
                {
                    //await Create(entidad);
                    try
                    {
                        entidad.activo = true;
                        _context.GENTEMAR_CLASE_LICENCIAS.Add(entidad);
                        await SaveAllAsync();
                        await CrateInCascadeClaseSeccion(seciones, entidad.id_clase);
                        trassaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        trassaction.Rollback();
                        ObtenerException(ex, entidad);
                    }
                }
            }
        }

        /// <summary>
        /// Metodo para crear los cargo licencia con limitaciones.
        /// </summary>
        /// <param name="entidad"></param>
        /// <param name="actividad"></param>
        /// <param name="IdLimitacion"></param>
        /// <returns></returns>
        public async Task ActualizarClaseSeccion(GENTEMAR_CLASE_LICENCIAS entidad, IList<GENTEMAR_SECCION_LICENCIAS> seciones)
        {
            using (_context)
            {
                using (var trassaction = _context.Database.BeginTransaction())
                {
                    //await Create(entidad);
                    try
                    {
                        _context.GENTEMAR_CLASE_LICENCIAS.Attach(entidad);
                        var entry = _context.Entry(entidad);
                        entry.State = EntityState.Modified;
                        await SaveAllAsync();
                        //elimina los registos ya creados 
                        _context.GENTEMAR_SECCION_CLASE.RemoveRange(
                            _context.GENTEMAR_SECCION_CLASE.Where(x => x.id_clase == entidad.id_clase)
                        );
                        //agrega nuevamente los registros  
                        await CrateInCascadeClaseSeccion(seciones, entidad.id_clase);
                        trassaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        trassaction.Rollback();
                        ObtenerException(ex, entidad);
                    }
                }
            }
        }
        private async Task CrateInCascadeClaseSeccion(IList<GENTEMAR_SECCION_LICENCIAS> actividad, int id)
        {
            foreach (GENTEMAR_SECCION_LICENCIAS item in actividad)
            {
                var ClaseSeccion = new GENTEMAR_SECCION_CLASE
                {
                    id_clase = id,
                    id_seccion = item.id_seccion
                };
                _context.GENTEMAR_SECCION_CLASE.Add(ClaseSeccion);
            }
            await SaveAllAsync();
        }
    }

}
