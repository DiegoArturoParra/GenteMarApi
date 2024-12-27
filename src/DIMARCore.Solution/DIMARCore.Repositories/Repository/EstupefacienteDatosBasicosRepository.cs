using DIMARCore.UIEntities.DTOs.Reports;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.Utilities.Enums;
using GenteMarCore.Entities;
using GenteMarCore.Entities.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class EstupefacienteDatosBasicosRepository : GenericRepository<GENTEMAR_ANTECEDENTES_DATOSBASICOS>
    {
        public EstupefacienteDatosBasicosRepository()
        {

        }
        public EstupefacienteDatosBasicosRepository(GenteDeMarCoreContext context) : base(context) // Llama al constructor de la clase base
        {
        }
        public async Task<long> GetAntecedenteDatosBasicosId(string identificacion) =>
             await Table.Where(x => x.identificacion.Equals(identificacion)).Select(x => x.id_gentemar_antecedente).FirstOrDefaultAsync();

        public async Task<VciteHistoricoPersonaDTO> GetPersonaConDocumento(DocumentFilter documentoFilter)
        {
            return await (from gentemarAntecedente in _context.GENTEMAR_ANTECEDENTES_DATOSBASICOS
                          join tipoDocumento in _context.APLICACIONES_TIPO_DOCUMENTO
                          on gentemarAntecedente.id_tipo_documento equals tipoDocumento.ID_TIPO_DOCUMENTO
                          where gentemarAntecedente.identificacion.Equals(documentoFilter.Identificacion)
                          select new VciteHistoricoPersonaDTO
                          {
                              GenteDeMarId = gentemarAntecedente.id_gentemar_antecedente,
                              DocumentoIdentificacion = gentemarAntecedente.identificacion,
                              TipoDocumento = tipoDocumento.SIGLA + "-" + tipoDocumento.DESCRIPCION,
                              Nombres = gentemarAntecedente.nombres,
                              Apellidos = gentemarAntecedente.apellidos,
                              FechaNacimiento = gentemarAntecedente.fecha_nacimiento
                          }).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<bool> ValidarEstupefacienteNegativoPersona(string identificacion)
        {
            var hayEstupefacienteNegativo = await (from datosBasicosEstupefaciente in _context.GENTEMAR_ANTECEDENTES_DATOSBASICOS
                                                   join estupefaciente in _context.GENTEMAR_ANTECEDENTES
                                                   on datosBasicosEstupefaciente.id_gentemar_antecedente equals estupefaciente.id_gentemar_antecedente
                                                   where datosBasicosEstupefaciente.identificacion == identificacion
                                                   && estupefaciente.id_estado_antecedente == (int)EstadoEstupefacienteEnum.Negativa
                                                   select estupefaciente).AnyAsync();
            return hayEstupefacienteNegativo;
        }

        public async Task<bool> ValidarEstupefacienteVigentePersona(string identificacion, DateTime fechaActual)
        {
            DateTime fechaActualHoraCero = fechaActual.Date; // Esto también devuelve la fecha actual con hora 00:00:00
            var hayEstupefacienteVigente = await (from datosBasicosEstupefaciente in _context.GENTEMAR_ANTECEDENTES_DATOSBASICOS
                                                  join estupefaciente in _context.GENTEMAR_ANTECEDENTES
                                                  on datosBasicosEstupefaciente.id_gentemar_antecedente equals estupefaciente.id_gentemar_antecedente
                                                  where datosBasicosEstupefaciente.identificacion == identificacion
                                                  && estupefaciente.id_estado_antecedente == (int)EstadoEstupefacienteEnum.Exitosa
                                                  && estupefaciente.fecha_vigencia >= fechaActualHoraCero
                                                  select estupefaciente).AnyAsync();
            return hayEstupefacienteVigente;
        }
    }
}
