using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class ObservacionEntidadEstupefacienteRepository : GenericRepository<GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES>
    {
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public async Task<IEnumerable<DetalleExpedienteObservacionEstupefacienteDTO>> GetObservacionesEntidadPorEstupefacienteId(long estupefacienteId)
        {
            var resultado = await (from observacionEntidad in _context.GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES
                                   join entidad in _context.GENTEMAR_ENTIDAD on observacionEntidad.id_entidad equals entidad.id_entidad
                                   join estupefaciente in _context.GENTEMAR_ANTECEDENTES on observacionEntidad.id_antecedente equals estupefaciente.id_antecedente
                                   join expediente in _context.GENTEMAR_EXPEDIENTE on observacionEntidad.id_expediente equals expediente.id_expediente
                                   where observacionEntidad.id_antecedente == estupefacienteId
                                   && observacionEntidad.fecha_respuesta_entidad != null
                                   select new DetalleExpedienteObservacionEstupefacienteDTO
                                   {
                                       ExpedienteObservacionId = observacionEntidad.id_expediente_observacion,
                                       AntecedenteId = observacionEntidad.id_antecedente,
                                       DetalleObservacion = observacionEntidad.descripcion_observacion,
                                       Entidad = entidad.entidad,
                                       VerificacionExitosa = observacionEntidad.verificacion_exitosa ?? false,
                                       EntidadId = entidad.id_entidad,
                                       NumeroDeExpediente = expediente.numero_expediente,
                                       FechaRespuestaEntidad = observacionEntidad.fecha_respuesta_entidad,
                                       Radicado = estupefaciente.numero_sgdea
                                   }).Where(x => x.FechaRespuestaEntidad != null && x.DetalleObservacion.Length > 0).ToListAsync();
            return resultado;
        }

        public async Task CrearObservacionesEntidadCascade(long antecedenteId, IList<GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES> data)
        {
            using (_context)
            {
                using (var trassaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var expedientes = await _context.GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES.Where(x => x.id_antecedente == antecedenteId).ToListAsync();

                        foreach (var item in expedientes)
                        {
                            try
                            {
                                var objetoEdicion = data.Where(y => y.id_entidad == item.id_entidad && y.id_antecedente == item.id_antecedente).FirstOrDefault()
                                    ?? throw new Exception("Relación no existente.");

                                item.descripcion_observacion = objetoEdicion.descripcion_observacion;
                                item.fecha_respuesta_entidad = objetoEdicion.fecha_respuesta_entidad;
                                item.verificacion_exitosa = objetoEdicion.verificacion_exitosa;
                                _context.Entry(item).State = EntityState.Modified;
                            }
                            catch (Exception ex)
                            {
                                var entidad = await _context.GENTEMAR_ENTIDAD.Where(x => x.id_entidad == item.id_entidad).Select(y => y.entidad).FirstOrDefaultAsync();
                                _logger.Error($"No se encuentra el antecedente {item.id_antecedente} con la entidad {entidad}", ex);
                                continue;
                            }
                        }
                        await SaveAllAsync();
                        await new EstupefacienteRepository().ChangeNarcoticStateIfAllVerifications(antecedenteId);
                        trassaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        trassaction.Rollback();
                        ObtenerException(ex, data[1]);
                    }
                }
            }


        }

        public async Task EditarObservacionesEntidadCascade(IList<GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES> data, GENTEMAR_OBSERVACIONES_ANTECEDENTES observacion,
            GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = null)
        {
            using (_context)
            {
                using (var trassaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES.RemoveRange(_context.GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES
                            .Where(x => x.id_antecedente == observacion.id_antecedente).ToList());

                        _context.GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES.AddRange(data);

                        _context.GENTEMAR_OBSERVACIONES_ANTECEDENTES.Add(observacion);

                        if (repositorio != null)
                        {
                            repositorio.IdModulo = observacion.id_observacion.ToString();
                            repositorio.IdUsuarioCreador = observacion.usuario_creador_registro;
                            repositorio.FechaHoraCreacion = observacion.fecha_hora_creacion;
                            _context.GENTEMAR_REPOSITORIO_ARCHIVOS.Add(repositorio);
                        }

                        await SaveAllAsync();

                        trassaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        trassaction.Rollback();
                        ObtenerException(ex, data[1]);
                    }
                }
            }
        }

        public async Task CrearObservacionPorEntidad(GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES dataActual)
        {
            using (_context)
            {
                using (var trassaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        await Update(dataActual);
                        await new EstupefacienteRepository().ChangeNarcoticStateIfAllVerifications(dataActual.id_antecedente);
                        trassaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        trassaction.Rollback();
                        ObtenerException(ex, dataActual);
                    }
                }
            }
        }
    }
}
