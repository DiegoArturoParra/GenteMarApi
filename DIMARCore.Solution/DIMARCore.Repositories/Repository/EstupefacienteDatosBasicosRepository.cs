using DIMARCore.UIEntities.DTOs.Reports;
using DIMARCore.UIEntities.QueryFilters;
using GenteMarCore.Entities.Models;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class EstupefacienteDatosBasicosRepository : GenericRepository<GENTEMAR_ANTECEDENTES_DATOSBASICOS>
    {
        public async Task<long> GetAntecedenteDatosBasicosId(string identificacion) =>
             await Table.Where(x => x.identificacion.Equals(identificacion)).Select(x => x.id_gentemar_antecedente).FirstOrDefaultAsync();

        public async Task<VciteHistoricoPersonaDTO> GetPersonaConDocumento(DocumentFilter documentoFilter)
        {
            return await (from gentemarAntecedente in _context.GENTEMAR_ANTECEDENTES_DATOSBASICOS
                          join tipoDocumento in _context.APLICACIONES_TIPO_DOCUMENTO on gentemarAntecedente.id_tipo_documento equals tipoDocumento.ID_TIPO_DOCUMENTO
                          where gentemarAntecedente.identificacion.Equals(documentoFilter.Identificacion)
                          select new VciteHistoricoPersonaDTO
                          {
                              GenteDeMarId = gentemarAntecedente.id_gentemar_antecedente,
                              DocumentoIdentificacion = gentemarAntecedente.identificacion,
                              TipoDocumento = tipoDocumento.SIGLA + "-" + tipoDocumento.DESCRIPCION,
                              Nombres = gentemarAntecedente.nombres,
                              Apellidos = gentemarAntecedente.apellidos,
                              FechaNacimiento = gentemarAntecedente.fecha_nacimiento
                          }).FirstOrDefaultAsync();
        }
    }
}
