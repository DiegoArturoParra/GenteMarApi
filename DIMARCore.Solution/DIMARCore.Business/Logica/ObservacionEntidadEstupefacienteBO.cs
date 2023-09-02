using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using DIMARCore.Utilities.Middleware;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class ObservacionEntidadEstupefacienteBO
    {
        #region creación y edición de una aclaración
        public async Task<Respuesta> CrearObservacionPorEntidad(GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES data)
        {
            await Validaciones(data);
            using (var repo = new ObservacionEntidadEstupefacienteRepository())
            {
                var dataActual = await repo.GetWithCondition(x => x.id_entidad == data.id_entidad && x.id_antecedente == data.id_antecedente);
                if (dataActual == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"El antecedente no tiene un consolidado y expediente aún, debe tener uno."));

                dataActual.verificacion_exitosa = data.verificacion_exitosa;
                dataActual.descripcion = data.descripcion;
                dataActual.fecha_respuesta_entidad = data.fecha_respuesta_entidad;

                await repo.Update(dataActual);
                return Responses.SetUpdatedResponse(dataActual);
            }
        }

        private async Task Validaciones(GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES data, bool isEdit = false)
        {
            var existeEstupefaciente = await new EstupefacienteRepository().AnyWithCondition(x => x.id_antecedente == data.id_antecedente);

            if (!existeEstupefaciente)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"El antecedente no se encuentra registrado."));

            var existeEntidad = await new EntidadEstupefacienteRepository().AnyWithCondition(x => x.id_entidad == data.id_entidad && x.activo == true);
            if (!existeEntidad)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"La entidad no se encuentra registrada."));

            var existeRegistroEntidadEnAclaracion = await new ObservacionEntidadEstupefacienteRepository().AnyWithCondition(x => x.id_entidad == data.id_entidad &&
            x.descripcion.Length > 0 && x.fecha_respuesta_entidad != null && x.id_antecedente == data.id_antecedente);

            if (existeRegistroEntidadEnAclaracion)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"Ya se encuentra un registro con la entidad asignada."));
        }

        #endregion
        #region Creación y edición masiva de aclaraciones
        public async Task<Respuesta> CrearObservacionesEntidad(IList<GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES> data, long antecedenteId)
        {
            var existeEstupefaciente = await new EstupefacienteRepository().AnyWithCondition(x => x.id_antecedente == antecedenteId);

            if (!existeEstupefaciente)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"El antecedente no se encuentra registrado."));

            await ValidacionesMasivas(data, antecedenteId);
            using (var repo = new ObservacionEntidadEstupefacienteRepository())
            {
                await repo.CrearObservacionesEntidadCascade(data);
                return Responses.SetCreatedResponse(data);
            }
        }

        public async Task<Respuesta> EditarObservacionesEntidad(IList<GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES> data, string pathActual,
            GENTEMAR_OBSERVACIONES_ANTECEDENTES observacion)
        {

            var respuesta = new Respuesta();
            var existeEstupefaciente = await new EstupefacienteRepository().AnyWithCondition(x => x.id_antecedente == observacion.id_antecedente);

            if (!existeEstupefaciente)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"El antecedente no se encuentra registrado."));

            await ValidacionesMasivas(data, observacion.id_antecedente, true);
            using (var repo = new ObservacionEntidadEstupefacienteRepository())
            {
                try
                {
                    if (observacion.Archivo != null)
                    {
                        string path = $"{Constantes.CARPETA_MODULO_ESTUPEFACIENTES}\\{Constantes.CARPETA_OBSERVACIONES}";
                        respuesta = Reutilizables.GuardarArchivo(observacion.Archivo, pathActual, path);
                        if (respuesta.Estado)
                        {
                            var archivo = (Archivo)respuesta.Data;
                            if (archivo != null)
                            {
                                observacion.ruta_archivo = archivo.PathArchivo;

                                GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = new GENTEMAR_REPOSITORIO_ARCHIVOS()
                                {
                                    IdAplicacion = Constantes.ID_APLICACION,
                                    NombreModulo = Constantes.CARPETA_MODULO_ESTUPEFACIENTES,
                                    TipoDocumento = Constantes.CARPETA_OBSERVACIONES,
                                    FechaCargue = DateTime.Now,
                                    NombreArchivo = observacion.Archivo.FileName,
                                    RutaArchivo = observacion.ruta_archivo,
                                    Nombre = Path.GetFileNameWithoutExtension(archivo.NombreArchivo),
                                    DescripcionDocumento = "observación de titulos.",
                                };
                                await repo.EditarObservacionesEntidadCascade(data, observacion, repositorio);
                            }
                        }
                    }
                    else
                    {
                        await repo.EditarObservacionesEntidadCascade(data, observacion);
                    }
                }
                catch (Exception ex)
                {
                    var archivo = (Archivo)respuesta.Data;
                    if (archivo != null)
                    {
                        Reutilizables.EliminarArchivo(pathActual, archivo.PathArchivo);
                    }
                    respuesta = Responses.SetInternalServerErrorResponse(ex);
                }
                object obj = new { Aclaraciones = data, Observacion = observacion };
                return Responses.SetUpdatedResponse(obj);
            }
        }

        private async Task ValidacionesMasivas(IList<GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES> data, long antecedenteId, bool isEdit = false)
        {
            var count = new EntidadEstupefacienteRepository().GetAllAsQueryable().Where(x => x.activo == true).Count();
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
                using (var repo = new EntidadEstupefacienteRepository())
                {
                    if (!await repo.AnyWithCondition(x => x.id_entidad == item.id_entidad && x.activo == true))
                        throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"La entidad no se encuentra registrada."));

                    item.id_antecedente = antecedenteId;
                }
            }
            if (!isEdit)
            {
                var yahayAclaraciones = new ObservacionEntidadEstupefacienteRepository().GetAllAsQueryable().Where(x => x.id_antecedente == antecedenteId).Count();
                if (count == yahayAclaraciones)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"No puede agregar mas observaciones al estupefaciente seleccionado."));
            }
        }

        #endregion

        public async Task<IEnumerable<DetalleExpedienteObservacionEstupefacienteDTO>> GetObservacionesEntidadPorEstupefacienteId(long estupefacienteId)
        {
            return await new ObservacionEntidadEstupefacienteRepository().GetObservacionesEntidadPorEstupefacienteId(estupefacienteId);
        }


    }
}
