using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class AclaracionEstupefacienteRepository : GenericRepository<GENTEMAR_ACLARACION_ANTECEDENTES>
    {
        public async Task<IEnumerable<DetalleAclaracionesEstupefacienteDTO>> GetAclaracionesPorEstupefacienteId(long estupefacienteId)
        {
            var resultado = await (from aclaracion in _context.GENTEMAR_ACLARACION_ANTECEDENTES
                                   join entidad in _context.GENTEMAR_ENTIDAD on
                                   aclaracion.id_entidad equals entidad.id_entidad
                                   join estupefaciente in _context.GENTEMAR_ANTECEDENTES on aclaracion.id_antecedente
                                   equals estupefaciente.id_antecedente
                                   where aclaracion.id_antecedente == estupefacienteId
                                   select new DetalleAclaracionesEstupefacienteDTO
                                   {
                                       AclaracionId = aclaracion.id_aclaracion,
                                       AntecedenteId = aclaracion.id_antecedente,
                                       Descripcion = aclaracion.descripcion,
                                       Entidad = entidad.entidad,
                                       EntidadId = entidad.id_entidad,
                                       FechaRespuestaEntidad = aclaracion.fecha_respuesta_entidad,
                                       Radicado = estupefaciente.numero_sgdea
                                   }
                            ).ToListAsync();
            return resultado;
        }

        public async Task CrearAclaracionesCascade(IList<GENTEMAR_ACLARACION_ANTECEDENTES> data)
        {
            using (_context)
            {
                using (var trassaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.GENTEMAR_ACLARACION_ANTECEDENTES.AddRange(data);
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

        public async Task EditarAclaracionesCascade(IList<GENTEMAR_ACLARACION_ANTECEDENTES> data, GENTEMAR_OBSERVACIONES_ANTECEDENTES observacion,
            GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = null)
        {
            using (_context)
            {
                using (var trassaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.GENTEMAR_ACLARACION_ANTECEDENTES.RemoveRange(_context.GENTEMAR_ACLARACION_ANTECEDENTES
                            .Where(x => x.id_antecedente == observacion.id_antecedente).ToList());

                        _context.GENTEMAR_ACLARACION_ANTECEDENTES.AddRange(data);

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
    }
}
