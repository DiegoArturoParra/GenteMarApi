using GenteMarCore.Entities;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace DIMARCore.Repositories
{
    public class ActividadRepository : GenericRepository<GENTEMAR_ACTIVIDAD>
    {
        GenteDeMarCoreContext contexto = new GenteDeMarCoreContext();

        /// <summary>
        /// Lista de Actividades
        /// </summary>
        /// <returns>Lista de Actividades</returns>
        /// <tabla>GENTEMAR_ACTIVIDAD</tabla>
        public IList<GENTEMAR_ACTIVIDAD> GetActividades()
        {
            try
            {
                var resultado = (from a in contexto.GENTEMAR_ACTIVIDAD
                                 select a
                                 ).OrderBy(p => p.actividad).ToList();

                //var resultado = contexto.GENTEMAR_ACTIVIDAD.OrderBy(e => e.actividad).ToList();

                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Actividad dado el Id
        /// </summary>
        /// <param name="id">Id del Actividad</param>
        /// <returns>Actividad</returns>
        /// <tabla>GENTEMAR_ACTIVIDAD</tabla>
        public GENTEMAR_ACTIVIDAD GetActividad(int id)
        {
            try
            {
                var resultado = (from c in this.contexto.GENTEMAR_ACTIVIDAD
                                 where c.id_actividad == id
                                 select c
                                ).FirstOrDefault();

                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
