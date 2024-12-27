using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Config;
using DIMARCore.Utilities.Enums;
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
    public class LicenciaBO
    {
        /// <summary>
        /// Obtener la licencia dado el id del usuario.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objeto licencia dado el id de usuario</returns>
        /// <entidad>GENTEMAR_ACTIVIDAD</entidad>
        /// <tabla>GENTEMAR_ACTIVIDAD</tabla>
        public async Task<IList<LicenciaDTO>> GetlicenciasPorUsuarioId(long id)
        {
            return await new LicenciaRepository().GetlicenciasPorUsuarioId(id);
        }


        /// <summary>
        /// Obtener la licencia dado el id de la licencia.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objeto licencia dado el id de la licencia </returns>
        /// <entidad>GENTEMAR_ACTIVIDAD</entidad>
        /// <tabla>GENTEMAR_ACTIVIDAD</tabla>
        public async Task<LicenciaDTO> GetlicenciaPorIdAsync(int id)
        {
            var data = await new LicenciaRepository().GetlicenciaId(id);
            if (data == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encuentra registrada la licencia.");

            data.MaxDateFechaVencimiento = data.FechaVencimiento.Value.AddMonths(1);
            return data;
        }

        /// <summary>
        /// Obtener la licencia dado el id de la licencia.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objeto licencia dado el id de la licencia </returns>
        public async Task<LicenciaDTO> GetlicenciaIdView(int id)
        {
            return await new LicenciaRepository().GetlicenciaIdView(id);
        }

        /// <summary>
        /// crea un nueva licencia 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Respuesta> CrearLicencia(GENTEMAR_LICENCIAS data, string rutaInicial)
        {
            using (var repo = new LicenciaRepository())
            {

                await ValidarRadicadoYaExisteToCreate(data.radicado);
                await ExisteRadicadoSgdea(data.radicado, data.id_cargo_licencia);
                await ExisteCargoLicencia(data.id_cargo_licencia);
                await ValidarPrevistaImpreso(data.radicado, false);
                await ValidarEstadoPersonaGenteMar(data.id_gentemar, data.id_cargo_licencia);
                //completa la informacion de la licencia
                var claimCapitania = ClaimsHelper.GetCapitaniaUsuario();
                data.id_capitania = claimCapitania;
                data.id_estado_licencia = (int)EstadosTituloLicenciaEnum.PROCESO;
                data.activo = Constantes.ACTIVO;

                if (data.Observacion == null && !data.ListaNaves.Any())
                {
                    await repo.Create(data);
                    return Responses.SetCreatedResponse(data);
                }

                if (data.Observacion == null && data.ListaNaves.Any())
                {
                    await repo.CrearLicencia(data);
                    return Responses.SetCreatedResponse(data);
                }

                GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = null;
                if (data.Observacion != null)
                {
                    if (data.Observacion.Archivo != null)
                    {
                        repositorio = SaveFileOfObservation(rutaInicial, data.Observacion.Archivo);
                    }
                }
                try
                {
                    await repo.CrearLicencia(data, repositorio);
                }
                catch (Exception ex)
                {
                    if (repositorio != null)
                    {
                        Reutilizables.EliminarArchivo(rutaInicial, repositorio.RutaArchivo);
                    }
                    throw new HttpStatusCodeException(Responses.SetInternalServerErrorResponse(ex));
                }
                return Responses.SetCreatedResponse(new { data.id_licencia });
            }
        }


        /// <summary>
        /// modificar un nueva licencia 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Respuesta> ModificarLicencia(GENTEMAR_LICENCIAS data, string rutaInicial)
        {
            if (data.Observacion == null)
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "La observación es requerida.");

            using (var licenciaRepository = new LicenciaRepository())
            {
                var licenciaActual = await licenciaRepository.GetWithConditionAsync(x => x.id_licencia == data.id_licencia)
                    ?? throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encuentra registrada la licencia.");
                if (licenciaActual.radicado != 0)
                {
                    await ValidarRadicadoYaExisteToEdit(data.radicado, licenciaActual.id_licencia);
                    await ExisteRadicadoSgdea(data.radicado, data.id_cargo_licencia);
                }

                await ExisteEstadoTramite(data.id_estado_licencia);
                await ExisteCargoLicencia(data.id_cargo_licencia);
                var claimCapitania = ClaimsHelper.GetCapitaniaUsuario();
                licenciaActual.id_capitania = claimCapitania;
                licenciaActual.id_estado_licencia = data.id_estado_licencia;
                licenciaActual.id_cargo_licencia = data.id_cargo_licencia;
                licenciaActual.fecha_expedicion = data.fecha_expedicion;
                licenciaActual.fecha_vencimiento = data.fecha_vencimiento;
                licenciaActual.id_capitania_firmante = data.id_capitania_firmante;
                licenciaActual.ListaNaves = data.ListaNaves;
                licenciaActual.radicado = data.radicado;
                licenciaActual.Observacion = data.Observacion;
                licenciaActual.id_licencia_titulo = data.id_licencia_titulo;

                GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = null;

                if (data.Observacion.Archivo != null)
                {
                    repositorio = SaveFileOfObservation(rutaInicial, data.Observacion.Archivo);
                }
                try
                {
                    await licenciaRepository.ActualizarLicencia(licenciaActual, repositorio);
                }
                catch (Exception ex)
                {
                    if (repositorio != null)
                    {
                        Reutilizables.EliminarArchivo(rutaInicial, repositorio.RutaArchivo);
                    }
                    throw new HttpStatusCodeException(Responses.SetInternalServerErrorResponse(ex));
                }
                return Responses.SetUpdatedResponse(new { licenciaActual.id_licencia });
            }

        }


        /// <summary>
        /// Cambiar el estado de la licencia
        /// </summary>
        /// <param name="data"></param>
        /// <param name="rutaInicial"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusCodeException"></exception>
        public async Task<Respuesta> CambiarEstadoLicencia(GENTEMAR_LICENCIAS licencia, string rutaInicial)
        {
            if (licencia.Observacion == null)
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "La observación es requerida.");

            using (var licenciaRepository = new LicenciaRepository())
            {
                var licenciaActual = await licenciaRepository.GetWithConditionAsync(x => x.id_licencia == licencia.id_licencia)
                    ?? throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encuentra registrada la licencia.");

                var claimCapitania = ClaimsHelper.GetCapitaniaUsuario();
                licenciaActual.id_capitania = claimCapitania;
                licenciaActual.activo = licencia.activo;
                licenciaActual.Observacion = licencia.Observacion;

                GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = null;

                if (licencia.Observacion.Archivo != null)
                {
                    repositorio = SaveFileOfObservation(rutaInicial, licencia.Observacion.Archivo);
                }
                try
                {
                    await licenciaRepository.ActualizarLicencia(licenciaActual, repositorio);
                }
                catch (Exception ex)
                {
                    if (repositorio != null)
                    {
                        Reutilizables.EliminarArchivo(rutaInicial, repositorio.RutaArchivo);
                    }
                    throw new HttpStatusCodeException(Responses.SetInternalServerErrorResponse(ex));
                }
                return Responses.SetUpdatedResponse(new { licenciaId = licenciaActual.id_licencia, Activo = licencia.activo });
            }

        }

        private GENTEMAR_REPOSITORIO_ARCHIVOS SaveFileOfObservation(string rutaInicial, HttpPostedFile fileObservation)
        {
            GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = null;
            string path = $"{Constantes.CARPETA_MODULO_LICENCIAS}\\{Constantes.CARPETA_OBSERVACIONES}";
            var respuesta = Reutilizables.GuardarArchivo(fileObservation, rutaInicial, path);
            if (!respuesta.Estado)
                throw new HttpStatusCodeException(respuesta);

            var archivo = (Archivo)respuesta.Data;
            if (archivo != null)
            {
                repositorio = new GENTEMAR_REPOSITORIO_ARCHIVOS()
                {
                    IdAplicacion = Constantes.ID_APLICACION,
                    NombreModulo = Constantes.CARPETA_MODULO_LICENCIAS,
                    TipoDocumento = Constantes.CARPETA_OBSERVACIONES,
                    FechaCargue = DateTime.Now,
                    NombreArchivo = fileObservation.FileName,
                    RutaArchivo = archivo.PathArchivo,
                    Nombre = Path.GetFileNameWithoutExtension(archivo.NombreArchivo),
                    DescripcionDocumento = "observación de Licencia.",
                };
            }
            return repositorio;
        }

        private async Task ValidarPrevistaImpreso(decimal radicado, bool isEdit)
        {
            var existRadicadoImpreso = await new SGDEARepository()
                                   .AnyWithConditionAsync(x => x.radicado.Equals(radicado) && x.estado == Constantes.PREVISTAGENERADA);
            if (existRadicadoImpreso)
            {
                var message = isEdit ? $"No se puede modificar la licencia con el radicado: {radicado} ya se encuentra generada la prevista." :
                    $"No se puede crear la licencia con el radicado: {radicado} ya se encuentra generada la prevista.";
                throw new HttpStatusCodeException(HttpStatusCode.Conflict, message);
            }
        }
        private async Task ExisteEstadoTramite(int idEstadoTramite)
        {
            var existEstadoTramite = await new EstadoLicenciaRepository()
                                   .AnyWithConditionAsync(x => x.id_estado_licencias == idEstadoTramite);
            if (!existEstadoTramite)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"No existe el estado de tramite.");
        }

        private async Task ExisteCargoLicencia(int idCargoLicencia)
        {
            var existCargoLicencia = await new CargoLicenciaRepository()
                                   .AnyWithConditionAsync(x => x.id_cargo_licencia == idCargoLicencia);
            if (!existCargoLicencia)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"No existe el cargo licencia.");
        }

        private async Task ValidarRadicadoYaExisteToCreate(decimal radicado)
        {
            var SeRepiteRadicado = await new LicenciaRepository().AnyWithConditionAsync(x => x.radicado.Equals(radicado) && x.id_estado_licencia != (int)EstadosTituloLicenciaEnum.SUSPENDIDA && x.activo);
            if (SeRepiteRadicado)
                throw new HttpStatusCodeException(HttpStatusCode.Conflict, $"Ya está en uso el radicado: {radicado}.");
        }

        private async Task ValidarRadicadoYaExisteToEdit(decimal radicado, long licenciaId)
        {
            var SeRepiteRadicado = await new LicenciaRepository().AnyWithConditionAsync(x => x.radicado.Equals(radicado) && x.id_licencia != licenciaId && x.id_estado_licencia != (int)EstadosTituloLicenciaEnum.SUSPENDIDA && x.activo);
            if (SeRepiteRadicado)
                throw new HttpStatusCodeException(HttpStatusCode.Conflict, $"Ya está en uso el radicado: {radicado}.");
        }

        private async Task ValidarEstadoPersonaGenteMar(long idGenteMar, long idCargaLicencia)
        {
            var licencia = await new CargoLicenciaBO().GetCargoLicenciaIdAsync(idCargaLicencia);
            if (licencia.IdTipoLicencia != (int)TipoLicenciaEnum.PERITOS && licencia.IdTipoLicencia != (int)TipoLicenciaEnum.PILOTOS && licencia.IdTipoLicencia != (int)TipoLicenciaEnum.ENTRENAMIENTO)
                await new DatosBasicosBO().GetDatosBasicosValidacionEstadoyVcitePersona(new ParametrosGenteMarDTO { Id = idGenteMar });
        }
        private async Task ExisteRadicadoSgdea(decimal radicado, int cargoLicenciaId)
        {
            var tipoLicencia = await new CargoLicenciaRepository().GetCargoLicenciaIdAsync(cargoLicenciaId);
            if (tipoLicencia.IdTipoLicencia == (int)TipoLicenciaEnum.ENTRENAMIENTO)
                return;
            await new SgdeaBO().ExisteRadicado(radicado);
        }
    }

}
