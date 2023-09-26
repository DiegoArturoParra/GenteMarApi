using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class ObservacionEntidadEstupefacienteBO
    {
        #region agregar observación a un expediente de estupefaciente por entidad
        public async Task<Respuesta> CrearObservacionPorEntidad(GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES data)
        {
            await ValidationsIsExistData(data);
            using (var repo = new ObservacionEntidadEstupefacienteRepository())
            {
                var dataActual = await repo.GetWithCondition(x => x.id_entidad == data.id_entidad && x.id_antecedente == data.id_antecedente)
                    ?? throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"El antecedente no tiene un consolidado y expediente aún, debe tener uno."));

                dataActual.verificacion_exitosa = data.verificacion_exitosa;
                dataActual.descripcion_observacion = data.descripcion_observacion;
                dataActual.fecha_respuesta_entidad = data.fecha_respuesta_entidad;
                await repo.CrearObservacionPorEntidad(dataActual);
                return Responses.SetUpdatedResponse();
            }
        }

        private async Task ValidationsIsExistData(GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES data, bool isEdit = false)
        {
            var existeEstupefaciente = await new EstupefacienteRepository().AnyWithCondition(x => x.id_antecedente == data.id_antecedente);

            if (!existeEstupefaciente)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"El antecedente no se encuentra registrado."));

            var existeEntidad = await new EntidadEstupefacienteRepository().AnyWithCondition(x => x.id_entidad == data.id_entidad && x.activo == true);
            if (!existeEntidad)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"La entidad no se encuentra registrada."));

            var existeRegistroEntidadEnAclaracion = await new ObservacionEntidadEstupefacienteRepository().AnyWithCondition(x => x.id_entidad == data.id_entidad &&
            x.descripcion_observacion.Length > 0 && x.fecha_respuesta_entidad != null && x.id_antecedente == data.id_antecedente);

            if (existeRegistroEntidadEnAclaracion)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"Ya se encuentra un registro con la entidad asignada."));
        }

        #endregion

        #region Creación y edición masiva de observaciones
        public async Task<Respuesta> CrearObservacionesEntidad(IList<GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES> data, long antecedenteId)
        {
            var existeEstupefaciente = await new EstupefacienteRepository().AnyWithCondition(x => x.id_antecedente == antecedenteId);

            if (!existeEstupefaciente)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"El antecedente no se encuentra registrado."));

            await ValidacionesMasivas(data, antecedenteId);
            using (var repo = new ObservacionEntidadEstupefacienteRepository())
            {
                await repo.CrearObservacionesEntidadCascade(antecedenteId, data);
                return Responses.SetCreatedResponse();
            }
        }

        private async Task ValidacionesMasivas(IList<GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES> data, long antecedenteId)
        {
            var repoEntidadEstupefaciente = new EntidadEstupefacienteRepository();
            var repoObservacionEntidadEstupefaciente = new ObservacionEntidadEstupefacienteRepository();
            var count = repoEntidadEstupefaciente.GetAllAsQueryable().Where(x => x.activo == true).Count();
            if (count != data.Count)
                throw new HttpStatusCodeException(Responses.SetConflictResponse($"El rango de observaciones debe ser: {count}, ya que existen {count} entidades."));


            var duplicates = data.GroupBy(x => x.id_entidad)
                            .SelectMany(g => g.Skip(1))
                            .Distinct()
                            .ToList();

            if (duplicates.Count() > 0)
                throw new HttpStatusCodeException(Responses.SetConflictResponse($"No puede duplicar observaciones con la misma entidad."));

            foreach (var item in data)
            {
                if (!await repoEntidadEstupefaciente.AnyWithCondition(x => x.id_entidad == item.id_entidad && x.activo == true))
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"La entidad no se encuentra registrada."));

                var dataActual = await repoObservacionEntidadEstupefaciente.GetWithCondition(x => x.id_entidad == item.id_entidad
                                                                                             && x.id_antecedente == antecedenteId)
                     ?? throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"El antecedente no tiene un consolidado y expediente aún, debe tener uno."));

                item.id_antecedente = antecedenteId;
            }
        }
        #endregion

        public async Task<IEnumerable<DetalleExpedienteObservacionEstupefacienteDTO>> GetObservacionesEntidadPorEstupefacienteId(long estupefacienteId)
        {
            return await new ObservacionEntidadEstupefacienteRepository().GetObservacionesEntidadPorEstupefacienteId(estupefacienteId);
        }


    }
}
