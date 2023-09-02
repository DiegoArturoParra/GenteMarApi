using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Config;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using GenteMarCore.Entities.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class AclaracionEstupefacienteBO
    {
        public async Task<Respuesta> AgregarAclaracionEstupefaciente(AclaracionEditDTO aclaracionEdit, string pathActual)
        {
            if (aclaracionEdit.Archivo == null)
                throw new HttpStatusCodeException(Responses.SetBadRequestResponse("El archivo no puede ir vacio."));

            var expedienteObservacion = await new ExpedienteObservacionEstupefacienteRepository().GetById(aclaracionEdit.ExpedienteObservacionId);
            if (expedienteObservacion == null)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El expediente con observación no existe."));

            var antecedente = await new EstupefacienteRepository().AnyWithCondition(x => x.id_antecedente == aclaracionEdit.AntecedenteId);
            if (!antecedente)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El estupefaciente no existe."));

            using (var aclaracionRepo = new HistorialAclaracionEstupefacienteRepository())
            {
                Archivo archivo = null;
                try
                {
                    string path = $"{Constantes.CARPETA_MODULO_ESTUPEFACIENTES}\\{Constantes.CARPETA_ACLARACION_EXPEDIENTE}";
                    var nombreArchivo = $"{Guid.NewGuid()}_.{aclaracionEdit.Extension}";
                    var response = Reutilizables.GuardarArchivoDeBytes(aclaracionEdit.Archivo, pathActual, path, nombreArchivo);

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
                            RutaArchivo = archivo.PathArchivo,
                        };


                        aclaracionEdit.ObservacionAnterior = new ObservacionAnteriorDTO()
                        {
                            DetalleAnterior = expedienteObservacion.descripcion,
                            VerificacionExitosaAnterior = expedienteObservacion.verificacion_exitosa.Value
                        };

                        var dataAclaracion = new GENTEMAR_HISTORIAL_ACLARACION_ANTECEDENTES()
                        {
                            detalle_aclaracion = aclaracionEdit.DetalleAclaracion,
                            fecha_hora_creacion = DateTime.Now,
                            usuario_creador_registro = ClaimsHelper.GetNombreCompletoUsuario(),
                            id_expediente_observacion = expedienteObservacion.id_expediente_observacion,
                            ruta_archivo = archivo.PathArchivo,
                            detalle_observacion_anterior_json = JsonConvert.SerializeObject(aclaracionEdit.ObservacionAnterior)
                        };

                        expedienteObservacion.descripcion = aclaracionEdit.DetalleObservacionNuevo;
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
                    return Responses.SetInternalServerErrorResponse(ex);
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
                        }
                    }
                }
            }
            return historial;
        }
    }
}
