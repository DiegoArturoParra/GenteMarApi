using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class DatosBasicosBO
    {
        /// <summary>
        ///  Metodo que crea un usuario con susu datos basicos 
        /// </summary>
        /// <param name="entidad"></param>
        /// <param name="file"></param>
        /// <returns> Respuesta </returns>
        public async Task<Respuesta> CrearAsync(GENTEMAR_DATOSBASICOS entidad, string rutaInicial)
        {
            Respuesta respuesta = new Respuesta();
            FotografiaBO archivos = new FotografiaBO();
            entidad.documento_identificacion = Reutilizables.ConvertirStringApuntosDeMil(entidad.documento_identificacion);
            using (var repo = new DatosBasicosRepository())
            {
                var validate = await repo.AnyWithCondition(x => x.documento_identificacion.Equals(entidad.documento_identificacion));
                if (validate)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse($@"El usuario con # de identificación: 
                                                                                      {entidad.documento_identificacion} ya se encuentra registrado."));
                try
                {
                    entidad.id_estado = (int)EstadoGenteMarEnum.ENPROCESO;
                    string path = $"{Constantes.CARPETA_MODULO_DATOSBASICOS}\\{Constantes.CARPETA_IMAGENES}";
                    var nombreArchivo = $"{entidad.id_gentemar}_{Guid.NewGuid()}_{entidad.Archivo.FileName}";
                    respuesta = Reutilizables.GuardarArchivo(entidad.Archivo, rutaInicial, path, nombreArchivo);
                    if (respuesta.Estado)
                    {
                        var archivo = (Archivo)respuesta.Data;
                        if (archivo != null)
                        {
                            GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = new GENTEMAR_REPOSITORIO_ARCHIVOS()
                            {
                                IdAplicacion = Constantes.ID_APLICACION,
                                NombreModulo = Constantes.CARPETA_MODULO_DATOSBASICOS,
                                TipoDocumento = Constantes.CARPETA_IMAGENES,
                                FechaCargue = DateTime.Now,
                                NombreArchivo = entidad.Archivo.FileName,
                                RutaArchivo = archivo.PathArchivo,
                            };
                            await repo.CrearDatosBasicos(entidad, repositorio);
                            respuesta.StatusCode = HttpStatusCode.Created;
                            respuesta.Mensaje = "Usuario registrado.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    respuesta = Responses.SetInternalServerErrorResponse(ex);
                }
            }
            return respuesta;
        }
        /// <summary>
        /// metodo que actualiza los datos basicos de una persona 
        /// </summary>
        /// <param name="entidad"></param>
        /// <param name="file"></param>
        /// <returns>Respuesta</returns>
        public async Task<Respuesta> ActualizarAsync(GENTEMAR_DATOSBASICOS entidad, string rutaInicial)
        {
            Respuesta respuesta = new Respuesta();
            FotografiaBO archivos = new FotografiaBO();
            entidad.documento_identificacion = Reutilizables.ConvertirStringApuntosDeMil(entidad.documento_identificacion);
            using (var repo = new DatosBasicosRepository())
            {
                var existeDocumento = await repo.AnyWithCondition(x => x.documento_identificacion.Equals(entidad.documento_identificacion)
                && x.id_gentemar != entidad.id_gentemar);
                if (existeDocumento)
                    throw new HttpStatusCodeException(HttpStatusCode.Conflict, "Ya se encuentra registrada una persona con el documento digitado.");
                try
                {
                    var validate = await repo.GetWithCondition(x => x.id_gentemar == entidad.id_gentemar);
                    if (validate != null)
                    {
                        if (entidad.Archivo != null)
                        {
                            string path = $"{Constantes.CARPETA_MODULO_DATOSBASICOS}\\{Constantes.CARPETA_IMAGENES}";
                            var nombreArchivo = $"{entidad.id_gentemar}_{Guid.NewGuid()}_{entidad.Archivo.FileName}";
                            respuesta = Reutilizables.GuardarArchivo(entidad.Archivo, rutaInicial, path, nombreArchivo);
                            if (respuesta.Estado)
                            {
                                var archivo = (Archivo)respuesta.Data;
                                if (archivo != null)
                                {
                                    GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = new GENTEMAR_REPOSITORIO_ARCHIVOS()
                                    {
                                        IdAplicacion = Constantes.ID_APLICACION,
                                        NombreModulo = Constantes.CARPETA_MODULO_DATOSBASICOS,
                                        TipoDocumento = Constantes.CARPETA_IMAGENES,
                                        FechaCargue = DateTime.Now,
                                        NombreArchivo = entidad.Archivo.FileName,
                                        RutaArchivo = archivo.PathArchivo,
                                    };
                                    var reemplazo = GENTEMAR_DATOSBASICOS.UpdateId(validate.id_gentemar, validate.id_estado, validate.fecha_hora_creacion,
                                        validate.usuario_creador_registro, entidad);
                                    await repo.ActualizarDatosBasicos(validate, reemplazo, repositorio);
                                    respuesta.StatusCode = System.Net.HttpStatusCode.OK;
                                    respuesta.Mensaje = "Usuario Actualizado.";
                                }
                            }
                        }
                        else
                        {
                            var reemplazo = GENTEMAR_DATOSBASICOS.UpdateId(validate.id_gentemar, validate.id_estado, validate.fecha_hora_creacion,
                                        validate.usuario_creador_registro, entidad);
                            await repo.ActualizarDatosBasicos(validate, reemplazo, null);
                            respuesta.StatusCode = System.Net.HttpStatusCode.OK;
                            respuesta.Mensaje = "Usuario Actualizado.";
                        }
                    }
                    else
                    {
                        respuesta.MensajeIngles = "User not found";
                        respuesta.StatusCode = HttpStatusCode.Conflict;
                        respuesta.Mensaje = "Usuario no encontrado";
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
        /// <summary>
        /// cambia el estado del usuario 
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        public async Task<Respuesta> ChangeStatus(GENTEMAR_DATOSBASICOS datos, string rutaInicial)
        {
            Respuesta respuesta = new Respuesta();
            using (var repo = new DatosBasicosRepository())
            {
                var data = await repo.GetWithCondition(x => x.id_gentemar.Equals(datos.id_gentemar));
                if (data != null)
                {
                    data.id_estado = datos.id_estado;
                    data.observacion = datos.observacion;
                    var obser = await new ObservacionesBO().CrearObservacionesDatosBasicos(datos.observacion, rutaInicial);
                    if (obser.Estado)
                    {
                        if (datos.id_estado == (int)EstadoGenteMarEnum.FALLECIDO || datos.id_estado == (int)EstadoGenteMarEnum.INACTIVO)
                        {
                            var licencia = new LicenciaBO();
                            await licencia.CambiarEstadoLicencia(data.id_gentemar, (int)EnumEstados.CANCELADO);
                            var titulo = new TituloBO();
                            await titulo.CambiarEstadoTitulos(data.id_gentemar, (int)EnumEstados.CANCELADO);
                        }
                        await repo.ActualizarDatosBasicosSinFoto(data);
                        respuesta.StatusCode = System.Net.HttpStatusCode.Created;
                        respuesta.Mensaje = "Usuario Actualizado.";
                    }
                    else
                    {
                        return obser;
                    }
                }
                else
                {
                    respuesta.MensajeIngles = "User not found";
                    respuesta.StatusCode = HttpStatusCode.Conflict;
                    respuesta.Mensaje = "Usuario no encontrado";
                }
            }
            return respuesta;
        }
        /// <summary>
        /// Metodo que obtiene un usuario en especifico por id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DatosBasicosDTO</returns>
        public DatosBasicosDTO GetDatosBasicosId(long id, string rutaInicial)
        {
            var data = new DatosBasicosRepository().GetDatosBasicosId(id);
            if (data != null)
            {
                if (!string.IsNullOrWhiteSpace(data.UrlArchivo))
                {
                    var rutaArchivo = $@"{rutaInicial}\{data.UrlArchivo}";
                    string archivoBase64 = null;

                    var respuestaBuscarArchivo = Reutilizables.DescargarArchivo(rutaArchivo, out archivoBase64);
                    if (respuestaBuscarArchivo != null && respuestaBuscarArchivo.Estado && !string.IsNullOrEmpty(archivoBase64))
                    {
                        data.FotoBase64 = archivoBase64;
                    }
                }
            }
            return data;
        }

        public IQueryable<ListadoDatosBasicosDTO> GetDatosBasicosQueryable(DatosBasicosQueryFilter filtro)
        {
            var datos = new DatosBasicosRepository().GetDatosBasicosQueryable(filtro);
            return datos;
        }

        public async Task<Respuesta> ExisteById(long id)
        {
            var existe = await new DatosBasicosRepository().ExisteById(id);
            if (!existe)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encuentra registrada la persona."));
            return Responses.SetOkResponse();
        }

        /// <summary>
        /// Obtener la licencia dado el documento del usuario.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objeto licencia dado el id de usaurio</returns>
        /// <entidad>GENTEMAR_ACTIVIDAD</entidad>
        /// <tabla>GENTEMAR_ACTIVIDAD</tabla>
        public LicenciasTitulosDTO GetlicenciaTituloDocumentoUsuario(string documento)
        {

            var dataDocumento = Reutilizables.ConvertirStringApuntosDeMil(documento);
            return new DatosBasicosRepository().GetlicenciaTituloDocumentoUsuario(dataDocumento);
        }

        public async Task<Respuesta> GetPersonaByIdentificacionOrId(ParametrosGenteMarDTO parametrosGenteMar)
        {
            var datos = await new DatosBasicosRepository()
                .GetPersonaByIdentificacionOrId(parametrosGenteMar.IdentificacionConPuntos, parametrosGenteMar.Id);
            if (datos == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encuentra registrada la persona");
            if (!datos.IsCreateTituloOrLicencia && (!parametrosGenteMar.IsEstupefacientes))
                throw new HttpStatusCodeException(HttpStatusCode.Conflict,
                    $"El usuario esta en estado {datos.NombreEstado} No puede generar titulos y licencias de navegación.");
            return Responses.SetOkResponse(datos);
        }

        /// <summary>
        /// cambia el estado del usuario por id 
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        public async Task<Respuesta> cambioEstadoIdUsuario(long idUsuario, int idEstado)
        {
            Respuesta respuesta = new Respuesta();
            using (var repo = new DatosBasicosRepository())
            {
                var data = await repo.GetWithCondition(x => x.id_gentemar.Equals(idUsuario));
                if (data != null)
                {
                    data.id_estado = idEstado;
                    await repo.ActualizarDatosBasicosSinFoto(data);
                }
                else
                {
                    respuesta.MensajeIngles = "User not found";
                    respuesta.StatusCode = HttpStatusCode.Conflict;
                    respuesta.Mensaje = "Usuario no encontrado";
                }
            }
            return respuesta;
        }

    }
}
