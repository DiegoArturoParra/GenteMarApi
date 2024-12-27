using GenteMarCore.Entities.Models;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class AutenticacionRepository : GenericRepository<GENTEMAR_AUTENTICACION>
    {
        public async Task RemoverTokensPorUsuarioId(int loginId)
        {
            _context.GENTEMAR_AUTENTICACION.RemoveRange(Table.Where(x => x.IdUsuario == loginId));
            await _context.SaveChangesAsync();
        }
    }
}
