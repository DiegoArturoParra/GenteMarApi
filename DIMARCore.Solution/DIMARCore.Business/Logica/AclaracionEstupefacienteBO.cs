using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class AclaracionEstupefacienteBO
    {
        public async Task<Respuesta> CrearAclaracionesPorEntidades(IList<GENTEMAR_ACLARACION_ANTECEDENTES> data, long antecedenteId)
        {
            var existeEstupefaciente = await new EstupefacienteRepository().AnyWithCondition(x => x.id_antecedente == antecedenteId);

            if (!existeEstupefaciente)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"El antecedente no se encuentra registrado."));

            await Validaciones(data, antecedenteId);
            using (var repo = new AclaracionEstupefacienteRepository())
            {
                await repo.CrearAclaracionesCascade(data);
                return Responses.SetCreatedResponse(data);
            }
        }

        private async Task Validaciones(IList<GENTEMAR_ACLARACION_ANTECEDENTES> data, long antecedenteId)
        {
            var count = new EntidadEstupefacienteRepository().GetAllAsQueryable().Where(x => x.activo == true).Count();
            if (count != data.Count)
                throw new HttpStatusCodeException(Responses.SetConflictResponse($"El rango de aclaraciones debe ser: {count}"));


            var duplicates = data.GroupBy(x => x.id_entidad)
                            .SelectMany(g => g.Skip(1))
                            .Distinct()
                            .ToList();

            if (duplicates.Count() > 0)
                throw new HttpStatusCodeException(Responses.SetConflictResponse($"No puede duplicar aclaraciones con la misma entidad."));

            foreach (var item in data)
            {
                using (var repo = new EntidadEstupefacienteRepository())
                {
                    if (!await repo.AnyWithCondition(x => x.id_entidad == item.id_entidad && x.activo == true))
                        throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"La entidad no se encuentra registrada."));

                    item.id_antecedente = antecedenteId;
                }
            }

            var yahayAclaraciones = new AclaracionEstupefacienteRepository().GetAllAsQueryable().Where(x => x.id_antecedente == antecedenteId).Count();
            if (count == yahayAclaraciones)
                throw new HttpStatusCodeException(Responses.SetConflictResponse($"No puede agregar mas aclaraciones al estupefaciente seleccionado."));
        }

        public async Task<Respuesta> EditarAclaracionesPorEntidades(IList<GENTEMAR_ACLARACION_ANTECEDENTES> data, string pathActual,
            GENTEMAR_OBSERVACIONES_ANTECEDENTES observacion)
        {

            var respuesta = new Respuesta();
            var existeEstupefaciente = await new EstupefacienteRepository().AnyWithCondition(x => x.id_antecedente == observacion.id_antecedente);

            if (!existeEstupefaciente)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"El antecedente no se encuentra registrado."));

            await Validaciones(data, observacion.id_antecedente);
            using (var repo = new AclaracionEstupefacienteRepository())
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
                                await repo.EditarAclaracionesCascade(data, observacion, repositorio);
                            }
                        }
                    }
                    else
                    {
                        await repo.EditarAclaracionesCascade(data, observacion);
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

        public async Task<IEnumerable<DetalleAclaracionesEstupefacienteDTO>> GetAclaracionesPorEstupefacienteId(long estupefacienteId)
        {
            return await new AclaracionEstupefacienteRepository().GetAclaracionesPorEstupefacienteId(estupefacienteId);
        }
    }
}
