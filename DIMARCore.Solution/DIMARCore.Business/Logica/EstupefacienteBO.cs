using DIMARCore.Business.Helpers;
using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace DIMARCore.Business.Logica
{
    public class EstupefacienteBO
    {
        private readonly int _numeroDeLotes;
        public EstupefacienteBO()
        {
            _numeroDeLotes = int.Parse(ConfigurationManager.AppSettings[Constantes.NUMERO_DE_LOTES]);
        }
        public IQueryable<ListadoEstupefacientesDTO> GetEstupefacientesByFiltro(EstupefacientesFilter filtro)
        {
            var data = new EstupefacienteRepository().GetAntecedentesByFiltro(filtro);
            return data.OrderByDescending(x => x.FechaRegistro);
        }


        public async Task<Respuesta> CrearAsync(GENTEMAR_ANTECEDENTES entidad, GENTEMAR_ANTECEDENTES_DATOSBASICOS datosBasicos)
        {
            using (var repo = new EstupefacienteRepository())
            {
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
                if (!datosBasicos.IsExist)
                {
                    await repo.CreateWithPersonGenteMar(entidad, datosBasicos);
                    return Responses.SetCreatedResponse(entidad);
                }
                await repo.Create(entidad);
                return Responses.SetCreatedResponse(entidad);
            }

        }
        public async Task<Respuesta> EditarAsync(GENTEMAR_ANTECEDENTES entidad, GENTEMAR_ANTECEDENTES_DATOSBASICOS datosBasicos, string pathActual)
        {

            if (entidad.Observacion == null)
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "La observación es requerida.");

            using (var estupefacienteRepository = new EstupefacienteRepository())
            {
                datosBasicos.id_gentemar_antecedente = entidad.id_gentemar_antecedente;
                await ValidacionesDeNegocio(entidad, datosBasicos, true);

                GENTEMAR_ANTECEDENTES estupefacienteActual = await estupefacienteRepository.GetByIdAsync(entidad.id_antecedente);
                if (estupefacienteActual == null)
                    throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encontro el estupefaciente.");

                GENTEMAR_ANTECEDENTES_DATOSBASICOS estupefacienteDatosBasicosActual = await new EstupefacienteDatosBasicosRepository()
                    .GetByIdAsync(datosBasicos.id_gentemar_antecedente);

                if (estupefacienteDatosBasicosActual == null)
                    throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encontro la persona de gente de mar.");

                estupefacienteActual.id_capitania = entidad.id_capitania;
                estupefacienteActual.fecha_vigencia = entidad.fecha_vigencia;
                estupefacienteActual.fecha_aprobacion = entidad.fecha_aprobacion;
                estupefacienteActual.id_tipo_tramite = entidad.id_tipo_tramite;
                estupefacienteActual.id_estado_antecedente = entidad.id_estado_antecedente;
                estupefacienteActual.Observacion = entidad.Observacion;
                estupefacienteActual.fecha_sgdea = entidad.fecha_sgdea;
                estupefacienteActual.numero_sgdea = entidad.numero_sgdea;
                estupefacienteActual.fecha_solicitud_sede_central = entidad.fecha_solicitud_sede_central;

                estupefacienteDatosBasicosActual.identificacion = datosBasicos.identificacion;
                estupefacienteDatosBasicosActual.fecha_nacimiento = datosBasicos.fecha_nacimiento;
                estupefacienteDatosBasicosActual.apellidos = datosBasicos.apellidos;
                estupefacienteDatosBasicosActual.nombres = datosBasicos.nombres;
                estupefacienteDatosBasicosActual.id_tipo_documento = datosBasicos.id_tipo_documento;

                GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = null;

                if (entidad.Observacion.Archivo != null)
                {
                    repositorio = SaveFileOfObservation(pathActual, entidad.Observacion.Archivo);
                }
                try
                {
                    await estupefacienteRepository.ActualizarAntecedenteWithGenteDeMar(estupefacienteActual, estupefacienteDatosBasicosActual, repositorio);
                    return Responses.SetUpdatedResponse(new { estupefacienteActual.id_antecedente });
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
            GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = new GENTEMAR_REPOSITORIO_ARCHIVOS();
            string path = $"{Constantes.CARPETA_MODULO_ESTUPEFACIENTES}\\{Constantes.CARPETA_OBSERVACIONES}";
            var respuesta = Reutilizables.GuardarArchivo(fileObservation, rutaInicial, path);
            if (!respuesta.Estado)
                throw new HttpStatusCodeException(respuesta);

            var archivo = (Archivo)respuesta.Data;
            if (archivo != null)
            {
                repositorio = new GENTEMAR_REPOSITORIO_ARCHIVOS()
                {
                    IdAplicacion = Constantes.ID_APLICACION,
                    NombreModulo = Constantes.CARPETA_MODULO_ESTUPEFACIENTES,
                    TipoDocumento = Constantes.CARPETA_OBSERVACIONES,
                    FechaCargue = DateTime.Now,
                    NombreArchivo = fileObservation.FileName,
                    RutaArchivo = archivo.PathArchivo,
                    Nombre = Path.GetFileNameWithoutExtension(archivo.NombreArchivo),
                    DescripcionDocumento = Reutilizables.DescribirDocumento(archivo.NombreArchivo),
                };
            }
            return repositorio;
        }

        private async Task<DateTime?> ValidacionesDeNegocio(GENTEMAR_ANTECEDENTES entidad, GENTEMAR_ANTECEDENTES_DATOSBASICOS datosBasicos, bool isEdit)
        {
            DateTime? dateRadicado = null;

            if (!isEdit && datosBasicos.IsExist)
                await GetDatosGenteMarEstupefacienteValidations(datosBasicos.identificacion);

            if (!isEdit)
            {
                var existeRadicado = await new EstupefacienteRepository().AnyWithConditionAsync(x => x.numero_sgdea.Equals(entidad.numero_sgdea));
                if (existeRadicado)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"Ya se encuentra registrado el radicado {entidad.numero_sgdea}"));
            }
            else
            {
                var existeRadicado = await new EstupefacienteRepository().AnyWithConditionAsync(x => x.numero_sgdea.Equals(entidad.numero_sgdea)
                && x.id_antecedente != entidad.id_antecedente);
                if (existeRadicado)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"Ya se encuentra registrado el radicado {entidad.numero_sgdea}"));
            }

            if (entidad.id_tipo_tramite == (int)TipoTramiteEstupefacienteEnum.GENTES)
            {
                var existeRadicadoSGDEA = await new SGDEARepository().FechaRadicado(entidad.numero_sgdea);
                if (!existeRadicadoSGDEA.Radicado.HasValue)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"El radicado {entidad.numero_sgdea} no se encuentra registrado en el SGDEA."));

                dateRadicado = existeRadicadoSGDEA.Fecha;
            }

            bool existeTramite = await new TramiteEstupefacienteRepository().AnyWithConditionAsync(x => x.id_tipo_tramite == entidad.id_tipo_tramite);
            if (!existeTramite)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El tramite no existe."));

            bool existeEstado = await new EstadoEstupefacienteRepository().AnyWithConditionAsync(x => x.id_estado_antecedente == entidad.id_estado_antecedente);
            if (!existeEstado)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El estado del estupefaciente no existe."));

            return dateRadicado;

        }

        public async Task ChangeNarcoticStateIfAllVerifications(long idAntecedente)
        {
            using (var estupefacienteRepository = new EstupefacienteRepository())
            {
                await estupefacienteRepository.ChangeNarcoticStateIfAllVerifications(idAntecedente);
            }
        }

        public async Task<IEnumerable<decimal>> GetRadicadosLicenciasByDocumento(string identificacion)
        {
            return await new EstupefacienteRepository().GetRadicadosLicenciasByDocumento(identificacion);
        }
        public async Task<IEnumerable<string>> GetRadicadosTitulosByDocumento(string identificacion)
        {
            return await new EstupefacienteRepository().GetRadicadosTitulosByDocumento(identificacion);
        }

        public async Task<Respuesta> GetDatosGenteMarEstupefacienteValidations(string identificacionConPuntos, bool IsExistInGenteDeMar = false)
        {
            using (var estupefacienteRepository = new EstupefacienteRepository())
            {
                var data = await estupefacienteRepository.GetDatosGenteMarEstupefaciente(identificacionConPuntos);

                if (!IsExistInGenteDeMar && data is null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"No se encontró la persona registrada en VCITE."));

                else if (IsExistInGenteDeMar && data is null)
                {
                    var response = Responses.SetNotFoundResponse($"No se encontró la persona registrada en VCITE.");
                    _ = new DbLoggerHelper().InsertLogToDatabase(response);
                    return response;
                }

                var containsVciteActually = await estupefacienteRepository.ContieneAntecedenteVigentePorEstado(data.Id);
                if (containsVciteActually.isContains)
                {
                    string fechaVigencia = containsVciteActually.FechaVigencia.HasValue
                        ? $" con fecha de vigencia {containsVciteActually.FechaVigencia.Value:dd/MM/yyyy}"
                        : string.Empty;
                    string mensaje = $"La persona {containsVciteActually.NombreCompleto} con No de Identificación " +
                                           $"{containsVciteActually.Identificacion} contiene un estupefaciente en estado " +
                                           $"{containsVciteActually.Estado}{fechaVigencia} y no es posible agregar uno nuevo.";

                    throw new HttpStatusCodeException(Responses.SetConflictResponse(mensaje));
                }

                data.ExisteEnDatosBasicosEstupefaciente = true;
                return Responses.SetOkResponse(data);
            }

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
            if (!ids.Any())
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "Es requerido tener ids de estupefacientes.");

            using (var repo = new EstupefacienteRepository())
            {
                return await repo.GetEstupefacientesSinObservaciones(ids);
            }
        }
    }
}
