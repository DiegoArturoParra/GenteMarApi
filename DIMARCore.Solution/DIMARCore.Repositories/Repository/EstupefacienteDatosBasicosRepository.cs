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

    }
}
