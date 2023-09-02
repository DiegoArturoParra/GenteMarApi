using DIMARCore.Business.Interfaces;
using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
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
            await ValidacionesDeNegocio(entidad);
            try
            {
                using (var reglaCargoRepo = new ReglaCargoRepository())
                {
                    foreach (var cargo in entidad.Cargos)
                    {
                        cargo.CargoReglaId = await reglaCargoRepo.GetIdReglaCargo(new IdsTablasForaneasDTO(cargo.IdsRelacion.NivelId,
                            cargo.IdsRelacion.ReglaId, cargo.IdsRelacion.CargoId, cargo.IdsRelacion.CapacidadId));
                    }
                }

                using (var repo = new TituloRepository())
                {
                    if (entidad.Observacion != null && entidad.Observacion.Archivo != null)
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
            await ValidacionesDeNegocio(entidad, true);
            using (var reglaCargoRepo = new ReglaCargoRepository())
            {
                foreach (var cargo in entidad.Cargos)
                {
                    cargo.CargoReglaId = await reglaCargoRepo.GetIdReglaCargo(new IdsTablasForaneasDTO(cargo.IdsRelacion.NivelId,
                        cargo.IdsRelacion.ReglaId, cargo.IdsRelacion.CargoId, cargo.IdsRelacion.CapacidadId));
                }

            }
            using (var repo = new TituloRepository())
            {
                GENTEMAR_TITULOS tituloActual = await repo.GetById(entidad.id_titulo);
                if (tituloActual == null)
                    throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encontro el id del titulo.");
                try
                {
                    tituloActual.id_capitania_firmante = entidad.id_capitania_firmante;
                    tituloActual.id_estado_tramite = entidad.id_estado_tramite;
                    tituloActual.fecha_expedicion = entidad.fecha_expedicion;
                    tituloActual.fecha_vencimiento = entidad.fecha_vencimiento;
                    tituloActual.id_tipo_solicitud = entidad.id_tipo_solicitud;
                    tituloActual.Cargos = entidad.Cargos;
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

        private async Task ValidacionesDeNegocio(GENTEMAR_TITULOS entidad, bool isEdit = false)
        {

            if (!isEdit)
            {
                var existeRadicadoSGDEA = await new SGDEARepository().AnyWithCondition(x => x.radicado.ToString().Equals(entidad.radicado));
                if (!existeRadicadoSGDEA)
                    throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"No existe el radicado: {entidad.radicado} en el SGDEA.");

                var SeRepiteRadicado = await new TituloRepository().AnyWithCondition(x => x.radicado.Equals(entidad.radicado));
                if (SeRepiteRadicado)
                    throw new HttpStatusCodeException(HttpStatusCode.Conflict, $"Ya se uso el radicado: {entidad.radicado}.");

            }
            var existeTipoRefrendo = await new ServiciosAplicacionesRepository<APLICACIONES_TIPO_REFRENDO>().AnyWithCondition(y => y.ID_TIPO_CERTIFICADO == entidad.id_tipo_refrendo);
            if (!existeTipoRefrendo)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No existe el tipo de refrendo.");

            await new DatosBasicosBO().GetPersonaByIdentificacionOrId(new ParametrosGenteMarDTO { Id = entidad.id_gentemar });
        }

        public IQueryable<ListadoTituloDTO> GetTitulosQueryable()
        {
            return new TituloRepository().GetTitulosQueryable();
        }

        public async Task<Respuesta> GetTituloById(long id)
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
                return !existe
                    ? throw new HttpStatusCodeException(Responses.SetNotFoundResponse($@"La persona con No de identificación:
                                                      {identificacionConPuntos} no se encuentra registrada."))
                    : Responses.SetOkResponse();
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

        public async Task<Respuesta> DesactivarCargoDelTitulo(DesactivateCargoDTO desactivateCargo)
        {
            using (var repositorio = new TituloReglaCargosRepository())
            {
                var data = await repositorio.GetById(desactivateCargo.TituloReglaCargoId);
                if (data == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"La persona no tiene ese titulo con el cargo relacionado."));

                data.es_eliminado = true;
                await repositorio.DesactivarCargoDelTitulo(data);
                return Responses.SetUpdatedResponse(data);
            }

        }

        /// <summary>
        /// metodo para obtener los datos del usuario y de la licencia para la plantilla
        /// por id de la licencia.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PlantillaTituloDTO> GetPlantillaTitulos(long id)
        {
            var data = await new TituloRepository().GetPlantillaTitulo(id);

            return data;
        }
    }
}
