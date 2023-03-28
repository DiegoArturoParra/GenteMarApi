using DIMARCore.Business.Interfaces;
using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
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
    public class TituloBO : ITitulosBO
    {

        public async Task<Respuesta> CrearAsync(GENTEMAR_TITULOS entidad, string pathActual)
        {
            Respuesta respuesta = new Respuesta();
            await ValidarFormularioAsync(entidad);
            try
            {
                using (var repo = new TituloRepository())
                {
                    if (entidad.Observacion != null)
                    {
                        if (entidad.Observacion.Archivo != null)
                        {
                            string path = $"{Constantes.CARPETA_MODULO_TITULOS}\\{Constantes.CARPETA_OBSERVACIONES}";
                            respuesta = Reutilizables.GuardarArchivo(entidad.Observacion.Archivo, pathActual, path);
                            if (respuesta.Estado)
                            {
                                var archivo = (Archivo)respuesta.Data;
                                if (archivo != null)
                                {
                                    entidad.Observacion.ruta_archivo = archivo.PathArchivo;

                                    GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = new GENTEMAR_REPOSITORIO_ARCHIVOS()
                                    {
                                        IdAplicacion = Constantes.ID_APLICACION,
                                        NombreModulo = Constantes.CARPETA_MODULO_TITULOS,
                                        TipoDocumento = Constantes.CARPETA_OBSERVACIONES,
                                        FechaCargue = DateTime.Now,
                                        NombreArchivo = entidad.Observacion.Archivo.FileName,
                                        RutaArchivo = entidad.Observacion.ruta_archivo,
                                        Nombre = Path.GetFileNameWithoutExtension(archivo.NombreArchivo),
                                        DescripcionDocumento = "observación de titulos.",
                                    };
                                    await repo.CrearTitulo(entidad, repositorio);
                                }
                            }
                        }
                        else
                        {
                            await repo.CrearTitulo(entidad);
                        }
                    }
                    else
                    {
                        await repo.CrearTitulo(entidad);
                    }
                    respuesta = Responses.SetCreatedResponse(entidad);
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
            return respuesta;
        }

        public async Task ExistById(long id)
        {
            using (var repo = new TituloRepository())
            {
                var existe = await repo.AnyWithCondition(x => x.id_titulo == id);
                if (!existe)
                    throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encontro el id del titulo.");
            }
        }

        public async Task<Respuesta> ActualizarAsync(GENTEMAR_TITULOS entidad, string pathActual)
        {
            Respuesta respuesta = new Respuesta();
            if (entidad.Observacion == null)
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "La observación es requerida.");
            await ValidarFormularioAsync(entidad, true);
            using (var repo = new TituloRepository())
            {
                GENTEMAR_TITULOS tituloActual = await repo.GetById(entidad.id_titulo);
                if (tituloActual == null)
                    throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encontro el id del titulo.");
                try
                {
                    tituloActual.id_capitania_firmante = entidad.id_capitania_firmante;
                    tituloActual.id_estado_tramite = entidad.id_estado_tramite;
                    tituloActual.id_cargo_regla = entidad.id_cargo_regla;
                    tituloActual.fecha_expedicion = entidad.fecha_expedicion;
                    tituloActual.fecha_vencimiento = entidad.fecha_vencimiento;
                    tituloActual.id_tipo_solicitud = entidad.id_tipo_solicitud;
                    tituloActual.HabilitacionesId = entidad.HabilitacionesId;
                    tituloActual.FuncionesId = entidad.FuncionesId;
                    tituloActual.Observacion = entidad.Observacion;
                    if (entidad.Observacion.Archivo != null)
                    {
                        string path = $"{Constantes.CARPETA_MODULO_TITULOS}\\{Constantes.CARPETA_OBSERVACIONES}";
                        respuesta = Reutilizables.GuardarArchivo(entidad.Observacion.Archivo, pathActual, path);
                        if (respuesta.Estado)
                        {
                            var archivo = (Archivo)respuesta.Data;
                            if (archivo != null)
                            {
                                entidad.Observacion.ruta_archivo = archivo.PathArchivo;

                                GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = new GENTEMAR_REPOSITORIO_ARCHIVOS()
                                {
                                    IdAplicacion = Constantes.ID_APLICACION,
                                    NombreModulo = Constantes.CARPETA_MODULO_TITULOS,
                                    TipoDocumento = Constantes.CARPETA_OBSERVACIONES,
                                    FechaCargue = DateTime.Now,
                                    NombreArchivo = entidad.Observacion.Archivo.FileName,
                                    RutaArchivo = entidad.Observacion.ruta_archivo,
                                    Nombre = Path.GetFileNameWithoutExtension(archivo.NombreArchivo),
                                    DescripcionDocumento = Reutilizables.DescribirDocumento(archivo.NombreArchivo),
                                };
                                await repo.ActualizarTitulo(tituloActual, repositorio);
                                respuesta = Responses.SetUpdatedResponse(repositorio);
                            }
                        }
                    }
                    else
                    {
                        await repo.ActualizarTitulo(tituloActual);
                        respuesta = Responses.SetUpdatedResponse();
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
            }
            return respuesta;
        }



        private async Task ValidarFormularioAsync(GENTEMAR_TITULOS entidad, bool isEdit = false)
        {
            if (!isEdit)
            {
                var existeRadicadoSGDEA = await new SGDEARepository().AnyWithCondition(x => x.radicado.ToString().Equals(entidad.radicado));
                if (!existeRadicadoSGDEA)
                    throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"No existe el radicado: {entidad.radicado} en el SGDEA");

                var SeRepiteRadicado = await new TituloRepository().AnyWithCondition(x => x.radicado.Equals(entidad.radicado));
                if (SeRepiteRadicado)
                    throw new HttpStatusCodeException(HttpStatusCode.Conflict, $"Ya se uso el radicado: {entidad.radicado}");

            }
            await new DatosBasicosBO().GetPersonaByIdentificacionOrId(new ParametrosGenteMarDTO { Id = entidad.id_gentemar });
        }

        public IQueryable<ListadoTituloDTO> GetTitulosQueryable()
        {
            return new TituloRepository().GetTitulosQueryable();
        }

        public async Task<Respuesta> GetByIdAsync(long id)
        {
            using (var repo = new TituloRepository())
            {
                return Responses.SetOkResponse(await repo.GetTituloById(id));
            }
        }

        public async Task<Respuesta> ExistePersonaByIdentificacion(string identificacionConPuntos)
        {

            using (var repo = new DatosBasicosRepository())
            {
                var existe = await repo.ExistePersonaByIdentificacion(identificacionConPuntos);
                if (!existe)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse($@"La persona con # de identificación:
                                                      {identificacionConPuntos} no se encuentra registrada."));
                return Responses.SetOkResponse();
            }

        }

        public async Task<Respuesta> GetFechasRadioOperadores(long idGenteMar)
        {
            await new DatosBasicosBO().ExisteById(idGenteMar);

            var obj = await new TituloRepository().GetFechasRadioOperadores(idGenteMar);
            if (!obj.HayTitulosPorSeccionPuente)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"La persona no tiene titulos de sección de puente."));
            return Responses.SetOkResponse(obj.fechas);

        }

        public async Task<IEnumerable<ListadoTituloDTO>> GetTitulosFiltro(string identificacionConPuntos, long Id = 0)
        {
            return await new TituloRepository().GetTitulosFiltro(identificacionConPuntos, Id);
        }

        /// <summary>
        /// Cambio de estado de los titulos de una pesona en especifico
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        public async Task CambiarEstadoTitulos(long idUsuario, int estado)
        {
            using (var repo = new TituloRepository())
            {
                var validate = await repo.GetAllWithConditionAsync(x => x.id_gentemar == idUsuario);
                if (validate.Count() > 0)
                {
                    foreach (GENTEMAR_TITULOS item in validate)
                    {
                        item.id_estado_tramite = estado;
                        await new TituloRepository().ActualizarTitulo(item);
                    }
                }
            }
        }
    }
}
