using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repo
{
    public class ObservacionesDatosBasicosRepository : GenericRepository<GENTEMAR_OBSERVACIONES_DATOSBASICOS>
    {
        public async Task CrearObservacion(GENTEMAR_OBSERVACIONES_DATOSBASICOS entidad, REPOSITORIO_ARCHIVOS repositorio)
        {
            using (_context)
            {
                using (var trassaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.GENTEMAR_OBSERVACIONES_DATOSBASICOS.Add(entidad);
                        await SaveAllAsync();
                        if (repositorio != null)
                        {

                            repositorio.IdModulo = entidad.id_observacion.ToString();
                            repositorio.IdUsuarioCreador = entidad.usuario_creador_registro;
                            repositorio.FechaHoraCreacion = entidad.fecha_hora_creacion;
                            _context.GENTEMAR_REPOSITORIO_ARCHIVOS.Add(repositorio);
                            await SaveAllAsync();
                        }
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
        /// Metodo para obtener datos basicos filtados por id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ObservacionDTO> GetObservacionesId(long id)
        {
            var query = (
                         from observaciones in _context.GENTEMAR_OBSERVACIONES_DATOSBASICOS
                         join archivo in _context.GENTEMAR_REPOSITORIO_ARCHIVOS on observaciones.id_observacion.ToString() equals archivo.IdModulo
                         into fo
                         from subFile in fo.DefaultIfEmpty()
                         where observaciones.id_gentemar == id && subFile != null ? subFile.TipoDocumento == Constantes.CARPETA_OBSERVACIONES : subFile.TipoDocumento == null
                         select new ObservacionDTO
                         {
                             IdTablaRelacion = observaciones.id_gentemar,
                             Observacion = observaciones.observacion,
                             FechaHoraCreacion = observaciones.fecha_hora_creacion,
                             RutaArchivo = subFile.RutaArchivo
                         }).ToList();
            return query;

        }
    }
}
