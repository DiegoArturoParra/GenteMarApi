using DIMARCore.UIEntities.DTOs.Reports;
using DIMARCore.UIEntities.QueryFilters.Reports;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class ReportesgdmRepository : GenericRepository<GENTEMAR_DATOSBASICOS>
    {
        public async Task<IEnumerable<DatosBasicosReportDTO>> GetDataByReportDatosBasicos(DatosBasicosReportFilter reportFilter)
        {
            var query = (from datosBasicos in _context.GENTEMAR_DATOSBASICOS
                         join formacionGrado in _context.GENTEMAR_FORMACION_GRADO on datosBasicos.id_formacion_grado equals formacionGrado.id_formacion_grado
                         join tipoDocumento in _context.APLICACIONES_TIPO_DOCUMENTO on datosBasicos.id_tipo_documento equals tipoDocumento.ID_TIPO_DOCUMENTO
                         join genero in _context.APLICACIONES_GENERO on datosBasicos.id_genero equals genero.ID_GENERO
                         join estado in _context.GENTEMAR_ESTADO on datosBasicos.id_estado equals estado.id_estado
                         join paisNacimiento in _context.TABLA_NAV_BAND on datosBasicos.id_pais_nacimiento.ToString() equals paisNacimiento.cod_pais
                         join paisResidencia in _context.TABLA_NAV_BAND on datosBasicos.id_pais_residencia.ToString() equals paisResidencia.cod_pais
                         select new
                         {
                             datosBasicos,
                             formacionGrado,
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
                query = query.Where(y => y.genero.ID_GENERO == reportFilter.GeneroId);
            }
            if (reportFilter.FormacionId.HasValue)
            {
                query = query.Where(y => y.formacionGrado.id_formacion == reportFilter.FormacionId);
            }
            if (reportFilter.GradoId.HasValue)
            {
                query = query.Where(y => y.formacionGrado.id_grado == reportFilter.GradoId);
            }

            if (reportFilter.FechaCreacionInicial.HasValue && reportFilter.FechaCreacionFinal.HasValue)
            {
                var formatFechaInicioFinal = Reutilizables.FormatDatesByRange(reportFilter.FechaCreacionInicial.Value, reportFilter.FechaCreacionFinal.Value);
                reportFilter.FechaCreacionInicial = formatFechaInicioFinal.DateInitial;
                reportFilter.FechaCreacionFinal = formatFechaInicioFinal.DateEnd;
                query = query.Where(x => x.datosBasicos.fecha_hora_creacion >= reportFilter.FechaCreacionInicial
                && x.datosBasicos.fecha_hora_creacion <= reportFilter.FechaCreacionFinal);
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
                PaisNacimiento = x.paisNacimiento.des_pais,
                FechaCreacion = x.datosBasicos.fecha_hora_creacion,
                CorreoElectronico = x.datosBasicos.correo_electronico,
                NumeroContacto = x.datosBasicos.numero_movil,
                PaisResidencia = x.paisResidencia.des_pais
            }).ToListAsync();
        }
    }
}
