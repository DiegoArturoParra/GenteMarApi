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
        public async Task<LicenciaDTO> GetlicenciaIdAsync(int id)
        {
            var entidad = await new LicenciaRepository().GetByIdAsync(id);
            await new DatosBasicosBO().ValidationsStatusPersona(new ParametrosGenteMarDTO { Id = entidad.id_gentemar });
            var data = await new LicenciaRepository().GetlicenciaId(id);
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
                await ValidacionesDeNegocio(data);
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

                await ValidacionesDeNegocio(data, true, licenciaActual);
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

        private async Task ValidacionesDeNegocio(GENTEMAR_LICENCIAS entidadNueva, bool isEdit = false, GENTEMAR_LICENCIAS entidadantigua = null)
        {
            var existRadicadoImpreso = await new SGDEARepository()
                                    .AnyWithConditionAsync(x => x.radicado.Equals(entidadNueva.radicado) && x.estado == Constantes.PREVISTAGENERADA);
            if (existRadicadoImpreso)
            {
                var message = isEdit ? $"No se puede modificar la licencia con el radicado: {entidadNueva.radicado} ya se encuentra generada la prevista." :
                    $"No se puede crear la licencia con el radicado: {entidadNueva.radicado} ya se encuentra generada la prevista.";
                throw new HttpStatusCodeException(HttpStatusCode.Conflict, message);
            }

            var tipoLicencia = await new CargoLicenciaRepository().GetCargoLicenciaIdAsync(entidadNueva.id_cargo_licencia);

            if (tipoLicencia.IdTipoLicencia != (int)TipoLicenciaEnum.ENTRENAMIENTO
                || (isEdit && entidadNueva.radicado != entidadantigua.radicado && tipoLicencia.IdTipoLicencia != (int)TipoLicenciaEnum.ENTRENAMIENTO))
            {
                var existeRadicadoSGDEA = await new SGDEARepository()
                                    .AnyWithConditionAsync(x => x.radicado.Equals(entidadNueva.radicado) && x.estado == Constantes.PREVISTATRAMITE);
                if (!existeRadicadoSGDEA)
                    throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"No existe el radicado: {entidadNueva.radicado} en el SGDEA.");

                if (!isEdit || entidadNueva.id_cargo_licencia != entidadantigua.id_cargo_licencia)
                {
                    var SeRepiteRadicado = await new LicenciaRepository().AnyWithConditionAsync(x => x.radicado.Equals(entidadNueva.radicado)
                                                                                    && x.id_licencia != entidadantigua.id_licencia);
                    if (SeRepiteRadicado)
                        throw new HttpStatusCodeException(HttpStatusCode.Conflict, $"Ya está en uso el radicado: {entidadNueva.radicado}.");
                }
            }
            else
            {
                var existeRadicadoSGDEA = await new SGDEARepository().AnyWithConditionAsync(x => x.radicado.Equals(entidadNueva.radicado));
                if (existeRadicadoSGDEA)
                    throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"No se puede asignar el radicado: {entidadNueva.radicado} por que pertenece a un registro en el sgda .");
                if (!isEdit || entidadNueva.id_cargo_licencia != entidadantigua.id_cargo_licencia)
                {
                    var SeRepiteRadicado = await new LicenciaRepository().AnyWithConditionAsync(x => x.radicado.Equals(entidadNueva.radicado));
                    if (SeRepiteRadicado)
                        throw new HttpStatusCodeException(HttpStatusCode.Conflict, $"Ya está en uso el radicado: {entidadNueva.radicado}.");
                }

            }

            var idGenteMar = entidadantigua == null ? entidadNueva.id_gentemar : entidadantigua.id_gentemar;
            await new DatosBasicosBO().ValidationsStatusPersona(new ParametrosGenteMarDTO { Id = idGenteMar });
        }
    }

}
