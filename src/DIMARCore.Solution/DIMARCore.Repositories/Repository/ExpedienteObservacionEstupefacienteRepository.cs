using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class ExpedienteObservacionEstupefacienteRepository : GenericRepository<GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES>
    {
        public async Task EdicionObservacionParcialDeEstupefacientes(IList<GENTEMAR_ANTECEDENTES> antecedentes,
            IList<GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES> expedientes,
            IList<GENTEMAR_HISTORIAL_ACLARACION_ANTECEDENTES> historialAclaracionDeExpedientes,
            GENTEMAR_REPOSITORIO_ARCHIVOS repositorio, int numeroDeLotes)
        {

            try
            {
                BeginTransaction();
                _context.GENTEMAR_HISTORIAL_ACLARACION_ANTECEDENTES.AddRange(historialAclaracionDeExpedientes);
                await SaveAllAsync();

                if (repositorio != null)
                {
                    var idsArray = historialAclaracionDeExpedientes.Select(y => y.id_aclaracion).ToArray();
                    repositorio.IdModulo = string.Join(",", idsArray);
                    repositorio.DescripcionDocumento = "Se guarda un unico archivo para los expedientes que se aclararon con la entidad.";
                    _context.GENTEMAR_REPOSITORIO_ARCHIVOS.Add(repositorio);
                }

                foreach (var item in antecedentes)
                {
                    _context.Entry(item).State = EntityState.Modified;
                }

                for (int i = 0; i < expedientes.Count(); i++)
                {
                    _context.Entry(expedientes[i]).State = EntityState.Modified;
                    if ((i + 1) % numeroDeLotes == 0 || i == expedientes.Count - 1)
                    {
                        await SaveAllAsync();
                    }
                }
                var isChange = await new EstupefacienteRepository().ChangeNarcoticsStateIfAllVerificationsAreTrue(antecedentes);
                if (isChange)
                {
                    await SaveAllAsync();
                }
                CommitTransaction();
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                ObtenerException(ex, expedientes[0]);
            }
        }

        public async Task EdicionObservacionesMasivaDeEstupefacientes(IList<GENTEMAR_ANTECEDENTES> antecedentes,
            IList<GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES> expedientes,
            IList<GENTEMAR_HISTORIAL_ACLARACION_ANTECEDENTES> historialAclaracionDeExpedientes,
            IList<GENTEMAR_REPOSITORIO_ARCHIVOS> repositorios, IList<ObservacionesEntidadDTO> observacionesPorEntidad, int numeroDeLotes)
        {

            try
            {
                BeginTransaction();
                _context.GENTEMAR_HISTORIAL_ACLARACION_ANTECEDENTES.AddRange(historialAclaracionDeExpedientes);
                await SaveAllAsync();

                foreach (var item in observacionesPorEntidad)
                {
                    var historicoPorExpediente = expedientes.Where(x => x.id_expediente == item.ExpedienteId).Select(x => x.id_expediente_observacion).ToList();
                    var idsArray = historialAclaracionDeExpedientes.Where(y => historicoPorExpediente.Contains(y.id_expediente_observacion))
                        .Select(y => y.id_aclaracion).ToArray();

                    var repositorio = repositorios.Where(x => x.IdExpedienteObservacion == item.ExpedienteId).FirstOrDefault();
                    if (repositorio != null)
                    {
                        repositorio.IdModulo = string.Join(",", idsArray);
                        repositorio.DescripcionDocumento = "Se guarda un unico archivo para los expedientes que se aclararon con la entidad.";
                        _context.GENTEMAR_REPOSITORIO_ARCHIVOS.Add(repositorio);
                    }
                }

                foreach (var item in antecedentes)
                {
                    _context.Entry(item).State = EntityState.Modified;
                }

                for (int i = 0; i < expedientes.Count(); i++)
                {
                    _context.Entry(expedientes[i]).State = EntityState.Modified;
                    if ((i + 1) % numeroDeLotes == 0 || i == expedientes.Count - 1)
                    {
                        await SaveAllAsync();
                    }
                }
                var isChange = await new EstupefacienteRepository().ChangeNarcoticsStateIfAllVerificationsAreTrue(antecedentes);
                if (isChange)
                {
                    await SaveAllAsync();
                }
                CommitTransaction();
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                ObtenerException(ex, expedientes[0]);
            }
        }
    }
}
