using GenteMarCore.Entities;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DIMARCore.Repositories
{
    public class AplicacionRepository
    {
        GenteDeMarCoreContext contexto = new GenteDeMarCoreContext();


        /// <summary>
        /// Obtiene una aplicación de la base de datos
        /// </summary>
        /// <param name="id">id de la aplicación</param>
        /// <returns>Una aplicación</returns>
        public APLICACIONES Find(decimal id)
        {
            try
            {
                return this.contexto.APLICACIONES.Find(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Obtiene una aplicación de la base de datos
        /// </summary>
        /// <param name="nombre">nombre de la aplicación</param>
        /// <returns>Una aplicación</returns>
        public APLICACIONES GetAplicacionByNombre(string nombre)
        {
            try
            {
                return this.contexto.APLICACIONES.Where(x => x.NOMBRE == nombre).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene un aplicación dada una condición de la tabla APLICACIONES
        /// </summary>
        /// <param name="whereCondition">Condición de búsqueda</param>
        /// <returns>Una aplicacion que cumple con la condición</returns>
        public APLICACIONES Get(Expression<Func<APLICACIONES, bool>> whereCondition)
        {
            try
            {
                return this.contexto.APLICACIONES.Where(whereCondition).FirstOrDefault<APLICACIONES>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene la lista de aplicaciones dada un condición de la tabla APLICACIONES
        /// </summary>
        /// <param name="whereCondition">Condición de búsqueda</param>
        /// <returns>Lista de aplicaciones que cumplen con la condición</returns>
        public IList<APLICACIONES> GetAll(Expression<Func<APLICACIONES, bool>> whereCondition)
        {
            try
            {
                return this.contexto.APLICACIONES.Where(whereCondition).ToList<APLICACIONES>();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Guarda los cambios
        /// </summary>
        public void Save()
        {
            try
            {
                this.contexto.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public APLICACIONES GetById(int idAplicacion)
        {
            try
            {
                return contexto.APLICACIONES.Where(x => x.ID_APLICACION == idAplicacion).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Permite usar el GC automáticamente
        /// </summary>
        public void Dispose()
        {
            try
            {
                this.contexto.Dispose();

            }
            catch (Exception)
            {
                throw;
            }
        }

    }

}
