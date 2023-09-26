using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq;
using DIMARCore.Utilities.Helpers;

namespace DIMARCore.Repositories.Repository
{
    public class HistorialAclaracionEstupefacienteRepository : GenericRepository<GENTEMAR_HISTORIAL_ACLARACION_ANTECEDENTES>
    {
        public async Task AgregarAclaracionPorExpedienteObservacion(GENTEMAR_HISTORIAL_ACLARACION_ANTECEDENTES dataAclaracion,
            GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES expedienteObservacion, GENTEMAR_REPOSITORIO_ARCHIVOS repositorio)
        {
            using (_context)
            {
                using (var trassaction = _context.Database.BeginTransaction())
                    try
                    {
                        await Create(dataAclaracion);
                        if (repositorio != null)
                        {
                            repositorio.IdModulo = dataAclaracion.id_aclaracion.ToString();
                            repositorio.IdUsuarioUltimaActualizacion = dataAclaracion.usuario_creador_registro;
                            repositorio.IdUsuarioCreador = dataAclaracion.usuario_creador_registro;
                            repositorio.FechaHoraUltimaActualizacion = DateTime.Now;
                            _context.GENTEMAR_REPOSITORIO_ARCHIVOS.Add(repositorio);

                        }
                        _context.Entry(expedienteObservacion).State = EntityState.Modified;
                        await SaveAllAsync();
                        await new EstupefacienteRepository().ChangeNarcoticStateIfAllVerifications(expedienteObservacion.id_antecedente);
                        trassaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        trassaction.Rollback();
                        ObtenerException(ex, dataAclaracion);
                    }
            }
        }

        public async Task<IEnumerable<HistorialAclaracionDTO>> GetHistorialPorEstupefacienteId(long idAntecedente)
        {
            var resultado = await (from aclaracion in _context.GENTEMAR_HISTORIAL_ACLARACION_ANTECEDENTES
                                   join observacionExpediente in _context.GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES
                                   on aclaracion.id_expediente_observacion equals observacionExpediente.id_expediente_observacion
                                   join entidad in _context.GENTEMAR_ENTIDAD on observacionExpediente.id_entidad equals entidad.id_entidad
                                   where observacionExpediente.id_antecedente == idAntecedente
                                   select new HistorialAclaracionDTO
                                   {
                                       DetalleAclaracion = aclaracion.detalle_aclaracion,
                                       Entidad = entidad.entidad,
                                       FechaHoraCreacion = aclaracion.fecha_hora_creacion,
                                       UsuarioCreador = aclaracion.usuario_creador_registro,
                                       ObservacionAnteriorJson = aclaracion.detalle_observacion_anterior_json,
                                       ArchivoBase = new ArchivoBaseDTO()
                                       {
                                           RutaArchivo = aclaracion.ruta_archivo != string.Empty || aclaracion.ruta_archivo != null
                                            ? (from archivo in _context.GENTEMAR_REPOSITORIO_ARCHIVOS
                                               where archivo.IdModulo == aclaracion.id_aclaracion.ToString()
                                               && archivo.TipoDocumento == Constantes.CARPETA_ACLARACION_EXPEDIENTE &&
                                               archivo.NombreModulo == Constantes.CARPETA_MODULO_ESTUPEFACIENTES
                                               select new
                                               {
                                                   archivo
                                               }).Select(x => x.archivo.RutaArchivo).FirstOrDefault() : null
                                       }
                                   }).ToListAsync();
            return resultado;
        }
    }
}
