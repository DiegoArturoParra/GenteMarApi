using AutoMapper;
using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities.Models;

namespace DIMARCore.Api.Core
{
    /// <summary>
    /// Clase para AutoMapper (se integra los perfiles de los dtos que se van a mapear)
    /// </summary>
    public class AutoMapperProfiles
    {
        /// <summary>
        /// Metodo que crea los automappers
        /// </summary>
        /// <returns></returns>
        public static IMapper CreateMapper()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<APLICACIONES_TIPO_REFRENDO, TipoRefrendoDTO>()
                .ForMember(ent => ent.Id, dto => dto.MapFrom(s => s.ID_TIPO_CERTIFICADO))
                .ForMember(ent => ent.Descripcion, dto => dto.MapFrom(s => s.DESCRIPCION)).ReverseMap();

                cfg.CreateMap<APLICACIONES_MUNICIPIO, MunicipioDTO>()
                .ForMember(x => x.Nombre, o => o.MapFrom(s => s.NOMBRE_MUNICIPIO))
                .ForMember(x => x.Id, o => o.MapFrom(s => s.ID_MUNICIPIO))
                .ForMember(x => x.Codigo, o => o.MapFrom(s => s.CODIGO_MUNICIPIO)).ReverseMap();

                cfg.CreateMap<PAISES, PaisDTO>()
                 .ForMember(x => x.Descripcion, o => o.MapFrom(s => s.des_pais))
                 .ForMember(x => x.Sigla2, o => o.MapFrom(s => s.sigla_2))
                 .ForMember(x => x.Codigo, o => o.MapFrom(s => s.cod_pais)).ReverseMap();

                cfg.CreateMap<GENTEMAR_ESTADO_TITULO, EstadoTituloDTO>()
                  .ForMember(ent => ent.Id, dto => dto.MapFrom(s => s.id_estado_tramite))
                  .ForMember(ent => ent.IsActive, dto => dto.MapFrom(s => s.activo))
                  .ForMember(ent => ent.Descripcion, dto => dto.MapFrom(s => s.descripcion_tramite)).ReverseMap();

                cfg.CreateMap<APLICACIONES_TIPO_SOLICITUD, TipoSolicitudDTO>()
                 .ForMember(x => x.Id, o => o.MapFrom(s => s.ID_TIPO_SOLICITUD))
                 .ForMember(x => x.Descripcion, o => o.MapFrom(s => s.DESCRIPCION)).ReverseMap();

                cfg.CreateMap<CreatedUpdateCargoTituloDTO, GENTEMAR_CARGO_TITULO>()
                 .ForMember(ent => ent.id_cargo_titulo, dto => dto.MapFrom(s => s.Id))
                 .ForMember(ent => ent.id_seccion, dto => dto.MapFrom(s => s.SeccionId))
                 .ForMember(ent => ent.id_clase, dto => dto.MapFrom(s => s.ClaseId))
                 .ForMember(ent => ent.cargo, dto => dto.MapFrom(s => s.Descripcion));

                cfg.CreateMap<GENTEMAR_CARGO_TITULO, CargoTituloInfoDTO>()
                 .ForMember(ent => ent.Id, dto => dto.MapFrom(s => s.id_cargo_titulo))
                 .ForMember(ent => ent.SeccionId, dto => dto.MapFrom(s => s.id_seccion))
                 .ForMember(ent => ent.ClaseId, dto => dto.MapFrom(s => s.id_clase))
                 .ForMember(ent => ent.Descripcion, dto => dto.MapFrom(s => s.cargo)).ReverseMap();


                cfg.CreateMap<GENTEMAR_REGLA_CARGO_HABILITACION, HabilitacionDTO>()
                 .ForMember(ent => ent.Id, dto => dto.MapFrom(s => s.GENTEMAR_HABILITACIONES.id_habilitacion))
                 .ForMember(ent => ent.Descripcion, dto => dto.MapFrom(s => s.GENTEMAR_HABILITACIONES.habilitacion)).ReverseMap();

                cfg.CreateMap<GENTEMAR_CLASE_TITULOS, ClaseDTO>()
                .ForMember(ent => ent.Id, dto => dto.MapFrom(s => s.id_clase))
                .ForMember(ent => ent.Sigla, dto => dto.MapFrom(s => s.sigla))
                 .ForMember(ent => ent.IsActive, dto => dto.MapFrom(s => s.activo))
                .ForMember(ent => ent.Descripcion, dto => dto.MapFrom(s => s.descripcion_clase)).ReverseMap();

                cfg.CreateMap<GENTEMAR_CLASE_LICENCIAS, ClaseDTO>()
                  .ForMember(ent => ent.Id, dto => dto.MapFrom(s => s.id_clase))
                  .ForMember(ent => ent.Sigla, dto => dto.MapFrom(s => s.sigla))
                  .ForMember(ent => ent.IsActive, dto => dto.MapFrom(s => s.activo))
                  .ForMember(ent => ent.Descripcion, dto => dto.MapFrom(s => s.descripcion_clase)).ReverseMap();

                cfg.CreateMap<GENTEMAR_REGLAS_CARGO, CargoReglaDTO>()
                .ForMember(ent => ent.CargoId, dto => dto.MapFrom(s => s.id_cargo_titulo))
                .ForMember(ent => ent.NivelId, dto => dto.MapFrom(s => s.id_nivel))
                .ForMember(ent => ent.ReglaId, dto => dto.MapFrom(s => s.id_regla))
                .ForMember(ent => ent.CargoReglaId, dto => dto.MapFrom(s => s.id_cargo_regla))
                .ForMember(ent => ent.CapacidadId, dto => dto.MapFrom(s => s.id_capacidad))
                .ForMember(ent => ent.HabilitacionesId, dto => dto.MapFrom(s => s.Habilitaciones)).ReverseMap();

                cfg.CreateMap<GENTEMAR_REGLAS_CARGO, NivelDTO>()
                 .ForMember(ent => ent.Id, dto => dto.MapFrom(s => s.GENTEMAR_NIVEL.id_nivel))
                 .ForMember(ent => ent.Descripcion, dto => dto.MapFrom(s => s.GENTEMAR_NIVEL.nivel)).ReverseMap();

                cfg.CreateMap<GENTEMAR_TRAMITE_ANTECEDENTE, TramiteEstupefacienteDTO>()
                .ForMember(ent => ent.Id, dto => dto.MapFrom(s => s.id_tipo_tramite))
                .ForMember(ent => ent.IsActive, dto => dto.MapFrom(s => s.activo))
                .ForMember(ent => ent.Descripcion, dto => dto.MapFrom(s => s.descripcion_tipo_tramite)).ReverseMap();

                cfg.CreateMap<GENTEMAR_ESTADO_ANTECEDENTE, EstadoEstupefacienteDTO>()
                .ForMember(ent => ent.Id, dto => dto.MapFrom(s => s.id_estado_antecedente))
                .ForMember(ent => ent.IsActive, dto => dto.MapFrom(s => s.activo))
                .ForMember(ent => ent.Descripcion, dto => dto.MapFrom(s => s.descripcion_estado_antecedente)).ReverseMap();

                cfg.CreateMap<GENTEMAR_ENTIDAD_ANTECEDENTE, EntidadDTO>()
                 .ForMember(ent => ent.Id, dto => dto.MapFrom(s => s.id_entidad))
                 .ForMember(ent => ent.IsActive, dto => dto.MapFrom(s => s.activo))
                 .ForMember(ent => ent.Descripcion, dto => dto.MapFrom(s => s.entidad)).ReverseMap();

                cfg.CreateMap<GENTEMAR_REGLA_FUNCION, FuncionDTO>()
                 .ForMember(ent => ent.Id, dto => dto.MapFrom(s => s.GENTEMAR_FUNCIONES.id_funcion))
                 .ForMember(ent => ent.Descripcion, dto => dto.MapFrom(s => s.GENTEMAR_FUNCIONES.funcion));

                cfg.CreateMap<GENTEMAR_REGLAS_CARGO, CapacidadDTO>()
                 .ForMember(ent => ent.Id, dto => dto.MapFrom(s => s.GENTEMAR_CAPACIDAD.id_capacidad))
                 .ForMember(ent => ent.Descripcion, dto => dto.MapFrom(s => s.GENTEMAR_CAPACIDAD.capacidad));

                cfg.CreateMap<GENTEMAR_SECCION_TITULOS, SeccionDTO>()
               .ForMember(ent => ent.Id, dto => dto.MapFrom(s => s.id_seccion))
               .ForMember(ent => ent.Descripcion, dto => dto.MapFrom(s => s.actividad_a_bordo))
               .ForMember(ent => ent.IsActive, dto => dto.MapFrom(s => s.activo)).ReverseMap();

                cfg.CreateMap<GENTEMAR_SECCION_LICENCIAS, SeccionDTO>()
               .ForMember(ent => ent.Id, dto => dto.MapFrom(s => s.id_seccion))
               .ForMember(ent => ent.Descripcion, dto => dto.MapFrom(s => s.actividad_a_bordo))
               .ForMember(ent => ent.IsActive, dto => dto.MapFrom(s => s.activo))
               .ReverseMap();

                cfg.CreateMap<GENTEMAR_REGLAS, ReglaDTO>()
                 .ForMember(ent => ent.Id, dto => dto.MapFrom(s => s.id_regla))
                 .ForMember(ent => ent.Descripcion, dto => dto.MapFrom(s => s.nombre_regla))
                 .ForMember(ent => ent.IsActive, dto => dto.MapFrom(s => s.activo)).ReverseMap();

                cfg.CreateMap<GENTEMAR_FUNCIONES, FuncionDTO>()
                .ForMember(ent => ent.Id, dto => dto.MapFrom(s => s.id_funcion))
                .ForMember(ent => ent.Descripcion, dto => dto.MapFrom(s => s.funcion))
                .ForMember(ent => ent.Limitacion, dto => dto.MapFrom(s => s.limitacion_funcion))
                .ForMember(ent => ent.IsActive, dto => dto.MapFrom(s => s.activo)).ReverseMap();

                cfg.CreateMap<GENTEMAR_HABILITACION, HabilitacionDTO>()
                .ForMember(ent => ent.Id, dto => dto.MapFrom(s => s.id_habilitacion))
                .ForMember(ent => ent.Descripcion, dto => dto.MapFrom(s => s.habilitacion))
                .ForMember(ent => ent.IsActive, dto => dto.MapFrom(s => s.activo)).ReverseMap();

                cfg.CreateMap<GENTEMAR_NIVEL, NivelDTO>()
                .ForMember(ent => ent.Id, dto => dto.MapFrom(s => s.id_nivel))
                .ForMember(ent => ent.Descripcion, dto => dto.MapFrom(s => s.nivel))
                .ForMember(ent => ent.IsActive, dto => dto.MapFrom(s => s.activo)).ReverseMap();

                cfg.CreateMap<GENTEMAR_CAPACIDAD, CapacidadDTO>()
                .ForMember(ent => ent.Id, dto => dto.MapFrom(s => s.id_capacidad))
                .ForMember(ent => ent.Descripcion, dto => dto.MapFrom(s => s.capacidad))
                .ForMember(ent => ent.IsActive, dto => dto.MapFrom(s => s.activo)).ReverseMap();

                cfg.CreateMap<APLICACIONES_CAPITANIAS, CapitaniaDTO>()
                 .ForMember(ent => ent.Id, dto => dto.MapFrom(s => s.ID_CAPITANIA))
                 .ForMember(ent => ent.Sigla, dto => dto.MapFrom(s => s.SIGLA_CAPITANIA))
                 .ForMember(ent => ent.Descripcion, dto => dto.MapFrom(s => s.DESCRIPCION)).ReverseMap();

                cfg.CreateMap<GENTEMAR_TITULOS, TituloDTO>()
                 .ForMember(ent => ent.TituloId, dto => dto.MapFrom(s => s.id_titulo))
                 .ForMember(ent => ent.GenteMarId, dto => dto.MapFrom(s => s.id_gentemar))
                 .ForMember(ent => ent.FechaExpedicion, dto => dto.MapFrom(s => s.fecha_expedicion))
                 .ForMember(ent => ent.FechaVencimiento, dto => dto.MapFrom(s => s.fecha_vencimiento))
                 .ForMember(ent => ent.CapitaniaId, dto => dto.MapFrom(s => s.id_capitania))
                 .ForMember(ent => ent.CapitaniaFirmanteId, dto => dto.MapFrom(s => s.id_capitania_firmante))
                 .ForMember(ent => ent.CodigoPais, dto => dto.MapFrom(s => s.cod_pais))
                 .ForMember(ent => ent.CargosDelTitulo, dto => dto.MapFrom(s => s.Cargos))
                 .ForMember(ent => ent.Radicado, dto => dto.MapFrom(s => s.radicado))
                 .ForMember(ent => ent.TipoSolicitudId, dto => dto.MapFrom(s => s.id_tipo_solicitud))
                 .ForMember(ent => ent.TipoRefrendoId, dto => dto.MapFrom(s => s.id_tipo_refrendo))
                 .ForMember(ent => ent.EstadoTramiteId, dto => dto.MapFrom(s => s.id_estado_tramite)).ReverseMap();

                cfg.CreateMap<CargosReglaDTO, CargosTitulo>()
                 .ForMember(ent => ent.IdsRelacion, dto => dto.MapFrom(s => s.IdsLlaveCompuesta))
                 .ForMember(ent => ent.HabilitacionesId, dto => dto.MapFrom(s => s.HabilitacionesId))
                 .ForMember(ent => ent.FuncionesId, dto => dto.MapFrom(s => s.FuncionesId))
                 .ForMember(ent => ent.TituloCargoReglaId, dto => dto.MapFrom(s => s.TituloCargoReglaId))
                 .ForMember(ent => ent.CargoReglaId, dto => dto.MapFrom(s => s.CargoReglaId));

                cfg.CreateMap<GENTEMAR_DATOSBASICOS, DatosBasicosDTO>()
                .ForMember(ent => ent.CodPais, dto => dto.MapFrom(s => s.cod_pais))
                .ForMember(ent => ent.CorreoElectronico, dto => dto.MapFrom(s => s.correo_electronico))
                .ForMember(ent => ent.DocumentoIdentificacion, dto => dto.MapFrom(s => s.documento_identificacion))
                .ForMember(ent => ent.FechaExpedicion, dto => dto.MapFrom(s => s.fecha_expedicion))
                .ForMember(ent => ent.FechaNacimiento, dto => dto.MapFrom(s => s.fecha_nacimiento))
                .ForMember(ent => ent.FechaVencimiento, dto => dto.MapFrom(s => s.fecha_vencimiento))
                .ForMember(ent => ent.IdEstado, dto => dto.MapFrom(s => s.id_estado))
                .ForMember(ent => ent.IdFormacionGrado, dto => dto.MapFrom(s => s.id_formacion_grado))
                .ForMember(ent => ent.IdGenero, dto => dto.MapFrom(s => s.id_genero))
                .ForMember(ent => ent.IdGentemar, dto => dto.MapFrom(s => s.id_gentemar))
                .ForMember(ent => ent.IdMunicipioExpedicion, dto => dto.MapFrom(s => s.id_municipio_expedicion))
                .ForMember(ent => ent.IdPaisNacimiento, dto => dto.MapFrom(s => s.id_pais_nacimiento))
                .ForMember(ent => ent.IdPaisResidencia, dto => dto.MapFrom(s => s.id_pais_residencia))
                .ForMember(ent => ent.IdMunicipioResidencia, dto => dto.MapFrom(s => s.id_municipio_residencia))
                .ForMember(ent => ent.IdTipoDocumento, dto => dto.MapFrom(s => s.id_tipo_documento))
                .ForMember(ent => ent.NumeroMovil, dto => dto.MapFrom(s => s.numero_movil))
                .ForMember(ent => ent.Apellidos, dto => dto.MapFrom(s => s.apellidos))
                .ForMember(ent => ent.Direccion, dto => dto.MapFrom(s => s.direccion))
                .ForMember(ent => ent.Nombres, dto => dto.MapFrom(s => s.nombres))
                .ForMember(ent => ent.Telefono, dto => dto.MapFrom(s => s.telefono))
                .ReverseMap();

                cfg.CreateMap<GENTEMAR_FORMACION, FormacionDTO>().ReverseMap();
                cfg.CreateMap<GENTEMAR_ESTADO, EstadoDTO>().ReverseMap();
                cfg.CreateMap<APLICACIONES_RANGO, RangoDTO>().ReverseMap();
                cfg.CreateMap<APLICACIONES_GRADO, GradoInfoDTO>().ReverseMap();

                cfg.CreateMap<ObservacionDTO, GENTEMAR_OBSERVACIONES_DATOSBASICOS>()
                   .ForMember(ent => ent.id_gentemar, dto => dto.MapFrom(s => s.IdTablaRelacion))
                   .ForMember(ent => ent.observacion, dto => dto.MapFrom(s => s.Observacion.Trim()))
                    .ForMember(ent => ent.Archivo, dto => dto.MapFrom(s => s.Archivo));

                cfg.CreateMap<ObservacionDTO, GENTEMAR_OBSERVACIONES_TITULOS>()
                   .ForMember(ent => ent.id_titulo, dto => dto.MapFrom(s => s.IdTablaRelacion))
                   .ForMember(ent => ent.observacion, dto => dto.MapFrom(s => s.Observacion.Trim()))
                   .ForMember(ent => ent.Archivo, dto => dto.MapFrom(s => s.Archivo));

                cfg.CreateMap<ObservacionDTO, GENTEMAR_OBSERVACIONES_ANTECEDENTES>()
                   .ForMember(ent => ent.id_antecedente, dto => dto.MapFrom(s => s.IdTablaRelacion))
                   .ForMember(ent => ent.observacion, dto => dto.MapFrom(s => s.Observacion.Trim()))
                   .ForMember(ent => ent.Archivo, dto => dto.MapFrom(s => s.Archivo));


                cfg.CreateMap<LimitacionDTO, GENTEMAR_LIMITACION>()
                   .ForMember(ent => ent.id_limitacion, dto => dto.MapFrom(s => s.IdLimitacion))
                   .ForMember(ent => ent.limitaciones, dto => dto.MapFrom(s => s.Limitaciones))
                    .ForMember(ent => ent.activo, dto => dto.MapFrom(s => s.Activo))
                    .ReverseMap();

                cfg.CreateMap<TipoLicenciaDTO, GENTEMAR_TIPO_LICENCIA>()
                  .ForMember(ent => ent.id_tipo_licencia, dto => dto.MapFrom(s => s.IdTipoLicencia))
                  .ForMember(ent => ent.tipo_licencia, dto => dto.MapFrom(s => s.TipoLicencia))
                  .ForMember(ent => ent.activo, dto => dto.MapFrom(s => s.Activo))
                   .ReverseMap();

                cfg.CreateMap<ActividadTipoLicenciaDTO, GENTEMAR_ACTIVIDAD>()
                 .ForMember(ent => ent.id_actividad, dto => dto.MapFrom(s => s.IdActividad))
                 .ForMember(ent => ent.actividad, dto => dto.MapFrom(s => s.Actividad))
                 .ForMember(ent => ent.activo, dto => dto.MapFrom(s => s.Activo))
                 .ForMember(ent => ent.GENTEMAR_TIPO_LICENCIA, dto => dto.MapFrom(s => s.TipoLicencia))
                  .ReverseMap();

                cfg.CreateMap<CargoInfoLicenciaDTO, GENTEMAR_CARGO_LICENCIA>()
                 .ForMember(ent => ent.activo, dto => dto.MapFrom(s => s.Activo))
                 .ForMember(ent => ent.cargo_licencia, dto => dto.MapFrom(s => s.CargoLicencia))
                 .ForMember(ent => ent.codigo_licencia, dto => dto.MapFrom(s => s.CodigoLicencia))
                 .ForMember(ent => ent.id_cargo_licencia, dto => dto.MapFrom(s => s.IdCargoLicencia))
                 .ForMember(ent => ent.vigencia, dto => dto.MapFrom(s => s.Vigencia))
                 .ForMember(ent => ent.id_actividad_seccion_licencia, dto => dto.MapFrom(s => s.IdActividadSeccion))
                 .ForMember(ent => ent.id_seccion_clase, dto => dto.MapFrom(s => s.IdSeccionClase))
                 .ForMember(ent => ent.nave, dto => dto.MapFrom(s => s.Nave))
                 .ReverseMap();

                cfg.CreateMap<CategoriaDTO, APLICACIONES_CATEGORIA>()
                 .ForMember(ent => ent.ID_CATEGORIA, dto => dto.MapFrom(s => s.IdCategoria))
                 .ForMember(ent => ent.SIGLA_CATEGORIA, dto => dto.MapFrom(s => s.SiglaCategoria))
                 .ForMember(ent => ent.DESCRIPCION, dto => dto.MapFrom(s => s.Descripcion))
                 .ReverseMap();

                cfg.CreateMap<LicenciaDTO, GENTEMAR_LICENCIAS>()
                 .ForMember(ent => ent.id_capitania, dto => dto.MapFrom(s => s.IdCapitania))
                 .ForMember(ent => ent.id_capitania_firmante, dto => dto.MapFrom(s => s.IdCapitaniaFirmante))
                 .ForMember(ent => ent.id_cargo_licencia, dto => dto.MapFrom(s => s.IdCargoLicencia))
                 .ForMember(ent => ent.id_estado_licencia, dto => dto.MapFrom(s => s.IdEstadoLicencia))
                 .ForMember(ent => ent.id_gentemar, dto => dto.MapFrom(s => s.IdGentemar))
                 .ForMember(ent => ent.id_licencia, dto => dto.MapFrom(s => s.IdLicencia))
                 .ForMember(ent => ent.radicado, dto => dto.MapFrom(s => s.Radicado))
                 .ForMember(ent => ent.activo, dto => dto.MapFrom(s => s.Activo))
                 .ForMember(ent => ent.fecha_expedicion, dto => dto.MapFrom(s => s.FechaExpedicion))
                 .ForMember(ent => ent.fecha_vencimiento, dto => dto.MapFrom(s => s.FechaVencimiento))
                 .ForMember(ent => ent.ListaNaves, dto => dto.MapFrom(s => s.ListaNaves))
                 .ReverseMap();

                cfg.CreateMap<CambioEstadoLicenciaDTO, GENTEMAR_LICENCIAS>()
               .ForMember(ent => ent.id_licencia, dto => dto.MapFrom(s => s.IdLicencia))
               .ForMember(ent => ent.activo, dto => dto.MapFrom(s => s.Activo));

                cfg.CreateMap<EstadoLicenciaDTO, GENTEMAR_ESTADO_LICENCIA>()
                .ForMember(ent => ent.id_estado_licencias, dto => dto.MapFrom(s => s.IdEstadoLicencias))
                .ForMember(ent => ent.descripcion_estado, dto => dto.MapFrom(s => s.DescripcionEstado))
                .ForMember(ent => ent.activo, dto => dto.MapFrom(s => s.Activo))
                .ReverseMap();

                cfg.CreateMap<ObservacionDTO, GENTEMAR_OBSERVACIONES_LICENCIAS>()
                .ForMember(ent => ent.id_licencia, dto => dto.MapFrom(s => s.IdTablaRelacion))
                .ForMember(ent => ent.observacion, dto => dto.MapFrom(s => s.Observacion.Trim()))
                .ForMember(ent => ent.Archivo, dto => dto.MapFrom(s => s.Archivo));

                cfg.CreateMap<LimitanteDTO, GENTEMAR_LIMITANTE>()
                .ForMember(ent => ent.id_limitante, dto => dto.MapFrom(s => s.IdLimitante))
                .ForMember(ent => ent.descripcion, dto => dto.MapFrom(s => s.Descripcion))
                .ReverseMap();

                cfg.CreateMap<EstupefacienteCrearDTO, GENTEMAR_ANTECEDENTES>()
                .ForMember(ent => ent.id_antecedente, dto => dto.MapFrom(s => s.EstupefacienteId))
                .ForMember(ent => ent.id_gentemar_antecedente, dto => dto.MapFrom(s => s.GenteDeMarId))
                .ForMember(ent => ent.numero_sgdea, dto => dto.MapFrom(s => s.Radicado))
                .ForMember(ent => ent.fecha_sgdea, dto => dto.MapFrom(s => s.FechaRadicadoSgdea))
                .ForMember(ent => ent.id_estado_antecedente, dto => dto.MapFrom(s => s.EstadoId))
                .ForMember(ent => ent.id_tipo_tramite, dto => dto.MapFrom(s => s.TramiteId));

                cfg.CreateMap<EstupefacienteDatosBasicosDTO, GENTEMAR_ANTECEDENTES_DATOSBASICOS>()
                .ForMember(ent => ent.identificacion, dto => dto.MapFrom(s => s.Identificacion))
                .ForMember(ent => ent.fecha_nacimiento, dto => dto.MapFrom(s => s.FechaNacimiento))
                .ForMember(ent => ent.nombres, dto => dto.MapFrom(s => s.Nombres.Trim()))
                .ForMember(ent => ent.apellidos, dto => dto.MapFrom(s => s.Apellidos.Trim()))
                .ForMember(ent => ent.id_tipo_documento, dto => dto.MapFrom(s => s.TipoDocumentoId))
                .ForMember(ent => ent.IsExist, dto => dto.MapFrom(s => s.IsExist));

                cfg.CreateMap<ObservacionEntidadEstupefacienteDTO, GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES>()
                .ForMember(ent => ent.id_entidad, dto => dto.MapFrom(s => s.EntidadId))
                .ForMember(ent => ent.descripcion_observacion, dto => dto.MapFrom(s => s.DetalleObservacion))
                .ForMember(ent => ent.verificacion_exitosa, dto => dto.MapFrom(s => s.VerificacionExitosa))
                .ForMember(ent => ent.fecha_respuesta_entidad, dto => dto.MapFrom(s => s.FechaRespuestaEntidad));

                cfg.CreateMap<NavesDTO, NAVES_BASE>()
                .ForMember(ent => ent.identi, dto => dto.MapFrom(s => s.Identi))
                .ForMember(ent => ent.nom_naves, dto => dto.MapFrom(s => s.NomNaves))
                .ReverseMap();

                cfg.CreateMap<GENTEMAR_CONSOLIDADO, ConsolidadoDTO>()
                .ForMember(ent => ent.Id, dto => dto.MapFrom(s => s.id_consolidado))
                .ForMember(ent => ent.Descripcion, dto => dto.MapFrom(s => s.numero_consolidado))
                .ReverseMap();

                cfg.CreateMap<GENTEMAR_EXPEDIENTE, ExpedienteDTO>()
                .ForMember(ent => ent.Id, dto => dto.MapFrom(s => s.id_expediente))
                .ForMember(ent => ent.Descripcion, dto => dto.MapFrom(s => s.numero_expediente))
                .ReverseMap();

                cfg.CreateMap<EditInfoEstupefacienteDTO, GENTEMAR_ANTECEDENTES>()
                .ForMember(ent => ent.id_antecedente, dto => dto.MapFrom(s => s.EstupefacienteId))
                .ForMember(ent => ent.id_estado_antecedente, dto => dto.MapFrom(s => s.EstadoId))
                .ForMember(ent => ent.id_gentemar_antecedente, dto => dto.MapFrom(s => s.GenteDeMarId))
                .ForMember(ent => ent.fecha_vigencia, dto => dto.MapFrom(s => s.FechaVigencia))
                .ForMember(ent => ent.fecha_solicitud_sede_central, dto => dto.MapFrom(s => s.FechaSolicitudSedeCentral))
                .ForMember(ent => ent.id_tipo_tramite, dto => dto.MapFrom(s => s.TramiteId))
                .ForMember(ent => ent.id_capitania, dto => dto.MapFrom(s => s.CapitaniaId))
                .ForMember(ent => ent.numero_sgdea, dto => dto.MapFrom(s => s.Radicado))
                .ForMember(ent => ent.fecha_sgdea, dto => dto.MapFrom(s => s.FechaRadicadoSgdea))
                .ForMember(ent => ent.fecha_aprobacion, dto => dto.MapFrom(s => s.FechaAprobacion));

            });
            return mapperConfiguration.CreateMapper();
        }
    }
}