using DIMARCore.UIEntities;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;


namespace DIMARCore.Repositories.Repository
{
    public class ActividadRepository : GenericRepository<GENTEMAR_ACTIVIDAD>
    {

        /// <summary>
        /// Lista de Actividades
        /// </summary>
        /// <returns>Lista de Actividades</returns>
        /// <tabla>GENTEMAR_ACTIVIDAD</tabla>
        public async Task<IEnumerable<ActividadTipoLicenciaDTO>> GetActividades()
        {

            var resultado = await (from actividad in _context.GENTEMAR_ACTIVIDAD
                                   join tipoLicencia in _context.GENTEMAR_TIPO_LICENCIA on
                                   actividad.id_tipo_licencia equals tipoLicencia.id_tipo_licencia
                                   select new ActividadTipoLicenciaDTO
                                   {
                                       Actividad = actividad.actividad,
                                       IdActividad = actividad.id_actividad,
                                       IdTipoLicencia = actividad.id_tipo_licencia,
                                       Activo = actividad.activo,
                                       TipoLicencia = tipoLicencia.tipo_licencia
                                   }
                             ).OrderBy(p => p.Actividad).ToListAsync();
            return resultado;
        }

        public async Task<IEnumerable<ActividadLicenciaDTO>> GetActividadesActivasPorTiposDeLicencia(List<int> idsTipoLicencia)
        {
            return await _context.GENTEMAR_ACTIVIDAD.Where(x => idsTipoLicencia.Contains(x.id_tipo_licencia) && x.activo == Constantes.ACTIVO).Select(x => new ActividadLicenciaDTO
            {
                Id = x.id_actividad,
                IsActive = x.activo,
                Descripcion = x.actividad,
            }).ToListAsync();
        }
    }
}
