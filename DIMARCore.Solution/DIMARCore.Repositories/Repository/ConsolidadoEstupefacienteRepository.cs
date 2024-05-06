using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Enums;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class ConsolidadoEstupefacienteRepository : GenericRepository<GENTEMAR_CONSOLIDADO>
    {
        public async Task<IEnumerable<ConsolidadoDTO>> GetConsolidadosEnUso()
        {
            return await (from expedienteObservacion in _context.GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES
                          join consolidado in Table on expedienteObservacion.id_consolidado equals consolidado.id_consolidado
                          group expedienteObservacion by new { expedienteObservacion.id_consolidado, consolidado.numero_consolidado } into grupo
                          select new ConsolidadoDTO
                          {
                              Id = grupo.Key.id_consolidado,
                              Descripcion = grupo.Key.numero_consolidado,
                          }).ToListAsync();
        }

        public async Task<IEnumerable<long>> GetAllIdsEstupefacienteByConsolidado(int consolidadoId)
        {
            return await (from expedienteObservacion in _context.GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES
                          join consolidado in Table on expedienteObservacion.id_consolidado equals consolidado.id_consolidado
                          where expedienteObservacion.id_consolidado == consolidadoId
                          group expedienteObservacion by new { expedienteObservacion.id_antecedente } into grupo
                          select grupo.Key.id_antecedente).ToListAsync();
        }

        public async Task<string> GetLastConsolidado()
        {
            return await Table.OrderByDescending(x => x.id_consolidado).Select(x => x.numero_consolidado).FirstOrDefaultAsync();
        }

        public async Task<List<EstupefacientesExcelDTO>> GetEstupefacientesEnConsultaPorConsolidado(string numeroConsolidado)
        {
            var query = (from antecedente in _context.GENTEMAR_ANTECEDENTES
                         join usuario in _context.GENTEMAR_ANTECEDENTES_DATOSBASICOS on antecedente.id_gentemar_antecedente equals usuario.id_gentemar_antecedente
                         join documento in _context.APLICACIONES_TIPO_DOCUMENTO on usuario.id_tipo_documento equals documento.ID_TIPO_DOCUMENTO
                         join tramite in _context.GENTEMAR_TRAMITE_ANTECEDENTE on antecedente.id_tipo_tramite equals tramite.id_tipo_tramite
                         join capitaniaFirma in _context.APLICACIONES_CAPITANIAS on antecedente.id_capitania equals capitaniaFirma.ID_CAPITANIA
                         where antecedente.id_estado_antecedente == (int)EstadoEstupefacienteEnum.Consulta
                         select new
                         {
                             antecedente,
                             usuario,
                             tramite,
                             capitaniaFirma,
                             documento
                         });

            return await query.OrderBy(x => x.antecedente.id_estado_antecedente).Select(m => new EstupefacientesExcelDTO
            {
                EstupefacienteId = m.antecedente.id_antecedente,
                Nombres = m.usuario.nombres,
                Apellidos = m.usuario.apellidos,
                Radicado = m.antecedente.numero_sgdea,
                TipoTramite = m.tramite.descripcion_tipo_tramite,
                LugarTramiteCapitania = m.capitaniaFirma.SIGLA_CAPITANIA + "-" + m.capitaniaFirma.DESCRIPCION,
                Documento = m.usuario.identificacion,
                FechaSolicitudSedeCentral = m.antecedente.fecha_solicitud_sede_central,
                FechaRadicado = m.antecedente.fecha_sgdea,
                FechaNacimiento = m.usuario.fecha_nacimiento,
                TipoDocumento = m.documento.SIGLA
            }).ToListAsync();
        }
    }
}
