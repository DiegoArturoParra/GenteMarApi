
using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class ObservacionesBO
    {
        #region Listar de las observaciones segun el modulo correspondiente
        /// <summary>
        /// Metodo que obtiene las observaciones segun el modulo correspondiente
        /// </summary>
        /// <param name="id">parametro id</param>
        /// <param name="rutaInicial">parametro que deja la ruta inicial para repositorio de archivos</param>
        /// <param name="modulo">modulo correspondiente al proyecto(datosBasicos, titulos, licencias, Antecedentes)</param>
        /// <returns></returns>
        /// <exception cref="HttpStatusCodeException">retorna exception modulo no corresponde a ninguno.</exception>
        public async Task<IEnumerable<ObservacionDTO>> GetObservacionesId(long id, string rutaInicial, ObservacionEnum modulo)
        {
            IEnumerable<ObservacionDTO> observaciones;
            switch (modulo)
            {
                case ObservacionEnum.DatosBasicos:
                    observaciones = await new ObservacionesDatosBasicosRepository().GetObservacionesId(id);
                    break;
                case ObservacionEnum.Titulos:
                    observaciones = await new ObservacionesTitulosRepository().GetObservacionesId(id);
                    break;
                case ObservacionEnum.Licencias:
                    observaciones = await new ObservacionesLicenciasRepository().GetObservacionesId(id);
                    break;
                case ObservacionEnum.Estupefacientes:
                    observaciones = await new ObservacionesEstupefacienteRepository().GetObservacionesId(id);
                    break;
                default:
                    throw new HttpStatusCodeException(HttpStatusCode.BadRequest, @"El modulo no corresponde al numero digitado.DatosBasicos = 1, 
                                                                                   Titulos = 2, Licencias = 3, Estupefacientes = 4");
            }

            if (observaciones.Count() > 0)
            {
                foreach (var item in observaciones)
                {
                    if (item.ArchivoBase != null)
                    {
                        if (!string.IsNullOrWhiteSpace(item.ArchivoBase.RutaArchivo))
                        {
                            var rutaArchivo = $@"{rutaInicial}\{item.ArchivoBase.RutaArchivo}";
                            string archivoBase64 = null;
                            var respuestaBuscarArchivo = Reutilizables.DescargarArchivo(rutaArchivo, out archivoBase64);
                            if (respuestaBuscarArchivo != null && respuestaBuscarArchivo.Estado && !string.IsNullOrEmpty(archivoBase64))
                            {
                                item.ArchivoBase.ArchivoBase64 = archivoBase64;
                            }
                        }
                    }
                }
            }
            return observaciones;
        }
        #endregion

        #region Crear Observaciones Datos Basicos
        public async Task<Respuesta> CrearObservacionesDatosBasicos(GENTEMAR_OBSERVACIONES_DATOSBASICOS datos, string rutaInicial)
        {
            Respuesta respuesta = new Respuesta();
            var valid = await new DatosBasicosRepository().AnyWithCondition(x => x.id_gentemar == datos.id_gentemar);
            if (!valid)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("Id gente de mar no encontrado."));

            using (var repo = new ObservacionesDatosBasicosRepository())
            {
                try
                {
                    if (datos.Archivo != null)
                    {
                        string path = $"{Constantes.CARPETA_MODULO_DATOSBASICOS}\\{Constantes.CARPETA_OBSERVACIONES}";
                        respuesta = Reutilizables.GuardarArchivo(datos.Archivo, rutaInicial, path);
                        if (respuesta.Estado)
                        {
                            var archivo = (Archivo)respuesta.Data;
                            if (archivo != null)
                            {
                                datos.ruta_archivo = archivo.PathArchivo;

                                GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = new GENTEMAR_REPOSITORIO_ARCHIVOS()
                                {
                                    IdAplicacion = Constantes.ID_APLICACION,
                                    NombreModulo = Constantes.CARPETA_MODULO_DATOSBASICOS,
                                    TipoDocumento = Constantes.CARPETA_OBSERVACIONES,
                                    FechaCargue = DateTime.Now,
                                    NombreArchivo = archivo.NombreArchivo,
                                    RutaArchivo = datos.ruta_archivo,
                                    Nombre = Path.GetFileNameWithoutExtension(archivo.NombreArchivo),
                                    DescripcionDocumento = Reutilizables.DescribirDocumento(Path.GetExtension(archivo.NombreArchivo))
                                };
                                await repo.CrearObservacion(datos, repositorio);
                                respuesta = Responses.SetCreatedResponse(repositorio);
                            }
                        }
                    }
                    else
                    {
                        await repo.Create(datos);
                        respuesta = Responses.SetCreatedResponse();
                    }
                }
                catch (Exception ex)
                {
                    var archivo = (Archivo)respuesta.Data;
                    if (archivo != null)
                    {
                        Reutilizables.EliminarArchivo(rutaInicial, archivo.PathArchivo);
                    }
                    respuesta = Responses.SetInternalServerErrorResponse(ex);
                }
            }
            return respuesta;
        }
        #endregion

        #region Crear Observaciones titulos
        public async Task<Respuesta> CrearObservacionesTitulos(GENTEMAR_OBSERVACIONES_TITULOS datos, string rutaInicial)
        {
            Respuesta respuesta = new Respuesta();
            var valid = await new TituloRepository().AnyWithCondition(x => x.id_titulo == datos.id_titulo);
            if (!valid)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("Titulo no encontrado."));
            using (var repo = new ObservacionesTitulosRepository())
            {
                try
                {
                    if (datos.Archivo != null)
                    {
                        string path = $"{Constantes.CARPETA_MODULO_TITULOS}\\{Constantes.CARPETA_OBSERVACIONES}";
                        respuesta = Reutilizables.GuardarArchivo(datos.Archivo, rutaInicial, path);
                        if (respuesta.Estado)
                        {
                            var archivo = (Archivo)respuesta.Data;
                            if (archivo != null)
                            {
                                datos.ruta_archivo = archivo.PathArchivo;

                                GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = new GENTEMAR_REPOSITORIO_ARCHIVOS()
                                {
                                    IdAplicacion = Constantes.ID_APLICACION,
                                    NombreModulo = Constantes.CARPETA_MODULO_TITULOS,
                                    TipoDocumento = Constantes.CARPETA_OBSERVACIONES,
                                    FechaCargue = DateTime.Now,
                                    NombreArchivo = archivo.NombreArchivo,
                                    RutaArchivo = datos.ruta_archivo,
                                    Nombre = Path.GetFileNameWithoutExtension(archivo.NombreArchivo),
                                    DescripcionDocumento = Reutilizables.DescribirDocumento(Path.GetExtension(archivo.NombreArchivo))
                                };
                                await repo.CrearObservacion(datos, repositorio);
                                respuesta = Responses.SetCreatedResponse(repositorio);
                            }
                        }
                    }
                    else
                    {
                        await repo.Create(datos);
                        respuesta = Responses.SetCreatedResponse();
                    }
                }
                catch (Exception ex)
                {
                    var archivo = (Archivo)respuesta.Data;
                    if (archivo != null)
                    {
                        Reutilizables.EliminarArchivo(rutaInicial, archivo.PathArchivo);
                    }
                    respuesta = Responses.SetInternalServerErrorResponse(ex);
                }
            }
            return respuesta;

        }
        #endregion

        #region Crear Observaciones Licencias
        public async Task<Respuesta> CrearObservacionesLicencias(GENTEMAR_OBSERVACIONES_LICENCIAS datos, string rutaInicial)
        {
            Respuesta respuesta = new Respuesta();
            var valid = await new LicenciaRepository().AnyWithCondition(x => x.id_licencia == datos.id_licencia);
            if (!valid)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("Licencia no encontrada."));

            using (var repo = new ObservacionesLicenciasRepository())
            {
                try
                {
                    if (datos.Archivo != null)
                    {
                        string path = $"{Constantes.CARPETA_MODULO_LICENCIAS}\\{Constantes.CARPETA_OBSERVACIONES}";
                        respuesta = Reutilizables.GuardarArchivo(datos.Archivo, rutaInicial, path);
                        if (respuesta.Estado)
                        {
                            var archivo = (Archivo)respuesta.Data;
                            if (archivo != null)
                            {
                                datos.ruta_archivo = archivo.PathArchivo;

                                GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = new GENTEMAR_REPOSITORIO_ARCHIVOS()
                                {
                                    IdAplicacion = Constantes.ID_APLICACION,
                                    NombreModulo = Constantes.CARPETA_MODULO_LICENCIAS,
                                    TipoDocumento = Constantes.CARPETA_OBSERVACIONES,
                                    FechaCargue = DateTime.Now,
                                    NombreArchivo = archivo.NombreArchivo,
                                    RutaArchivo = datos.ruta_archivo,
                                    Nombre = Path.GetFileNameWithoutExtension(archivo.NombreArchivo),
                                    DescripcionDocumento = Reutilizables.DescribirDocumento(Path.GetExtension(archivo.NombreArchivo))
                                };
                                await repo.CrearObservacion(datos, repositorio);
                                respuesta = Responses.SetCreatedResponse(repositorio);
                            }
                        }
                    }
                    else
                    {
                        await repo.Create(datos);
                        respuesta = Responses.SetCreatedResponse();
                    }
                }
                catch (Exception ex)
                {
                    var archivo = (Archivo)respuesta.Data;
                    if (archivo != null)
                    {
                        Reutilizables.EliminarArchivo(rutaInicial, archivo.PathArchivo);
                    }
                    respuesta = Responses.SetInternalServerErrorResponse(ex);
                }
            }
            return respuesta;
        }
        #endregion

        #region Crear Observaciones De estupefacientes
        public async Task<Respuesta> CrearObservacionesEstupefacientes(GENTEMAR_OBSERVACIONES_ANTECEDENTES datos, string rutaInicial)
        {
            Respuesta respuesta = new Respuesta();
            var valid = await new EstupefacienteRepository().AnyWithCondition(x => x.id_antecedente == datos.id_antecedente);
            if (!valid)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("Estupefaciente no encontrado."));

            using (var repo = new ObservacionesEstupefacienteRepository())
            {
                try
                {
                    if (datos.Archivo != null)
                    {
                        string path = $"{Constantes.CARPETA_MODULO_ESTUPEFACIENTES}\\{Constantes.CARPETA_OBSERVACIONES}";
                        respuesta = Reutilizables.GuardarArchivo(datos.Archivo, rutaInicial, path);
                        if (respuesta.Estado)
                        {
                            var archivo = (Archivo)respuesta.Data;
                            if (archivo != null)
                            {
                                datos.ruta_archivo = archivo.PathArchivo;

                                GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = new GENTEMAR_REPOSITORIO_ARCHIVOS()
                                {
                                    IdAplicacion = Constantes.ID_APLICACION,
                                    NombreModulo = Constantes.CARPETA_MODULO_ESTUPEFACIENTES,
                                    TipoDocumento = Constantes.CARPETA_OBSERVACIONES,
                                    FechaCargue = DateTime.Now,
                                    NombreArchivo = archivo.NombreArchivo,
                                    RutaArchivo = datos.ruta_archivo,
                                    Nombre = Path.GetFileNameWithoutExtension(archivo.NombreArchivo),
                                    DescripcionDocumento = Reutilizables.DescribirDocumento(Path.GetExtension(archivo.NombreArchivo))
                                };
                                await repo.CrearObservacion(datos, repositorio);
                                respuesta = Responses.SetCreatedResponse(repositorio);
                            }
                        }
                    }
                    else
                    {
                        await repo.Create(datos);
                        respuesta = Responses.SetCreatedResponse();
                    }
                }
                catch (Exception ex)
                {
                    var archivo = (Archivo)respuesta.Data;
                    if (archivo != null)
                    {
                        Reutilizables.EliminarArchivo(rutaInicial, archivo.PathArchivo);
                    }
                    respuesta = Responses.SetInternalServerErrorResponse(ex);
                }
            }
            return respuesta;
        }
        #endregion

    }
}
