using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Linq;


namespace DIMARCore.Repositories.Repository
{
    public class ActividadRepository : GenericRepository<GENTEMAR_ACTIVIDAD>
    {

        /// <summary>
        /// Lista de Actividades
        /// </summary>
        /// <returns>Lista de Actividades</returns>
        /// <tabla>GENTEMAR_ACTIVIDAD</tabla>
        public IList<ActividadDTO> GetActividades()
        {

            var resultado = (from actividad in _context.GENTEMAR_ACTIVIDAD
                             join tipoLicencia in _context.GENTEMAR_TIPO_LICENCIA on
                             actividad.id_tipo_licencia equals tipoLicencia.id_tipo_licencia
                             select new ActividadDTO
                             {
                                 Actividad = actividad.actividad,
                                 IdActividad = actividad.id_actividad,
                                 IdTipoLicencia = actividad.id_tipo_licencia,
                                 Activo = actividad.activo,
                                 TipoLicencia = tipoLicencia.tipo_licencia
                             }
                             ).OrderBy(p => p.Actividad).ToList();

            //var resultado = contexto.GENTEMAR_ACTIVIDAD.OrderBy(e => e.actividad).ToList();
            return resultado;
        }
    }
}
