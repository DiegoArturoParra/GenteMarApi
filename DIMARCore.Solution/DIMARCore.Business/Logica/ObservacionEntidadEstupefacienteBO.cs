using DIMARCore.Business.Helpers;
using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Config;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using GenteMarCore.Entities.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class ObservacionEntidadEstupefacienteBO
    {
        private readonly int _numeroDeLotes;
        public ObservacionEntidadEstupefacienteBO()
        {
            _numeroDeLotes = int.Parse(ConfigurationManager.AppSettings[Constantes.NUMERO_DE_LOTES]);
        }
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
                var responses = await repo.CrearObservacionesEntidadCascade(antecedenteId, data);
                if (responses.Any())
                {
                    _ = new DbLogger().InsertLogsToDatabase(responses);
                }
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

        public async Task<Respuesta> EdicionParcialDeEstupefacientes(EditBulkPartialEstupefacientesDTO observacionDeEstupefacientes, string pathInitial)
        {
            using (var observacionExpedienteRepository = new ExpedienteObservacionEstupefacienteRepository())
            {
                var antecedentes = await new EstupefacienteRepository().GetAllWithConditionAsync
                    (x => observacionDeEstupefacientes.EstupefacientesId.Contains(x.id_antecedente));
                if (!antecedentes.Any())
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"No se encontraron los ids de los estupefacientes."));

                IList<GENTEMAR_ANTECEDENTES> antecedentesList = new List<GENTEMAR_ANTECEDENTES>();


                antecedentesList = antecedentes.Select(entidad =>
                {
                    // Realizar los cambios necesarios en cada objeto de la lista se cambia el estado
                    entidad.fecha_aprobacion = observacionDeEstupefacientes.ObservacionEntidad.FechaAprobacion;
                    entidad.fecha_vigencia = observacionDeEstupefacientes.ObservacionEntidad.FechaVigencia;
                    return entidad;
                }).ToList();

                var expedientes = await observacionExpedienteRepository
                      .GetAllWithConditionAsync(x => observacionDeEstupefacientes.EstupefacientesId.Contains(x.id_antecedente)
                      && x.id_entidad == observacionDeEstupefacientes.ObservacionEntidad.EntidadId
                      && x.id_expediente == observacionDeEstupefacientes.ObservacionEntidad.ExpedienteId
                      && x.id_consolidado == observacionDeEstupefacientes.ConsolidadoId);

                IList<GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES> expedientesList = new List<GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES>();


                expedientesList = expedientes.Select(entidad =>
                {
                    // Realizar los cambios necesarios en cada objeto de la lista se cambia el estado
                    entidad.verificacion_exitosa = observacionDeEstupefacientes.EstadoAntecedenteId == (int)EstadoEstupefacienteEnum.Exitosa;
                    entidad.descripcion_observacion = observacionDeEstupefacientes.ObservacionEntidad.Observacion;
                    entidad.fecha_respuesta_entidad = observacionDeEstupefacientes.ObservacionEntidad.FechaRespuestaEntidad;
                    entidad.descripcion_observacion = Constantes.SIN_OBSERVACION;
                    return entidad;
                }).ToList();
                Archivo archivo = null;
                try
                {

                    string rutaModulo = $"{Constantes.CARPETA_MODULO_ESTUPEFACIENTES}\\{Constantes.CARPETA_ACLARACION_EXPEDIENTE}";
                    var nombreArchivo = $"{Guid.NewGuid()}.{observacionDeEstupefacientes.ObservacionEntidad.Extension}";
                    var respuesta = Reutilizables.GuardarArchivoDeBytes(observacionDeEstupefacientes.ObservacionEntidad.FileBytes, pathInitial, rutaModulo, nombreArchivo);
                    archivo = (Archivo)respuesta.Data;
                    if (archivo != null)
                    {
                        IList<GENTEMAR_HISTORIAL_ACLARACION_ANTECEDENTES> historialAclaracionDeExpedientes = new List<GENTEMAR_HISTORIAL_ACLARACION_ANTECEDENTES>();

                        GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = new GENTEMAR_REPOSITORIO_ARCHIVOS()
                        {
                            IdAplicacion = Constantes.ID_APLICACION,
                            NombreModulo = Constantes.CARPETA_MODULO_ESTUPEFACIENTES,
                            TipoDocumento = Constantes.CARPETA_ACLARACION_EXPEDIENTE,
                            FechaCargue = DateTime.Now,
                            NombreArchivo = nombreArchivo,
                            Nombre = Path.GetFileNameWithoutExtension(archivo.NombreArchivo),
                            RutaArchivo = archivo.PathArchivo,
                            DescripcionDocumento = Reutilizables.DescribirDocumento(Path.GetExtension(archivo.NombreArchivo))
                        };
                        var usuarioCreador = ClaimsHelper.GetNombreCompletoUsuario();
                        foreach (var item in expedientes)
                        {
                            var json = new ObservacionAnteriorDTO()
                            {
                                DetalleAnterior = item.descripcion_observacion,
                                VerificacionExitosaBefore = item.verificacion_exitosa.Value,
                                VerificacionExitosaAfter = item.verificacion_exitosa.Value
                            };
                            var dataAclaracion = new GENTEMAR_HISTORIAL_ACLARACION_ANTECEDENTES()
                            {
                                detalle_aclaracion = Constantes.SIN_ACLARACION,
                                fecha_hora_creacion = DateTime.Now,
                                usuario_creador_registro = usuarioCreador,
                                id_expediente_observacion = item.id_expediente_observacion,
                                ruta_archivo = archivo.PathArchivo,
                                detalle_observacion_anterior_json = JsonConvert.SerializeObject(json)
                            };
                            historialAclaracionDeExpedientes.Add(dataAclaracion);
                        }
                        await observacionExpedienteRepository.EdicionObservacionParcialDeEstupefacientes(antecedentesList, expedientesList,
                            historialAclaracionDeExpedientes, repositorio, _numeroDeLotes);
                    }
                }
                catch (Exception ex)
                {
                    if (archivo != null)
                    {
                        Reutilizables.EliminarArchivo(pathInitial, archivo.PathArchivo);
                    }
                    var response = Responses.SetInternalServerErrorResponse(ex);
                    _ = new DbLogger().InsertLogToDatabase(response);
                    return response;
                }
            }
            return Responses.SetUpdatedResponse();
        }
    }
}
