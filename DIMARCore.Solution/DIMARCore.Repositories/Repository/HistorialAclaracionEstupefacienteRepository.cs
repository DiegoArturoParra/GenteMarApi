using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Config;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class HistorialAclaracionEstupefacienteRepository : GenericRepository<GENTEMAR_HISTORIAL_ACLARACION_ANTECEDENTES>
    {
        public async Task AgregarAclaracionPorExpedienteObservacion(GENTEMAR_HISTORIAL_ACLARACION_ANTECEDENTES dataAclaracion,
            GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES expedienteObservacion, GENTEMAR_REPOSITORIO_ARCHIVOS repositorio)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await Create(dataAclaracion);
                    if (repositorio != null)
                    {
                        repositorio.IdModulo = dataAclaracion.id_aclaracion.ToString();
                        repositorio.IdUsuarioCreador = ClaimsHelper.GetLoginName();
                        repositorio.FechaHoraCreacion = DateTime.Now;
                        _context.GENTEMAR_REPOSITORIO_ARCHIVOS.Add(repositorio);

                    }
                    _context.Entry(expedienteObservacion).State = EntityState.Modified;
                    await SaveAllAsync();
                    await new EstupefacienteRepository().ChangeNarcoticStateIfAllVerifications(expedienteObservacion.id_antecedente);
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ObtenerException(ex, dataAclaracion);
                }
            }
        }

        public async Task<IEnumerable<HistorialAclaracionDTO>> GetHistorialPorEstupefacienteId(long idAntecedente)
        {
            var resultado = await (from aclaracion in _context.GENTEMAR_HISTORIAL_ACLARACION_ANTECEDENTES
                                   join observacionExpediente in _context.GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES
                                   on aclaracion.id_expediente_observacion equals observacionExpediente.id_expediente_observacion
                                   join entidad in _context.GENTEMAR_ENTIDAD_ANTECEDENTE on observacionExpediente.id_entidad equals entidad.id_entidad
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
                                           RutaArchivo = aclaracion.ruta_archivo
                                       }
                                   }).ToListAsync();
            return resultado;
        }
    }
}
