using GenteMarCore.Entities;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace DIMARCore.Repositories
{
    public class UsuarioRepository : IDisposable
    {
        GenteDeMarCoreContext contexto = new GenteDeMarCoreContext();
        // private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Obtiene la lista de usuarios de la base de datos
        /// </summary>
        /// <returns>Lista de usuarios</returns>
        public IList<APLICACIONES_LOGINS> GetAll()
        {
            try
            {
                // Obtiene la lista
                contexto.ANT_ROL.ToListAsync();
                return this.contexto.APLICACIONES_LOGINS.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Obtiene la lista de usuarios dada un condición
        /// </summary>
        /// <param name="whereCondition">Condición de búsqueda</param>
        /// <returns>Lista de usuarios que cumplen con la condición</returns>
        public IList<APLICACIONES_LOGINS> GetAll(Expression<Func<APLICACIONES_LOGINS, bool>> whereCondition)
        {
            try
            {
                return this.contexto.APLICACIONES_LOGINS.Where(whereCondition).ToList<APLICACIONES_LOGINS>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Obtiene un usuario dada una condición
        /// </summary>
        /// <param name="whereCondition">Condición de búsqueda</param>
        /// <returns>Un usuario que cumple con la condición</returns>
        public APLICACIONES_LOGINS Get(Expression<Func<APLICACIONES_LOGINS, bool>> whereCondition)
        {
            try
            {
                return this.contexto.APLICACIONES_LOGINS.Where(whereCondition).FirstOrDefault<APLICACIONES_LOGINS>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Obtiene un usuario de la base de datos
        /// </summary>
        /// <param name="id">id del usuario</param>
        /// <returns>Un Usuarios_Usuario</returns>
        public APLICACIONES_LOGINS Find(decimal id)
        {
            try
            {
                return this.contexto.APLICACIONES_LOGINS.Find(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// Guarda los cambios del estado
        /// </summary>
        public void Save()
        {
            try
            {
                this.contexto.SaveChanges();
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
            throw new NotImplementedException();
        }

    }
}