using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class ObservacionesDatosBasicosRepository : GenericRepository<GENTEMAR_OBSERVACIONES_DATOSBASICOS>
    {
        public ObservacionesDatosBasicosRepository()
        {

        }
        public ObservacionesDatosBasicosRepository(GenteDeMarCoreContext context) : base(context)
        {
        }
        public async Task CrearObservacion(GENTEMAR_OBSERVACIONES_DATOSBASICOS entidad, GENTEMAR_REPOSITORIO_ARCHIVOS repositorio)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.GENTEMAR_OBSERVACIONES_DATOSBASICOS.Add(entidad);
                    await SaveAllAsync();
                    if (repositorio != null)
                    {

                        repositorio.IdModulo = entidad.id_observacion.ToString();
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
        /// <summary>
        /// Metodo para obtener las observaciones datos basicos filtrados por id gente de mar
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<ObservacionDTO>> GetObservacionesId(long id)
        {

            var query = await (from observaciones in _context.GENTEMAR_OBSERVACIONES_DATOSBASICOS
                               where observaciones.id_gentemar == id
                               select new ObservacionDTO
                               {
                                   IdTablaRelacion = observaciones.id_gentemar,
                                   Observacion = observaciones.observacion,
                                   FechaHoraCreacion = observaciones.FechaCreacion,
                                   UsuarioCreador = _context.APLICACIONES_LOGINS
                                                    .Where(x => x.ID_LOGIN == observaciones.LoginCreacionId)
                                                    .Select(x => x.LOGIN_NAME.ToLower()).FirstOrDefault(),
                                   ArchivoBase = new ArchivoBaseDTO()
                                   {
                                       RutaArchivo = observaciones.ruta_archivo != string.Empty || observaciones.ruta_archivo != null
                                       ? (from archivo in _context.GENTEMAR_REPOSITORIO_ARCHIVOS
                                          where archivo.IdModulo == observaciones.id_observacion.ToString()
                                          && archivo.TipoDocumento == Constantes.CARPETA_OBSERVACIONES &&
                                          archivo.NombreModulo == Constantes.CARPETA_MODULO_DATOSBASICOS
                                          select new
                                          {
                                              archivo

                                          }).Select(x => x.archivo.RutaArchivo).FirstOrDefault() : null
                                   }
                               }).AsNoTracking().ToListAsync();

            return query;

        }
    }
}
