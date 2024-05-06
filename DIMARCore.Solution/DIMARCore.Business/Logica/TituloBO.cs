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
using System.Web;

namespace DIMARCore.Business.Logica
{
    public class TituloBO : ITitulosBO
    {
        public async Task<Respuesta> CrearAsync(GENTEMAR_TITULOS entidad, string pathActual)
        {
            await ValidacionesDeNegocio(entidad);

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
                if (entidad.Observacion == null)
                {
                    await repo.CrearTitulo(entidad);
                    return Responses.SetCreatedResponse(new { entidad.id_titulo });
                }
                GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = null;
                if (entidad.Observacion != null)
                {
                    if (entidad.Observacion.Archivo != null)
                    {
                        repositorio = SaveFileOfObservation(pathActual, entidad.Observacion.Archivo);
                    }
                }
                try
                {
                    await repo.CrearTitulo(entidad, repositorio);
                    return Responses.SetCreatedResponse(new { entidad.id_titulo });
                }
                catch (Exception ex)
                {
                    if (repositorio != null)
                    {
                        Reutilizables.EliminarArchivo(pathActual, repositorio.RutaArchivo);
                    }
                    throw new HttpStatusCodeException(Responses.SetInternalServerErrorResponse(ex));
                }
            }
        }

        public async Task ExistById(long id)
        {
            using (var repo = new TituloRepository())
            {
                var existe = await repo.AnyWithConditionAsync(x => x.id_titulo == id);
                if (!existe)
                    throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encontro el id del titulo.");
            }
        }

        public async Task<Respuesta> ActualizarAsync(GENTEMAR_TITULOS entidad, string pathActual)
        {
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

                GENTEMAR_TITULOS tituloActual = await repo.GetByIdAsync(entidad.id_titulo);
                if (tituloActual == null)
                    throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encontro el id del titulo.");

                tituloActual.id_capitania_firmante = entidad.id_capitania_firmante;
                tituloActual.id_estado_tramite = entidad.id_estado_tramite;
                tituloActual.fecha_expedicion = entidad.fecha_expedicion;
                tituloActual.fecha_vencimiento = entidad.fecha_vencimiento;
                tituloActual.id_tipo_solicitud = entidad.id_tipo_solicitud;
                tituloActual.Cargos = entidad.Cargos;
                tituloActual.Observacion = entidad.Observacion;

                GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = null;

                if (entidad.Observacion.Archivo != null)
                {
                    repositorio = SaveFileOfObservation(pathActual, entidad.Observacion.Archivo);
                }

                try
                {
                    await repo.ActualizarTitulo(tituloActual, repositorio);
                    return Responses.SetUpdatedResponse(new { tituloActual.id_titulo });
                }
                catch (Exception ex)
                {
                    if (repositorio != null)
                    {
                        Reutilizables.EliminarArchivo(pathActual, repositorio.RutaArchivo);
                    }
                    throw new HttpStatusCodeException(Responses.SetInternalServerErrorResponse(ex));
                }
            }
        }
        private GENTEMAR_REPOSITORIO_ARCHIVOS SaveFileOfObservation(string rutaInicial, HttpPostedFile fileObservation)
        {
            GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = null;
            string path = $"{Constantes.CARPETA_MODULO_TITULOS}\\{Constantes.CARPETA_OBSERVACIONES}";
            var respuesta = Reutilizables.GuardarArchivo(fileObservation, rutaInicial, path);
            if (!respuesta.Estado)
                throw new HttpStatusCodeException(respuesta);

            var archivo = (Archivo)respuesta.Data;
            if (archivo != null)
            {
                repositorio = new GENTEMAR_REPOSITORIO_ARCHIVOS()
                {
                    IdAplicacion = Constantes.ID_APLICACION,
                    NombreModulo = Constantes.CARPETA_MODULO_TITULOS,
                    TipoDocumento = Constantes.CARPETA_OBSERVACIONES,
                    FechaCargue = DateTime.Now,
                    NombreArchivo = fileObservation.FileName,
                    RutaArchivo = archivo.PathArchivo,
                    Nombre = Path.GetFileNameWithoutExtension(archivo.NombreArchivo),
                    DescripcionDocumento = "Observación de titulos.",
                };
            }
            return repositorio;
        }
        private async Task ValidacionesDeNegocio(GENTEMAR_TITULOS entidad, bool isEdit = false)
        {
            decimal radicado = Convert.ToDecimal(entidad.radicado);
            if (!isEdit)
            {
                var existeRadicadoSGDEA = await new SGDEARepository().AnyWithConditionAsync(x => x.radicado.Equals(radicado));
                if (!existeRadicadoSGDEA)
                    throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"No existe el radicado: {radicado} en el SGDEA.");

                var SeRepiteRadicado = await new TituloRepository().AnyWithConditionAsync(x => x.radicado.Equals(entidad.radicado));
                if (SeRepiteRadicado)
                    throw new HttpStatusCodeException(HttpStatusCode.Conflict, $"Ya está en uso el radicado {radicado}.");
            }
            var existRadicadoImpreso = await new SGDEARepository().AnyWithConditionAsync(x => x.radicado.Equals(radicado) && x.estado == Constantes.PREVISTAGENERADA);
            if (existRadicadoImpreso)
            {
                var message = isEdit ? $"No se puede modificar el título con el radicado: {radicado} ya se encuentra generada la prevista." :
                    $"No se puede crear el título con el radicado: {radicado} ya se encuentra generada la prevista.";
                throw new HttpStatusCodeException(HttpStatusCode.Conflict, message);
            }
            var existeTipoRefrendo = await new AplicacionTipoRefrendoRepository().AnyWithConditionAsync(y => y.ID_TIPO_CERTIFICADO == entidad.id_tipo_refrendo);
            if (!existeTipoRefrendo)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No existe el tipo de refrendo.");

            await new DatosBasicosBO().ValidationsStatusPersona(new ParametrosGenteMarDTO { Id = entidad.id_gentemar });
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
                var data = await repositorio.GetByIdAsync(desactivateCargo.TituloReglaCargoId);
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
