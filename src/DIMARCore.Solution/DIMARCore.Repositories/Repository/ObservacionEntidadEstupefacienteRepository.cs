using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Config;
using DIMARCore.Utilities.Helpers;
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
        public async Task<IEnumerable<DetalleExpedienteObservacionVciteDTO>> GetObservacionesEntidadPorEstupefacienteId(long estupefacienteId)
        {
            var resultado = await (from observacionEntidad in _context.GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES
                                   join entidad in _context.GENTEMAR_ENTIDAD_ANTECEDENTE on observacionEntidad.id_entidad equals entidad.id_entidad
                                   join estupefaciente in _context.GENTEMAR_ANTECEDENTES on observacionEntidad.id_antecedente equals estupefaciente.id_antecedente
                                   join expediente in _context.GENTEMAR_EXPEDIENTE on observacionEntidad.id_expediente equals expediente.id_expediente
                                   where observacionEntidad.id_antecedente == estupefacienteId
                                   && observacionEntidad.descripcion_observacion.Length > 0
                                   select new DetalleExpedienteObservacionVciteDTO
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
                                   }).AsNoTracking().ToListAsync();
            return resultado;
        }

        public async Task<List<Respuesta>> CrearObservacionesEntidadCascade(long antecedenteId, IList<GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES> data)
        {
            List<Respuesta> errores = new List<Respuesta>();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var expedientes = await _context.GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES.Where(x => x.id_antecedente == antecedenteId).ToListAsync();

                    foreach (var item in expedientes)
                    {
                        var objetoEdicion = data.Where(y => y.id_entidad == item.id_entidad && y.id_antecedente == item.id_antecedente).FirstOrDefault();
                        if (objetoEdicion is null)
                        {
                            _logger.Error($"No se encuentra el antecedente {item.id_antecedente} relacionado con la entidad: {item.id_entidad}");
                            var respuesta = Responses.SetNotFoundResponse($"No se encuentra el antecedente {item.id_antecedente} relacionado con la entidad: {item.id_entidad}");
                            errores.Add(respuesta);
                        }
                        else
                        {
                            item.descripcion_observacion = objetoEdicion.descripcion_observacion;
                            item.fecha_respuesta_entidad = objetoEdicion.fecha_respuesta_entidad;
                            item.verificacion_exitosa = objetoEdicion.verificacion_exitosa;
                            _context.Entry(item).State = EntityState.Modified;
                        }
                    }
                    await SaveAllAsync();
                    await new EstupefacienteRepository().ChangeNarcoticStateIfAllVerifications(antecedenteId);
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ObtenerException(ex, data[1]);
                }
                return errores;
            }
        }

        public async Task EditarObservacionesEntidadCascade(IList<GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES> data, GENTEMAR_OBSERVACIONES_ANTECEDENTES observacion,
            GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = null)
        {
            string loginName = ClaimsHelper.GetLoginName();
            using (var transaction = _context.Database.BeginTransaction())
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
                        _context.GENTEMAR_REPOSITORIO_ARCHIVOS.Add(repositorio);
                    }

                    await SaveAllAsync();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ObtenerException(ex, data[1]);
                }
            }
        }

        public async Task CrearObservacionPorEntidad(GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES dataActual)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await Update(dataActual);
                    await new EstupefacienteRepository().ChangeNarcoticStateIfAllVerifications(dataActual.id_antecedente);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ObtenerException(ex, dataActual);
                }
            }
        }
    }
}
