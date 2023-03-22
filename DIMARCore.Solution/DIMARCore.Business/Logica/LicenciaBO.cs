using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Config;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.UI;

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
        public LicenciaDTO GetlicenciaId(int id)
        {
            return new LicenciaRepository().GetlicenciaId(id);
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
                var validate = await repo.GetWithCondition(x => x.radicado == data.radicado);
                if (validate == null)
                {
                    //completa la informacion de la licencia
                    var claimCapitania = ClaimsHelper.ObtenerCapitaniaUsuario();
                    data.id_capitania = claimCapitania;
                    data.id_estado_licencia = (int)EnumEstados.PROCESO;
                    data.activo = Constantes.ACTIVO;

                    if (data.Observacion != null)
                    {
                        if (data.Observacion.Archivo != null)
                        {
                            string path = $"{Constantes.CARPETA_MODULO_LICENCIAS}\\{Constantes.CARPETA_OBSERVACIONES}";
                            respuesta = Reutilizables.GuardarArchivo(data.Observacion.Archivo, rutaInicial, path);
                            if (respuesta.Estado)
                            {
                                var archivo = (Archivo)respuesta.Data;
                                if (archivo != null)
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
                                }
                            }
                        }
                        else
                        {
                            await repo.CrearLicencia(data);
                        }

                    }
                    else
                    {
                        await repo.Create(data);
                    }

                    respuesta.StatusCode = HttpStatusCode.Created;
                    respuesta.Mensaje = "Creado satisfactoriamente.";
                    respuesta.Estado = true;

                }
                else
                {
                    respuesta.StatusCode = HttpStatusCode.Conflict;
                    respuesta.Mensaje = "la licencia con el radicado ya se encuentra creado.";
                    respuesta.Estado = false;
                }
                return respuesta;
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

            using (var repo = new LicenciaRepository())
            {
                var validate = await repo.GetWithCondition(x => x.id_licencia == data.id_licencia);
                if (validate != null)
                {

                    var claimCapitania = ClaimsHelper.ObtenerCapitaniaUsuario();
                    validate.id_capitania = claimCapitania;
                    validate.id_estado_licencia = data.id_estado_licencia != null ? data.id_estado_licencia : validate.id_estado_licencia;
                    validate.id_cargo_licencia = data.id_cargo_licencia != null ? data.id_cargo_licencia : validate.id_cargo_licencia;
                    validate.fecha_expedicion = data.fecha_expedicion != null ? data.fecha_expedicion : validate.fecha_expedicion;
                    validate.fecha_vencimiento = data.fecha_vencimiento != null ? data.fecha_vencimiento : validate.fecha_vencimiento;
                    validate.id_capitania_firmante = data.id_capitania_firmante != null ? data.id_capitania_firmante : validate.id_capitania_firmante;
                    validate.activo = data.activo == null ? validate.activo : data.activo;

                    if (data.Observacion != null)
                    {
                        validate.Observacion = data.Observacion;
                        if (data.Observacion.Archivo != null)
                        {
                            string path = $"{Constantes.CARPETA_MODULO_LICENCIAS}\\{Constantes.CARPETA_OBSERVACIONES}";
                            respuesta = Reutilizables.GuardarArchivo(data.Observacion.Archivo, rutaInicial, path);
                            if (respuesta.Estado)
                            {
                                var archivo = (Archivo)respuesta.Data;
                                if (archivo != null)
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
                                    await new LicenciaRepository().ActualizarLicencia(validate, repositorio);
                                }
                            }
                        }
                        else
                        {
                            await new LicenciaRepository().ActualizarLicencia(validate);
                        }

                    }
                    else
                    {
                        await repo.Update(validate);
                    }

                    ValidarEstadoUsuarioLicenciaTitulo(validate.id_gentemar);


                    respuesta.StatusCode = HttpStatusCode.Created;
                    respuesta.Mensaje = "Licencia actualizada";
                    respuesta.Estado = true;
                }
                else
                {
                    respuesta.StatusCode = HttpStatusCode.Conflict;
                    respuesta.Mensaje = "la licencia ha modificar no se encontro.";
                    respuesta.Estado = false;
                }
                return respuesta;
            }

        }
        /// <summary>
        /// Cambio de estado de las licencias de una pesona en especifico
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        public async Task CambiarEstadoLicencia(long idUsuario, int estado)
        {
            using (var repo = new LicenciaRepository())
            {
                var validate = await repo.GetAllWithConditionAsync(x => x.id_gentemar == idUsuario);
                if (validate.Count() > 0)
                {
                    foreach (GENTEMAR_LICENCIAS item in validate)
                    {
                        item.id_estado_licencia = estado;
                        await new LicenciaRepository().ActualizarLicencia(item);
                    }
                }
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
                if (data.Where(x => x.IdEstadoLicencia == (int)EnumEstados.VIGENTE).ToList().Count > 0)
                {
                    await repo.cambioEstadoIdUsuario(idUsuario, (int)EstadoGenteMarEnum.ACTIVO);
                    return;

                }
                if (data.Where(x => x.IdEstadoLicencia == (int)EnumEstados.PROCESO).ToList().Count > 0)
                {
                    await repo.cambioEstadoIdUsuario(idUsuario, (int)EstadoGenteMarEnum.ENPROCESO);
                    return;

                }
                if (data.Where(x => x.IdEstadoLicencia == (int)EnumEstados.NOVIGENTE).ToList().Count > 0)
                {
                    await repo.cambioEstadoIdUsuario(idUsuario, (int)EstadoGenteMarEnum.INACTIVO);
                    return;

                }
                if (data.Where(x => x.IdEstadoLicencia == (int)EnumEstados.CANCELADO).ToList().Count > 0)
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

                    var validate = await repo.GetAllWithConditionAsync(x => x.fecha_vencimiento <= DateTime.Now && x.id_estado_licencia != (int)EnumEstados.NOVIGENTE && x.id_estado_licencia != (int)EnumEstados.CANCELADO);
                    if (validate.Count() > 0)
                    {
                        foreach (GENTEMAR_LICENCIAS item in validate)
                        {
                            item.id_estado_licencia = (int)EnumEstados.NOVIGENTE;
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
    }

}
