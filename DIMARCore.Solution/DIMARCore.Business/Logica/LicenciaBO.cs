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
        public IList<LicenciaDTO> GetlicenciaIdUsuario(long id)
        {
            return new LicenciaRepository().GetlicenciaIdUsuario(id);
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
            var entidad = await new LicenciaRepository().GetById(id);
            await new DatosBasicosBO().ValidationsStatusPersona(new ParametrosGenteMarDTO { Id = entidad.id_gentemar });
            return await new LicenciaRepository().GetlicenciaId(id);
        }



        /// <summary>
        /// crea un nueva licencia 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Respuesta> CrearLicencia(GENTEMAR_LICENCIAS data, string rutaInicial)
        {
            Respuesta respuesta = new Respuesta();
            using (var repo = new LicenciaRepository())
            {
                var validate = await repo.AnyWithCondition(x => x.radicado == data.radicado);
                if (validate)
                    throw new HttpStatusCodeException(HttpStatusCode.Conflict, "Ya se encuentra registrado el radicado.");
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
                if (data.Observacion != null && data.Observacion.Archivo != null)
                {
                    string path = $"{Constantes.CARPETA_MODULO_LICENCIAS}\\{Constantes.CARPETA_OBSERVACIONES}";
                    respuesta = Reutilizables.GuardarArchivo(data.Observacion.Archivo, rutaInicial, path);

                    if (!respuesta.Estado)
                        throw new HttpStatusCodeException(respuesta);

                    var archivo = (Archivo)respuesta.Data;
                    if (archivo != null)
                    {
                        try
                        {
                            data.Observacion.ruta_archivo = archivo.PathArchivo;
                            GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = new GENTEMAR_REPOSITORIO_ARCHIVOS()
                            {
                                IdAplicacion = Constantes.ID_APLICACION,
                                NombreModulo = Constantes.CARPETA_MODULO_LICENCIAS,
                                TipoDocumento = Constantes.CARPETA_OBSERVACIONES,
                                FechaCargue = DateTime.Now,
                                NombreArchivo = data.Observacion.Archivo.FileName,
                                RutaArchivo = data.Observacion.ruta_archivo,
                                Nombre = Path.GetFileNameWithoutExtension(archivo.NombreArchivo),
                                DescripcionDocumento = "observación de Licencia.",
                            };
                            await repo.CrearLicencia(data, repositorio);
                            return Responses.SetCreatedResponse(data);
                        }
                        catch (Exception ex)
                        {
                            Reutilizables.EliminarArchivo(rutaInicial, archivo.PathArchivo);
                            return Responses.SetInternalServerErrorResponse(ex);
                        }
                    }
                }
                await repo.CrearLicencia(data);
                return Responses.SetCreatedResponse(data);
            }
        }
        /// <summary>
        /// Obtener la licencia dado el id de la licencia.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objeto licencia dado el id de la licencia </returns>
        /// <entidad>GENTEMAR_ACTIVIDAD</entidad>
        /// <tabla>GENTEMAR_ACTIVIDAD</tabla>
        public LicenciaDTO GetlicenciaIdView(int id)
        {
            return new LicenciaRepository().GetlicenciaIdView(id);
        }

        /// <summary>
        /// modificar un nueva licencia 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Respuesta> ModificarLicencia(GENTEMAR_LICENCIAS data, string rutaInicial)
        {
            Respuesta respuesta = new Respuesta();
            await ValidacionesDeNegocio(data, true);

            using (var repo = new LicenciaRepository())
            {
                var licenciaActual = await repo.GetWithCondition(x => x.id_licencia == data.id_licencia)
                    ?? throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encuentra registrada la licencia.");

                var claimCapitania = ClaimsHelper.GetCapitaniaUsuario();
                licenciaActual.id_capitania = claimCapitania;
                licenciaActual.id_estado_licencia = data.id_estado_licencia != null ? data.id_estado_licencia : licenciaActual.id_estado_licencia;
                licenciaActual.id_cargo_licencia = data.id_cargo_licencia != null ? data.id_cargo_licencia : licenciaActual.id_cargo_licencia;
                licenciaActual.fecha_expedicion = data.fecha_expedicion != null ? data.fecha_expedicion : licenciaActual.fecha_expedicion;
                licenciaActual.fecha_vencimiento = data.fecha_vencimiento != null ? data.fecha_vencimiento : licenciaActual.fecha_vencimiento;
                licenciaActual.id_capitania_firmante = data.id_capitania_firmante != null ? data.id_capitania_firmante : licenciaActual.id_capitania_firmante;
                licenciaActual.activo = data.activo == null ? licenciaActual.activo : data.activo;
                licenciaActual.ListaNaves = data.ListaNaves;

                if (data.Observacion == null && !data.ListaNaves.Any())
                {
                    await repo.Update(licenciaActual);
                    return Responses.SetCreatedResponse(data);
                }

                if (data.Observacion == null && data.ListaNaves.Any())
                {
                    await new LicenciaRepository().ActualizarLicencia(licenciaActual);
                    return Responses.SetUpdatedResponse(licenciaActual);
                }

                if (data.Observacion != null && data.Observacion.Archivo != null)

                {
                    licenciaActual.Observacion = data.Observacion;

                    string path = $"{Constantes.CARPETA_MODULO_LICENCIAS}\\{Constantes.CARPETA_OBSERVACIONES}";
                    respuesta = Reutilizables.GuardarArchivo(data.Observacion.Archivo, rutaInicial, path);
                    if (!respuesta.Estado)
                        throw new HttpStatusCodeException(respuesta);

                    var archivo = (Archivo)respuesta.Data;
                    if (archivo != null)
                    {
                        try
                        {
                            data.Observacion.ruta_archivo = archivo.PathArchivo;

                            GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = new GENTEMAR_REPOSITORIO_ARCHIVOS()
                            {
                                IdAplicacion = Constantes.ID_APLICACION,
                                NombreModulo = Constantes.CARPETA_MODULO_LICENCIAS,
                                TipoDocumento = Constantes.CARPETA_OBSERVACIONES,
                                FechaCargue = DateTime.Now,
                                NombreArchivo = data.Observacion.Archivo.FileName,
                                RutaArchivo = data.Observacion.ruta_archivo,
                                Nombre = Path.GetFileNameWithoutExtension(archivo.NombreArchivo),
                                DescripcionDocumento = "observación de Licencia.",
                            };
                            await new LicenciaRepository().ActualizarLicencia(licenciaActual, repositorio);
                            return Responses.SetUpdatedResponse(licenciaActual);
                        }

                        catch (Exception ex)
                        {
                            Reutilizables.EliminarArchivo(rutaInicial, archivo.PathArchivo);
                            return Responses.SetInternalServerErrorResponse(ex);
                        }
                    }
                }
                await repo.ActualizarLicencia(licenciaActual);
                return Responses.SetUpdatedResponse(licenciaActual);
            }

        }
        /// <summary>
        /// cambia el estado del usuario dependiendo el estado de las licencias.
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        public async void ValidarEstadoUsuarioLicenciaTitulo(long idUsuario)
        {
            Respuesta respuesta = new Respuesta();
            var repo = new DatosBasicosBO();
            var data = GetlicenciaIdUsuario(idUsuario).Where(x => x.Activo == Constantes.ACTIVO).ToList();
            if (data != null)
            {
                if (data.Where(x => x.IdEstadoLicencia == (int)EstadosTituloLicenciaEnum.VIGENTE).ToList().Count > 0)
                {
                    await repo.cambioEstadoIdUsuario(idUsuario, (int)EstadoGenteMarEnum.ACTIVO);
                    return;

                }
                if (data.Where(x => x.IdEstadoLicencia == (int)EstadosTituloLicenciaEnum.PROCESO).ToList().Count > 0)
                {
                    await repo.cambioEstadoIdUsuario(idUsuario, (int)EstadoGenteMarEnum.ENPROCESO);
                    return;

                }
                if (data.Where(x => x.IdEstadoLicencia == (int)EstadosTituloLicenciaEnum.NOVIGENTE).ToList().Count > 0)
                {
                    await repo.cambioEstadoIdUsuario(idUsuario, (int)EstadoGenteMarEnum.INACTIVO);
                    return;

                }
                if (data.Where(x => x.IdEstadoLicencia == (int)EstadosTituloLicenciaEnum.CANCELADO).ToList().Count > 0)
                {
                    await repo.cambioEstadoIdUsuario(idUsuario, (int)EstadoGenteMarEnum.INACTIVO);
                    return;

                }
            }
        }

        /// <summary>
        /// Cambio de estado de las licencias cuando su fecha caduca
        /// </summary>
        /// <returns></returns>
        public async Task<RegistrosActualizadosDTO> CambiarEstadoLicenciaFecha()
        {
            RegistrosActualizadosDTO respuesta = new RegistrosActualizadosDTO();
            List<DatosBasicosUsuarioDTO> usaurioActualizado = new List<DatosBasicosUsuarioDTO>();
            using (var repo = new LicenciaRepository())
            {
                using (var repoDatos = new DatosBasicosRepository())
                {

                    var validate = await repo.GetAllWithConditionAsync(x => x.fecha_vencimiento <= DateTime.Now
                    && x.id_estado_licencia != (int)EstadosTituloLicenciaEnum.NOVIGENTE && x.id_estado_licencia != (int)EstadosTituloLicenciaEnum.CANCELADO);
                    if (validate.Count() > 0)
                    {
                        foreach (GENTEMAR_LICENCIAS item in validate)
                        {
                            item.id_estado_licencia = (int)EstadosTituloLicenciaEnum.NOVIGENTE;
                            await new LicenciaRepository().ActualizarLicencia(item);
                            //valida en el estado en que debe quedar el usuario
                            ValidarEstadoUsuarioLicenciaTitulo(item.id_gentemar);
                            var documentoUsuario = await repoDatos.GetPersonaByIdentificacionOrId(null, item.id_gentemar);
                            usaurioActualizado.Add(new DatosBasicosUsuarioDTO()
                            {
                                DocumentoIdentificacion = documentoUsuario.DocumentoIdentificacion,
                                Id = documentoUsuario.Id,
                                NombreCompleto = documentoUsuario.NombreCompleto,
                                NombreEstado = documentoUsuario.NombreEstado,
                                IdLicencia = item.id_licencia

                            });
                        }

                    }
                    respuesta.UsuarioActualizado = usaurioActualizado;
                    respuesta.TotalRegistros = usaurioActualizado.Count();
                    return respuesta;

                }
            }
        }

        private async Task ValidacionesDeNegocio(GENTEMAR_LICENCIAS entidad, bool isEdit = false)
        {

            if (!isEdit)
            {
                var existeRadicadoSGDEA = await new SGDEARepository().AnyWithCondition(x => x.radicado.ToString().Equals(entidad.radicado));
                if (!existeRadicadoSGDEA)
                    throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"No existe el radicado: {entidad.radicado} en el SGDEA.");

                var SeRepiteRadicado = await new LicenciaRepository().AnyWithCondition(x => x.radicado.Equals(entidad.radicado));
                if (SeRepiteRadicado)
                    throw new HttpStatusCodeException(HttpStatusCode.Conflict, $"Ya se uso el radicado: {entidad.radicado}.");

            }

            await new DatosBasicosBO().ValidationsStatusPersona(new ParametrosGenteMarDTO { Id = entidad.id_gentemar });
        }
    }

}
