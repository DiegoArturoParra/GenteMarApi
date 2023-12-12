using Dapper;
using DIMARCore.UIEntities.DTOs.Reports;
using DIMARCore.UIEntities.QueryFilters.Reports;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using GenteMarCore.Entities;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class ReportesgdmRepository : GenericRepository<GENTEMAR_DATOSBASICOS>
    {
        private readonly CoreContextDapper _coreContextDapper;
        public ReportesgdmRepository()
        {
            _coreContextDapper = new CoreContextDapper();
        }
        public async Task<IEnumerable<DatosBasicosReportDTO>> GetDataByReportDatosBasicos(DatosBasicosReportFilter reportFilter, CancellationTokenSource tokenSource)
        {
            var query = (from datosBasicos in _context.GENTEMAR_DATOSBASICOS
                         join formacionGrado in _context.GENTEMAR_FORMACION_GRADO on datosBasicos.id_formacion_grado equals formacionGrado.id_formacion_grado
                         join formacion in _context.GENTEMAR_FORMACION on formacionGrado.id_formacion equals formacion.id_formacion
                         join grado in _context.APLICACIONES_GRADO on formacionGrado.id_grado equals grado.id_grado
                         join tipoDocumento in _context.APLICACIONES_TIPO_DOCUMENTO on datosBasicos.id_tipo_documento equals tipoDocumento.ID_TIPO_DOCUMENTO
                         join genero in _context.APLICACIONES_GENERO on datosBasicos.id_genero equals genero.ID_GENERO
                         join estado in _context.GENTEMAR_ESTADO on datosBasicos.id_estado equals estado.id_estado
                         join paisNacimiento in _context.TABLA_NAV_BAND on datosBasicos.id_pais_nacimiento.ToString() equals paisNacimiento.cod_pais
                         join paisResidencia in _context.TABLA_NAV_BAND on datosBasicos.id_pais_residencia.ToString() equals paisResidencia.cod_pais
                         select new
                         {
                             datosBasicos,
                             formacionGrado,
                             formacion,
                             grado,
                             tipoDocumento,
                             genero,
                             estado,
                             paisNacimiento,
                             paisResidencia
                         });
            if (reportFilter.EstadosId.Any())
            {
                query = query.Where(y => reportFilter.EstadosId.Contains(y.estado.id_estado));
            }
            if (reportFilter.GeneroId.HasValue)
            {
                query = query.Where(y => y.genero.ID_GENERO == reportFilter.GeneroId.Value);
            }
            if (reportFilter.FormacionId.HasValue)
            {
                query = query.Where(y => y.formacionGrado.id_formacion == reportFilter.FormacionId.Value);
            }
            if (reportFilter.GradosId.Any())
            {
                query = query.Where(y => reportFilter.GradosId.Contains(y.formacionGrado.id_grado.Value));
            }

            if (reportFilter.FechaCreacionInicial.HasValue && reportFilter.FechaCreacionFinal.HasValue)
            {
                var (DateInitial, DateEnd) = Reutilizables.FormatDatesByRange(reportFilter.FechaCreacionInicial.Value, reportFilter.FechaCreacionFinal.Value);
                query = query.Where(x => x.datosBasicos.fecha_hora_creacion >= DateInitial && x.datosBasicos.fecha_hora_creacion <= DateEnd);
            }
            else
            {
                DateTime fechaActual = DateTime.Now;
                // Establecer el mes y el día a 01
                DateTime fechaDeseada = new DateTime(fechaActual.Year, 1, 1);
                var (DateInitial, DateEnd) = Reutilizables.FormatDatesByRange(fechaDeseada, fechaActual);
                query = query.Where(x => x.datosBasicos.fecha_hora_creacion >= DateInitial && x.datosBasicos.fecha_hora_creacion <= DateEnd);
            }
            return await query.Select(x => new DatosBasicosReportDTO
            {
                Nombres = x.datosBasicos.nombres,
                Apellidos = x.datosBasicos.apellidos,
                TipoDocumento = x.tipoDocumento.DESCRIPCION,
                DocumentoIdentificacion = x.datosBasicos.documento_identificacion,
                FechaNacimiento = x.datosBasicos.fecha_nacimiento,
                Genero = x.genero.DESCRIPCION,
                Estado = x.estado.descripcion,
                Formacion = x.formacion.formacion,
                Grado = x.grado.grado,
                PaisNacimiento = x.paisNacimiento.des_pais,
                FechaCreacion = x.datosBasicos.fecha_hora_creacion,
                CorreoElectronico = x.datosBasicos.correo_electronico,
                NumeroContacto = x.datosBasicos.numero_movil,
                PaisResidencia = x.paisResidencia.des_pais
            }).AsNoTracking().ToListAsync(tokenSource.Token);
        }

        public async Task<IEnumerable<LicenciasReportDTO>> GetDataByReportLicencias(LicenciasReportFilter reportFilter, CancellationTokenSource tokenSource)
        {
            var query = (from datosBasicos in _context.GENTEMAR_DATOSBASICOS
                         join tipoDocumento in _context.APLICACIONES_TIPO_DOCUMENTO on datosBasicos.id_tipo_documento equals tipoDocumento.ID_TIPO_DOCUMENTO
                         join genero in _context.APLICACIONES_GENERO on datosBasicos.id_genero equals genero.ID_GENERO
                         join licencias in _context.GENTEMAR_LICENCIAS on datosBasicos.id_gentemar equals licencias.id_gentemar
                         join capitania in _context.APLICACIONES_CAPITANIAS on licencias.id_capitania equals capitania.ID_CAPITANIA
                         join estadoLicencia in _context.GENTEMAR_ESTADO_LICENCIAS on licencias.id_estado_licencia equals estadoLicencia.id_estado_licencias
                         join cargoLicencia in _context.GENTEMAR_CARGO_LICENCIA on licencias.id_cargo_licencia equals cargoLicencia.id_cargo_licencia
                         join seccionClase in _context.GENTEMAR_SECCION_CLASE on cargoLicencia.id_seccion_clase equals seccionClase.id_seccion_clase
                         join categoria in _context.GENTEMAR_CLASE_LICENCIAS on seccionClase.id_clase equals categoria.id_clase
                         join seccion in _context.GENTEMAR_SECCION_LICENCIAS on seccionClase.id_seccion equals seccion.id_seccion
                         join actividadSeccion in _context.GENTEMAR_ACTIVIDAD_SECCION_LICENCIA on cargoLicencia.id_actividad_seccion_licencia equals actividadSeccion.id_actividad_seccion_licencia
                         join actividad in _context.GENTEMAR_ACTIVIDAD on actividadSeccion.id_actividad equals actividad.id_actividad
                         join tipoLicencia in _context.GENTEMAR_TIPO_LICENCIA on actividad.id_tipo_licencia equals tipoLicencia.id_tipo_licencia
                         where estadoLicencia.activo == true && cargoLicencia.activo == true && categoria.activo == true && seccion.activo == true
                         && actividad.activo == true && tipoLicencia.activo == true
                         select new
                         {
                             datosBasicos,
                             tipoDocumento,
                             genero,
                             licencias,
                             capitania,
                             estadoLicencia,
                             cargoLicencia,
                             seccionClase,
                             categoria,
                             seccion,
                             actividadSeccion,
                             actividad,
                             tipoLicencia
                         });
            if (reportFilter.EstadosTramiteId.Any())
            {
                query = query.Where(y => reportFilter.EstadosTramiteId.Contains(y.estadoLicencia.id_estado_licencias));
            }
            if (reportFilter.GeneroId.HasValue)
            {
                query = query.Where(y => y.genero.ID_GENERO == reportFilter.GeneroId.Value);
            }
            if (reportFilter.CapitaniasId.Any())
            {
                query = query.Where(y => reportFilter.CapitaniasId.Contains(y.capitania.ID_CAPITANIA));
            }
            if (reportFilter.TiposDeLicenciaId.Any())
            {
                query = query.Where(y => reportFilter.TiposDeLicenciaId.Contains(y.tipoLicencia.id_tipo_licencia));
            }
            if (reportFilter.SeccionesId.Any())
            {
                query = query.Where(y => reportFilter.SeccionesId.Contains(y.seccion.id_seccion));
            }
            if (reportFilter.ClasesId.Any())
            {
                query = query.Where(y => reportFilter.ClasesId.Contains(y.categoria.id_clase));
            }

            if (reportFilter.ActividadesId.Any())
            {
                query = query.Where(y => reportFilter.ActividadesId.Contains(y.actividad.id_actividad));
            }

            if (reportFilter.CargosLicenciaId.Any())
            {
                query = query.Where(y => reportFilter.CargosLicenciaId.Contains(y.cargoLicencia.id_cargo_licencia));
            }


            if (reportFilter.FechaExpedicionInicial.HasValue && reportFilter.FechaExpedicionFinal.HasValue)
            {
                var (DateInitial, DateEnd) = Reutilizables.FormatDatesByRange(reportFilter.FechaExpedicionInicial.Value, reportFilter.FechaExpedicionFinal.Value);
                query = query.Where(x => x.licencias.fecha_expedicion >= DateInitial && x.licencias.fecha_expedicion <= DateEnd);
            }
            else
            {
                DateTime fechaActual = DateTime.Now;
                // Establecer el mes y el día a 01
                DateTime fechaDeseada = new DateTime(fechaActual.Year, 1, 1);
                var (DateInitial, DateEnd) = Reutilizables.FormatDatesByRange(fechaDeseada, fechaActual);
                query = query.Where(x => x.licencias.fecha_expedicion >= DateInitial && x.licencias.fecha_expedicion <= DateEnd);
            }


            if (reportFilter.FechaVencimientoInicial.HasValue && reportFilter.FechaVencimientoFinal.HasValue)
            {
                var (DateInitial, DateEnd) = Reutilizables.FormatDatesByRange(reportFilter.FechaVencimientoInicial.Value, reportFilter.FechaVencimientoFinal.Value);
                query = query.Where(x => x.licencias.fecha_vencimiento >= DateInitial && x.licencias.fecha_vencimiento <= DateEnd);
            }
            return await query.Select(x => new LicenciasReportDTO
            {
                Nombres = x.datosBasicos.nombres,
                Apellidos = x.datosBasicos.apellidos,
                TipoDocumento = x.tipoDocumento.DESCRIPCION,
                DocumentoIdentificacion = x.datosBasicos.documento_identificacion,
                FechaNacimiento = x.datosBasicos.fecha_nacimiento,
                Genero = x.genero.DESCRIPCION,
                EstadoTramite = x.estadoLicencia.descripcion_estado,
                FechaExpedicion = x.licencias.fecha_expedicion,
                FechaVencimiento = x.licencias.fecha_vencimiento,
                Capitania = x.capitania.DESCRIPCION,
                Radicado = x.licencias.radicado,
                CodigoLicencia = x.cargoLicencia.codigo_licencia,
                TipoLicencia = x.tipoLicencia.tipo_licencia,
                Seccion = x.seccion.actividad_a_bordo,
                Actividad = x.actividad.actividad,
                Categoria = x.categoria.descripcion_clase,
                CargoLicencia = x.cargoLicencia.cargo_licencia
            }).AsNoTracking().ToListAsync(tokenSource.Token);
        }

        public async Task<IEnumerable<TitulosReportDTO>> GetDataByReportTitulosDapper(TitulosReportFilter reportFilter, CancellationTokenSource tokenSource)
        {
            try
            {
                using (IDbConnection db = _coreContextDapper.Context)
                {

                    const string nameProcedure = "DBA.GDM_SP_LISTAR_REPORTE_TITULOS ?, ?, ?, ?, ?, ?, ?, ?, ?";
                    if (reportFilter.FechaExpedicionInicial.HasValue && reportFilter.FechaExpedicionFinal.HasValue)
                    {
                        var (DateInitial, DateEnd) = Reutilizables.FormatDatesByRange(reportFilter.FechaExpedicionInicial.Value, reportFilter.FechaExpedicionFinal.Value);
                        reportFilter.FechaExpedicionInicial = DateInitial;
                        reportFilter.FechaExpedicionFinal = DateEnd;
                    }
                    else
                    {
                        DateTime fechaActual = DateTime.Now;
                        // Establecer el mes y el día a 01
                        DateTime fechaDeseada = new DateTime(fechaActual.Year, 1, 1);
                        var (DateInitial, DateEnd) = Reutilizables.FormatDatesByRange(fechaDeseada, fechaActual);
                        reportFilter.FechaExpedicionInicial = DateInitial;
                        reportFilter.FechaExpedicionFinal = DateEnd;
                    }

                    if (reportFilter.FechaVencimientoInicial.HasValue && reportFilter.FechaVencimientoFinal.HasValue)
                    {
                        var (DateInitial, DateEnd) = Reutilizables.FormatDatesByRange(reportFilter.FechaVencimientoInicial.Value, reportFilter.FechaVencimientoFinal.Value);
                        reportFilter.FechaVencimientoInicial = DateInitial;
                        reportFilter.FechaVencimientoFinal = DateEnd;
                    }

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@GeneroId", reportFilter.GeneroId);
                    queryParameters.Add("@FechaExpedicionInicio", reportFilter.FechaExpedicionInicial);
                    queryParameters.Add("@FechaExpedicionFin", reportFilter.FechaExpedicionFinal);
                    queryParameters.Add("@FechaVencimientoInicio", reportFilter.FechaVencimientoInicial);
                    queryParameters.Add("@FechaVencimientoFin", reportFilter.FechaVencimientoFinal);
                    queryParameters.Add("@CapitaniasIdArrayIn", reportFilter.CapitaniasIdArrayIn);
                    queryParameters.Add("@EstadosTramiteIdArrayIn", reportFilter.EstadosTramiteIdArrayIn);
                    queryParameters.Add("@SeccionesIdArrayIn", reportFilter.SeccionesIdArrayIn);
                    queryParameters.Add("@CargosTituloIdArrayIn", reportFilter.CargosTituloIdArrayIn);


                    var data = await db.QueryAsync<TitulosReportDTO>(
                       new CommandDefinition(nameProcedure, queryParameters, commandType: CommandType.StoredProcedure, cancellationToken: tokenSource.Token));

                    if (tokenSource.Token.IsCancellationRequested)
                    {
                        throw new OperationCanceledException();
                    }
                    return data;
                }
            }
            catch (OperationCanceledException ex)
            {
                // Handle the cancellation
                // Aquí puedes realizar las acciones necesarias si se ha solicitado la cancelación
                throw new HttpStatusCodeException(Responses.SetRequestCanceledResponse(ex, "La operación ha sido cancelada,GetDataByReportTitulosDapper"));
            }
            catch (Exception ex)
            {
                throw new HttpStatusCodeException(System.Net.HttpStatusCode.InternalServerError, ex);
            }
            finally
            {
                _coreContextDapper.CloseConnection();
            }
        }

        /// <summary>
        /// es demasiada lenta al momento de obtener los datos
        /// </summary>
        /// <param name="reportFilter"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TitulosReportDTO>> GetDataByReportTitulosEntityFramework(TitulosReportFilter reportFilter)
        {
            var query = (from datosBasicos in _context.GENTEMAR_DATOSBASICOS
                         join tipoDocumento in _context.APLICACIONES_TIPO_DOCUMENTO on datosBasicos.id_tipo_documento equals tipoDocumento.ID_TIPO_DOCUMENTO
                         join genero in _context.APLICACIONES_GENERO on datosBasicos.id_genero equals genero.ID_GENERO
                         join titulos in _context.GENTEMAR_TITULOS on datosBasicos.id_gentemar equals titulos.id_gentemar
                         join capitania in _context.APLICACIONES_CAPITANIAS on titulos.id_capitania equals capitania.ID_CAPITANIA
                         join estadoTitulo in _context.GENTEMAR_ESTADO_TITULO on titulos.id_estado_tramite equals estadoTitulo.id_estado_tramite
                         join cargosDelTitulo in _context.GENTEMAR_TITULO_REGLA_CARGOS on titulos.id_titulo equals cargosDelTitulo.id_titulo
                         join relacionCargoRegla in _context.GENTEMAR_REGLAS_CARGO on cargosDelTitulo.id_cargo_regla equals relacionCargoRegla.id_cargo_regla
                         join reglas in _context.GENTEMAR_REGLAS on relacionCargoRegla.id_regla equals reglas.id_regla
                         join cargo in _context.GENTEMAR_CARGO_TITULO on relacionCargoRegla.id_cargo_titulo equals cargo.id_cargo_titulo
                         join seccion in _context.GENTEMAR_SECCION_TITULOS on cargo.id_seccion equals seccion.id_seccion
                         group new
                         {
                             datosBasicos,
                             tipoDocumento,
                             genero,
                             titulos,
                             capitania,
                             estadoTitulo,
                             cargosDelTitulo,
                             relacionCargoRegla,
                             reglas,
                             cargo,
                             seccion
                         }
                         by titulos.id_titulo into grupo
                         select new
                         {
                             grupo.Key,
                             grupo.FirstOrDefault().datosBasicos,
                             grupo.FirstOrDefault().tipoDocumento,
                             grupo.FirstOrDefault().genero,
                             grupo.FirstOrDefault().titulos,
                             grupo.FirstOrDefault().capitania,
                             grupo.FirstOrDefault().estadoTitulo,
                             grupo.FirstOrDefault().cargosDelTitulo,
                             grupo.FirstOrDefault().relacionCargoRegla,
                             grupo.FirstOrDefault().reglas,
                             grupo.FirstOrDefault().cargo,
                             grupo.FirstOrDefault().seccion

                         });

            if (reportFilter.EstadosTramiteId.Any())
            {
                query = query.Where(y => reportFilter.EstadosTramiteId.Contains(y.estadoTitulo.id_estado_tramite));
            }
            if (reportFilter.GeneroId.HasValue)
            {
                query = query.Where(y => y.genero.ID_GENERO == reportFilter.GeneroId.Value);
            }
            if (reportFilter.CapitaniasId.Any())
            {
                query = query.Where(y => reportFilter.CapitaniasId.Contains(y.capitania.ID_CAPITANIA));
            }

            if (reportFilter.FechaExpedicionInicial.HasValue && reportFilter.FechaExpedicionFinal.HasValue)
            {
                var (DateInitial, DateEnd) = Reutilizables.FormatDatesByRange(reportFilter.FechaExpedicionFinal.Value, reportFilter.FechaExpedicionFinal.Value);
                query = query.Where(x => x.titulos.fecha_expedicion >= DateInitial && x.titulos.fecha_expedicion <= DateEnd);
            }
            else
            {
                DateTime fechaActual = DateTime.Now;

                // Establecer el mes y el día a 01
                DateTime fechaDeseada = new DateTime(fechaActual.Year, 1, 1);
                var (DateInitial, DateEnd) = Reutilizables.FormatDatesByRange(fechaDeseada, fechaActual);
                query = query.Where(x => x.titulos.fecha_expedicion >= DateInitial && x.titulos.fecha_expedicion <= DateEnd);
            }

            if (reportFilter.FechaVencimientoInicial.HasValue && reportFilter.FechaVencimientoFinal.HasValue)
            {
                var (DateInitial, DateEnd) = Reutilizables.FormatDatesByRange(reportFilter.FechaVencimientoInicial.Value, reportFilter.FechaVencimientoInicial.Value);
                query = query.Where(x => x.titulos.fecha_vencimiento >= DateInitial && x.titulos.fecha_vencimiento <= DateEnd);
            }

            if (reportFilter.SeccionesId.Any())
            {
                query = query.Where(y => reportFilter.SeccionesId.Contains(y.seccion.id_seccion));
            }

            if (reportFilter.CargosTituloId.Any())
            {
                query = query.Where(y => reportFilter.CargosTituloId.Contains(y.cargo.id_cargo_titulo));
            }


            return await query.Select(x => new TitulosReportDTO
            {
                Nombres = x.datosBasicos.nombres,
                Apellidos = x.datosBasicos.apellidos,
                TipoDocumento = x.tipoDocumento.DESCRIPCION,
                DocumentoIdentificacion = x.datosBasicos.documento_identificacion,
                FechaNacimiento = x.datosBasicos.fecha_nacimiento,
                Genero = x.genero.DESCRIPCION,
                Capitania = x.capitania.DESCRIPCION,
                FechaExpedicion = x.titulos.fecha_expedicion,
                FechaVencimiento = x.titulos.fecha_vencimiento,
                EstadoTramite = x.estadoTitulo.descripcion_tramite,
                Radicado = x.titulos.radicado,
                Seccion = x.seccion.actividad_a_bordo
            }).AsNoTracking().ToListAsync();
        }
    }
}
