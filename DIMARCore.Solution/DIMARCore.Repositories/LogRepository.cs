using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DIMARCore.Repositories
{
    public class LogRepository : GenericRepository<GENTEMAR_LOGS>
    {
        public async Task CreateSomeLogs(List<GENTEMAR_LOGS> errores)
        {
            _context.GENTEMAR_LOGS.AddRange(errores);
            await _context.SaveChangesAsync();
        }
    }
}
