using DIMARCore.Business.Helpers;
using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using GenteMarCore.Entities.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class AclaracionEstupefacienteBO
    {
        public async Task<Respuesta> AgregarAclaracionEstupefaciente(AclaracionEditDTO aclaracionEdit, string pathActual)
        {
            if (aclaracionEdit.FileBytes == null)
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "El archivo no puede ir vacio.");

            var expedienteObservacion = await new ExpedienteObservacionEstupefacienteRepository().GetByIdAsync(aclaracionEdit.ExpedienteObservacionId);
            if (expedienteObservacion == null)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El expediente con observación no existe."));

            var antecedente = await new EstupefacienteRepository().AnyWithConditionAsync(x => x.id_antecedente == aclaracionEdit.AntecedenteId);
            if (!antecedente)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El estupefaciente no existe."));

            using (var aclaracionRepo = new HistorialAclaracionEstupefacienteRepository())
            {
                Archivo archivo = null;
                try
                {
                    string path = $"{Constantes.CARPETA_MODULO_ESTUPEFACIENTES}\\{Constantes.CARPETA_ACLARACION_EXPEDIENTE}";
                    var nombreArchivo = $"{Guid.NewGuid()}.{aclaracionEdit.Extension}";
                    var response = Reutilizables.GuardarArchivoDeBytes(aclaracionEdit.FileBytes, pathActual, path, nombreArchivo);
                    archivo = (Archivo)response.Data;
                    if (archivo != null)
                    {
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


                        aclaracionEdit.ObservacionAnterior = new ObservacionAnteriorDTO()
                        {
                            DetalleAnterior = expedienteObservacion.descripcion_observacion,
                            VerificacionExitosaBefore = expedienteObservacion.verificacion_exitosa.Value,
                            VerificacionExitosaAfter = aclaracionEdit.VerificacionExitosa
                        };

                        var dataAclaracion = new GENTEMAR_HISTORIAL_ACLARACION_ANTECEDENTES()
                        {
                            detalle_aclaracion = aclaracionEdit.DetalleAclaracion,
                            id_expediente_observacion = expedienteObservacion.id_expediente_observacion,
                            ruta_archivo = archivo.PathArchivo,
                            detalle_observacion_anterior_json = JsonConvert.SerializeObject(aclaracionEdit.ObservacionAnterior)
                        };

                        expedienteObservacion.descripcion_observacion = aclaracionEdit.DetalleObservacionNuevo;
                        expedienteObservacion.verificacion_exitosa = aclaracionEdit.VerificacionExitosa;
                        await aclaracionRepo.AgregarAclaracionPorExpedienteObservacion(dataAclaracion, expedienteObservacion, repositorio);
                    }
                }
                catch (Exception ex)
                {
                    if (archivo != null)
                    {
                        Reutilizables.EliminarArchivo(pathActual, archivo.PathArchivo);
                    }
                    var response = Responses.SetInternalServerErrorResponse(ex);
                    _ = new DbLoggerHelper().InsertLogToDatabase(response);
                    return response;
                }
                return Responses.SetCreatedResponse(null, "La aclaración se agregó satisfactoriamente.");
            }
        }

        public async Task<IEnumerable<HistorialAclaracionDTO>> GetHistorialPorEstupefacienteId(long id, string pathActual)
        {
            var historial = await new HistorialAclaracionEstupefacienteRepository().GetHistorialPorEstupefacienteId(id);
            if (historial.Any())
            {
                foreach (var item in historial)
                {
                    if (item.ArchivoBase != null)
                    {
                        if (!string.IsNullOrWhiteSpace(item.ArchivoBase.RutaArchivo))
                        {
                            var rutaArchivo = $@"{pathActual}\{item.ArchivoBase.RutaArchivo}";
                            var respuestaBuscarArchivo = Reutilizables.DescargarArchivo(rutaArchivo, out string archivoBase64);
                            if (respuestaBuscarArchivo != null && respuestaBuscarArchivo.Estado && !string.IsNullOrEmpty(archivoBase64))
                            {
                                item.ArchivoBase.ArchivoBase64 = archivoBase64;
                            }
                            else
                            {
                                _ = new DbLoggerHelper().InsertLogToDatabase(respuestaBuscarArchivo);
                            }
                        }
                    }
                }
            }
            return historial;
        }
    }
}
