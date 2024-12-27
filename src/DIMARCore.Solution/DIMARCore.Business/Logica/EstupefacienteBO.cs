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
using System.Web;

namespace DIMARCore.Business.Logica
{
    public class EstupefacienteBO
    {
        private readonly EstupefacienteRepository _estupefacienteRepository;
        private readonly EstupefacienteDatosBasicosRepository _estupefacienteDatosBasicosRepository;
        private readonly DatosBasicosRepository _datosBasicosRepository;
        private readonly TramiteEstupefacienteRepository _tramiteEstupefacienteRepository;
        private readonly EstadoEstupefacienteRepository _estadoEstupefacienteRepository;
        private readonly ObservacionesBO _observacionesBO;

        public EstupefacienteBO()
        {
            _estupefacienteRepository = new EstupefacienteRepository();
            _estupefacienteDatosBasicosRepository = new EstupefacienteDatosBasicosRepository();
            _datosBasicosRepository = new DatosBasicosRepository();
            _tramiteEstupefacienteRepository = new TramiteEstupefacienteRepository();
            _estadoEstupefacienteRepository = new EstadoEstupefacienteRepository();
            _observacionesBO = new ObservacionesBO();
        }

        public EstupefacienteBO(EstupefacienteRepository estupefacienteRepository,
            EstupefacienteDatosBasicosRepository estupefacienteDatosBasicosRepository,
            DatosBasicosRepository datosBasicosRepository,
            TramiteEstupefacienteRepository tramiteEstupefacienteRepository,
            EstadoEstupefacienteRepository estadoEstupefacienteRepository,
            ObservacionesBO observacionesBO)
        {
            _estupefacienteRepository = estupefacienteRepository;
            _estupefacienteDatosBasicosRepository = estupefacienteDatosBasicosRepository;
            _datosBasicosRepository = datosBasicosRepository;
            _tramiteEstupefacienteRepository = tramiteEstupefacienteRepository;
            _estadoEstupefacienteRepository = estadoEstupefacienteRepository;
            _observacionesBO = observacionesBO;
        }
        public IQueryable<ListarEstupefacientesDTO> GetEstupefacientesByFiltro(EstupefacientesFilter filtro)
        {
            if (filtro == null)
            {
                filtro = new EstupefacientesFilter
                {
                    Paginacion = new ParametrosPaginacion()
                };
            }
            else if (filtro.Paginacion == null)
            {
                filtro.Paginacion = new ParametrosPaginacion();
            }
            var data = _estupefacienteRepository.GetEstupefacientesByFiltro(filtro);
            if (!data.Any())
            {

                if (filtro.FechaInicial.HasValue && filtro.FechaFinal.HasValue)
                    throw new HttpStatusCodeException(HttpStatusCode.NotFound,
                                                    $"No se encontraron resultados entre la fecha {filtro.FechaInicial:dd-MM-yyyy} - {filtro.FechaFinal:dd-MM-yyyy}");
                else
                {
                    var fechaActual = DateTime.Now;
                    if (filtro.EstadosId.Any() && string.IsNullOrWhiteSpace(filtro.Identificacion) && string.IsNullOrWhiteSpace(filtro.Radicado) && filtro.ConsolidadoId == 0)
                    {
                        throw new HttpStatusCodeException(HttpStatusCode.NotFound,
                                                         $"No se encontraron resultados de la fecha: {fechaActual:dd-MM-yyyy}");
                    }
                    else if (!string.IsNullOrWhiteSpace(filtro.Identificacion) || filtro.EstadosId.Any() || filtro.ConsolidadoId > 0 || !string.IsNullOrWhiteSpace(filtro.Radicado))
                    {
                        throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encontraron resultados.");
                    }
                    throw new HttpStatusCodeException(HttpStatusCode.NotFound,
                                                 $"No se encontraron resultados en estado {EnumConfig.GetDescription(EstadoEstupefacienteEnum.ParaEnviar)}" +
                                                $" y {EnumConfig.GetDescription(EstadoEstupefacienteEnum.Consulta)} de la fecha: {fechaActual:dd-MM-yyyy}");
                }

            }

            return data.OrderByDescending(x => x.FechaCreacion);
        }


        public async Task<Respuesta> CrearAsync(GENTEMAR_ANTECEDENTES entidad, GENTEMAR_ANTECEDENTES_DATOSBASICOS datosBasicos)
        {

            long idGenteDeMarEstupefaciente = await _estupefacienteDatosBasicosRepository.GetAntecedenteDatosBasicosId(datosBasicos.identificacion);
            if (idGenteDeMarEstupefaciente > 0)
            {
                datosBasicos.IsExist = true;
                datosBasicos.id_gentemar_antecedente = idGenteDeMarEstupefaciente;
                entidad.id_gentemar_antecedente = idGenteDeMarEstupefaciente;
            }

            if (datosBasicos.IsExist)
                await GetDatosGenteMarValidarVciteEnConsultaOVigente(datosBasicos.identificacion);

            await ExisteTramite(entidad.id_tipo_tramite);
            await ValidarRadicadoToCreate(entidad.numero_sgdea, entidad.id_tipo_tramite);
            await ExisteEstadoEstupefaciente(entidad.id_estado_antecedente);
            entidad.fecha_sgdea = entidad.fecha_sgdea ?? DateTime.Now;
            entidad.fecha_solicitud_sede_central = DateTime.Now;
            if (!datosBasicos.IsExist)
            {
                await _estupefacienteRepository.CreateWithPersonGenteMar(entidad, datosBasicos);
            }
            else
            {
                await _estupefacienteRepository.Create(entidad);
            }
            await CrearObservacion(entidad.id_antecedente, entidad.id_estado_antecedente);

            return Responses.SetCreatedResponse(entidad);
        }

        private async Task CrearObservacion(long id_antecedente, int estado)
        {
            var estadoEnum = (EstadoEstupefacienteEnum)estado;
            var observacion = new GENTEMAR_OBSERVACIONES_ANTECEDENTES
            {
                id_antecedente = id_antecedente,
                observacion = $"Registro de estupefaciente creado, en estado {EnumConfig.GetDescription(estadoEnum)}",
            };
            await _observacionesBO.CrearObservacionesEstupefacientes(observacion, string.Empty);
        }

        public async Task<Respuesta> EditarAsync(GENTEMAR_ANTECEDENTES entidad, GENTEMAR_ANTECEDENTES_DATOSBASICOS datosBasicos, string pathActual)
        {

            if (entidad.Observacion == null)
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "La observación es requerida.");


            datosBasicos.id_gentemar_antecedente = entidad.id_gentemar_antecedente;
            await ExisteTramite(entidad.id_tipo_tramite);
            await ValidarRadicadoToEdit(entidad.numero_sgdea, entidad.id_antecedente, entidad.id_tipo_tramite);
            await ExisteEstadoEstupefaciente(entidad.id_estado_antecedente);

            GENTEMAR_ANTECEDENTES estupefacienteActual = await _estupefacienteRepository.GetByIdAsync(entidad.id_antecedente);
            if (estupefacienteActual == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encontro el estupefaciente.");

            GENTEMAR_ANTECEDENTES_DATOSBASICOS estupefacienteDatosBasicosActual = await _estupefacienteDatosBasicosRepository.GetByIdAsync(datosBasicos.id_gentemar_antecedente);

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
                await _estupefacienteRepository.ActualizarAntecedenteWithGenteDeMar(estupefacienteActual, estupefacienteDatosBasicosActual, repositorio);
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


        public async Task ValidarRadicadoToCreate(string radicado, int tipoTramiteId)
        {

            var validarRadicadoIlimitadoPorTramite = ValidarRadicadoSiEsIlimitadoPorTramite(tipoTramiteId);
            if (validarRadicadoIlimitadoPorTramite.existe)
            {
                var existeRadicadoEnTiposTramite = await _estupefacienteRepository.AnyWithConditionAsync(x =>
                                                                     !validarRadicadoIlimitadoPorTramite.tramites.Contains(x.id_tipo_tramite) && x.numero_sgdea.Equals(radicado));

                if (existeRadicadoEnTiposTramite)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"Ya se encuentra registrado el radicado {radicado} en otro tipo de tramite."));
            }
            else
            {
                var existeRadicado = await _estupefacienteRepository.AnyWithConditionAsync(x => x.numero_sgdea.Equals(radicado));
                if (existeRadicado)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"Ya se encuentra registrado el radicado {radicado}"));
            }
        }

        public (List<int> tramites, bool existe) ValidarRadicadoSiEsIlimitadoPorTramite(int tipoTramiteId)
        {
            List<int> tramitesConRadicadoIlimitado = _tramiteEstupefacienteRepository.GetAllAsQueryable()
                                                     .Where(x => x.activo == true && x.id_tipo_tramite != (int)TipoTramiteEstupefacienteEnum.GENTES)
                                                     .Select(y => y.id_tipo_tramite).ToList();

            return (tramitesConRadicadoIlimitado, tramitesConRadicadoIlimitado.Contains(tipoTramiteId));
        }

        public async Task ValidarRadicadoToEdit(string radicado, long idAntecedente, int tipoTramiteId)
        {
            var validarRadicadoIlimitadoPorTramite = ValidarRadicadoSiEsIlimitadoPorTramite(tipoTramiteId);
            if (validarRadicadoIlimitadoPorTramite.existe)
            {
                var existeRadicadoEnTiposTramite = await _estupefacienteRepository.AnyWithConditionAsync(x => !validarRadicadoIlimitadoPorTramite.tramites.Contains(x.id_tipo_tramite)
                                                                 && x.numero_sgdea.Equals(radicado)
                                                                 && x.id_antecedente != idAntecedente);

                if (existeRadicadoEnTiposTramite)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"Ya se encuentra registrado el radicado {radicado} en otro tipo de tramite."));
            }
            else
            {
                var existeRadicado = await _estupefacienteRepository.AnyWithConditionAsync(x => x.numero_sgdea.Equals(radicado)
                                                                          && x.id_antecedente != idAntecedente);
                if (existeRadicado)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"Ya se encuentra registrado el radicado {radicado}"));
            }

        }

        public async Task ExisteTramite(int tipoTramite)
        {
            bool existeTramite = await _tramiteEstupefacienteRepository.AnyWithConditionAsync(x => x.id_tipo_tramite == tipoTramite);
            if (!existeTramite)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El tipo de tramite no existe."));
        }

        public async Task ExisteEstadoEstupefaciente(int estadoEstupefaciente)
        {
            bool existeEstado = await _estadoEstupefacienteRepository.AnyWithConditionAsync(x => x.id_estado_antecedente == estadoEstupefaciente);
            if (!existeEstado)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El estado del estupefaciente no existe."));
        }

        public async Task<IEnumerable<decimal>> GetRadicadosLicenciasByDocumento(string identificacion)
        {
            return await _estupefacienteRepository.GetRadicadosLicenciasByDocumento(identificacion);
        }
        public async Task<IEnumerable<string>> GetRadicadosTitulosByDocumento(string identificacion)
        {
            return await _estupefacienteRepository.GetRadicadosTitulosByDocumento(identificacion);
        }

        public async Task<Respuesta> GetDatosBasicosExisteEnGenteDeMar(string identificacion)
        {
            var dataGenteMar = await _datosBasicosRepository.GetPersonaByIdentificacionOrId(identificacion);
            if (dataGenteMar == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encuentra registrada la persona en Datos Básicos.");

            var idAntecedente = await _estupefacienteDatosBasicosRepository.GetAntecedenteDatosBasicosId(identificacion);
            if (idAntecedente > 0)
            {
                await ValidarVciteConsultaOVigente(idAntecedente);
            }
            return Responses.SetOkResponse(dataGenteMar);
        }

        public async Task<Respuesta> GetDatosGenteMarValidarVciteEnConsultaOVigente(string identificacionConPuntos)
        {

            var data = await _estupefacienteRepository.GetDatosGenteMarEstupefaciente(identificacionConPuntos);

            if (data is null)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"No se encontró la persona registrada en VCITE."));


            await ValidarVciteConsultaOVigente(data.Id);

            data.ExisteEnDatosBasicosEstupefaciente = true;
            return Responses.SetOkResponse(data);
        }

        public async Task ValidarVciteConsultaOVigente(long idVcite)
        {
            var containsVciteActually = await _estupefacienteRepository.ContieneEstupefacienteVigentePorEstado(idVcite);
            if (containsVciteActually != null)
            {
                string fechaVigencia = containsVciteActually.FechaVigencia.HasValue
                    ? $" con fecha de vigencia {containsVciteActually.FechaVigencia.Value:dd/MM/yyyy}"
                    : string.Empty;
                string mensaje = $"La persona {containsVciteActually.NombreCompleto} con No de Identificación " +
                                       $"{containsVciteActually.Identificacion} contiene un estupefaciente en estado " +
                                       $"{containsVciteActually.Estado}{fechaVigencia} y no es posible agregar uno nuevo.";

                throw new HttpStatusCodeException(Responses.SetConflictResponse(mensaje));
            }
        }

        public async Task<Respuesta> GetDetallePersonaEstupefaciente(long id)
        {
            var data = await _estupefacienteRepository.GetDetallePersonaEstupefaciente(id);
            return data == null
                ? throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"No existe el registro del estupefaciente.")
                : Responses.SetOkResponse(data);
        }

        public async Task<IEnumerable<ListarEstupefacientesEdicionMasivaDTO>> GetEstupefacientesSinObservaciones(IList<long> ids)
        {
            if (!ids.Any())
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "Es requerido tener ids de estupefacientes.");

            return await _estupefacienteRepository.GetEstupefacientesSinObservaciones(ids);
        }
    }
}
