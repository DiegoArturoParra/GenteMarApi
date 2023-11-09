using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Config;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class ObservacionesLicenciasRepository : GenericRepository<GENTEMAR_OBSERVACIONES_LICENCIAS>
    {
        public async Task CrearObservacion(GENTEMAR_OBSERVACIONES_LICENCIAS entidad, GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = null)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.GENTEMAR_OBSERVACIONES_LICENCIAS.Add(entidad);
                    await SaveAllAsync();
                    if (repositorio != null)
                    {

                        repositorio.IdModulo = entidad.id_observacion.ToString();
                        repositorio.IdUsuarioCreador = ClaimsHelper.GetLoginName();
                        repositorio.FechaHoraCreacion = DateTime.Now;
                        _context.GENTEMAR_REPOSITORIO_ARCHIVOS.Add(repositorio);
                        await SaveAllAsync();
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ObtenerException(ex, entidad);
                }
            }
        }

        public async Task<List<ObservacionDTO>> GetObservacionesId(long id)
        {
            var query = await (from observaciones in _context.GENTEMAR_OBSERVACIONES_LICENCIAS
                               where observaciones.id_licencia == id
                               select new ObservacionDTO
                               {
                                   IdTablaRelacion = observaciones.id_licencia,
                                   Observacion = observaciones.observacion,
                                   FechaHoraCreacion = observaciones.fecha_hora_creacion,
                                   ArchivoBase = new ArchivoBaseDTO()
                                   {
                                       RutaArchivo = observaciones.ruta_archivo != string.Empty || observaciones.ruta_archivo != null
                                       ? (from archivo in _context.GENTEMAR_REPOSITORIO_ARCHIVOS
                                          where archivo.IdModulo == observaciones.id_observacion.ToString()
                                          && archivo.TipoDocumento == Constantes.CARPETA_OBSERVACIONES &&
                                          archivo.NombreModulo == Constantes.CARPETA_MODULO_LICENCIAS
                                          select new
                                          {
                                              archivo

                                          }).Select(x => x.archivo.RutaArchivo).FirstOrDefault() : null,
                                   }
                               }).ToListAsync();
            return query;
        }
    }
}
