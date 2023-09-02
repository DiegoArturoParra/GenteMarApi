using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
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
    public class EstupefacienteBO
    {
        public IQueryable<ListadoEstupefacientesDTO> GetEstupefacientesByFiltro(EstupefacientesFilter filtro)
        {
            return new EstupefacienteRepository().GetAntecedentesByFiltro(filtro);
        }


        public async Task<Respuesta> CrearAsync(GENTEMAR_ANTECEDENTES entidad, GENTEMAR_ANTECEDENTES_DATOSBASICOS datosBasicos)
        {
            using (var repo = new EstupefacienteRepository())
            {
                datosBasicos.identificacion = Reutilizables.ConvertirStringApuntosDeMil(datosBasicos.identificacion);
                long idGenteDeMarEstupefaciente = await new EstupefacienteDatosBasicosRepository().GetAntecedenteDatosBasicosId(datosBasicos.identificacion);

                if (idGenteDeMarEstupefaciente > 0)
                {
                    datosBasicos.IsExist = true;
                    datosBasicos.id_gentemar_antecedente = idGenteDeMarEstupefaciente;
                    entidad.id_gentemar_antecedente = idGenteDeMarEstupefaciente;
                }
                var fechaRadicado = await ValidacionesDeNegocio(entidad, datosBasicos, false);
                entidad.fecha_sgdea = entidad.fecha_sgdea ?? fechaRadicado;
                entidad.fecha_solicitud_sede_central = DateTime.Now;
                if (datosBasicos.IsExist)
                {
                    await repo.CreateWithPersonGenteMar(entidad, datosBasicos);
                    return Responses.SetCreatedResponse(entidad);
                }
                await repo.Create(entidad);
                return Responses.SetCreatedResponse(entidad);

            }

        }

        private async Task<DateTime?> ValidacionesDeNegocio(GENTEMAR_ANTECEDENTES entidad, GENTEMAR_ANTECEDENTES_DATOSBASICOS datosBasicos, bool isEdit)
        {
            DateTime? dateRadicado = null;

            if (!isEdit && datosBasicos.IsExist)
            {
                var validacion = await new EstupefacienteRepository().ContieneAntecedenteVigentePorEstado(datosBasicos.id_gentemar_antecedente);
                if (validacion.isContains)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($@"La persona {validacion.NombreCompleto} con No de Identificación 
                                                            {validacion.Identificacion} contiene un estupefaciente en estado {validacion.Estado} y no es posible agregar uno nuevo."));
            }
            if (!isEdit)
            {
                var existeRadicado = await new EstupefacienteRepository().AnyWithCondition(x => x.numero_sgdea.Equals(entidad.numero_sgdea));
                if (existeRadicado)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"Ya está registrado el radicado {entidad.numero_sgdea}"));

                if (entidad.id_tipo_tramite == (int)TipoTramiteEstupefacienteEnum.GENTES)
                {
                    var existeRadicadoSGDEA = await new SGDEARepository().FechaRadicado(entidad.numero_sgdea);
                    if (!existeRadicadoSGDEA.Radicado.HasValue)
                        throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"El radicado {entidad.numero_sgdea} no está registrado en el SGDEA."));

                    dateRadicado = existeRadicadoSGDEA.Fecha;
                }
            }
            bool existeTramite = await new TramiteEstupefacienteRepository().AnyWithCondition(x => x.id_tipo_tramite == entidad.id_tipo_tramite);
            if (!existeTramite)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El tramite no existe."));

            bool existeEstado = await new EstadoEstupefacienteRepository().AnyWithCondition(x => x.id_estado_antecedente == entidad.id_estado_antecedente);
            if (!existeEstado)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El estado del estupefaciente no existe."));

            return dateRadicado;

        }

        public async Task<Respuesta> EditBulk(EditBulkEstupefacientesDTO estupefacientesBulk)
        {
            using (var repository = new EstupefacienteRepository())
            {
                var existeEstado = await new EstadoEstupefacienteRepository()
                    .AnyWithCondition(x => x.id_estado_antecedente == estupefacientesBulk.EstadoAntecedenteId);

                if (!existeEstado)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No existe el estado."));

                var data = await repository.GetAllWithConditionAsync(x => estupefacientesBulk.EstupefacientesId.Select(id => id).Contains(x.id_antecedente));
                if (data.Count() != estupefacientesBulk.EstupefacientesId.Count())
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encontraron todos los registros a actualizar."));
                try
                {
                    var newData = data.Select(entidad =>
                    {
                        // Realizar los cambios necesarios en cada objeto de la lista
                        // por ejemplo, modificar una propiedad
                        entidad.id_estado_antecedente = estupefacientesBulk.EstadoAntecedenteId;
                        entidad.fecha_aprobacion = estupefacientesBulk.FechaAprobacion;
                        entidad.fecha_vigencia = estupefacientesBulk.FechaVigencia;
                        return entidad;
                    }).ToList();
                    await repository.EditBulk(newData, 100);

                    return Responses.SetUpdatedResponse();
                }
                catch (Exception ex)
                {
                    return Responses.SetInternalServerErrorResponse(ex);
                }
            }

        }

        public async Task<Respuesta> EditarAsync(GENTEMAR_ANTECEDENTES entidad, GENTEMAR_ANTECEDENTES_DATOSBASICOS datosBasicos, string pathActual)
        {
            datosBasicos.id_gentemar_antecedente = entidad.id_gentemar_antecedente;
            Respuesta respuesta = new Respuesta();
            if (entidad.Observacion == null)
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "La observación es requerida.");

            using (var estupefacienteRepository = new EstupefacienteRepository())
            {
                await ValidacionesDeNegocio(entidad, datosBasicos, true);

                GENTEMAR_ANTECEDENTES estupefacienteActual = await estupefacienteRepository.GetById(entidad.id_antecedente);
                if (estupefacienteActual == null)
                    throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encontro el estupefaciente.");

                GENTEMAR_ANTECEDENTES_DATOSBASICOS estupefacienteDatosBasicosActual = await new EstupefacienteDatosBasicosRepository()
                    .GetById(datosBasicos.id_gentemar_antecedente);

                if (estupefacienteDatosBasicosActual == null)
                    throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encontro la persona de gente de mar.");

                try
                {
                    estupefacienteActual.id_capitania = entidad.id_capitania;
                    estupefacienteActual.fecha_vigencia = entidad.fecha_vigencia;
                    estupefacienteActual.fecha_aprobacion = entidad.fecha_aprobacion;
                    estupefacienteActual.id_tipo_tramite = entidad.id_tipo_tramite;
                    estupefacienteActual.id_estado_antecedente = entidad.id_estado_antecedente;
                    estupefacienteActual.Observacion = entidad.Observacion;
                    if (entidad.Observacion.Archivo != null)
                    {
                        string path = $"{Constantes.CARPETA_MODULO_ESTUPEFACIENTES}\\{Constantes.CARPETA_OBSERVACIONES}";
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
                                    NombreModulo = Constantes.CARPETA_MODULO_ESTUPEFACIENTES,
                                    TipoDocumento = Constantes.CARPETA_OBSERVACIONES,
                                    FechaCargue = DateTime.Now,
                                    NombreArchivo = entidad.Observacion.Archivo.FileName,
                                    RutaArchivo = entidad.Observacion.ruta_archivo,
                                    Nombre = Path.GetFileNameWithoutExtension(archivo.NombreArchivo),
                                    DescripcionDocumento = Reutilizables.DescribirDocumento(archivo.NombreArchivo),
                                };
                                await estupefacienteRepository.ActualizarAntecedenteWithGenteDeMar(estupefacienteActual, estupefacienteDatosBasicosActual, repositorio);
                                respuesta = Responses.SetUpdatedResponse();
                            }
                        }
                    }
                    else
                    {
                        await estupefacienteRepository.ActualizarAntecedenteWithGenteDeMar(estupefacienteActual, estupefacienteDatosBasicosActual);
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

        public async Task<IEnumerable<decimal>> GetRadicadosLicenciasByDocumento(string identificacion)
        {
            return await new EstupefacienteRepository().GetRadicadosLicenciasByDocumento(identificacion);
        }
        public async Task<IEnumerable<string>> GetRadicadosTitulosByDocumento(string identificacion)
        {
            return await new EstupefacienteRepository().GetRadicadosTitulosByDocumento(identificacion);
        }

        public async Task<Respuesta> GetDatosGenteMarEstupefaciente(string identificacionConPuntos)
        {
            var data = await new EstupefacienteRepository().GetDatosGenteMarEstupefaciente(identificacionConPuntos)
                ?? throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encuentra registrada la persona.");
            data.ExisteEnDatosBasicosEstupefaciente = true;
            return Responses.SetOkResponse(data);
        }

        public async Task<Respuesta> GetDetallePersonaEstupefaciente(long id)
        {
            var data = await new EstupefacienteRepository().GetDetallePersonaEstupefaciente(id);
            return data == null
                ? throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"No existe el registro del estupefaciente.")
                : Responses.SetOkResponse(data);
        }

        public async Task<IEnumerable<EstupefacientesBulkDTO>> GetEstupefacientesSinObservaciones(IList<long> ids)
        {
            using (var repo = new EstupefacienteRepository())
            {
                return await repo.GetEstupefacientesSinObservaciones(ids);
            }
        }
    }
}
